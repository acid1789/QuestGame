using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

// this class will take care of switching turns and counting down time until the turn expires
using System.Collections.Generic;


public class TurnManager : MonoBehaviour {

	//Logic
	public MessageManager messMan;
	public List<Player> playerOrder = new List<Player>();
	public int currentPlayerIndex = -1;

    private Timer timer;

    public static TurnManager Instance;

	public GameManager gameManager;

    void Awake()
    {
        Instance = this;
        timer = GetComponent<Timer>();
    }

    void Start()
    {
        OnGameStart();
    }

	void OnGameStart()
	{
		//Listen for end of timer
		timer.TimerExpired.AddListener(TurnEnded);

		//Start turn for first player
		StartNextTurn();
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndTurn();
    }

	public void EndTurnTest()
	{
		if (!playerOrder [currentPlayerIndex].isAI) {
			EndTurn ();
		}
	}

    public void EndTurn()
    {
        timer.StopTimer();
		TurnEnded ();
    }

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

	/// <summary>
	/// Starts the next turn and alternates between players
	/// </summary>
	public void StartNextTurn()
	{
		timer.StopTimer ();
		gameManager.currentGameState = GameState.None;
		gameManager.aiOverride = false;
		gameManager.neededStages = -1;
		gameManager.neededForTournament = 0;
		playerOrder [0].currentPlayCardIndex = 0;
		playerOrder [1].currentPlayCardIndex = 0;

		messMan.Reset ();

		//hide the cards in play
		gameManager.HideCards (gameManager.upperCardsInPlay, 0, gameManager.upperCardsInPlay.Count);
		gameManager.HideCards (gameManager.lowerCardsInPlay, 0, gameManager.lowerCardsInPlay.Count);

		currentPlayerIndex++;
		if (currentPlayerIndex >= playerOrder.Count) {
			currentPlayerIndex = 0;
		}

		playerOrder [currentPlayerIndex].StartTurn ();
		timer.StartTimer();
	}

	/// <summary>
	/// If timer ended early
	/// </summary>
	public void TurnEnded()
	{
		StartNextTurn ();
	}
}

