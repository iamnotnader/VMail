using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using EmailData = EmailUtility.EmailData;

public class MailCanvasesController : MonoBehaviour {
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
