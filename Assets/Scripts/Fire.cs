using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void Burn(float time)
    {
        anim.SetFloat("Speed", 1f/time);
        StartCoroutine(nameof(DestroyMe), time);
    }

    private IEnumerator DestroyMe(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyImmediate(gameObject);
    }
}
