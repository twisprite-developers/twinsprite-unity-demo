using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;
using TwinSpriteSDK;
using System.Security.Cryptography;

public class Main : MonoBehaviour  {

	string API_KEY = "API_KEY";
	string SECRET_KEY = "SECRET_KEY";
	string toyxId = "";

	Toyx toyx;





	// Use this for initialization
	void Start () {


		// Make context
		TwinSpriteContext twinSpriteContext = new TwinSpriteContext ();
		twinSpriteContext.logLevel = TwinSpriteContext.TwinSpriteInfoLog;
			
		// Init twinsprite with context
		TwinSprite.initialize(twinSpriteContext, API_KEY, SECRET_KEY);
		

	}


	private string infoMessage = "";
	private bool guiEnabled = true;

	void OnGUI () {

		if (guiEnabled) {
			GUI.enabled = true;
		} else {
			GUI.enabled = false;
		}


		float x = 30;
		float y = 30;
		float marginX = 30;
		float marginY = 30;

		GUI.skin.label.fontSize = 20;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.textArea.alignment = TextAnchor.UpperCenter;
		GUI.skin.label.wordWrap = true;
		GUI.skin.textField.fontSize = 20;
		GUI.skin.button.fontSize = 20;

		// Toyx Id
		GUI.Label( new Rect(x,y,Screen.width - (marginX * 2), marginY), "ToyxId");

		y+= marginY * 1;

		toyxId = GUI.TextField( new Rect(x,y,Screen.width - (marginX * 2), marginY), toyxId);


		y+= marginY * 1.5f;

		// Create session
		if (GUI.Button( new Rect(x,y,Screen.width - (marginX * 2), marginY), "Create session")) {

			// Check api key and secret key
			if (TwinSprite.GetApiKey().Equals("API_KEY") || TwinSprite.GetSecretKey().Equals("API_KEY")) {
				infoMessage = "Insert valid API_KEY and SECRET_KEY";

			// Check toyx
			} else if (toyxId.Length == 0) {
				infoMessage = "No Toyx Id";

			// Creating session
			} else {			
				infoMessage = "Creating session...";
				guiEnabled = false;

				CreateSession();
			}
		}

		y+= marginY * 1.5f;

		// Fecth
		if (GUI.Button( new Rect(x,y,Screen.width - (marginX * 2), marginY), "Fetch")) {

			// Check toyx
			if (toyx == null) {
				infoMessage = "No Toyx, create session before";
			} else {

				infoMessage = "Fetching...";
				guiEnabled = false;
				
				Get();
			}
			
		}

		y+= marginY * 1.5f;


		// Save
		if(GUI.Button( new Rect(x,y,Screen.width - (marginX * 2), marginY), "Save")) {
			// Check toyx
			if (toyx == null) {
				infoMessage = "No Toyx, create session before";
			} else {
				
				infoMessage = "Saving...";
				guiEnabled = false;
				
				Save();
			}
		}

		y+= marginY * 1.5f;

		// Save if needed
		if(GUI.Button( new Rect(x,y,Screen.width - (marginX * 2), marginY), "Save eventually")) {
			// Check toyx
			if (toyx == null) {
				infoMessage = "No Toyx, create session before";
			} else {
				
				infoMessage = "Saving...";
				guiEnabled = false;
				
				SaveEventually();
			}
		}
		
		y+= marginY * 1.5f;

		// Info
		GUI.enabled = true;
		GUI.TextArea( new Rect(x,y,Screen.width - (marginX * 2), marginY * 4), infoMessage);


	}


	public void CreateSession() {

		// Create session with toyx id
		CreateSessionRequest.CreateSessionInBackground (toyxId, delegate(Toyx newToyx, TwinSpriteError error) {
		
			if (error != null) {
				infoMessage = "Error creating session: "+error.message+"\nError code: " +error.errorCode;
			} else {

				infoMessage = "Created session!!!";
				toyx = newToyx;
			}

			guiEnabled = true;


		});
		
	}
		

	public void Get() {

		// Get toyx in background
		ToyxQuery.GetInBackground (toyxId, delegate(Toyx newToyx, TwinSpriteError error) {

			if (error != null) {
				infoMessage = "Error fetching: "+error.message+"\nError code: " +error.errorCode;
			} else {
				infoMessage = "Feched: "+toyx;
				toyx = newToyx;
			}

			guiEnabled = true;
		});
		
	}

	public void Save() {

		// Save toyx in background
		toyx.SaveInBackground(delegate(TwinSpriteError error) {
			if (error != null) {
				infoMessage = "Error saving: "+error.message+"\nError code: " +error.errorCode;
			} else {
				infoMessage = "Saved: "+toyx;
			}
			
			guiEnabled = true;
		});
	}

	public void SaveEventually() {
		
		toyx.SaveEventually(delegate(TwinSpriteError error) {
			if (error != null) {
				infoMessage = "Error saving: "+error.message+"\nError code: " +error.errorCode;
			} else {
				infoMessage = "Saved: "+toyx;
			}
			
			guiEnabled = true;
		});
	}


	
	
	
}
