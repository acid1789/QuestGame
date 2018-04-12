using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum GameState
{
	None,
	DecideToSponsorQuest,
	SettingUpQuestCards,
	PlayingQuest,
	LostQuest,
	WonQuest,
	DecideQuest,
	Event,
	StartedCard,
	AskEnterTournament,
	PlayTournament,
	WinTournament,
	LoseTournament//Add tournament
}

public class GameManager : NetworkBehaviour 
{

	#region Fields
	//Assignments
	public TurnManager turnManager;

	public GameState currentGameState;
	public int neededStages = 0; //For quest
	public int neededForTournament = 0;

	public bool aiOverride = false;
	
	public AdventureDeck adventureDeck;	

	//Story cards
	public StoryDeck storyDeck;
	public StoryCardManager currentStoryCard;
	public List<AdventureCardManager> upperCardsInPlay = new List<AdventureCardManager> ();
	public List<AdventureCardManager> lowerCardsInPlay = new List<AdventureCardManager> ();

	public Player BottomPlayer;
	public Player TopPlayer;

	PlayerController PC_Local;
	PlayerController PC_Remote;
	PlayerController[] PlayOrder;

	[SyncVar]
	int CurrentPlayer;
	
	int[] adventureDeckOrder;
	int adventureDeckPosition;
	int[] storyDeckOrder;
	int storyDeckPosition;

	int[] questSponsorship = new int[2];
	int currentQuestPicker;
	int[] tournamentJoins = new int[2];
	#endregion

	void Start()
	{
		ResetCardsInPlay ();
	}

	void Update()
	{
		if (PC_Local == null)
		{
			PlayerController[] pcs = FindObjectsOfType<PlayerController>();
			if (pcs.Length == 2)
			{
				foreach (PlayerController pc in pcs)
				{
					if (pc.isLocalPlayer)
						PC_Local = pc;
					else
						PC_Remote = pc;
				}
				PC_Local.Player = BottomPlayer;
				PC_Local.CardsInPlay = lowerCardsInPlay;
				PC_Remote.Player = TopPlayer;
				PC_Remote.CardsInPlay = upperCardsInPlay;
				PlayOrder = isServer ? new PlayerController[] { PC_Local, PC_Remote } : new PlayerController[] { PC_Remote, PC_Local };

				if (isServer)
					StartCoroutine(SetupGame());
			}
		}
	}

	IEnumerator SetupGame()
	{
		do
		{
			yield return new WaitForSeconds(2);
		} while (!PC_Local.NetInitilaized || !PC_Remote.NetInitilaized);

		// Shuffle the story deck
		storyDeckOrder = storyDeck.Shuffle();
		storyDeckPosition = 0;

		// Shuffle the adventure deck
		adventureDeckOrder = adventureDeck.Shuffle();
		adventureDeckPosition = 0;

		// Deal initial hands
		for (int i = 0; i < Player.maxCards; i++)
		{
			PC_Local.RpcAddCardToHand(adventureDeckOrder[adventureDeckPosition++]);
			PC_Remote.RpcAddCardToHand(adventureDeckOrder[adventureDeckPosition++]);
		}

		yield return new WaitForSeconds(1);
		CurrentPlayer = -1;
		StartNextTurn();
	}

	void StartNextTurn()
	{
		currentGameState = GameState.None;
		aiOverride = false;
		neededStages = -1;
		neededForTournament = 0;

		turnManager.messMan.Reset();

		//hide the cards in play
		PC_Local.ClearCardsInPlay();
		PC_Remote.ClearCardsInPlay();

		CurrentPlayer++;
		if (CurrentPlayer >= PlayOrder.Length)
			CurrentPlayer = 0;

		DrawStoryCard();
		RpcShowMessage("New Turn", 2.0f, GetCurrentPlayer().netId);
		currentGameState = GameState.StartedCard;
	}

	public void MessageComplete()
	{
		if (isServer)
		{
			Debug.Log("GameState: " + currentGameState);

			switch (currentGameState)
			{
				case GameState.WonQuest:
				case GameState.LostQuest:
				case GameState.WinTournament:
				case GameState.LoseTournament:
					StartNextTurn();
					break;
				case GameState.DecideToSponsorQuest:
					// Quest message timed out
					StartNextTurn();
					break;
				case GameState.Event:
					ProcessEvent();
					StartNextTurn();					
					break;
				case GameState.StartedCard:
					ProcessDrawnCard();
					break;
				case GameState.AskEnterTournament:
					throw new System.NotImplementedException();
			}			
		}
	}

