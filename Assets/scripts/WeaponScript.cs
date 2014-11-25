using UnityEngine;

public class WeaponScript : MonoBehaviour
{
	//projectile prefab
	public Transform prefab;
	
	//cooldown in seconds - lower = higher fire rate
	public float shootingRate = 0.25f;
	
	//cooldown variable, keeps track of last time a shot was fired
	private float shootCooldown;
	
	//startup, initialize cooldown
	void Start()
	{
		shootCooldown = 0f;
	}
	
	//every game update
	void Update()
	{
		if (shootCooldown > 0)
		{
			//decrease cooldown by time since last frame
			shootCooldown -= Time.deltaTime;
		}
		//if player has a bunch of upgrades, prevent negative cooldown time
		//it might break the game
		if(shootingRate < 0){shootingRate = 0;}
	}
	
	//shooting from other scripts
	public void Attack(bool isEnemy, bool flip)
	{
		if (CanAttack)
		{
			//if the player fired this, reduce ammo by one
			if(isEnemy == false){
				PlayerPrefs.SetInt ("ammo", (PlayerPrefs.GetInt ("ammo")-1));
			}
			shootCooldown = shootingRate;
			float a = 0.0f;
			float b = 0.2f;
			Vector3 offset;
			
			float angle = transform.eulerAngles.z;
			
			float w = (Mathf.Sin(((angle/180)*Mathf.PI)))*a;
			float x = (Mathf.Cos(((angle/180)*Mathf.PI)))*b;
			float y = (Mathf.Cos(((angle/180)*Mathf.PI)))*a;
			float z = (Mathf.Sin(((angle/180)*Mathf.PI)))*b;
			b = w + x;
			a = y + z;
			
			if(flip == false){
				offset = new Vector3(transform.position.x-a, transform.position.y+b, transform.position.z);
			}
			else{
				offset = new Vector3(transform.position.x+a, transform.position.y+b, transform.position.z);
			}
			
			//instantiate the new bullet object
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
			
			// Make the weapon shoot always forwards relative to the shooter
			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
			if (move != null)
			{
				move.direction = this.transform.up; // Up sends it in the direction the sprite is facing
			}
		}
	}
	
	//canattack if no cooldown and has at least one bullet
	public bool CanAttack
	{
		get
		{
			return ((shootCooldown <= 0f) && (PlayerPrefs.GetInt ("ammo")>0));
		}
	}
	
}


