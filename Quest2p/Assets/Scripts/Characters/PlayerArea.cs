using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaPosition
{
    Top,
    Low
}

public class PlayerArea : MonoBehaviour
{
    public AreaPosition owner;
    public bool ControlsON = true;
    public HandVisual handVisual;

    public bool AllowedToControlThisPlayer
    {
        get;
        set;
    }

}