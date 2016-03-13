using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EmailData = EmailUtility.EmailData;

public class MailPreviewCanvasesController : MonoBehaviour {
	public Transform singleCanvasPrefab;
	public int numPanels = 16;
	private IList<Transform> listOfCanvases = new List<Transform>();

	private IList<EmailData> emailList;
	public IList<EmailData> EmailList {
		get {
			return emailList;
		}
		set {
			Debug.Log ("Filling canvases.");
			emailList = value;					
			EmailUtility.clearAllText (transform);
			fillCanvasesWithEmail (emailList);
		}
	}

	private void fillCanvasesWithEmail(IList<EmailData> emailList) {
		// We fill up as many canvases as we can with emails. If we have K canvases
		// and L emails, we want to populat min(K, L).
		int maxIndex = Mathf.Min (emailList.Count, transform.childCount);

		// For each email, populate a single child canvas.
		for (int ii = 0; ii < maxIndex; ii++) {
			EmailData emailData = emailList [ii];
			Transform childCanvasTransform = transform.GetChild (ii);
			EmailUtility.setTextRecursively (
				childCanvasTransform.gameObject, emailData.subject, emailData.body);
		}
	}

	// Use this for initialization
	void Start () {
		float radiansPerTurn = Mathf.PI * 2 / numPanels;
		float radius = Mathf.Abs(singleCanvasPrefab.localPosition.z);

		// There is one canvas that we actually create in the scene non-programmatically.
		// We then use this canvas to create all the others.
		listOfCanvases.Add (singleCanvasPrefab);
		for (int ii = 1; ii < numPanels; ii++) {
			float currentAngleRads = radiansPerTurn * ii - Mathf.PI / 2;
			Transform currentCanvas = Instantiate (singleCanvasPrefab);
			currentCanvas.SetParent (transform);
			currentCanvas.localPosition =
				new Vector3 (
					0,
					Mathf.Cos (currentAngleRads) * radius,
					Mathf.Sin (currentAngleRads) * radius);
			currentCanvas.localEulerAngles =
				new Vector3 (Mathf.Rad2Deg * radiansPerTurn * ii, 0, 0);
			listOfCanvases.Add (currentCanvas);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
