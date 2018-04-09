using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class QuestCardManager : MonoBehaviour {

	public QuestAsset questAsset;
	public QuestCardManager PreviewManager;
    
	[Header("Text Component References")]

	public Text QuestCardDescriptionText;
	public Text QuestFaceText;
	public Text QuestCardStages;
	public Text QuestCardAdditionalPower;


    void Awake()
    {
		if (questAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {

		QuestCardDescriptionText.text = questAsset.CardDescription;

		QuestFaceText.text = questAsset.QuestFaceText;

		QuestCardStages.text = questAsset.QuestCardStages;

		QuestCardAdditionalPower.text = questAsset.QuestCardAdditionalPower;

		if (PreviewManager != null)
		{
			// this is a card and not a preview
			// Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
			PreviewManager.questAsset = questAsset;
			PreviewManager.ReadCardFromAsset();
		}
	
    }

}
