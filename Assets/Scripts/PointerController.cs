using UnityEngine;
using System.Collections;

public class PointerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePosition = Input.mousePosition;
		transform.localEulerAngles = new Vector3 (
			120 - mousePosition.y / Screen.height * 80, 0, 60 - mousePosition.x / Screen.width * 120);
	}
}
