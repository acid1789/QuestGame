using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.UIElements;

public class HandVisual : MonoBehaviour
{

    public AreaPosition owner;
    public bool TakeCardsOpenly = true;
    public SameDistanceChildren slots;

    //A list of all card visual representations as GameObjects
    private List<GameObject> CardsInHand = new List<GameObject>();

    //Add a new Card GameObject to hand
    public void AddCard(GameObject card)
    {
        CardsInHand.Insert(0, card);
        card.transform.SetParent(slots.transform);
        //Re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    //Get a card GameObject with a given index in hand
    public GameObject GetCardAtIndex(int index)
    {
        return CardsInHand[index];
    }

    //Remove a card GameObject from hand
    public void RemoveCard(GameObject card)
    {
        CardsInHand.Remove(card);

        //Re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    //Remove card with a given index from hand
    public void RemoveCardAtIndex(int index)
    {
        this.CardsInHand.RemoveAt(index);

        //Re-calculate the position of the hand
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    /**
     * MANAGING CARDS AND SLOTS
     */
    //move Slots GameObject according to the number of cards in hand
    void UpdatePlacementOfSlots()
    {
        float posX;
        if (CardsInHand.Count > 0)
            posX = (slots.Children[0].transform.localPosition.x -
            slots.Children[CardsInHand.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;
        //Tween slots GameObject to new position in 0.3 seconds
        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in CardsInHand)
        {
            //tween this card to a new slot
            g.transform.DOLocalMoveX(slots.Children[CardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);
        }
    }

    public void GivePlayerACard(AdventureAsset ca, int UniqueID)
    {
        GameObject card;
        card = CreateACardAtPosition(ca, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        foreach (Transform t in card.GetComponentInChildren<Transform>())
            t.tag = owner.ToString() + "Card";
    }

    GameObject CreateACardAtPosition(AdventureAsset c, Vector3 position, Vector3 eulerAngles)
    {
        return null;
    }
}
