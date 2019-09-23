using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {

	public SlingshotProjectile projectile;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialize(GameObject owner, Vector2 direction, Vector2 velocity){
		
		direction = new Vector2(0, 1);
		OnInitialized();
	}

	protected virtual void OnInitialized(){
		return; 
	}
}
