using UnityEngine;
using System.Collections;

public class Speed : MonoBehaviour {

	public float speed = 0;
	Vector3 lastPosition = Vector3.zero;

	void FixedUpdate(){
		speed = (transform.position - lastPosition).magnitude;
		lastPosition = transform.position;
	}
}
