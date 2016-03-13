using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EmailData = EmailUtility.EmailData;
using Constants = VMailUtilities.Constants;

// Note: This needs to run AFTER the Start() methods on its member controllers
// have been called. We ensure this is the case by setting the script execution
// order in the UI.
public class EmailController : MonoBehaviour {
	public MailCanvasesController mailCanvasesController;
	public MailPreviewCanvasesController mailPreviewController;
	public float canvasRotationSpeed = 10;
	private float currentPreviewAngle;
	private float previewOffsetAngle;
	// Our Start() can take a while because it executes a coroutine, so
	// make sure we don't Update() until it completes.
	private bool startComplete = false;

	// Use this for initialization
	IEnumerator Start () {
		// Use our Gmail access token to get the list of emails.
		IList<EmailData> emailList = new List<EmailData> ();
		yield return StartCoroutine(
			EmailUtility.getListOfEmails (Constants.GMAIL_ACCESS_TOKEN, emailList));
		mailCanvasesController.EmailList = emailList;
		mailPreviewController.EmailList = emailList;

		currentPreviewAngle = mailPreviewController.transform.localEulerAngles.x;
		previewOffsetAngle = currentPreviewAngle;
		startComplete = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!startComplete) {
			return;
		}
		float amountScrolled = Input.GetAxis ("Mouse ScrollWheel");
		currentPreviewAngle -= amountScrolled * canvasRotationSpeed;
		mailPreviewController.transform.localEulerAngles =  new Vector3 (currentPreviewAngle, 0, 0);
		mailCanvasesController.transform.localEulerAngles = new Vector3 (0, currentPreviewAngle - previewOffsetAngle, 0);
	}
}