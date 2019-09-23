using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public ZombieDispenser zombieDispenser;
	 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		HandleKeyboard();
	}

	public void HandleKeyboard (){

		if (Input.GetKeyDown(KeyCode.Alpha1)){
			startSlowGame();
		}else if (Input.GetKeyDown(KeyCode.Alpha2)){  
			startMediumGame();      
		}else if (Input.GetKeyDown(KeyCode.Alpha3)){  
			startFastGame();
		}
		return;
	}

	public void startSlowGame(){
		zombieDispenser.slowZombieSpeed = .8f;
		zombieDispenser.mediumZombieSpeed = 1.2f;
		zombieDispenser.fastZombieSpeed = 1.6f; 
		Application.LoadLevel(0);
		//zombieDispenser.initiateRandomZombieSpawnScenario();
	}

	public void startMediumGame(){
		zombieDispenser.slowZombieSpeed = 1.2f;
		zombieDispenser.mediumZombieSpeed = 1.6f;
		zombieDispenser.fastZombieSpeed = 2;
		Application.LoadLevel(0);
	}

	public void startFastGame(){
		zombieDispenser.slowZombieSpeed = 1.6f;
		zombieDispenser.mediumZombieSpeed = 2f;
		zombieDispenser.fastZombieSpeed = 2.6f;
		Application.LoadLevel(0);
	}

}
