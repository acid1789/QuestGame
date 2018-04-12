using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
//using UnityEditor.VersionControl;

public class StoryDeck : Deck
{
    public List<StoryAsset> storyDeck = new List<StoryAsset>();

    private int totalCardinStoryDeck = 0;

    // Update is called once per frame
    void Awake()
    {
        //shuffleDeck();
    }

    protected override void shuffleDeck()
    {
        storyDeck.Shuffle();
    }

    public override int DeckSize
    {
        get
        {
            totalCardinStoryDeck = storyDeck.Count;
            return totalCardinStoryDeck;
        }
        set
        {
            totalCardinStoryDeck = value;
        }
    }

	public StoryAsset DrawNewCard()
	{
		try{
			StoryAsset nc = storyDeck [0];
			if (storyDeck.Count > 0)
				storyDeck.RemoveAt (0);
			return nc;
		}catch(System.Exception error) {
			Debug.Log ("Argument Out of range error. Will be fixed later: " + error);
		}
		return null;
	}

    protected override void drawCard()
    {

    }

    protected override void removeCard()
    {

    }

    protected override void addCard()
    {

    }

    protected override void distributeCard()
    {

    }

	public int[] Shuffle()
	{
		List<int> order = new List<int>();
		List<int> shuffled = new List<int>();

		for (int i = 0; i < storyDeck.Count; i++)
			order.Add(i);

		System.Random rand = new System.Random();
		while (order.Count > 0)
		{
			int index = rand.Next(order.Count);
			shuffled.Add(order[index]);
			order.RemoveAt(index);
		}

		return shuffled.ToArray();
	}
}
