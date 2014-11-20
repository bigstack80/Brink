//remos
using UnityEngine;
using System;
/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
	//player speeds
	public float speed = 1F;
	public float rotationSpeed = 0.25F;

	Animator anim;

	//player health
	public int hp = 10;
	public int hp_max = 20;
	public Vector2 pos;
	public Vector2 size;
	public float barDisplay; //current health
	public Texture2D emptyTex;
	public Texture2D fullTex;


	void Start() {
		pos = new Vector2(60,20);
		size = new Vector2(100,20);
		anim = GetComponent<Animator> ();
	}

	void OnGUI() {
		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
		GUI.EndGroup();
		GUI.EndGroup();
		if (GUI.Button(new Rect(1250, 20, 60, 40), "Menu"))
		{
			print("You clicked the menu");
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{

		if (otherCollider.gameObject.name == "Bullet(Clone)") {
			ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
			if (shot != null)
			{
				// Avoid friendly fire
				//true for enemy, false for player
				if (shot.isEnemyShot != false)
				{	
					print ("hit player with enemy bullet");
					hp--;
					// Destroy the shot
					Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
				}
			}
		}

		if (otherCollider.gameObject.name == "Finish1") {
			print ("made it to the finish");

			// load level 2
			gameObject.AddComponent<FinishScript>();
		}

		if (otherCollider.gameObject.name == "Finish2") {
			print ("made it to the finish");
			
			// load level 2
			gameObject.AddComponent<FinishScript2>();
		}

		if (otherCollider.gameObject.name == "Chest") {
			print ("found a chest");
			if ( hp < hp_max ) {
				hp++;
			}
			Destroy(otherCollider.gameObject);
		}

		if (otherCollider.gameObject.name == "GunUpgrade") {
			print ("found a gun upgrade");

			// increase the players weapon fire rate
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{			
				weapon.shootingRate -= .1F;
			}

			// destroy the gun ugrade object
			Destroy(otherCollider.gameObject);

		}

	}

	void FixedUpdate()
	{
		barDisplay = hp*10;


		if(hp > 0)
		{
			// movement and rotation
			float translation = Input.GetAxis("Vertical") * speed;
			float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
			translation *= Time.deltaTime/5;
			transform.Translate(0, translation, 0);
			transform.Rotate(0, 0, -rotation);

			anim.SetFloat ("Speed", Mathf.Abs (translation));

			// shooting
			bool shoot = Input.GetButtonDown("Fire1");
			shoot |= Input.GetButtonDown("Fire2");
			// Careful: For Mac users, ctrl + arrow is a bad idea
			
			if (shoot)
			{
				WeaponScript weapon = GetComponent<WeaponScript>();
				if (weapon != null)
				{
					// false because the player is not an enemy
					weapon.Attack(false);
				}
			}
		}
		else if(hp <= 0){
			print ("you died!");
			if(gameObject.name == "Player"){
				print ("game over!");

				// display the game over screen
				gameObject.AddComponent<GameOverScript>();
			}
		}
	}
}

