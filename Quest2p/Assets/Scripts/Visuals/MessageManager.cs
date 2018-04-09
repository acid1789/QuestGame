using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour 
{
    public Text MessageText;
    public GameObject MessagePanel;

	public Button YesButton, NoButton;

    public static MessageManager Instance;

	public GameManager gameManager;

    void Awake()
    {
        Instance = this;
        MessagePanel.SetActive(false);

		ShowButtons (false);
    }

	public void ShowMessage(string Message, float Duration, bool buttonShow)
    {
		StartCoroutine(ShowMessageCoroutine(Message, Duration, buttonShow));
    }

	IEnumerator ShowMessageCoroutine(string Message, float Duration, bool buttonShow)
    {
        //Debug.Log("Showing some message. Duration: " + Duration);
        MessageText.text = Message;
        MessagePanel.SetActive(true);

		ShowButtons (buttonShow);

        yield return new WaitForSeconds(Duration);

        MessagePanel.SetActive(false);
		ShowButtons (false);

		Debug.Log (gameManager.currentGameState);


		if (gameManager.turnManager.currentPlayerIndex == 1) 
		{
			gameManager.aiOverride = true;

			gameManager.SelectYesOrNo (true);
		}

		if (gameManager.currentGameState == GameState.WonQuest || gameManager.currentGameState == GameState.LostQuest 
			|| gameManager.currentGameState == GameState.WinTournament 
			|| gameManager.currentGameState == GameState.LoseTournament || gameManager.currentGameState == GameState.Event) {

			if (gameManager.currentGameState == GameState.Event)
			{
				gameManager.ProcessEvent ();
			}

			gameManager.turnManager.EndTurn ();
		}
		else if (gameManager.currentGameState == GameState.DecideToSponsorQuest) {
			gameManager.turnManager.EndTurn ();
		}
		else if (gameManager.currentGameState == GameState.StartedCard) {
			gameManager.turnManager.playerOrder [gameManager.turnManager.currentPlayerIndex].ProcessDrawnCard ();
		}
		else if (gameManager.currentGameState == GameState.AskEnterTournament) {
			gameManager.neededForTournament++;
			if (gameManager.neededForTournament >= 2) {
				gameManager.StartTournament ();
			}
		}

    }

	public void ShowButtons(bool show)
	{
		YesButton.gameObject.SetActive (show);
		NoButton.gameObject.SetActive (show);
	}

	public void Reset() {
		MessagePanel.SetActive (false);
		ShowButtons (false);
	}
}
