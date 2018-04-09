using UnityEngine;
using System.Collections;

//this class will take all decisions for AI. 

public class AITurnMaker: TurnMaker {

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        new ShowMessageCommand("Enemy`s Turn!", 2.0f, false).AddToQueue();

    }
		
}