	void ProcessDrawnCard()
	{
		switch (currentStoryCard.CardDescriptionText.text)
		{
			case "EVENT":
				currentGameState = GameState.Event;
				RpcShowMessage(currentStoryCard.storyAsset.CharacterPower, 3.0f, GetCurrentPlayer().netId);
				break;
			case "QUEST":
				for( int i = 0; i < questSponsorship.Length; i++ )
					questSponsorship[i] = -1;
				currentGameState = GameState.DecideToSponsorQuest;
				currentQuestPicker = CurrentPlayer;
				RpcShowMessage("Sponsor Quest?", 27f, GetCurrentPlayer().netId);
				break;
			case "TOURNAMENT":
				for (int i = 0; i < tournamentJoins.Length; i++)
					tournamentJoins[i] = -1;
				currentGameState = GameState.AskEnterTournament;
				RpcShowMessage("Player Join Tournament?", 27.0f, GetCurrentPlayer().netId);
				break;
		}
	}

	public void ResetCardsInPlay()
	{
		HideCards (upperCardsInPlay, 0, upperCardsInPlay.Count);
		HideCards (lowerCardsInPlay, 0, lowerCardsInPlay.Count);
	}

	public void ShowUpperCards()
	{
		for (int i = 0; i < upperCardsInPlay.Count; i++) {
			upperCardsInPlay [i].gameObject.GetComponent<BetterCardRotation> ().CardBack.gameObject.SetActive (true);
			upperCardsInPlay [i].gameObject.GetComponent<HoverPreview> ().ThisPreviewEnabled = true;
		}
	}

	public void HideCards(List<AdventureCardManager> whichCardList, int startIndex, int howMany) 
	{
		for (int i = startIndex; i < whichCardList.Count; i++)
		{
			if (i < howMany)
			{
				whichCardList [i].gameObject.SetActive (false);
			}
		}
	}

	public void AddCardToList(List<AdventureCardManager> whichCardList, int cardIndex, AdventureAsset ca)
	{
		whichCardList [cardIndex].adventureAsset = ca;
		whichCardList [cardIndex].ReadCardFromAsset ();
		whichCardList [cardIndex].gameObject.SetActive (true);
	}

	public void DrawStoryCard()
	{
		int storyCard = storyDeckOrder[storyDeckPosition++];
		RpcSetStoryCard(storyCard);
	}
		
	/// Called after event is played
	public void ProcessEvent()
	{
		int numShields;
		switch (currentStoryCard.storyAsset.CharacterPower)
		{
			//Handeling the "POX"
			case "Pox":
				// Other player looses a shield
				PlayerController opc = GetOtherPlayer();
				numShields = opc.Player.shields.NumberOfAvailableShields;
				if (numShields > 0)
					opc.RpcSetShields(numShields - 1);
				break;

			//Handeling the "PLAGUE"
			case "Plague":
				PlayerController pc = GetCurrentPlayer();
				numShields = pc.Player.shields.NumberOfAvailableShields;
				if (numShields > 1)
					pc.RpcSetShields(numShields - 2);
				break;
		}
	}

	public void SelectYesOrNo(bool selectedYes)
	{
		PC_Local.CmdSelectYesOrNo(selectedYes);
	}
	
	public void DoYesNo(bool selectedYes, PlayerController clicker)
	{
		turnManager.messMan.Reset ();

		bool isCurrentPlayer = GetCurrentPlayer() == clicker;
		switch (currentGameState)
		{
			case GameState.DecideToSponsorQuest:
				if (clicker == PlayOrder[currentQuestPicker])
				{
					// Mark this player has chosen already
					questSponsorship[currentQuestPicker] = selectedYes ? 1 : 0;

					if (selectedYes)
					{
						currentGameState = GameState.SettingUpQuestCards;
						int needStages = int.Parse(currentStoryCard.storyAsset.CharacterPower.Substring(0, 1));
						RpcStartQuest(needStages);
					}
					else
					{
						currentQuestPicker++;
						if (currentQuestPicker >= questSponsorship.Length)
							currentQuestPicker = 0;
						if (questSponsorship[currentQuestPicker] < 0)
						{
							// Ask this player
							RpcShowMessage("Sponsor Quest?", 27f, PlayOrder[currentQuestPicker].netId);
						}
						else
						{
							// All players chose no
							StartNextTurn();
						}
					}
				}
				break;
			case GameState.AskEnterTournament:
				tournamentJoins[GetPlayerIndex(clicker)] = selectedYes ? 1 : 0;
				int clicked = 0;
				int join = 0;
				for (int i = 0; i < tournamentJoins.Length; i++)
				{
					if (tournamentJoins[i] >= 0)
						clicked++;
					if (tournamentJoins[i] == 1)
						join++;
				}
				if (clicked >= tournamentJoins.Length)
				{
					// Done asking about tournament
					if (join > 0)
					{
						// Do the tournament
						StartTournament();
					}
					else
					{
						// Not enough people
						StartNextTurn();
					}
				}
				break;
		}
		/*
		if (turnManager.playerOrder[turnManager.currentPlayerIndex].isAI)
		{
			switch (currentGameState)
			{

			//Have the AI set it's cards up if deciding to sponsor a quest
			case GameState.DecideToSponsorQuest:				
				turnManager.playerOrder [turnManager.currentPlayerIndex].AIQuestHandling ();
				break;
			
			//Have the AI set it's cards up if deciding to participate the tournament
			case GameState.AskEnterTournament:
				neededForTournament++;
				if (neededForTournament < 2) {
					new ShowMessageCommand ("Player Join Tournament?", 15.0f, true).AddToQueue ();
				}
				else {
					StartTournament ();
				}
				break;
			}
		}
		else
		{

			if (aiOverride)
			{
				if (turnManager.gameManager.currentGameState == GameState.AskEnterTournament) {
					neededForTournament++;
					StartTournament ();
				}
				return;
			}

			if (turnManager.gameManager.currentGameState == GameState.DecideToSponsorQuest) {
				turnManager.playerOrder [turnManager.currentPlayerIndex].PlayerQuestHandling (selectedYes);
			} else if (turnManager.gameManager.currentGameState == GameState.AskEnterTournament) {
				
				neededForTournament++;
				if (neededForTournament < 2) {
					new ShowMessageCommand ("Enemy Joins Tournament?", 3.0f, false).AddToQueue ();
				}
				else {
					StartTournament ();
				}
			}
		}*/
	}

