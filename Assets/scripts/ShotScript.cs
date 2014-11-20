using UnityEngine;

/// <summary>
/// Projectile behavior
/// </summary>
public class ShotScript : MonoBehaviour
{
	// 1 - Designer variables
	
	/// <summary>
	/// Damage inflicted
	/// </summary>
	public int damage = 1;
	
	/// <summary>
	/// Projectile damage player or enemies?
	/// </summary>
	public bool isEnemyShot = false;

	//workaround to make bullets not go through walls
	private int skipper = 0;

	void Start()
	{
		// 2 - Limited time to live to avoid any leak
		if(gameObject.name == "Bullet(Clone)" || gameObject.name == "Bullet"){
			Destroy(gameObject, 5); // 5sec
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		skipper++;
		//I have no idea why it doesn't work when I just check if I hit enemy or player
		//rather than fiddling with the skipper variable
		//but now it does what i want it to do
		if( coll.gameObject.name == "Enemy" || coll.gameObject.name == "Player"){
			skipper--;
		}
		if (skipper > 2) { 
			print ("hit wall");
			if (gameObject.name == "Bullet(Clone)" || gameObject.name == "Bullet") {
				Destroy (gameObject);
			}
		}
	}
}


