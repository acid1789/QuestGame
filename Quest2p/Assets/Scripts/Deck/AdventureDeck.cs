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

    void Awake()
    {
        shuffleDeck();
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
}
