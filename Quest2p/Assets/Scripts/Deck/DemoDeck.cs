using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class DemoDeck : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    CardFlipper flipper;

    public QuestAsset[] questFaceDeck;
    public QuestAsset questBackCard;

    public int questCardIndex;
    int Index = 0;

    //Sprite prefabSprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        flipper = GetComponent<CardFlipper>();
        //prefabSprite = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/QuestCard.prefab", (typeof(Sprite))) as Sprite;
        shuffleDeck();
    }

    protected void shuffleDeck()
    {
    }

    public void ToggleFace(bool showFace)
    {
        /*if (showFace)
            spriteRenderer.sprite = questFaceDeck[questCardIndex];
        else
            spriteRenderer.sprite = questBackCard;*/
        //spriteRenderer.sprite = prefabSprite;
    }

    void OnMouseDown()
    {
        if (Index >= questFaceDeck.Length)
        {
            Index = 0;
            flipper.FlipCard(questFaceDeck[questFaceDeck.Length - 1], questBackCard, -1);
        }
        else
        {
            if (Index > 0)
            {
                flipper.FlipCard(questFaceDeck[Index - 1], questFaceDeck[Index], Index);
            }
            else
            {
                flipper.FlipCard(questBackCard, questFaceDeck[Index], Index);
            }
            Index++;
        }
    }
}
