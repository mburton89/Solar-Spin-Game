using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieDispenser : MonoBehaviour {

	public virtual Vector2 direction { get; protected set; }
	//public Vector3 zombieSpawnLocation;
	public float walkingSpeed;
	public ZombieAI zombie;
	public Vector3 screenDimensions;
	private static System.Random random = new System.Random();
	public float slowZombieSpeed;// = 1.2f;
	public float mediumZombieSpeed;// = 1.6f;
	public float fastZombieSpeed;// = 2;
	public int activeZombies;
	public int numberOfSpawnGroups; 
	//private DateTime _started;
	//public TimeSpan RunningTime{get{return DateTime.UtcNow - _started;}}
	 
	void Start () {
		screenDimensions = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, (Screen.height), 1));
		//initiateRandomZombieSpawnScenario(); 
	}

	void Update () {
		HandleKeyboard();
		HandleUserTouches();  
		//printRandomNumber();
	}

	private void HandleKeyboard(){
		if (Input.GetKeyDown(KeyCode.Z)){
			initiateRandomZombieSpawnScenario(); 
		}
	}

	public void HandleUserTouches(){
			for (int i = 0; i < Input.touchCount; i++){
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began && touch.tapCount == 1){
					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);  
					if(touchPosition.y > 0){
						activeZombies = 0; 
						initiateRandomZombieSpawnScenario();   
					}
				}
			}

	}
//
//	public void spawnZombie(){
//		Vector3 zombieSpawnLocation = new Vector3(transform.position.x + direction.x, transform.position.y, transform.position.z); 
//		var liveZombie = (ZombieAI)Instantiate (zombie, zombieSpawnLocation, transform.rotation); 
//		liveZombie.Initialize (gameObject,
//		                     direction,
//		                     new Vector2(0.3f, 0));   
//	}


//	public void spawnZombieEveryFewSeconds(){
//		StartCoroutine(FlashEmojiCo()); 
//	}
//	
//	private IEnumerator FlashEmojiCo(){   
//	
//		yield return new WaitForSeconds (1);  
//		spawnZombie();
//	}

	public void initiateRandomZombieSpawnScenario(){
		float scenarioNumber = random.Next (1, 8);
		float yPosistion = random.Next (1, 5);
		float yPosistion2 = random.Next (1, 5);
		if(yPosistion2 == yPosistion){
			yPosistion2 = yPosistion - 1;
		}
		 
		if(scenarioNumber == 1){
			spawnOneZombieFromLeft(fastZombieSpeed, -screenDimensions.x, yPosistion);
		}else if(scenarioNumber == 2){ 
			spawnOneZombieFromRight(fastZombieSpeed, screenDimensions.x, yPosistion);
		}else if(scenarioNumber == 3){
			spawnOneZombieFromLeft(mediumZombieSpeed, -screenDimensions.x,  yPosistion);
			spawnOneZombieFromRight(slowZombieSpeed, screenDimensions.x,  yPosistion2);
		}else if(scenarioNumber == 4){
			spawnOneZombieFromLeft(slowZombieSpeed,-screenDimensions.x, yPosistion);
			spawnOneZombieFromRight(mediumZombieSpeed, screenDimensions.x, yPosistion2);
		}else if(scenarioNumber == 5){
			spawnOneZombieFromLeft(mediumZombieSpeed,-screenDimensions.x, yPosistion);
			spawnOneZombieFromLeft(mediumZombieSpeed,-screenDimensions.x - .35f, yPosistion - 1);
		}else if(scenarioNumber == 6){
			spawnOneZombieFromRight(mediumZombieSpeed,screenDimensions.x, yPosistion);
			spawnOneZombieFromRight(mediumZombieSpeed,screenDimensions.x + .35f, yPosistion - 1);
		}else if(scenarioNumber == 7){
			spawnOneZombieFromLeft(mediumZombieSpeed, -screenDimensions.x,  1);
			spawnOneZombieFromRight(mediumZombieSpeed, screenDimensions.x,  2);
			spawnOneZombieFromLeft(mediumZombieSpeed, -screenDimensions.x,  3); 
			spawnOneZombieFromRight(mediumZombieSpeed, screenDimensions.x,  4);
		}
		GameManager.Instance.intialBallHasBeenDestroyed = false;
		numberOfSpawnGroups ++;
	}

	public void spawnOneZombieFromLeft(float speed, float xPosition, float yPosition){
		//float scenarioNumber = random.Next (1, 5);
		Vector3 zombieSpawnLocation = new Vector3(xPosition, yPosition, transform.position.z); 
		var liveZombie = (ZombieAI)Instantiate (zombie, zombieSpawnLocation, transform.rotation); 
		liveZombie.Initialize (gameObject,
		                       direction,
		                       new Vector2(0.3f, 0),
		                       speed);  
		activeZombies ++;
	}

	public void spawnOneZombieFromRight(float speed, float xPosition, float yPosition){
		//float scenarioNumber = random.Next (1, 5);
		Vector3 zombieSpawnLocation = new Vector3(xPosition, yPosition, transform.position.z); 
		var liveZombie = (ZombieAI)Instantiate (zombie, zombieSpawnLocation, transform.rotation); 
		liveZombie.InitializeFromRight (gameObject,
		                       direction,
		                       new Vector2(0.3f, 0),  
		                       -speed);  

		activeZombies ++;
	}

	public void spawnTwoZombiesFastLeftSlowRight(){
   
	}

	public void printRandomNumber(){
		float scenarioNumber = random.Next (1, 5);
		Debug.Log(screenDimensions.x);  
	}
}
