using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class RankCardManager : MonoBehaviour {

    public RankAsset rankAsset;
	public RankCardManager PreviewManager;
    
	[Header("Text Component References")]
    public Text CardDescriptionText;
    public Text CardNameText;
    public Text CardPower;


    [Header("Image References")]
    public Image CardFaceImage;


    void Awake()
    {
		if (rankAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {

		CardDescriptionText.text = rankAsset.Description;

		CardNameText.text = rankAsset.characterName;

		CardPower.text = rankAsset.Power;

		CardFaceImage.sprite = rankAsset.CardImage;




        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
			PreviewManager.rankAsset = rankAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
}
