using UnityEngine;
using System.Collections;

public class PointerRodController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] otherObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject obj in otherObjects) {
			Renderer otherRenderer = obj.GetComponent<Renderer> ();
			if (otherRenderer == null || obj.CompareTag("PointerSphere") || obj.CompareTag("test")) {
				continue;
			}
			if (obj.GetComponent<Renderer> ().bounds.Intersects (
					transform.gameObject.GetComponent<Renderer>().bounds)) {
				Debug.Log ("INTERSECTION! " + obj.tag);
			}
		}
	}
}
