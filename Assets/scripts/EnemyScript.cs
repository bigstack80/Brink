using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Enemy generic behavior
/// </summary>
public class EnemyScript : MonoBehaviour
{
	//know where the player is
	private Transform player;

	//health
	public int hp = 2;
	public float speed;

	Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();
		}
	void Awake()
	{
		// Retrieve the player object
		player = GameObject.FindGameObjectWithTag("Player").transform;

	}
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			// Avoid friendly fire
			//true for enemy, false for player
			if (shot.isEnemyShot != true)
			{	
				print ("hit enemy with player bullet");
				hp--;
				// Destroy the shot
				Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
			}
		}
	}
	void Update()
	{
		if (hp > 0) {
			Vector3 line = transform.position - player.position;
			double range = Math.Sqrt (Math.Pow ((double)line.x, 2) + Math.Pow ((double)line.y, 2));
			if (range < 5.75) {
				transform.LookAt (player.position);
				transform.Rotate (0, 90, 90);
				speed = 0.02F;
				if (range <= 2.75) {
					speed = 0.0F;
				} else if (range <= 1) {
					speed = -0.02F;
				}
				anim.SetFloat ("Speed", Mathf.Abs (speed));
				transform.Translate (0, speed, 0);
			}
			if (range <= 4) {

				WeaponScript[] weapon = GetComponentsInChildren<WeaponScript> ();
				foreach (WeaponScript w in weapon) {
					if (w != null) {
						// true because this is the enemy
						w.Attack (true);
					}
				}
			}

		}
		else if(hp <= 0)
		{
			print ("enemy dead!");
			if(gameObject.name == "Enemy"){
				Destroy(gameObject);
			}
		}

	}
	void FixedUpdate(){
		//anim.SetFloat ("Speed", 1.0f);
		}
}