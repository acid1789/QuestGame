using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour, IEventSystemHandler
{

	public float TimeForOneTurn;
    public Text TimerText;

    private float timeTillZero;
    private bool counting = false;

    [SerializeField]
    public UnityEvent TimerExpired = new UnityEvent();

    public void StartTimer()
	{
        timeTillZero = TimeForOneTurn;
		counting = true;

	} 

	public void StopTimer()
	{
		counting = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (counting) 
		{
			timeTillZero -= Time.deltaTime;
            if (TimerText != null)
                TimerText.text = ToString();

            // check for TimeExpired
			if(timeTillZero <= 0)
			{
				counting = false;
                TimerExpired.Invoke();
			}
		}
	
	}

	public override string ToString ()
	{
		int inSeconds = Mathf.RoundToInt (timeTillZero);
		string justSeconds = (inSeconds % 60).ToString ();

		if (justSeconds.Length == 1)
			justSeconds = "0" + justSeconds;

		string justMinutes = (inSeconds / 60).ToString ();

		if (justMinutes.Length == 1)
			justMinutes = "0" + justMinutes;

		return string.Format ("{0}:{1}", justMinutes, justSeconds);
	}
}
