using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class GameManager : MonoBehaviour 
{

	#region Fields
	//Assignments
	public TurnManager turnManager;

	public GameState currentGameState;
	public int neededStages = 0; //For quest
	public int neededForTournament = 0;

	public bool aiOverride = false;

	//Story cards
	public StoryDeck storyDeck;
	public StoryCardManager currentStoryCard;
	public List<AdventureCardManager> upperCardsInPlay = new List<AdventureCardManager> ();
	public List<AdventureCardManager> lowerCardsInPlay = new List<AdventureCardManager> ();

	#endregion

	void Start()
	{
		ResetCardsInPlay ();
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
		//while (currentStoryCard.CardDescriptionText.text != "TOURNAMENT")  //Only test quest cards right now
		{
			currentStoryCard.storyAsset = storyDeck.DrawNewCard ();
			currentStoryCard.ReadCardFromAsset ();
		}
	}
		
	/// Called after event is played
	public void ProcessEvent()
	{
		switch (currentStoryCard.storyAsset.CharacterPower) {

		//Handeling the "POX"
		case "Pox":
			turnManager.playerOrder [0].shields.NumberOfAvailableShields -= 1;
			turnManager.playerOrder [1].shields.NumberOfAvailableShields -= 1;
			turnManager.playerOrder [turnManager.currentPlayerIndex].shields.NumberOfAvailableShields++;

			if (turnManager.playerOrder[0].shields.NumberOfAvailableShields < 0) {
				turnManager.playerOrder [0].shields.NumberOfAvailableShields = 0;
			}

			if (turnManager.playerOrder[1].shields.NumberOfAvailableShields < 0) {
				turnManager.playerOrder [1].shields.NumberOfAvailableShields = 0;
			}
			break;
		
		//Handeling the "PLAGUE"
		case "Plague":
			if (turnManager.playerOrder[turnManager.currentPlayerIndex].shields.NumberOfAvailableShields > 1) {
				turnManager.playerOrder [turnManager.currentPlayerIndex].shields.NumberOfAvailableShields -= 2;
			}
			break;
		}


	}

	public void SelectYesOrNo(bool selectedYes) 
	{
		turnManager.messMan.Reset ();

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
		}
	}

	public void StartTournament()
	{
		Debug.Log ("start tourn");
		currentGameState = GameState.PlayTournament;
		neededStages = 3;

		//Play AI moves
		turnManager.playerOrder[1].PlayAITournament();
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
}
