using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.PackageManager.Requests;

public abstract class Deck : MonoBehaviour
{
    //Virtual can have body inside, subclasses can OVERRIDE it if needed
    //Can be inside abstract or non-abstract class
    protected virtual void printCard()
    {

    }

    public abstract int DeckSize
    {
        get;
        set;
    }

    //Abstract can NOT have body inside, subclasses HAVE TO OVERRIDE it
    //Must be inside abstract class
    protected abstract void drawCard();

    protected abstract void removeCard();

    protected abstract void addCard();

    protected abstract void distributeCard();

    protected abstract void shuffleDeck();

}
