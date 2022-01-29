using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    private void Start()
    {
        BeatEmmiter.OnBeat += () =>
        {
            Debug.Log("Beat!");
        };
    }
}
