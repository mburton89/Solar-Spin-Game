using System;
using UnityEngine;

public class AlwaysActive : MonoBehaviour{
	
	public void Start(){
		DontDestroyOnLoad(transform.gameObject);   
		
		//iOS
		//Application.targetFrameRate = 60;  
	}
}
