using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {

	Vector3 positionActual;
	float paso = .1f;
	Rigidbody rb;
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		positionActual = this.transform.position;
		if (Input.GetKey(KeyCode.D)) {
			this.transform.position = new Vector3 (positionActual.x+paso, positionActual.y, positionActual.z);
			rb.MovePosition(this.transform.position);
		}
		if (Input.GetKey(KeyCode.A)) {
			this.transform.position = new Vector3 (positionActual.x-paso, positionActual.y, positionActual.z);
			rb.MovePosition(this.transform.position);
		}
		if (Input.GetKey(KeyCode.W)) {
			this.transform.position = new Vector3 (positionActual.x, positionActual.y, positionActual.z+paso);
			rb.MovePosition(this.transform.position);
		}
		if (Input.GetKey(KeyCode.S)) {
			this.transform.position = new Vector3 (positionActual.x, positionActual.y, positionActual.z-paso);
			rb.MovePosition(this.transform.position);
		}
	}
}
