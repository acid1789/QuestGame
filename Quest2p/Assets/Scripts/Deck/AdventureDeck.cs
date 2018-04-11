using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureDeck : Deck
{
    //public List<CardAsset> allyWeaponDeck = new List<CardAsset>();
    //public List<FoeAsset> foeDeck = new List<FoeAsset>();
    //public List<TestAsset> testDeck = new List<TestAsset>();
    public List<AdventureAsset> adventureDeck = new List<AdventureAsset>();

    private int totalCardinAdventureDeck = 0;
	int nextCard;

    void Awake()
    {
		nextCard = 0;
	}

    protected override void shuffleDeck()
    {
        //allyWeaponDeck.Shuffle();
        //foeDeck.Shuffle();
        //testDeck.Shuffle();
        adventureDeck.Shuffle();
    }

    public override int DeckSize
    {
        get
        {
            totalCardinAdventureDeck = adventureDeck.Count;
            return totalCardinAdventureDeck;
        }
        set
        {
            totalCardinAdventureDeck = value;
        }
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

		for (int i = 0; i < adventureDeck.Count; i++)
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
