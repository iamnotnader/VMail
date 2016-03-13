using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using EmailData = EmailUtility.EmailData;

// This class controls the main email scroller the user will see.
// TODO(nader): This class should generate its panels programmatically
// like MailPreviewCanvasesController.
public class MailCanvasesController : MonoBehaviour {
	// When our EmailList property is set, update the pages in the scroller
	// to actually contain the text.
	private IList<EmailData> emailList;
	public IList<EmailData> EmailList {
		get {
			return emailList;
		}
		set {
			emailList = value;					
			EmailUtility.clearAllText(transform);
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

	// Use this for initialization
	IEnumerator Start () {
		yield return null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
