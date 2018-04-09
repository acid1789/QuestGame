using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerX: MonoBehaviour
{

	public Deck2 oldDeck;
	public NewPlace newDeck;
	public GameObject questCardPrefab, eventCardPrefab, tournamentCardPrefab;
	static int index = 0;

	private List<StoryAsset> discardPile = new List<StoryAsset>();

	void Update()
	{

	}

//	void reset(){
//		if (oldDeck.storyDeck.Count == 0) {
//			oldDeck.storyDeck =  new List<StoryAsset>();
//		}
//	}

	void OnMouseDown()
	{
		DistributeCards();
		//reset ();
	}

	//LOGIC
	public void DistributeCards()
	{
		Vector3 tempNewDeck = newDeck.transform.position;
		GameObject createNewCardInHand;
		if (oldDeck.storyDeck.Count > 0) {
			StoryAsset newCard = oldDeck.storyDeck [0];
			newDeck.storyDeck.Insert (0, newCard);
			createNewCardInHand = CreateACardAtPositionX (newCard, new Vector3 (0, 0, 0), new Vector3 (0f, 0f, 0f));
			//oldDeck.storyDeck.Insert (21, newCard);
		}

		oldDeck.storyDeck.RemoveAt (0);
		//discardPile.Add (newCard);


		if (oldDeck.storyDeck.Count == 0) {
			oldDeck.storyDeck.AddRange(newDeck.storyDeck);
			oldDeck.storyDeck.Shuffle ();
			newDeck.transform.position = new Vector3 (-14.7f, -7.4f, -0.3f);
			newDeck.storyDeck.Clear ();
			index = 0;
		}



	
		//Check if newDeck not empty
		//remove at 0
		//if not, nothing happens

//		if (oldDeck.storyDeck.Count == 0) {
//			DestroyDeck();
//		}

}		
	
//	deck destruction is working with this function and above if statement...
//	void DestroyDeck(){
//		Destroy(this.gameObject);
//	}



	//VISUAL
	GameObject CreateACardAtPositionX(StoryAsset c, Vector3 position, Vector3 eulerAngles)
	{
		// Instantiate a card depending on its type
		GameObject NewCard;
		//GameObject OldCard;

		Vector3 temp;

		//List<Vector3> newtemp = new List<Vector3> ();
		//int ind = new Random(0,newtemp.Count);
		//Vector3 newtemp;

		if (c.Description.ToString().Equals("EVENT"))
		{
			NewCard = GameObject.Instantiate(eventCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
		}
		else if (c.Description.ToString().Equals("QUEST"))
		{
			NewCard = GameObject.Instantiate(questCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
		}
		else
		{
			NewCard = GameObject.Instantiate(tournamentCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;
		}

		temp = new Vector3(newDeck.transform.position.x + 0f * index, newDeck.transform.position.y, newDeck.transform.position.z);

		NewCard.transform.position = temp;

		//newtemp.Add(new Vector3(newDeck.transform.position.x + 0.5f * index, newDeck.transform.position.y, newDeck.transform.position.z));
		//NewCard.transform.position = newtemp;

		print (temp);



		//this fucntion destroys newCards, as soon as they are created.
		if(newDeck.storyDeck.Count > 0){
			DestroyObject (NewCard, 1);
		}

		StoryCardManager manager = NewCard.GetComponent<StoryCardManager>();
		manager.storyAsset = c;
		manager.ReadCardFromAsset();
		index++;
		return NewCard;

	}
}
