using UnityEngine;
using System.Collections;

public class ProjectileDragging : MonoBehaviour {

	public float maxStretch = 3f;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;
	
	public SpringJoint2D spring;
	public Transform catapult;
	public Ray rayToMouse;
	public Ray leftCatapultToProjectile;
	public float maxStretchSqr;
	public float circleRadius;
	public bool clickedOn;
	public Vector2 prevVelocity;

	// Use this for initialization
	void Awake(){
		spring.GetComponent<SpringJoint2D>();
		catapult = spring.connectedBody.transform;
	}

	void Start () {
		LineRendererSetup();
		rayToMouse = new Ray(catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		CircleCollider2D circle = GetComponent<Collider2D>() as CircleCollider2D;
		circleRadius = circle.radius;
	}
	
	// Update is called once per frame
	void Update () {

		determineKeyHolding();

		if(clickedOn)
			determineMouseDragging();

		if(spring != null){
			if(!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude){
				Destroy(spring);
				GetComponent<Rigidbody2D>().velocity = prevVelocity;
			}

			if(!clickedOn)
				prevVelocity = GetComponent<Rigidbody2D>().velocity;

			LineRendererUpdate();

		}else{
			catapultLineFront.enabled = false;
			catapultLineBack.enabled = false;
		}
	}

	void LineRendererSetup(){
		catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition(0, catapultLineBack.transform.position);
		
		catapultLineFront.sortingLayerName = "Foreground";
		catapultLineBack.sortingLayerName = "Foreground";

		catapultLineFront.sortingOrder = 3;
		catapultLineBack.sortingOrder = 1;
	}

	void determineKeyHolding(){
		if(Input.GetKeyDown(KeyCode.A)){
			spring.enabled = false;
			clickedOn = true;
		}else if (Input.GetKeyUp(KeyCode.A)){
			spring.enabled = true;
			GetComponent<Rigidbody2D>().isKinematic = false;
			clickedOn = false;
		}
	}

	void onMouseDown(){
		spring.enabled = false;
		clickedOn = true;
	}

	void onMouseUp(){
		spring.enabled = true;
		GetComponent<Rigidbody2D>().isKinematic = false;
		clickedOn = false;
	}

	void determineMouseDragging(){
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

		if(catapultToMouse.sqrMagnitude > maxStretchSqr){
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}

		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineRendererUpdate(){
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;	
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude );
		catapultLineFront.SetPosition(1, holdPoint);
		catapultLineBack.SetPosition(1, holdPoint);
	}
}
