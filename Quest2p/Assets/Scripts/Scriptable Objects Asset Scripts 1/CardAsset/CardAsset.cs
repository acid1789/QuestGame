﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardAsset : ScriptableObject 
{
	// this object will hold the info about the most general card
	[Header("Card info")]

	[TextArea(2,3)]
	public string Description;  // Description for the card

	public Sprite CardImage;

	[TextArea(2,3)]
	public string characterName;  // if this is null, it`s null

	[TextArea(2,3)]
	public string CharacterPower;

	[TextArea(4,3)]
	public string AdditionalPower;


}
