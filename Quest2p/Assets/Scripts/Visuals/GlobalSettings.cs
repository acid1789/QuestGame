using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GlobalSettings: MonoBehaviour 
{

	[Header("Other")]
    public Button EndTurnButton;

    // SINGLETON
    public static GlobalSettings Instance;
}
