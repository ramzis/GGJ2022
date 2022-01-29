using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnBomb : MonoBehaviour
{
    [SerializeField]
    private GameObject bombPrefab;

    [SerializeField] 
    private GameObject firePrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            var go = Instantiate(bombPrefab, new Vector3(3, 0, 3), bombPrefab.transform.rotation);
            var bomb = go.GetComponent<Bomb>();
            bomb.Explode(1f);
            StartCoroutine(nameof(SetupBomb), 1f);
        }
    }

    private IEnumerator SetupBomb(float time)
    {
        yield return new WaitForSeconds(time);

        yield return Burn(new Vector3(3, 0, 3), new List<Vector3>()
        {
            new Vector3(3,0,8),
            new Vector3(3,0,-2),
            new Vector3(8,0,3),
            new Vector3(-2,0,3),
        });
    }

    private IEnumerator Burn(Vector3 start, List<Vector3> ends)
    {
        var delayBetweenSpawns = 1f;
        var overlapBetweenSpawns = 0.4f;
        
        var north = start;
        var south = start;
        var west = start;
        var east = start;

        // Find longest distance from start to end
        var maxDistance = 0f;
        foreach (var end in ends)
        {
            var d = Vector3.Distance(start, end);
            if (d > maxDistance) maxDistance = d;
        }
        
        // Normalize delay so all spawns happen in 1 second
        delayBetweenSpawns /= maxDistance;

        // Spawn in a circle from the start to all ends
        while (ends[0] != north || ends[1] != south || ends[2] != west || ends[3] != east)
        {
            if(ends[0] != north)
            {
                north += new Vector3(0, 0, 1);
                var go = Instantiate(firePrefab, north, Quaternion.Euler(0, -90, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }
            if(ends[1] != south)
            {
                south += new Vector3(0, 0, -1);
                var go = Instantiate(firePrefab, south, Quaternion.Euler(0, 90, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }
            if(ends[2] != west)
            {
                west += new Vector3(-1, 0, 0);
                var go = Instantiate(firePrefab, west, Quaternion.Euler(0, 180, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }
            if(ends[3] != east)
            {
                east += new Vector3(1, 0, 0);
                var go = Instantiate(firePrefab, east, Quaternion.Euler(0, 0, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
}
