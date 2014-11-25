using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour
{
	//player speeds
	public float speed = 1F;
	public float rotationSpeed = 0.25F;
	
	//walk animation
	Animator anim;
	
	//player health
	public int hp = 100;
	//health cap
	public int hp_max = 200;
	//current level
	public int level;
	//current score
	public int score = 0;
	//current ammo
	public int ammunition = 100;
	//location variables 
	public Vector2 pos;
	public Vector2 size;
	
	//on startup - happens each time a level is loaded
	void Start() {
		//playerprefs are used to carry data over between levels, because
		//loading a new level completely re-initializes a new player object
		if(PlayerPrefs.HasKey ("score")){
			score = PlayerPrefs.GetInt ("score");
		}else{
			score = 0;
			PlayerPrefs.SetInt("score",0);
		}
		if (PlayerPrefs.HasKey ("hp")) {
			hp = PlayerPrefs.GetInt("hp");
		}else{
			hp = 100;
			PlayerPrefs.SetInt("hp",100);
		}
		if(PlayerPrefs.HasKey ("ammo")){
			ammunition = PlayerPrefs.GetInt ("ammo");
		}else{
			ammunition = 100;
			PlayerPrefs.SetInt("ammo",100);
		}
		if (PlayerPrefs.HasKey ("wep")) {
			transform.FindChild("sniper").GetComponent<WeaponScript>().shootingRate = (PlayerPrefs.GetInt("wep")/100);
		}else{
			transform.FindChild("sniper").GetComponent<WeaponScript>().shootingRate = 0.25f;
			PlayerPrefs.SetInt("wep",(int)(transform.FindChild("sniper").GetComponent<WeaponScript>().shootingRate*100));
		}
		
		//sets start position initializes walk animation
		pos = new Vector2(60,20);
		size = new Vector2(100,20);
		anim = GetComponent<Animator> ();
		level = (int) Application.loadedLevel;
	}
	
	//function to take damage
	public void Hit(int power)
	{
		hp -= power;
		saveHP ();
	}
	//increase score for hits
	public void Score()
	{
		score += level * 10;
		saveScore ();
	}
	//these save functions are separate so that they can be called outside of this script
	//if that becomes necessary for something
	//store score as a playerpref
	public void saveScore (){
		PlayerPrefs.SetInt ("score", score);
	}
	//store hp
	public void saveHP(){
		PlayerPrefs.SetInt ("hp", hp);
	}
	//store weapon shooting speed
	public void saveWep(){
		PlayerPrefs.SetInt ("wep", (int)(transform.FindChild("sniper").GetComponent<WeaponScript>().shootingRate*100));
	}
	
	void OnGUI() {
		//displays health, score, and ammo
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, (size.y*3)));
		if( hp <= 0){hp=0;}
		string hpDisplay = "Health: " + hp.ToString();
		GUI.Box(new Rect(0,0, size.x, size.y), hpDisplay);
		string scoreDisplay = "Score: " + score.ToString();
		GUI.Box(new Rect(0,size.y, size.x, size.y), scoreDisplay);
		string ammo = "Ammo: " + ammunition.ToString();
		GUI.Box(new Rect(0,(size.y*2), size.x, size.y), ammo);
		GUI.EndGroup();
		//menu button
		if (GUI.Button(new Rect(1250, 20, 60, 40), "Menu"))
		{
			print("You clicked the menu");
		}
	}
	
	//handles all non-bullet collisions
	//bullet collisions are handled in shotscript
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		if (otherCollider.gameObject.name.Contains ("Finish")) {
			print ("made it to the finish");
			// load next level
			gameObject.AddComponent<FinishScript>();
		}
		
		//chests increase health and give the player 20 rounds of ammo
		if (otherCollider.gameObject.name == "Chest") {
			print ("found a chest");
			if ( hp < hp_max ) {
				hp += 10 + (10 * level);
			}
			PlayerPrefs.SetInt ("ammo", PlayerPrefs.GetInt ("ammo")+20);
			Destroy(otherCollider.gameObject);
		}
		
		if (otherCollider.gameObject.name == "GunUpgrade") {
			print ("found a gun upgrade");
			// increase the players weapon fire rate
			WeaponScript weapon = transform.FindChild("sniper").GetComponent<WeaponScript>();
			if (weapon != null)
			{			
				weapon.shootingRate -= .1F;
				saveWep();
			}
			// destroy the gun ugrade object
			Destroy(otherCollider.gameObject);
		}
	}
	
	void FixedUpdate()
	{
		ammunition = PlayerPrefs.GetInt("ammo");
		//if alive
		if(hp > 0)
		{
			// movement and rotation
			float translation = Input.GetAxis("Vertical") * speed;
			float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
			translation *= Time.deltaTime/5;
			transform.Translate(0, translation, 0);
			transform.Rotate(0, 0, -rotation);
			
			anim.SetFloat("Speed", Mathf.Abs (translation));
			
			// shooting
			bool shoot = Input.GetButtonDown("Fire1");
			shoot |= Input.GetButtonDown("Fire2");
			// Careful: For Mac users, ctrl + arrow is a bad idea
			
			if (shoot)
			{
				WeaponScript weapon = transform.FindChild("sniper").GetComponent<WeaponScript>();
				if (weapon != null)
				{
					// false because the player is not an enemy
					weapon.Attack(false,false);
				}
			}
		}
		else if(hp <= 0){
			hp = 0;
			print ("you died!");
			if(gameObject.name == "Player"){
				print ("game over!");
				
				// display the game over screen
				gameObject.AddComponent<GameOverScript>();
			}
		}
	}
}

