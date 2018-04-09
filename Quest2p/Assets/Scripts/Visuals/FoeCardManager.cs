using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class FoeCardManager : MonoBehaviour {

    public FoeAsset foeAsset;
	public FoeCardManager PreviewManager;
    
	[Header("Text Component References")]
    public Text CardDescriptionText;
    public Text CardNameText;
    public Text CardPower;
    public Text CardAdditionalPower;


    [Header("Image References")]
    public Image CardFaceImage;


    void Awake()
    {
		if (foeAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {

		CardDescriptionText.text = foeAsset.Description;

		CardNameText.text = foeAsset.characterName;

		CardPower.text = foeAsset.CharacterPower;

		CardAdditionalPower.text = foeAsset.AdditionalPower;

		CardFaceImage.sprite = foeAsset.CardImage;




        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
			PreviewManager.foeAsset = foeAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
}
