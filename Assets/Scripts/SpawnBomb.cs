using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBomb : MonoBehaviour
{
    [SerializeField]
    private GameObject bombPrefab;

    [SerializeField] 
    private GameObject firePrefab;

    private struct FireParameters
    {
        public Vector3 Start;
        // NORTH SOUTH WEST EAST
        public List<Vector3> Ends;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlantBomb(new Vector3(3, 0, 3), 1f);
            DetonateBomb(new Vector3(3, 0, 3), new List<Vector3>()
                {
                    new Vector3(3, 0, 8),
                    new Vector3(3, 0, -2),
                    new Vector3(8, 0, 3),
                    new Vector3(-2, 0, 3),
                }
            );
        }
    }

    public void PlantBomb(Vector3 location, float timeDelay)
    {
        Instantiate(bombPrefab, new Vector3(3, 0, 3), bombPrefab.transform.rotation)
            .GetComponent<Bomb>()
            .Explode(timeDelay);
    }

    public void DetonateBomb(Vector3 start, List<Vector3> ends)
    {
        StartCoroutine(nameof(Burn), new FireParameters()
        {
            Start = start,
            Ends = ends
        });
    }

    private IEnumerator Burn(FireParameters parameters)
    {
        var delayBetweenSpawns = 1f;
        var overlapBetweenSpawns = 0.3f;
        
        var north = parameters.Start;
        var south = parameters.Start;
        var west = parameters.Start;
        var east = parameters.Start;

        // Find longest distance from start to end
        var maxDistance = 0f;
        foreach (var end in parameters.Ends)
        {
            var d = Vector3.Distance(parameters.Start, end);
            if (d > maxDistance) maxDistance = d;
        }
        
        // Normalize delay so all spawns happen in 1 second
        delayBetweenSpawns /= maxDistance;

        // Spawn in a circle from the start to all ends
        while (parameters.Ends[0] != north || 
               parameters.Ends[1] != south || 
               parameters.Ends[2] != west || 
               parameters.Ends[3] != east)
        {
            if(parameters.Ends[0] != north)
            {
                north += new Vector3(0, 0, 1);
                var go = Instantiate(firePrefab, north, Quaternion.Euler(0, -90, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }
            if(parameters.Ends[1] != south)
            {
                south += new Vector3(0, 0, -1);
                var go = Instantiate(firePrefab, south, Quaternion.Euler(0, 90, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }
            if(parameters.Ends[2] != west)
            {
                west += new Vector3(-1, 0, 0);
                var go = Instantiate(firePrefab, west, Quaternion.Euler(0, 180, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }
            if(parameters.Ends[3] != east)
            {
                east += new Vector3(1, 0, 0);
                var go = Instantiate(firePrefab, east, Quaternion.Euler(0, 0, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
}
