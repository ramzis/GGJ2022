using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSequencer : MonoBehaviour
{
    [SerializeField]
    private AudioSource sequencerSource;

    [SerializeField]
    private AudioSource player1;

    [SerializeField]
    private AudioSource player2;

    public List<AudioSource> bombSources;
    public List<AudioSource> tileSources;

    [SerializeField]
    private AudioClip ping;
    [SerializeField]
    private AudioClip kick;

    [SerializeField]
    private AudioClip move;
    [SerializeField]
    private AudioClip placeTile;
    [SerializeField]
    private AudioClip placeBomb;
    [SerializeField]
    private AudioClip spawnTile;
    [SerializeField]
    private AudioClip explodeBomb;

    private int beatPosition = 0;
    private int sequenceLength = 16;




    // Start is called before the first frame update
    void Start()
    {
        var bombSources = new List<AudioSource>();
        BeatEmmiter.OnFastBeat += TriggerAudio;
    }

    private void TriggerAudio()
    {
        // Placeholder for soundtrack sequence

        // Trigger Basic kick
        if (beatPosition % 4 == 0)
        {
            sequencerSource.PlayOneShot(kick);
        }

        // Trigger ping 
        if (beatPosition % 2 == 0)
        {
            sequencerSource.PlayOneShot(ping);
        }

        beatPosition = (beatPosition + 1) % sequenceLength;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
