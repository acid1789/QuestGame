using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardLogic
{
    public Player owner;

    //an ID of this card
    public int UniqueCardID;
    //A reference to the card asset that stores all the info about this card
    public AdventureAsset ca;

	public int characterPower = 0;

    //STATIC (for managing IDs)
    public static Dictionary<int, CardLogic> CardsCreatedThisTime = new Dictionary<int, CardLogic>();

    //PROPERTIES
    public int ID
    {
        get { return UniqueCardID; }
    }

    public CardLogic(AdventureAsset newCardAsset)
    {
        this.ca = newCardAsset;

		if (newCardAsset.Description == "F O E") 
		{
			
			string stage1 = newCardAsset.CharacterPower.Split (' ') [0];
			string stage2 = stage1.Split ('/') [0];
			characterPower = int.Parse (stage2);
		}
        UniqueCardID = IDFactory.GetUniqueID();
        CardsCreatedThisTime.Add(UniqueCardID, this);
    }
}
