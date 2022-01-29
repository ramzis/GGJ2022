using System;
using System.Collections;
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

    private IEnumerator Beat() {
        while(true) {
            yield return new WaitForSeconds(beatDelay);
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
