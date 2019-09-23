using UnityEngine;

public class GameHud : MonoBehaviour{
	
	public GUISkin Skin;
	//public GUISkin SkinB;
	//public Ball ball;

	public void Start(){
		//ball = FindObjectOfType<Ball>();
	}
	
	public void OnGUI(){
		//GUI.skin = Skin;
		
		GUILayout.BeginArea(new Rect(0 ,0, Screen.width, Screen.height)); //also added padding for GameSkin on Inspector
		{
			GUILayout.BeginVertical(Skin.GetStyle("EnemyKillText2")); 
			{
				if(Application.loadedLevel == 0){
					GUILayout.Label(string.Format("{0}", GameManager.Instance.Points), Skin.GetStyle("EnemyKillText2")); 
					GUILayout.Label(string.Format("{0}", PlayerPrefs.GetInt("HighScore")), Skin.GetStyle("EnemyKillText2")); 
				}

			}
			GUILayout.EndVertical();
 
//			GUILayout.BeginVertical(SkinB.GetStyle("EnemyKillText3")); 
//			{
//				if(Application.loadedLevel == 0){
//					GUILayout.Label(string.Format("{0}", GameManager.Instance.Points), SkinB.GetStyle("EnemyKillText2")); 
//					GUILayout.Label(string.Format("{0}", PlayerPrefs.GetInt("HighScore")), SkinB.GetStyle("EnemyKillText2")); 
//				}
//				
//			}
//			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
}