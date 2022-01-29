using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private GameObject tile;
    [SerializeField]
    private int size;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for(int y=0; y<size; y++)
            for(int x=0; x<size; x++)
            {
                GameObject go = Instantiate(tile);
                tile.transform.position = new Vector3(x, 0, y);
            }
    }
}
