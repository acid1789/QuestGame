using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck2 : MonoBehaviour
{

	public List<StoryAsset> storyDeck = new List<StoryAsset>();

	void Start()
	{
		storyDeck.Shuffle ();
	}


}
