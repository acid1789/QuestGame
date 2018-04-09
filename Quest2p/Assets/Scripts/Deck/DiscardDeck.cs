using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardDeck : Deck
{
    private int totalCardsInDiscardDeck = 0;

    // Use this for initialization
    void Start()
    {
		
    }
	
    // Update is called once per frame
    void Update()
    {
		
    }

    public override int DeckSize
    {
        get
        {
            return totalCardsInDiscardDeck;
        }
        set
        {
            totalCardsInDiscardDeck = value;
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

    protected override void shuffleDeck()
    {
        //WARNING: stupid design here. DiscardDeck doesn't need to be shuffled
    }
}
