using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using Constants = VMailUtilities.Constants;

public static class EmailUtility {
	// A simple class that contains the data we'll use to populate a single
	// email.
	public class EmailData {
		public string subject;
		public string body;
		public EmailData(string subject, string body) {
			this.subject = subject;
			this.body = body;
		}
	}

	// Clears the email messages in the transform passed in.
	public static void clearAllText(Transform inputTransform) {
		foreach (Transform tt in inputTransform) {
			EmailUtility.setTextRecursively (tt.gameObject, "", "");
		}
	}

	// Searches for GameObjects in the hierarchy that should contain subject
	// or body text and sets them accordingly.
	public static void setTextRecursively(GameObject rootNode, string subjectText, string bodyText) {
		if (rootNode.CompareTag (Constants.SUBJECT_TAG_NAME)) {
			rootNode.GetComponent<Text>().text = subjectText;
		}
		if (rootNode.CompareTag (Constants.BODY_TAG_NAME)) {
			rootNode.GetComponent<Text>().text = bodyText;
		}
		foreach (Transform tt in rootNode.transform) {
			setTextRecursively(tt.gameObject, subjectText, bodyText);
		}
	}

	// Queries the Gmail API for a list of recent emails and retains the
	// relevant data.
	//
	// TODO(nader): This should be paginated or something to avoid
	// hammering the Gmail API all at once in Start().
	public static IEnumerator getListOfEmails(
		string accessToken, IList<EmailData> emailsToReturn)
	{
		// TODO(nader): Soon this will actually fetch from Gmail.
		// See https://goo.gl/0TN9Hf for instructions on how to use the token
		// to hit the API. Leaving an example of WWW here to show what it'll
		// look like.
		//
		// WWW www = new WWW(url);
		// yield return www;
		//
		// Once this returns, we will loop through all of the emails we get back and add them
		// to emailsToReturn. Once that's done, we'll exit the function.

		IList<string> listOfSubjects = new List<string>() {
			"Uh-oh, your prescription is expiring",
			"Hey bro, guess who just learned Unity!",
			"Best of Groupon: The Deals That Make Us Proud (Unlike Our Nephew, Steve)",
			"Hey",
			"Security threat *Don't Open This Email*",
			"What Can You Afford?",
			"Where to Drink Beer Right Now",
			"Stop! Read this important message about Googlyness!",
		};
		string bodyStub =
			"Hey friend,\n\n" +
			"Have you heard about \"Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
			"sed do eiusmod tempor " +
			"incididunt ut labore et " +
			"dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco " +
			"laboris nisi ut aliquip " +
			"ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit " +
			"esse cillum dolore " +
			"eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in " +
			"culpa qui officia " +
			"deserunt mollit anim id est laborum.\"\n\nIt's pretty great. Check it out!";
		for (int ii = 0; ii < 100; ii++) {
			emailsToReturn.Add (new EmailData(listOfSubjects[ii % listOfSubjects.Count], bodyStub));
		}
		yield return null;
	}
}