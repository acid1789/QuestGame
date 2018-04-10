using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMenu : MonoBehaviour {

	public GameObject MenuOptions;
	public GameObject WaitingForOpponent;
	public UnityEngine.Networking.NetworkLobbyManager NetworkManager;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ShowWaiting()
	{
		WaitingForOpponent.SetActive(true);
		gameObject.SetActive(false);
	}

	public void OnHost()
	{
		NetworkManager.StartHost();
		ShowWaiting();
	}

	public void OnJoin()
	{
		NetworkManager.StartClient();
		ShowWaiting();
	}

	public void OnBack()
	{
		MenuOptions.SetActive(true);
		gameObject.SetActive(false);
	}

	public void Show()
	{
		MenuOptions.SetActive(false);
		gameObject.SetActive(true);
	}

	public void CancelWaiting()
	{
		WaitingForOpponent.SetActive(false);
		MenuOptions.SetActive(true);
	}
}
