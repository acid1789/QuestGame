using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventAsset : ScriptableObject 
{

	// this object will hold the info about the most general card
	[Header("Card info")]

	[TextArea(2,3)]
	public string EventCardName;

	[TextArea(3,3)]
	public string EventName;

	[TextArea(7,5)]
	public string EventCardDescription;
}
