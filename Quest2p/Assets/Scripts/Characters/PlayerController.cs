using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
	Player _player;
	GameManager _gm;

	bool _netInitialized;

	public List<AdventureCardManager> CardsInPlay;
	int _nextInPlaySlot;


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

	public Player Player { get { return _player; } set { _player = value; _player.PC = this; } }
	public bool NetInitilaized { get { return _netInitialized; } }
	public int CardsPlayed { get { return _nextInPlaySlot; } }

	public void ClearCardsInPlay()
	{
		foreach (AdventureCardManager card in CardsInPlay)
			card.gameObject.SetActive(false);
		_nextInPlaySlot = 0;
	}

	public void PlayCard(int cardIndex)
	{		
		RpcPutCardIntoPlay(_nextInPlaySlot++, cardIndex);
	}

	public void ReplaceCard(int advCardIndex)
	{
		_nextInPlaySlot--;
		RpcReplaceCard(_nextInPlaySlot, advCardIndex);
	}

	[Command]
	void CmdSetInitialized()
	{
		_netInitialized = true;
	}

	[Command]
	public void CmdClickCard(int card)
	{
		_gm.ClickCard(this, card);
	}

	[Command]
	public void CmdSelectYesOrNo(bool selectedYes)
	{
		_gm.DoYesNo(selectedYes, this);
	}

	[ClientRpc]
	public void RpcAddCardToHand(int cardIndex)
	{
		if (_gm == null)
			_gm = FindObjectOfType<GameManager>();

		AdventureAsset card = _gm.adventureDeck.adventureDeck[cardIndex];
		AdventureCardManager cm = _player.AddCardToHand(!_player.isAI, card);
		cm.Index = cardIndex;
	}

	[ClientRpc]
	public void RpcAddShields(int add)
	{
		_player.shields.NumberOfAvailableShields += add;
	}

	[ClientRpc]
	public void RpcSetShields(int shields)
	{
		_player.shields.NumberOfAvailableShields = shields;
	}

	[ClientRpc]
	public void RpcPutCardIntoPlay(int slot, int cardIndex)
	{
		AdventureAsset card = _gm.adventureDeck.adventureDeck[cardIndex];
		CardsInPlay[slot].adventureAsset = card;
		CardsInPlay[slot].ReadCardFromAsset();
		CardsInPlay[slot].gameObject.SetActive(true);
		if (_player.isAI)
		{
			CardsInPlay[slot].GetComponent<HoverPreview>().ThisPreviewEnabled = false;
			CardsInPlay[slot].GetComponent<BetterCardRotation>().CardBack.gameObject.SetActive(true);
		}
		//else
		//	_player.HideHandCard(card);
	}

	[ClientRpc]
	public void RpcReplaceCard(int slot, int advCardIndex)
	{
		AdventureAsset oldCard = CardsInPlay[slot].adventureAsset;
		AdventureAsset card = _gm.adventureDeck.adventureDeck[advCardIndex];
		_player.ReplaceCardInHand(oldCard, card);
	}

	[ClientRpc]
	public void RpcShowMessage(string message, float duration, bool showButtons)
	{
		new ShowMessageCommand(message, duration, showButtons).AddToQueue();
	}
}
