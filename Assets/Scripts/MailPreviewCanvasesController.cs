using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EmailData = EmailUtility.EmailData;

// This class controls the preview scroller the user sees.
public class MailPreviewCanvasesController : MonoBehaviour {
	// We programmatically create all of the preview panels from a single
	// prefab. The radius of the panels is taken from this prefab.
	public Transform singleCanvasPrefab;
	// The number of panels we'll generate.
	public int numPanels = 8;
	private IList<Transform> listOfCanvases = new List<Transform>();

	// When the EmailList property is set, fill all the preview panels
	// with emails.
	private IList<EmailData> emailList;
	public IList<EmailData> EmailList {
		get {
			return emailList;
		}
		set {
			emailList = value;					
			EmailUtility.clearAllText (transform);
			fillCanvasesWithEmail (emailList);
		}
	}

	// Loop through all of this object's children and fill them up with emails.
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

	// Programmatically generate all of the preview panels.
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
