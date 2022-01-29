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
        Component[] components = sequencer.GetComponents(typeof(Component));
        foreach (Component component in components)
        {
            Debug.Log(component.ToString());
        }
        AudioSequencer audioSequencer = sequencer.GetComponent<AudioSequencer>();
        Debug.Log(audioSequencer.beatPosition);

        audioSequencer.CreateTileBoardRhythm(0, 0);
        audioSequencer.CreateTileBoardRhythm(4, 4);
        audioSequencer.CreateTileBoardMelody(0, 0);
        audioSequencer.CreateTileBoardMelody(3, 2);
        audioSequencer.CreateTileBoardMelody(4, 6);
        audioSequencer.CreateTileBoardMelody(2, 1);
        audioSequencer.CreateTileBoardMelody(2, 1);
        audioSequencer.CreateTileBoardMelody(3, 4);
    }
}
