using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class EventCardManager : MonoBehaviour {

	public EventAsset eventAsset;
	public EventCardManager PreviewManager;
    
	[Header("Text Component References")]
    
	public Text EventCardName;
	public Text EventName;
	public Text EventCardDescription;


    void Awake()
    {
        if (eventAsset != null)
            ReadCardFromAsset();
    }

    public void ReadCardFromAsset()
    {

		EventCardName.text = eventAsset.EventCardName;

		EventName.text = eventAsset.EventName;

		EventCardDescription.text = eventAsset.EventCardDescription;

		if (PreviewManager != null)
		{
			// this is a card and not a preview
			// Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
			PreviewManager.eventAsset = eventAsset;
			PreviewManager.ReadCardFromAsset();
		}
	
    }


}
