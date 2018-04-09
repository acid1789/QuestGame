using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBtnListene : MonoBehaviour {

	public void quit () {
		Debug.Log ("User has Quit the game");
		Application.Quit();

	}
}