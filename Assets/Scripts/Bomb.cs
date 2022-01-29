using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    
    public void Explode(float time)
    {
        // The animation is 10 frames/second.
        // The speed of the animation clip is set to 10, which means it runs in 1 second.
        // We scale it by the given time to make it longer.
        anim.SetFloat("Speed", 1f/time);
        Destroy(gameObject, time);
    }
}
