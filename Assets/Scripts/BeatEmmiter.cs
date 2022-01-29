using System;
using System.Collections;
using UnityEngine;

public class BeatEmmiter : MonoBehaviour
{
    public static event Action OnBeat;

    private void Start()
    {
        StartCoroutine(nameof(Beat));
    }

    private IEnumerator Beat() {
        while(true) {
            yield return new WaitForSeconds(1);
            OnBeat?.Invoke();
        }
    }
}
