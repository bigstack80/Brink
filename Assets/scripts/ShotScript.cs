using UnityEngine;

public class ShotScript : MonoBehaviour
{
	//damage done by this bullet
	public int damage = 1;
	
	//origin boolean
	public bool isEnemyShot = false;
	
	//loads player reference
	private GameObject player;
	
	//this is due to how the bullet object spawns, prevents bullet from
	//hitting the thing that fired it
	private int skipper = 1;
	
	//sets a 5 second timer on the bullet to prevent memory leaks
	void Start()
	{
		// 2 - Limited time to live to avoid any leak
		if(gameObject.name == "Bullet(Clone)" || gameObject.name == "Bullet"){
			Destroy(gameObject, 5); // 5sec
		}
	}
	
	//destroy as soon as it hits a wall
	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.name.Contains("wall")){
			if(gameObject.name.Contains("Bullet")){
				Destroy(gameObject);
			}
		}
	}
	
	//handling for when it hits anything else
	void OnTriggerExit2D(Collider2D coll)
	{
		//increment skipper, first "hit" is on the thing that fired it
		skipper++;
		if (skipper>1){
			//load the player object
			player  = GameObject.Find("Player");
			//extra security to make sure nothing gets shot through a wall
			if (coll.gameObject.name.Contains("wall") || coll.gameObject.name.Contains ("Wall")){
				if (gameObject.name.Contains("Bullet")) {
					Destroy (gameObject);
				}
				//player hit by bullet
			}else if(coll.gameObject.name.Contains("Player")){
				//make sure you cant suicide
				if(isEnemyShot == true){
					player.GetComponent<PlayerScript>().Hit(damage);
				}
				//enemy hit by bullet
			}else if(coll.gameObject.name.Contains("Enemy")){
				//make sure only player bullets count
				if(isEnemyShot == false){
					player.GetComponent<PlayerScript>().Score();
					coll.gameObject.GetComponent<EnemyScript>().Hit(damage);
				}
			}
		}
		if(coll.gameObject.name.Contains("Player") || coll.gameObject.name.Contains("Enemy")){
			if(gameObject.name.Contains("Bullet")){
				Destroy(gameObject);
			}
		}
	}
}


