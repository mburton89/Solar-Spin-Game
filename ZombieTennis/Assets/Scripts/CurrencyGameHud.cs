using UnityEngine;

public class CurrencyGameHud : MonoBehaviour{
	
	public GUISkin Skin;
	
	public void Start(){

	}
	
	public void OnGUI(){
		//GUI.skin = Skin;
		
		GUILayout.BeginArea(new Rect(0 ,0, Screen.width, Screen.height)); //also added padding for GameSkin on Inspector
		{
			GUILayout.BeginVertical(Skin.GetStyle("EnemyKillText3")); 
			{
				if(Application.loadedLevel == 0){
					GUILayout.Label(string.Format("{0}", GameManager.Instance.zombieCash), Skin.GetStyle("EnemyKillText3")); 
					GUILayout.Label(string.Format("{0}", GameManager.Instance.multiplierString), Skin.GetStyle("EnemyKillText3")); 
				}
				
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndArea();
	}
}