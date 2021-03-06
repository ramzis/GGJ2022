using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBomb : MonoBehaviour
{
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField]
    private GameObject darkBombPrefab;

    [SerializeField]
    private GameObject firePrefab;
    [SerializeField]
    private GameObject darkFirePrefab;
    [SerializeField]
    private GameObject Box;
    [SerializeField]
    private GameObject darkBox;

    private struct FireParameters
    {
        public Vector3 Start;
        // NORTH SOUTH WEST EAST
        public List<Vector3> Ends;
        public bool Flip;
        public GameState.PlayerType PlayerType;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlantBomb(new Vector3(3, 0, 3), 1f, true);
            DetonateBomb(new Vector3(3, 0, 3), new List<Vector3>()
                {
                    new Vector3(3, 0, 8),
                    new Vector3(3, 0, -2),
                    new Vector3(8, 0, 3),
                    new Vector3(-2, 0, 3),
                }, true
            );
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnBox(new Vector3(2, 0, 3), 2f, true);
        }
#endif
    }
    public GameObject SpawnBox(Vector3 location, float speed, bool flip=false, GameState.PlayerType playerType=GameState.PlayerType.Player1)
    {
        var p = playerType == GameState.PlayerType.Player1 ? Box : darkBox;
        var rot = p.transform.rotation.eulerAngles;
        GameObject temp = Instantiate(p, location, Quaternion.Euler(rot.x + (flip ? 180f : 0f), rot.y, rot.z));
            temp.GetComponent<BoxHolder>()
            .AllBoxControl(speed);
        return temp;
    }


    public void PlantBomb(Vector3 location, float timeDelay, bool flip=false, GameState.PlayerType playerType=GameState.PlayerType.Player1)
    {
        var p = playerType == GameState.PlayerType.Player1 ? bombPrefab : darkBombPrefab;
        var rot = p.transform.rotation.eulerAngles;
        Instantiate(p, location, 
                Quaternion.Euler(rot.x + (flip ? 180f : 0f), rot.y, rot.z) 
            )
            .GetComponent<Bomb>()
            .Explode(timeDelay);
    }

    public void DetonateBomb(Vector3 start, List<Vector3> ends, bool flip=false, GameState.PlayerType playerType=GameState.PlayerType.Player1)
    {
        StartCoroutine(nameof(Burn), new FireParameters()
        {
            Start = start,
            Ends = ends,
            Flip = flip,
            PlayerType = playerType
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
        var p = parameters.PlayerType == GameState.PlayerType.Player1 ? firePrefab : darkFirePrefab;
        while (parameters.Ends[0] != north ||
               parameters.Ends[1] != south ||
               parameters.Ends[2] != west ||
               parameters.Ends[3] != east)
        {
            if (parameters.Ends[0] != north)
            {
                north += new Vector3(0, 0, 1);
                var go = Instantiate(p, north, Quaternion.Euler(parameters.Flip?180:0, -90, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }
            if (parameters.Ends[1] != south)
            {
                south += new Vector3(0, 0, -1);
                var go = Instantiate(p, south, Quaternion.Euler(parameters.Flip?180:0, 90, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }
            if (parameters.Ends[2] != west)
            {
                west += new Vector3(-1, 0, 0);
                var go = Instantiate(p, west, Quaternion.Euler(parameters.Flip?180:0, 180, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }
            if (parameters.Ends[3] != east)
            {
                east += new Vector3(1, 0, 0);
                var go = Instantiate(p, east, Quaternion.Euler(parameters.Flip?180:0, 0, 0));
                go.GetComponent<Fire>().Burn(delayBetweenSpawns + overlapBetweenSpawns);
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }
}
