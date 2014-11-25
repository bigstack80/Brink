using UnityEngine;
using System.Collections;

//loads next level
public class FinishScript : MonoBehaviour {
	
	//player reference object
	private GameObject player;
	
	//draws GUI buttons
	void OnGUI()
	{
		const int buttonWidth = 120;
		const int buttonHeight = 60;
		
		if (
			GUI.Button(
			// Center in X, 1/3 of the height in Y
			new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(1 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			),
			"Next Level"
			)
			)
		{
			// loads this level as an int
			int thisLevel = Application.loadedLevel;
			//increments
			thisLevel++;
			//concatenate to create next levels name
			string nextLevel = "Stage"+thisLevel.ToString();
			//load next level by name
			Application.LoadLevel(nextLevel);
		}
		
		//or go back to main menu
		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
			new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			),
			"Back to menu"
			)
			)
		{
			// Reload the level
			Application.LoadLevel("Menu");
		}
	}
}
