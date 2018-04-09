using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class TournamentCardManager : MonoBehaviour {

    public TournamentAsset tournamentAsset;
	public TournamentCardManager PreviewManager;
    
	[Header("Text Component References")]
    public Text CardDescriptionText;
    public Text CardNameText;
    public Text CardBonusShields;


    [Header("Image References")]
    public Image CardFaceImage;


    void Awake()
    {
		if (tournamentAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {

		CardDescriptionText.text = tournamentAsset.Description;

		CardNameText.text = tournamentAsset.tournamentName;

		CardBonusShields.text = tournamentAsset.bonusShields;

		CardFaceImage.sprite = tournamentAsset.CardImage;




        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
			PreviewManager.tournamentAsset = tournamentAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
}
