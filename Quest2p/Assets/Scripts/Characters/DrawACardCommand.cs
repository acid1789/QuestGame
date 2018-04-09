using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawACardCommand : Command
{
    private CardLogic cardLogic;
    private Player player;

    public DrawACardCommand(CardLogic cl, Player p)
    {
        this.cardLogic = cl;
        this.player = p;
    }

    public override void StartCommandExecution()
    {
        player.PArea.handVisual.GivePlayerACard(cardLogic.ca, cardLogic.UniqueCardID);
    }
}