	public void StartTournament()
	{
		Debug.Log ("start tourn");
		currentGameState = GameState.PlayTournament;
		neededStages = 3;

		//Play AI moves
		//turnManager.playerOrder[1].PlayAITournament();
	}

	public void DecideTournament()
	{
		ShowUpperCards ();
		currentGameState = GameState.LoseTournament;

		for (int i = 0; i < neededStages; i++)
		{
			if (turnManager.currentPlayerIndex == 0) {
				if (upperCardsInPlay[i].adventureAsset.bp < lowerCardsInPlay[i].adventureAsset.bp) {
					currentGameState = GameState.WinTournament;
				}
			}
			else {
				if (upperCardsInPlay[i].adventureAsset.bp > lowerCardsInPlay[i].adventureAsset.bp) {
					currentGameState = GameState.WinTournament;
				}
			}
		}

		//Give rewards and show message
		if (turnManager.currentPlayerIndex == 0)
		{
			if (currentGameState == GameState.WinTournament) {
				new ShowMessageCommand("You Won!", 2.0f, false).AddToQueue();

				turnManager.playerOrder [0].shields.NumberOfAvailableShields += 1;
			}
			else
			{
				new ShowMessageCommand("You Lost!", 2.0f, false).AddToQueue();
				turnManager.playerOrder [1].shields.NumberOfAvailableShields += 1;
			}
		}
		else
		{
			if (currentGameState == GameState.WinTournament) {
				new ShowMessageCommand("You Lost!", 2.0f, false).AddToQueue();

				turnManager.playerOrder [1].shields.NumberOfAvailableShields += 1;
			}
			else
			{
				new ShowMessageCommand("You Won!", 2.0f, false).AddToQueue();
				turnManager.playerOrder [0].shields.NumberOfAvailableShields += 1;
			}
		}
	}

	public void DecideQuest()
	{
		ShowUpperCards ();

		currentGameState = GameState.LostQuest;

		for (int i = 0; i < neededStages; i++)
		{
			if (turnManager.currentPlayerIndex == 0) {
				if (upperCardsInPlay[i].adventureAsset.bp < lowerCardsInPlay[i].adventureAsset.bp) {
					currentGameState = GameState.WonQuest;
				}
			}
			else {
				if (upperCardsInPlay[i].adventureAsset.bp > lowerCardsInPlay[i].adventureAsset.bp) {
					currentGameState = GameState.WonQuest;
				}
			}
		}

		//Give rewards and show message
		if (turnManager.currentPlayerIndex == 0)
		{
			if (currentGameState == GameState.WonQuest) {
				new ShowMessageCommand("You Won!", 2.0f, false).AddToQueue();

				turnManager.playerOrder [0].shields.NumberOfAvailableShields += neededStages;
			}
			else
			{
				new ShowMessageCommand("You Lost!", 2.0f, false).AddToQueue();
				turnManager.playerOrder [1].shields.NumberOfAvailableShields += neededStages;
			}
		}
		else
		{
			if (currentGameState == GameState.WonQuest) {
				new ShowMessageCommand("You Lost!", 2.0f, false).AddToQueue();

				turnManager.playerOrder [1].shields.NumberOfAvailableShields += neededStages;
			}
			else
			{
				new ShowMessageCommand("You Won!", 2.0f, false).AddToQueue();
				turnManager.playerOrder [0].shields.NumberOfAvailableShields += neededStages;
			}
		}
	}

