using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QLobbyPlayer : NetworkLobbyPlayer {

	bool _enteredLobby;
	System.DateTime _lobbyEnterTime;

	public void Update()
	{
		if (isLocalPlayer && _enteredLobby && (System.DateTime.Now - _lobbyEnterTime).TotalSeconds > 1)
		{
			_enteredLobby = false;
			readyToBegin = true;
			SendReadyToBeginMessage();
		}
	}

	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby();
		_lobbyEnterTime = System.DateTime.Now;
		_enteredLobby = true;
	}
}
