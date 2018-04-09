using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class StoryCardManager : MonoBehaviour {

	public StoryAsset storyAsset;
	public StoryCardManager PreviewManager;
    
	[Header("Text Component References")]
    public Text CardDescriptionText;
    public Text CardNameText;
    public Text CardPower;
    public Text CardAdditionalPower;


    [Header("Image References")]
    public Image CardFaceImage;


    void Awake()
    {
		if (storyAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {

		CardDescriptionText.text = storyAsset.Description;

		CardNameText.text = storyAsset.characterName;

		CardPower.text = storyAsset.CharacterPower;

		CardAdditionalPower.text = storyAsset.AdditionalPower;

		CardFaceImage.sprite = storyAsset.CardImage;

        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
			PreviewManager.storyAsset = storyAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
		
}
