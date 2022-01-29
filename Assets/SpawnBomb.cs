using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBomb : MonoBehaviour
{
    [SerializeField]
    private GameObject bombPrefab;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            var go = Instantiate(bombPrefab);
            var bomb = go.GetComponent<Bomb>();
            bomb.Explode(1f);
        }
    }
}
