using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	Player _player;
	GameManager _gm;

	bool _netInitialized;


	// Use this for initialization
	void Start()
	{
		CmdSetInitialized();
	}

	// Update is called once per frame
	void Update()
	{
		if( _gm == null )
			_gm = FindObjectOfType<GameManager>();
	}

	public void SetPlayer(Player p) { _player = p; }
	public bool NetInitilaized { get { return _netInitialized; } }

	[Command]
	void CmdSetInitialized()
	{
		_netInitialized = true;
	}
	
	[ClientRpc]
	public void RpcAddCardToHand(int cardIndex)
	{
		if (_gm == null)
			_gm = FindObjectOfType<GameManager>();

		AdventureAsset card = _gm.adventureDeck.adventureDeck[cardIndex];
		_player.AddCardToHand(!_player.isAI, card);
	}
}
