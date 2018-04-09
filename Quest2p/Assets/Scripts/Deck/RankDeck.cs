using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public enum RANK
{
    SQUIRE,
    KNIGHT,
    CHAMPION
}

public class RankDeck : MonoBehaviour
{
    public List<RankAsset> rankDeck = new List<RankAsset>();
    private Dictionary<int, RankAsset> indexDeck = new Dictionary<int, RankAsset>();
    private int currentRank = 0;

    void Awake()
    {
        foreach (RankAsset r in rankDeck)
        {
            Debug.Log("RankAsset: " + r.characterName);
           
            if (r.characterName.ToString().Equals("Squire"))
                indexDeck.Add(0, r);
            if (r.characterName.ToString().Equals("Knight"))
                indexDeck.Add(1, r);
            if (r.characterName.ToString().Contains("Champion"))
                indexDeck.Add(2, r);
        }
    }

    public void LevelUp()
    {
        currentRank++;
    }

    public int GetCurrentRank
    {
        get{ return currentRank; }
    }

    public RankAsset SquireCard()
    {
        return indexDeck[0];
    }

    public RankAsset KnightCard()
    {
        return indexDeck[1];
    }

    public RankAsset ChampionKnightCard()
    {
        return indexDeck[2];
    }
}
