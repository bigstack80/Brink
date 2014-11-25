using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
	//variable to load in player location later
	private Transform player;
	
	//Health int- related to game level
	public int hp = 2;
	
	//Speed - set later
	public float speed;
	
	//used for drawing the weapon
	public bool flip = false;
	
	//used for walking animation
	Animator anim;
	
	//on startup, loads animator
	void Start(){
		anim = GetComponent<Animator> ();
	}
	
	//while awake
	void Awake()
	{
		// Retrieve the player object
		player = GameObject.FindGameObjectWithTag("Player").transform;
		hp*=Application.loadedLevel;
		
	}
	
	//function to take damage
	public void Hit(int power)
	{
		hp -= power;
	}
	
	//on every game update
	void Update()
	{
		//if alive
		if (hp > 0) {
			//draw line to player to get range
			Vector3 line = transform.position - player.position;
			double range = Math.Sqrt (Math.Pow ((double)line.x, 2) + Math.Pow ((double)line.y, 2));
			//range based behavior
			if (range < 5.75) {
				transform.LookAt (player.position);
				//fixes issue with enemy sprite orientation
				transform.Rotate (0, 90, 90);
				//sets normal speed
				speed = 0.02F;
				//if under certain range, stop chasing
				if (range <= 2.75) {
					speed = 0.0F;
					//if too close, back up
				} else if (range <= 1) {
					speed = -0.02F;
				}
				//controls walk animation
				anim.SetFloat ("Speed", Mathf.Abs (speed));
				//moves the enemy
				transform.Translate (0, speed, 0);
			}
			//if within shooting range
			if (range <= 4) {
				//loads weaponscript and tries to fire
				WeaponScript[] weapon = GetComponentsInChildren<WeaponScript> ();
				foreach (WeaponScript w in weapon) {
					if (w != null) {
						// true because this is the enemy
						if(transform.rotation.eulerAngles.y == 180)
						{
							flip=true;
						}
						w.Attack (true,flip);
					}
				}
			}
		}
		//if dead
		else if(hp <= 0)
		{
			print ("enemy dead!");
			if(gameObject.name == "Enemy"){
				Destroy(gameObject);
			}
		}
	}
}