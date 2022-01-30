using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    [SerializeField]
    private GameObject sequencer;

    private void Start()
    {
        BeatEmmiter.OnBeat += () =>
        {
            Debug.Log("Beat!");
        };
        AudioSequencer audioSequencer = sequencer.GetComponent<AudioSequencer>();


        for (int i = 0; i < 8; i++)
        {
            int randomx1 = UnityEngine.Random.Range(0, 8);
            int randomy1 = UnityEngine.Random.Range(0, 8);

            int randomx2 = UnityEngine.Random.Range(0, 8);
            int randomy2 = UnityEngine.Random.Range(0, 8);

            audioSequencer.CreateTileBoardRhythm(randomx1, randomy1);
            
            audioSequencer.CreateTileBoardMelody(randomx2, randomy2);
        }
    }
}
