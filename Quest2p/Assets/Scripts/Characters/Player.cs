using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.ConstrainedExecution;
//using UnityEditorInternal.VR.iOS;
using UnityEngineInternal;
//using UnityEditor.VersionControl;
using System;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
	public GameManager gameManager;
	public TurnManager turnManager;

	//AI logic
	public bool isAI;
	public int currentPlayCardIndex = 0;

    //Constant variables
    private const int maxCards = 12;
    private const int maxShields = 10;

    //Player information
    private string playerName;
    private string avatar;
    public int PlayerID;
    public PlayerArea PArea;

    //RANK, SHIELD, LevelUp
    public RankDeck rank;
    public ShieldVisual shields;

    //Adventure Deck and cards
    public AdventureDeck adventureDeck;
    public GameObject allyCardPrefab, weaponCardPrefab, amourCardPrefab, foeCardPrefab, testCardPrefab, questPrefab, rankPrefab;
    public static int cardIndex = 0;

    //Player Hand
    public Hand hand;

    // a static array that will store both players, should always have 2 players
    public static Player[] Players;

    // PROPERTIES
    // this property is a part of interface ICharacter
    public int ID
    {
        get{ return PlayerID; }
    }

    // opponent player
    public Player otherPlayer
    {
        get
        {
            if (Players[0] == this)
                return Players[1];
            else
                return Players[0];
        }
    }

    // ALL METHODS
    void Awake()
    {
        // find all scripts of type Player and store them in Players array
        // (we should have only 2 players in the scene)
        Players = GameObject.FindObjectsOfType<Player>();
        // obtain unique id from IDFactory
        PlayerID = IDFactory.GetUniqueID();
    }

    //Call once at the start of the game
    void Start()
    {
        CreateRankCard(rank.SquireCard());
    }

    //-----------------------

    public Player()
    {
        playerName = "NoName";
        avatar = "NoAvatar";
        rank = new RankDeck();
    }

    public Player(string _name, string _avatar)
    {
        playerName = _name;
        avatar = _avatar;
        rank = new RankDeck();
    }

    void Update()
    {
        DistributeCards();
        AdvancedRank();
    }

    /*
    void OnMouseDown()
    {
        //TESTING Shield and Rank
        //shields.NumberOfAvailableShields++;
    }*/

    public void DistributeCards()
    {

        if (adventureDeck.adventureDeck.Count > 0 && hand.cardsInHand.Count < maxCards)
        {
            if (PArea.owner == AreaPosition.Low)
            {
                AddCardToHand(isCardFaceUp : true);
            }
            else
            {
                AddCardToHand(isCardFaceUp : false);
            }
            //new DrawACardCommand(hand.cardsInHand[0], this);
        }
    }

    void AddCardToHand(bool isCardFaceUp)
    {
        //Get the first card of adventureDeck
        AdventureAsset newCard = adventureDeck.adventureDeck[0];
        //Created CardLogic for that card with assigned owner and index
        CardLogic newCardLogic = new CardLogic(newCard);
        newCardLogic.owner = this;
        //Added to player's hand
        hand.cardsInHand.Insert(0, newCardLogic);
        //Created the card on visual
        if (isCardFaceUp)
            CreateACardAtPosition(newCard, newCardLogic);
        else
            CreateACardAtPosition(newCard, newCardLogic, new Vector3(0f, -180f, 0f));
        //Remove the card from AdventureDeck
        adventureDeck.adventureDeck.RemoveAt(0);
    }

    //VISUAL
    GameObject CreateACardAtPosition(AdventureAsset newAdventureCard, CardLogic newLogic, Vector3 eulerAngles = default(Vector3), Vector3 position = default(Vector3))
    {
        // Instantiate a card depending on its type
        GameObject NewCard;
        Vector3 temp;

        if (newAdventureCard.Description.ToString().Equals("ALLY"))
        {
            NewCard = GameObject.Instantiate(allyCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
        }
        else if (newAdventureCard.Description.ToString().Equals("WEAPON"))
        {
            NewCard = GameObject.Instantiate(weaponCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
        }
        else if (newAdventureCard.Description.ToString().Equals("AMOUR"))
        {
            NewCard = GameObject.Instantiate(amourCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
        }
        else if (newAdventureCard.Description.ToString().Equals("TEST"))
        {
            NewCard = GameObject.Instantiate(testCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
        }
        else
        {
            NewCard = GameObject.Instantiate(foeCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
        }

		EventTrigger trigger = NewCard.gameObject.GetComponent<EventTrigger> ();
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener ((evenData) => {
			UseCard (NewCard.GetComponent<AdventureCardManager>());
		});
		trigger.triggers.Add (entry);

		temp = new Vector3(hand.transform.position.x + cardIndex, hand.transform.position.y, hand.transform.position.z);

        NewCard.transform.position = temp;
        cardIndex++;

        AdventureCardManager manager = NewCard.GetComponent<AdventureCardManager>();
        manager.adventureAsset = newAdventureCard;
        manager.ReadCardFromAsset();

        IDHolder id = NewCard.AddComponent<IDHolder>();
        id.UniqueID = newLogic.UniqueCardID;

		//add to cards dealt
		hand.cardsDealt.Add (NewCard);

		//hide enemy card from player
		if (isAI)
		{
			NewCard.GetComponent<HoverPreview> ().ThisPreviewEnabled = false;
		}

        return NewCard;
    }

    /**Call this function only after finishing Quest, Event, Tournament*/
    void AdvancedRank()
    {
        if (shields.NumberOfAvailableShields >= 5 && rank.GetCurrentRank == (int)RANK.SQUIRE) //rank is Squire
        {
            shields.NumberOfAvailableShields -= 5;
            //Level up to Knight
            rank.LevelUp();
            //Create Knight Card
            CreateRankCard(rank.KnightCard());
        }
        else if (shields.NumberOfAvailableShields >= 7 && rank.GetCurrentRank == (int)RANK.KNIGHT)
        {
            shields.NumberOfAvailableShields -= 7;
            //Level up to Champion Knight
            rank.LevelUp();
            //Create Champion Card
            CreateRankCard(rank.ChampionKnightCard());
        }
        else if (shields.NumberOfAvailableShields >= 10 && rank.GetCurrentRank == (int)RANK.CHAMPION)
        {
            shields.NumberOfAvailableShields -= 10;
            //Level up to Knight of the Round Table
            rank.LevelUp();
            //TODO: Finish Game
        }


    }

    void CreateRankCard(RankAsset currentRank)
    {
        // Instantiate a card depending on its type
        GameObject NewCard;
        Vector3 temp;
        NewCard = GameObject.Instantiate(rankPrefab, new Vector3(0f, 0f, 0f), Quaternion.Euler(new Vector3(0f, 0f, 0f))) as GameObject;
        //New Rank Card will stay to the left of the player's hand
        if (PArea.owner == AreaPosition.Low)
            temp = new Vector3(hand.transform.position.x - 2f, hand.transform.position.y - 0.5f, hand.transform.position.z);
        else
            temp = new Vector3(hand.transform.position.x - 2.3f, hand.transform.position.y - 0.5f, hand.transform.position.z);
        NewCard.transform.position = temp;

        RankCardManager manager = NewCard.GetComponent<RankCardManager>();
        manager.rankAsset = currentRank;
        manager.ReadCardFromAsset();
    }

	public void UseCard(AdventureCardManager adventureCard)
	{
		if (gameManager.currentGameState == GameState.PlayingQuest) 
		{
			gameManager.AddCardToList (gameManager.lowerCardsInPlay, currentPlayCardIndex, adventureCard.adventureAsset);

			//hand.cardsDealt.Remove (adventureCard.gameObject);
			//hand.cardsInHand.Remove (adventureCard.gameObject.GetComponent<CardLogic> ());

			currentPlayCardIndex++;

			if (currentPlayCardIndex >= gameManager.neededStages) 
			{
				gameManager.DecideQuest ();
			}
		}
		else if (gameManager.currentGameState == GameState.PlayTournament) {
				gameManager.AddCardToList (gameManager.lowerCardsInPlay, currentPlayCardIndex, adventureCard.adventureAsset);

				currentPlayCardIndex++;

				if (currentPlayCardIndex >= 3) 
				{
					gameManager.DecideTournament ();
				}
		}
		else if (gameManager.currentGameState == GameState.SettingUpQuestCards) {
			if (!turnManager.playerOrder[turnManager.currentPlayerIndex].isAI) {

				gameManager.AddCardToList (gameManager.lowerCardsInPlay, currentPlayCardIndex, adventureCard.adventureAsset);

				currentPlayCardIndex++;

				if (currentPlayCardIndex >= gameManager.neededStages) 
				{
					turnManager.playerOrder [1].DoIParticipateInQuest ();
					gameManager.DecideQuest ();
				}

			}
		}
	}

	//Turn logic
	public void StartTurn()
	{
		gameManager.DrawStoryCard ();

		if (isAI) 
		{
			new ShowMessageCommand("Enemy Turn!", 2.0f, false).AddToQueue();
		} 
		else
		{
			new ShowMessageCommand("Your Turn!", 2.0f, false).AddToQueue();
		}

		gameManager.currentGameState = GameState.StartedCard;
	}

	public void ProcessDrawnCard() {

		if (isAI) 
		{
			switch (gameManager.currentStoryCard.CardDescriptionText.text)
			{
			case "QUEST":
				gameManager.currentGameState = GameState.DecideToSponsorQuest;
				new ShowMessageCommand ("Sponsor Quest?", 3.0f, false).AddToQueue ();
				break;
			case "EVENT":
				gameManager.currentGameState = GameState.Event;
				new ShowMessageCommand (gameManager.currentStoryCard.storyAsset.CharacterPower, 3.0f, false);
				break;
			case "TOURNAMENT":
				gameManager.currentGameState = GameState.AskEnterTournament;
				new ShowMessageCommand ("Enemy Join Tournament?", 3.0f, false).AddToQueue ();
				break;
			}
		} 
		else
		{
			switch (gameManager.currentStoryCard.CardDescriptionText.text)
			{
			case "EVENT":
				gameManager.currentGameState = GameState.Event;
				new ShowMessageCommand (gameManager.currentStoryCard.storyAsset.CharacterPower, 3.0f, false);
				break;
			case "QUEST":
				gameManager.currentGameState = GameState.DecideToSponsorQuest;
				new ShowMessageCommand ("Sponsor Quest?", 27f, true);
				break;
			case "TOURNAMENT":
				gameManager.currentGameState = GameState.AskEnterTournament;
				new ShowMessageCommand ("Player Join Tournament?", 27.0f, true).AddToQueue ();
				break;
			}
		}
	}

	public void PlayerQuestHandling(bool willSponsor) {
		if (willSponsor) {
			currentPlayCardIndex = 0;
			gameManager.currentGameState = GameState.SettingUpQuestCards;
			gameManager.neededStages = int.Parse(gameManager.currentStoryCard.storyAsset.CharacterPower.Substring(0, 1));
		}
		else {
			turnManager.StartNextTurn ();
		}
	}

	/// <summary>
	/// Called after sponsor quest? window is shown
	/// </summary>
	public void AIQuestHandling() 
	{

		Debug.Log ("AI is handling quest");

		if (DoISponsorAQuest())
		{
			currentPlayCardIndex = 0;

			//Create a list of cards the player can play
			List<CardLogic> playableCards = new List<CardLogic> ();
			foreach (CardLogic cl in hand.cardsInHand)
			{
				if (cl.ca.Description == "F O E" || cl.ca.Description == "TEST")
				{
					playableCards.Add (cl);
				}
			}

			//sort by bp
			playableCards.Sort (SortByBp);

			//Change test to index 1
			int testIndex = -1;
			foreach (CardLogic c1 in playableCards)
			{
				if (c1.ca.Description == "TEST")
				{
					testIndex = playableCards.IndexOf (c1);
				}
			}
			if (testIndex != -1) 
			{
				CardLogic temp = playableCards [1];
				playableCards [1] = playableCards [testIndex];
				playableCards [testIndex] = temp;
			}

			for (int i = 0; i < gameManager.neededStages; i++)
			{
				gameManager.AddCardToList (gameManager.upperCardsInPlay, i, playableCards [i].ca);
				gameManager.upperCardsInPlay [i].gameObject.GetComponent<BetterCardRotation> ().CardBack.gameObject.SetActive (true);
				gameManager.upperCardsInPlay [i].gameObject.GetComponent<HoverPreview> ().ThisPreviewEnabled = false;
			}
				
			gameManager.HideCards (gameManager.upperCardsInPlay, gameManager.neededStages, gameManager.upperCardsInPlay.Count - gameManager.neededStages);

			gameManager.currentGameState = GameState.PlayingQuest;
			currentPlayCardIndex = 0;
		}
		else
		{
			//Advance if not sponsoring quest to next turn
			turnManager.StartNextTurn();
		}
	}

	static int SortByBp(CardLogic cl1, CardLogic cl2)
	{
		return cl1.characterPower.CompareTo (cl2.characterPower);
	}

	/// <summary>
	/// This method is just for AI
	/// </summary>
	/// <returns><c>true</c>, if I participate in quest was done, <c>false</c> otherwise.</returns>
	public bool DoIParticipateInQuest()
	{
		currentPlayCardIndex = 0;

		//Create a list of cards the player can play
		List<CardLogic> playableCards = new List<CardLogic> ();
		foreach (CardLogic cl in hand.cardsInHand)
		{
			playableCards.Add (cl);
		}

		//sort by bp
		playableCards.Sort (SortByBp);

		for (int i = 0; i < gameManager.neededStages; i++)
		{
			gameManager.AddCardToList (gameManager.upperCardsInPlay, i, playableCards [i].ca);
			gameManager.upperCardsInPlay [i].gameObject.GetComponent<BetterCardRotation> ().CardBack.gameObject.SetActive (true);
			gameManager.upperCardsInPlay [i].gameObject.GetComponent<HoverPreview> ().ThisPreviewEnabled = false;
		}
		gameManager.HideCards (gameManager.upperCardsInPlay, gameManager.neededStages, gameManager.upperCardsInPlay.Count - gameManager.neededStages);

		gameManager.currentGameState = GameState.PlayingQuest;
		currentPlayCardIndex = 0;
		return true;
	}

	/// <summary>
	/// Just for AI
	/// </summary>
	/// <returns><c>true</c>, if I sponsor A quest was done, <c>false</c> otherwise.</returns>
	public bool DoISponsorAQuest()
	{
		gameManager.neededStages = int.Parse(gameManager.currentStoryCard.storyAsset.CharacterPower.Substring(0, 1));

		foreach (Player p in turnManager.playerOrder)
		{
			if (p.shields.availableShields + 2 >= 5 || p.shields.availableShields + 2 >= 7 || p.shields.availableShields + 2 >= 10)
			{
				//don't sponsor, player is close to victory
				Debug.Log("too many shields for ai to sponsor");
				turnManager.StartNextTurn();
				return false;
			}
		}

		//Sufficient foes
		int numberOfFoes = 0;

		foreach (CardLogic cl in hand.cardsInHand)
		{
			if (cl.ca.Description == "F O E" || cl.ca.Description == "TEST")
			{
				numberOfFoes++;
			}
		}

		if (numberOfFoes >= gameManager.neededStages) {
			return true;
		}

		Debug.Log ("Enough foes and tests to sponsor tournament");

		return false;
	}

	public void PlayAITournament() {
		//Create a list of cards the player can play
		List<CardLogic> playableCards = new List<CardLogic> ();
		foreach (CardLogic cl in hand.cardsInHand)
		{
			playableCards.Add (cl);
		}

		//sort by bp
		playableCards.Sort (SortByBp);

		for (int i = 0; i < 3; i++)
		{
			gameManager.AddCardToList (gameManager.upperCardsInPlay, i, playableCards [i].ca);
			gameManager.upperCardsInPlay [i].gameObject.GetComponent<BetterCardRotation> ().CardBack.gameObject.SetActive (true);
			gameManager.upperCardsInPlay [i].gameObject.GetComponent<HoverPreview> ().ThisPreviewEnabled = false;
		}
		gameManager.HideCards (gameManager.upperCardsInPlay, gameManager.neededStages, gameManager.upperCardsInPlay.Count - gameManager.neededStages);

		gameManager.currentGameState = GameState.PlayTournament;
		currentPlayCardIndex = 0;
	}
}
