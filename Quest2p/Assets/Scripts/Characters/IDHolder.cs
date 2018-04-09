using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDHolder : MonoBehaviour
{
    private static List<IDHolder> allIDHolders = new List<IDHolder>();
    public int UniqueID;

    void Awake()
    {
        allIDHolders.Add(this);
    }

    public static GameObject GetGameObjectWithID(int ID)
    {
        foreach (IDHolder i in allIDHolders)
        {
            if (i.UniqueID == ID)
                return i.gameObject;
        }
        return null;
    }

    public static void ClearIDHoldersList()
    {
        allIDHolders.Clear();
    }
}
