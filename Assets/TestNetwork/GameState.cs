using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int P1Action = 0;
    public int P2Action = 0;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Beat", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Beat()
    {
        Debug.Log(P1Action + " " + P2Action);
    }
}
