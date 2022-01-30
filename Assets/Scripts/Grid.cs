using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private GameObject tile;
    [SerializeField]
    private int size;

    private GameObject[,] tiles;

    [Range(0,1)]
    public float amplitude;
    
    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        var parentTile = new GameObject();
        parentTile.name = "Tiles";
        tiles = new GameObject[8, 8];
        for(int y=0; y<size; y++)
            for(int x=0; x<size; x++)
            {
                GameObject go = Instantiate(tile, new Vector3(x, 0, y), Quaternion.identity, parentTile.transform);
                tiles[y,x] = go;
            }
    }

    private void FixedUpdate()
    {
        for(int y=0; y<size; y++)
            for (int x = 0; x < size; x++)
            {
                tiles[y, x].transform.position =
                    new Vector3(x, (Mathf.Cos(x * Time.time) + Mathf.Sin(y * Time.time)) * amplitude, y);
            }
    }
}
