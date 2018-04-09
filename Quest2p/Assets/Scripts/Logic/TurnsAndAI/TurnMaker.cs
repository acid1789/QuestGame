using UnityEngine;
using System.Collections;

public abstract class TurnMaker : MonoBehaviour {

    protected Player p;

    void Awake()
    {
		if (GetComponent<Player> () != null) {
			p = GetComponent<Player> ();
		}
    }

    public virtual void OnTurnStart()
    {

    }

}
