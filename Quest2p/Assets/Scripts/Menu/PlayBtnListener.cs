using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBtnListener : MonoBehaviour {

	public NetworkMenu NetworkOptions;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown () {
		print ("Play Button clicked");
		//SceneManager.LoadScene ("BattleScene");

		NetworkOptions.Show();
	}
}
