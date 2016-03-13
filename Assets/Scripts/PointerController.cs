using UnityEngine;
using System.Collections;

// The pointer that users use to interact with the email panels.
// TODO(nader): This class is really crappy. Couldn't get intersection
// to work so it basically does nothing. Also it's full of magic numbers.
// Should delete this if we want to take this project seriously.
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
