using UnityEngine;
using System.Collections;
using System;

public class ShowMessageCommand : Command {

    string message;
	float duration = 0f;
	bool showButtons;

    /*void Update()
	{
		if (duration > 0f) {
			duration -= Time.deltaTime;

			if (duration < 0f) {
				CommandExecutionComplete ();

				duration = -1f;
			}
		}
	}*/

	public ShowMessageCommand(string message, float duration, bool showButtons)
    {
        this.message = message;
        this.duration = duration;
		this.showButtons = showButtons;

		MessageManager.Instance.ShowMessage(message, duration, showButtons);
    }

    public override void StartCommandExecution()
    {
		//MessageManager.Instance.ShowMessage(message, duration, showButtons);

		//StartCoroutine(WaitForDuration(duration));
    }
}
