using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GUISkin Skin;
	private static GameManager _instance;
	public static GameManager Instance{get{return _instance ?? (_instance = new GameManager());}}
	public FlashText flashText;
	public TimeSpan RunningTime{get{return DateTime.UtcNow - started;}}
	public DateTime started;
	public bool intialBallHasBeenDestroyed;
	public bool hasHitZombie;
	
	public int Points {get; private set;}
	public int spawnGroupsHitInARow; //{get; private set;}
	public float multiplier = 1f;
	public string multiplierString = "1.00";
	public float zombieCash;
	public int zombiesKilledOnServe;

	private GameManager(){
		//contructor to instantiate GameManager Singleton
	}
	
	void Start () {
		//flashText = FindObjectOfType<FlashText> ();
		started = DateTime.UtcNow;
		//multiplier = 1; 
	}
	
	void Update () {
		//Debug.Log ("multiplier " + multiplier);
	}
	
	public void ResetPointsToZero(){
		spawnGroupsHitInARow = 0;
		Points = 0;
	} 
	  
	public void ResetPoints(int points){ 
		Points = points;
	}
	
	public void AddPoints(int pointsToAdd){
		Points += pointsToAdd;
	}

	public void AddZombieCash(float zombieCashToAdd){
		zombieCash += zombieCashToAdd;
	}

	public void AddToMultiplier(float multiplierToAdd){
		multiplier += multiplierToAdd;
		multiplierString = multiplier.ToString();
	}

	public void ResetMultiplierToOne(){
		multiplier = 1;
		multiplierString = "1.00";
		Debug.Log(multiplierString + "multiplierString"); 
	}

	public void ResetZSGHIARToZero(){
		Points = 0;
	} 
	
	public void ResetZSGHIAR(int points){ 
		Points = points;
	}
	
	public void AddZSGHIAR(int pointsToAdd){
		Points += pointsToAdd;
	}

//	public void OnGUI(){
//		GUI.skin = Skin;
//		
//		GUILayout.BeginArea(new Rect(0 ,0, Screen.width, Screen.height)); //also added padding for GameSkin on Inspector
//		{
//			GUILayout.BeginVertical(Skin.GetStyle("EnemyKillText3")); 
//			{
//				if(Application.loadedLevel == 0){
//					GUILayout.Label(string.Format("{0}", GameManager.Instance.spawnGroupsHitInARow), Skin.GetStyle("EnemyKillText3")); 
//				}
//				//		UNCOMMENT FOR TIME GUI 
//				//				var time = LevelManager.Instance.RunningTime;
//				//				GUILayout.Label(string.Format(
//				//					"{0:00}:{1:00} with {2} bonus", 
//				//					time.Minutes + (time.Hours * 60), 
//				//					time.Seconds,
//				//				    LevelManager.Instance.CurrentTimeBonus), Skin.GetStyle("TimeText"));
//			}
//			GUILayout.EndVertical();
//		}
//		GUILayout.EndArea();
//	}

	public void displayGameOverScreen(){

	}

	public void displayMultiKillGraphic(){
		flashText = FindObjectOfType<FlashText> ();
		if(zombiesKilledOnServe == 2){
			flashText.flashDoubleKill();
			AddPoints(200);
			//Debug.Log("DOUBLE KILL");
		}else if(zombiesKilledOnServe == 3){
			flashText.flashTripleKill();
			AddPoints(300);
			//Debug.Log("TRIPLE KILL");
		}else if(zombiesKilledOnServe == 4){
			flashText.flashQuadroupleKill();
			AddPoints(400);
			//Debug.Log("QUADROUPLE KILL");
		}
	}
}