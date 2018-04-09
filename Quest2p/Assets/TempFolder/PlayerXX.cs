using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXX: MonoBehaviour
{

    public AllyDeck oldDeck;
    public NewPlace newDeck;
    public GameObject allyCardPrefab;

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        DistributeCards();
    }

    public void DistributeCards()
    {
        GameObject createNewCardInHand;
        if (oldDeck.adventureDeck.Count > 0)
        {
            AdventureAsset newCard = oldDeck.adventureDeck[0];
            //enemyHand.allyWeaponDeck.Add(newCard);
            newDeck.adventureDeck.Insert(0, newCard);
            createNewCardInHand = CreateACardAtPositionX(newCard, new Vector3(0, 0, 0), new Vector3(0f, 0f, 0f));
            //testing.ThisPreviewEnabled = true;
        }

        oldDeck.adventureDeck.RemoveAt(0);
        //new DrawACardCommand(hand.cardsInHand[0], this);
    }


    GameObject CreateACardAtPositionX(AdventureAsset c, Vector3 position, Vector3 eulerAngles)
    {
        // Instantiate a card depending on its type
        GameObject NewCard;
        Vector3 temp;
        NewCard = GameObject.Instantiate(allyCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;

        temp = new Vector3(newDeck.transform.position.x + 0.5f, newDeck.transform.position.y, newDeck.transform.position.z);
        NewCard.transform.position = temp;
        AdventureCardManager manager = NewCard.GetComponent<AdventureCardManager>();
        manager.adventureAsset = c;
        manager.ReadCardFromAsset();
        return NewCard;
    }
}
