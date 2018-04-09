using UnityEngine;
using System.Collections;
//using UnityEditor;
//susing UnityEditor;

public class CardFlipper : MonoBehaviour
{
	
    SpriteRenderer spriteRenderer;
    DemoDeck model;

    public AnimationCurve scaleCurve;
    public float duration = 0.5f;

    //Sprite prefabSprite;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        model = GetComponent<DemoDeck>();
        //prefabSprite = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/QuestCard.prefab", (typeof(Sprite))) as Sprite;
    }

    public void FlipCard(QuestAsset startImage, QuestAsset endImage, int cardIndex)
    {
        StopCoroutine(Flip(startImage, endImage, cardIndex));
        StartCoroutine(Flip(startImage, endImage, cardIndex));
    }

    IEnumerator Flip(QuestAsset startImage, QuestAsset endImage, int cardIndex)
    {
        float time = 0f;
        while (time <= 1f)
        {
            float scale = scaleCurve.Evaluate(time);
            time += Time.deltaTime / duration;

            Vector3 localScale = transform.localScale;
            localScale.x = scale;
            transform.localScale = localScale;
            
            yield return new WaitForFixedUpdate();
        }

        if (cardIndex == -1)
        {
            model.ToggleFace(false);
        }
        else
        {
            model.questCardIndex = cardIndex;
            model.ToggleFace(true);
        }
    }
}
