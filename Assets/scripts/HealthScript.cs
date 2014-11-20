using System.Collections;
using UnityEngine;
using System;
/// <summary>
/// Handle hitpoints and damages
/// </summary>
public class HealthScript : MonoBehaviour
{
	/// <summary>
	/// Total hitpoints
	/// </summary>
	public int hp = 10;
	
	/// <summary>
	/// Enemy or player?
	/// </summary>
	public bool isEnemy = true;

	void Start(){
		print ("Health script started");

	}

	void OnTriggerCollision2D(Collider2D otherCollider)
	{
		print ("hit");
		// Is this a shot?
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			// Avoid friendly fire
			if (shot.isEnemyShot != isEnemy)
			{
				Damage(shot.damage);
				
				// Destroy the shot
				Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
			}
		}
	}
	/// <summary>
	/// Inflicts damage and check if the object should be destroyed
	/// </summary>
	/// <param name="damageCount"></param>
	public void Damage(int damageCount)
	{
		hp -= damageCount;
		Debug.Log ("damage");
		if (hp <= 0)
		{
			// Dead!
			
			if(isEnemy){
				Destroy(GameObject.FindGameObjectWithTag("Enemy"));
				Debug.Log ("Enemy killed");
			}
		}
	}
}

