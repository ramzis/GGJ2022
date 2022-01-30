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

        boardRhythm[0, 0] = true;
        boardRhythm[4, 4] = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        BeatEmmiter.OnFastBeat += TriggerAudio;
    }

    private void TriggerAudio()
    {
        if (sequencerSource == null)
            return;
        if (rhythm1 == null || rhythm2 == null || rhythm3 == null || rhythm4 == null || rhythm5 == null || rhythm6 == null || rhythm7 == null || rhythm8 == null ||
            melody1 == null || melody2 == null || melody3 == null || melody4 == null || melody5 == null || melody6 == null || melody7 == null || melody8 == null) return;


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
            sequencerSource.PlayOneShot(melody1, 0.5f);
        }

        if (boardMelody[1, beatPosition])
        {
            sequencerSource.PlayOneShot(melody2, 0.5f);
        }

        if (boardMelody[2, beatPosition])
        {
            sequencerSource.PlayOneShot(melody3, 0.5f);
        }

        if (boardMelody[3, beatPosition])
        {
            sequencerSource.PlayOneShot(melody4, 0.5f);
        }

        if (boardMelody[4, beatPosition])
        {
            sequencerSource.PlayOneShot(melody5, 0.5f);
        }

        if (boardMelody[5, beatPosition])
        {
            sequencerSource.PlayOneShot(melody6, 0.5f);
        }

        if (boardMelody[6, beatPosition])
        {
            sequencerSource.PlayOneShot(melody7, 0.5f);
        }

        if (boardMelody[7, beatPosition])
        {
            sequencerSource.PlayOneShot(melody8, 0.5f);
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
