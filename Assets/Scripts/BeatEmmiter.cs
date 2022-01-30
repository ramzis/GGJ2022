using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class BeatEmmiter : MonoBehaviour
{
    private int beatsPerMinute = 110;
    private float beatDelay;

    public static event Action OnBeat;
    public static event Action OnFastBeat;

    private void Start()
    {
        beatDelay = 60f / beatsPerMinute;
        StartCoroutine(nameof(Beat));
        StartCoroutine(nameof(FastBeat));
    }

    private double netTime, wait;
    private IEnumerator Beat()
    {
        while (true)
        {
            netTime = NetworkTime.time;
            wait = Math.Ceiling(NetworkTime.time * 2) / 2 - netTime;
            yield return new WaitForSeconds((float) wait);
            //Debug.Log(NetworkTime.time);
            OnBeat?.Invoke();
        }
    }

    private IEnumerator FastBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(beatDelay/4);
            OnFastBeat?.Invoke();
        }
    }
}
