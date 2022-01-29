using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSequencer : MonoBehaviour
{
    [SerializeField]
    private int boardSize;

    [SerializeField]
    private AudioSource sequencerSource;

    [SerializeField]
    private AudioClip rhythm1;
    [SerializeField]
    private AudioClip rhythm2;
    [SerializeField]
    private AudioClip rhythm3;
    [SerializeField]
    private AudioClip rhythm4;
    [SerializeField]
    private AudioClip rhythm5;
    [SerializeField]
    private AudioClip rhythm6;
    [SerializeField]
    private AudioClip rhythm7;
    [SerializeField]
    private AudioClip rhythm8;

    [SerializeField]
    private AudioClip melody1;
    [SerializeField]
    private AudioClip melody2;
    [SerializeField]
    private AudioClip melody3;
    [SerializeField]
    private AudioClip melody4;
    [SerializeField]
    private AudioClip melody5;
    [SerializeField]
    private AudioClip melody6;
    [SerializeField]
    private AudioClip melody7;
    [SerializeField]
    private AudioClip melody8;

    private bool[,] boardRhythm;
    private bool[,] boardMelody;

    public int beatPosition = 0;


    private void Awake()
    {
        boardRhythm = new bool[boardSize, boardSize];
        boardMelody = new bool[boardSize, boardSize];
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Fast beat");
        BeatEmmiter.OnFastBeat += TriggerAudio;
    }

    private void TriggerAudio()
    {

        Debug.Log("Trigger");


        if (boardRhythm[0, beatPosition])
        {
            sequencerSource.PlayOneShot(rhythm1);
        }

        if (boardRhythm[1, beatPosition])
        {
            sequencerSource.PlayOneShot(rhythm2);
        }

        if (boardRhythm[2, beatPosition])
        {
            sequencerSource.PlayOneShot(rhythm3);
        }

        if (boardRhythm[3, beatPosition])
        {
            sequencerSource.PlayOneShot(rhythm4);
        }

        if (boardRhythm[4, beatPosition])
        {
            sequencerSource.PlayOneShot(rhythm5);
        }

        if (boardRhythm[5, beatPosition])
        {
            sequencerSource.PlayOneShot(rhythm6);
        }

        if (boardRhythm[6, beatPosition])
        {
            sequencerSource.PlayOneShot(rhythm7);
        }

        if (boardRhythm[7, beatPosition])
        {
            sequencerSource.PlayOneShot(rhythm8);
        }

        if (boardMelody[0, beatPosition])
        {
            sequencerSource.PlayOneShot(melody1);
        }

        if (boardMelody[1, beatPosition])
        {
            sequencerSource.PlayOneShot(melody2);
        }

        if (boardMelody[2, beatPosition])
        {
            sequencerSource.PlayOneShot(melody3);
        }

        if (boardMelody[3, beatPosition])
        {
            sequencerSource.PlayOneShot(melody4);
        }

        if (boardMelody[4, beatPosition])
        {
            sequencerSource.PlayOneShot(melody5);
        }

        if (boardMelody[5, beatPosition])
        {
            sequencerSource.PlayOneShot(melody6);
        }

        if (boardMelody[6, beatPosition])
        {
            sequencerSource.PlayOneShot(melody7);
        }

        if (boardMelody[7, beatPosition])
        {
            sequencerSource.PlayOneShot(melody8);
        }

        beatPosition = (beatPosition + 1) % boardSize;
    }

    public void CreateTileBoardRhythm(int x, int y)
    {
        boardRhythm[x, y] = true;
    }

    public void CreateTileBoardMelody(int x, int y)
    {
        boardMelody[x, y] = true;
    }

    public void DestroyTileBoardRhythm(int x, int y)
    {
        boardRhythm[x, y] = false;
    }

    public void DestroyTileBoardMelody(int x, int y)
    {
        boardMelody[x, y] = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
