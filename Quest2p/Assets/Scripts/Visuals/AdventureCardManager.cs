using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

// holds the refs to all the Text, Images on the card
public class AdventureCardManager : NetworkBehaviour {

	public AdventureAsset adventureAsset;
	public AdventureCardManager PreviewManager;
    
	[Header("Text Component References")]
    public Text CardDescriptionText;
    public Text CardNameText;
    public Text CardPower;
    public Text CardAdditionalPower;


    [Header("Image References")]
    public Image CardFaceImage;

	public int Index;


    void Awake()
    {
		if (adventureAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {

		CardDescriptionText.text = adventureAsset.Description;

		CardNameText.text = adventureAsset.characterName;

		CardPower.text = adventureAsset.CharacterPower;

		CardAdditionalPower.text = adventureAsset.AdditionalPower;

		CardFaceImage.sprite = adventureAsset.CardImage;




        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
			PreviewManager.adventureAsset = adventureAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
}
