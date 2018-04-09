using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestAsset : ScriptableObject 
{

	// this object will hold the info about the most general card
	[Header("Card info")]

	[TextArea(2,3)]
	public string CardDescription;

	[TextArea(4,3)]
	public string QuestFaceText;

	[TextArea(4,3)]
	public string QuestCardStages;

	[TextArea(4,3)]
	public string QuestCardAdditionalPower;
}
