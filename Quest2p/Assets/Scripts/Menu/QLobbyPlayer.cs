﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QLobbyPlayer : NetworkLobbyPlayer {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby();
		Debug.Log("OnClientEnterLobby");
		readyToBegin = true;
		SendReadyToBeginMessage();
	}
}
