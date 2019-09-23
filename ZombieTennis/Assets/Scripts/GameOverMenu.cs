using UnityEngine;
using System.Collections;

public class GameOverMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		HandleKeyboard();
	}
	
	public void HandleKeyboard (){
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			restartGame();
		}
	}
	
	public void restartGame(){
//		zombieDispenser.slowZombieSpeed = .8f;
//		zombieDispenser.mediumZombieSpeed = 1.2f;
//		zombieDispenser.fastZombieSpeed = 1.6f; 
		Application.LoadLevel(0);
		//zombieDispenser.initiateRandomZombieSpawnScenario();
	}
}
