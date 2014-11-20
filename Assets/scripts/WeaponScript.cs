using UnityEngine;

/// <summary>
/// Launch projectile
/// </summary>
public class WeaponScript : MonoBehaviour
{
	//--------------------------------
	// 1 - Designer variables
	//--------------------------------
	//projectile prefabe - maybe?
	public Transform prefab;
	
	//cooldown in seconds - lower = higher fire rate
	public float shootingRate = 0.25f;
	
	//--------------------------------
	// 2 - Cooldown
	//--------------------------------
	private float shootCooldown;
	
	void Start()
	{
		shootCooldown = 0f;
	}
	
	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
	}
	
	//--------------------------------
	// 3 - Shooting from another script
	//--------------------------------
	
	/// <summary>
	/// Create a new projectile if possible
	/// </summary>
	public void Attack(bool isEnemy)
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;
			// Create a new shot
			//y = sin
			//x = cos
			//x y for 0-90 and 180-270
			//y x for 90-180 and 270-360
			float a = 0.2f;
			float b = 0.15f;

			float angle = transform.eulerAngles.z;

			a = (Mathf.Cos(((angle/180)*Mathf.PI)))*a - (Mathf.Sin (((angle/180)*Mathf.PI)))*b;
			b = (Mathf.Sin(((angle/180)*Mathf.PI)))*a + (Mathf.Cos (((angle/180)*Mathf.PI)))*b;
			print (a);
			print (b);
			a = b = 0;

			Vector3 offset = new Vector3(transform.position.x+a, transform.position.y+b, transform.position.z);

			GameObject shotTransform = GameObject.Instantiate(Resources.Load ("Bullet"), offset, transform.rotation) as GameObject;

			// The is enemy property
			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
			if (shot != null)
			{
				shot.isEnemyShot = isEnemy;
			}

			if ( isEnemy == false ) {
				SoundEffectsHelper.Instance.MakePlayerShotSound();
			}
			
			// Make the weapon shoot always forwards
			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
			if (move != null)
			{
				move.direction = this.transform.up; // Up sends it in the direction the sprite is facing
			}
		}
	}
	
	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}
	
}


