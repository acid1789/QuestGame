using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class TestCardManager : MonoBehaviour {

    public TestAsset testAsset;
	public TestCardManager PreviewManager;
    
	[Header("Text Component References")]
    public Text CardDescriptionText;
    public Text CardNameText;
    public Text CardAdditionalPower;


    [Header("Image References")]
    public Image CardFaceImage;


    void Awake()
    {
		if (testAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {

		CardDescriptionText.text = testAsset.Description;

		CardNameText.text = testAsset.characterName;

		CardAdditionalPower.text = testAsset.AdditionalPower;

		CardFaceImage.sprite = testAsset.CardImage;




        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
			PreviewManager.testAsset = testAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
}