	public void ExecuteQuest()
	{
		// Reveal all cards
		RpcRevealCardsInPlay();

		// Calculate score
		int scoreA = 0;
		int scoreB = 0;
		for (int i = 0; i < neededStages; i++)
		{
			if (upperCardsInPlay[i].adventureAsset.bp < lowerCardsInPlay[i].adventureAsset.bp)
				scoreB++;
			else
				scoreA++;
		}
		int winner = scoreA > scoreB ? 0 : 1;
		int loser = winner == 0 ? 1 : 0;
		PlayOrder[winner].RpcAddShields(neededStages);
		PlayOrder[winner].RpcShowMessage("You Won!", 2.0f, false);
		PlayOrder[loser].RpcShowMessage("You Lost!", 2.0f, false);

		currentGameState = GameState.WonQuest;

		// Refill hands
		//for (int i = 0; i < neededStages; i++)
		//{
		//	PC_Local.ReplaceCard(adventureDeckOrder[adventureDeckPosition++]);
		//	PC_Remote.ReplaceCard(adventureDeckOrder[adventureDeckPosition++]);
		//}
	}

	public void ExecuteTournament()
	{
		// Reveal all cards
		RpcRevealCardsInPlay();

		// Calculate score
		int scoreA = 0;
		int scoreB = 0;
		for (int i = 0; i < neededStages; i++)
		{
			if (upperCardsInPlay[i].adventureAsset.bp < lowerCardsInPlay[i].adventureAsset.bp)
				scoreB++;
			else
				scoreA++;
		}
		int winner = scoreA > scoreB ? 0 : 1;
		int loser = winner == 0 ? 1 : 0;
		PlayOrder[winner].RpcAddShields(1);
		PlayOrder[winner].RpcShowMessage("You Won!", 2.0f, false);
		PlayOrder[loser].RpcShowMessage("You Lost!", 2.0f, false);

		currentGameState = GameState.WinTournament;
	}

	public void ClickCard(PlayerController pc, int card)
	{
		switch (currentGameState)
		{
			case GameState.SettingUpQuestCards:
			case GameState.PlayTournament:
				if ( pc.CardsPlayed < neededStages )
					pc.PlayCard(card);
				if (PC_Local.CardsPlayed >= neededStages && PC_Remote.CardsPlayed >= neededStages)
				{
					if (currentGameState == GameState.SettingUpQuestCards)
						ExecuteQuest();
					else
						ExecuteTournament();
				}
				break;
		}
	}

	public int GetPlayerIndex(PlayerController pc)
	{
		for (int i = 0; i < PlayOrder.Length; i++)
		{
			if (PlayOrder[i] == pc)
				return i;
		}
		return -1;
	}

	public PlayerController GetCurrentPlayer()
	{
		return PlayOrder[CurrentPlayer];
	}

	public PlayerController GetOtherPlayer()
	{
		int other = CurrentPlayer + 1;
		if (other > 1)
			other = 0;
		return PlayOrder[other];
	}

	public PlayerController FindPlayer(NetworkInstanceId id)
	{
		foreach (PlayerController pc in PlayOrder)
		{
			if (pc.netId == id)
				return pc;
		}
		return null;
	}

	[Command]
	public void CmdEnterTournament()
	{
		neededForTournament++;
	}
	
	[ClientRpc]
	public void RpcSetStoryCard(int storyCard)
	{
		currentStoryCard.storyAsset = storyDeck.storyDeck[storyCard];
		currentStoryCard.ReadCardFromAsset();
	}

	[ClientRpc]
	public void RpcShowMessage(string message, float duration, NetworkInstanceId currentTurnPc)
	{
		bool showButtons = false;
		bool isLocalPlayerTurn = (PC_Local.netId == currentTurnPc);
		switch (message)
		{
			case "New Turn":
				message = isLocalPlayerTurn ? "Your Turn" : "Enemy Turn";
				break;
			case "Sponsor Quest?":
				showButtons = isLocalPlayerTurn;
				break;
			case "Player Join Tournament?":
				showButtons = true;
				break;
		}

		new ShowMessageCommand(message, duration, showButtons).AddToQueue();
	}

	[ClientRpc]
	public void RpcStartQuest(int needCards)
	{
		turnManager.messMan.Reset();
		neededStages = needCards;
	}

	[ClientRpc]
	public void RpcRevealCardsInPlay()
	{
		for (int i = 0; i < upperCardsInPlay.Count; i++)
		{
			upperCardsInPlay[i].gameObject.GetComponent<BetterCardRotation>().CardBack.gameObject.SetActive(false);
			upperCardsInPlay[i].gameObject.GetComponent<HoverPreview>().ThisPreviewEnabled = true;
		}
	}
}
