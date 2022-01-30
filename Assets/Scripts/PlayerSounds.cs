using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip move1;
    [SerializeField]
    private AudioClip move2;
    [SerializeField]
    private AudioClip move3;
    [SerializeField]
    private AudioClip move4;
    [SerializeField]
    private AudioClip move5;
    [SerializeField]
    private AudioClip move6;
    [SerializeField]
    private AudioClip move7;
    [SerializeField]
    private AudioClip move8;

    [SerializeField]
    private AudioClip placeBomb;

    [SerializeField]
    private AudioClip placeTile;

    private AudioClip nextTrigger;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        BeatEmmiter.OnBeat += TriggerAudio;
        audioSource = GetComponent<AudioSource>();
    }

    private void TriggerAudio()
    {
        if (nextTrigger != null)
        {
            audioSource.PlayOneShot(nextTrigger);
            nextTrigger = null;
        }
    }


    public void MoveSound(int playerId, int direction)
    {
        if (playerId == 0)
        {
            if (direction == 0)
                nextTrigger = move1;
            if (direction == 1)
                nextTrigger = move2;
            if (direction == 2)
                nextTrigger = move3;
            if (direction == 3)
                nextTrigger = move4;
        }
        if (playerId == 1)
        {
            if (direction == 0)
                nextTrigger = move5;
            if (direction == 1)
                nextTrigger = move6;
            if (direction == 2)
                nextTrigger = move7;
            if (direction == 3)
                nextTrigger = move8;
        }
    }

    public void BombSound()
    {
        nextTrigger = placeBomb;
    }

    public void TileSound()
    {
        nextTrigger = placeTile;
    }

}
