using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EmailData = EmailUtility.EmailData;
using Constants = VMailUtilities.Constants;

// Note: This needs to run AFTER the Start() methods on its member controllers
// have been called. We ensure this is the case by setting the script execution
// order in the UI.
public class EmailController : MonoBehaviour {
	// This class updates the main scroller, the preview scroller, and the
	// camera based on user input and on emails received.
	public MailCanvasesController mailCanvasesController;
	public MailPreviewCanvasesController mailPreviewController;
	public MainCameraController mainCamera;

	// We use these variables when a user wants to zoom into a particular email.
	private Transform oldMailCanvasParentTransform;
	private Vector3 oldMailCanvasPosition;
	private Vector3 oldMailCanvasAngles;
	private bool isZoomedView = false;
	private float degreesPerPanel;

	// We use these variables when the user is rotating around the scene.
	public float turnSpeed = 4.0f;
	private Vector3 mouseOrigin;
	private bool isRotating;

	// This variable controls the angle of rotation of both the preview
	// scroller and the main scroller.
	private float currentPreviewAngle;

	// Controls how fast we scroll through the preview scroller and the main
	// scroller.
	public float canvasRotationSpeed = 10;

	// This is an offset we use to sync up the preview scroller with the main
	// scroller. Basically, we start the preview scroller slightly off.
	private float previewOffsetAngle;

	// Our Start() can take a while because it makes an API call to Gmail in a
	// coroutine, so make sure we don't Update() until it completes.
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

		degreesPerPanel = 360f / mailPreviewController.numPanels;
		startComplete = true;
	}

	private void updateOnScroll() {
		float amountScrolled = Input.GetAxis ("Mouse ScrollWheel");
		currentPreviewAngle -= amountScrolled * canvasRotationSpeed;
		mailPreviewController.transform.localEulerAngles = new Vector3 (
			currentPreviewAngle - mainCamera.transform.eulerAngles.y, 0, 0);
		if (isZoomedView) {
			mailCanvasesController.transform.localEulerAngles = new Vector3 (
				0, currentPreviewAngle - previewOffsetAngle - mainCamera.transform.eulerAngles.y, 0);
		} else {
			mailCanvasesController.transform.localEulerAngles = new Vector3 (
				0, currentPreviewAngle - previewOffsetAngle, 0);
		}
	}

	private IEnumerator animateZoomIn() {
		for (int ii = 0; ii <= 1500; ii += 100) {
			mailCanvasesController.transform.localPosition = new Vector3 (0, 0, -1 * ii);
			yield return new WaitForSeconds (.001f);
		}
		yield return null;
	}

	private void updateOnLeftClick() {
		// Get the left mouse button
		if (!Input.GetMouseButtonDown (0)) {
			return;
		}
		Debug.Log ("Click detected");
		// If we're already zoomed in, zoom out.
		if (isZoomedView) {
			mailCanvasesController.transform.parent = oldMailCanvasParentTransform;
			mailCanvasesController.transform.position = oldMailCanvasPosition;
			// Note we leave the y as it was so the user is still looking at the same email.
			mailCanvasesController.transform.eulerAngles = new Vector3(
				oldMailCanvasAngles.x,currentPreviewAngle - previewOffsetAngle, oldMailCanvasAngles.z
			);
			isZoomedView = false;
			return;
		}

		// First we snap the camera angle and the currentPreviewAngle to a multiple of
		// degreesPerPanel. This makes the email panel we're on snap nicely to the camera.
		currentPreviewAngle = Mathf.Round (
			(currentPreviewAngle - previewOffsetAngle) / degreesPerPanel) *
			degreesPerPanel + previewOffsetAngle;
		mailCanvasesController.transform.localEulerAngles = new Vector3 (
			0, currentPreviewAngle - previewOffsetAngle, 0);
		mainCamera.transform.eulerAngles = new Vector3 (
			mainCamera.transform.eulerAngles.x,
			Mathf.Round(mainCamera.transform.eulerAngles.y / degreesPerPanel) * degreesPerPanel,
			mainCamera.transform.eulerAngles.z);

		// If we're not zoomed in, grab the panel in front of us, center it,
		// and put it up close to the camera.
		//
		// We do this by nesting the mailCanvasesController underneath the camera's
		// transform and setting new values for its relative position.
		oldMailCanvasParentTransform = mailCanvasesController.transform.parent;
		oldMailCanvasPosition = mailCanvasesController.transform.position;
		oldMailCanvasAngles = mailCanvasesController.transform.eulerAngles;

		mailCanvasesController.transform.parent = mainCamera.transform;
		mailCanvasesController.transform.localEulerAngles = new Vector3 (
			0, oldMailCanvasAngles.y - mainCamera.transform.eulerAngles.y, 0);
		StartCoroutine (animateZoomIn());

		// Now we're zoomed in so set the variable.
		isZoomedView = true;
	}

	private void updateOnRightClick() {
		// Get the right mouse button
		if(Input.GetMouseButtonDown(1))
		{
			// Get mouse origin
			mouseOrigin = Input.mousePosition;
			isRotating = true;
		}

		// Disable movements on button release
		if (!Input.GetMouseButton(1)) {
			isRotating=false;
		}

		// Rotate camera along X and Y axis
		if (isRotating)
		{
        	Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
			mainCamera.transform.RotateAround(
				mainCamera.transform.position, mainCamera.transform.right, -pos.y * turnSpeed);
			mainCamera.transform.RotateAround(
				mainCamera.transform.position, Vector3.up, pos.x * turnSpeed);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!startComplete) {
			return;
		}
		updateOnScroll ();
		updateOnLeftClick ();
		updateOnRightClick ();
	}
}