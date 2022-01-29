using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public Player P1, P2;

    

    public void StartGame()
    {
        Debug.Log("As cia3");
        InvokeRepeating("Beat", 1, 1);
    }

    void Beat()
    {
        Debug.Log(P1.ActionId + " " + P2.ActionId);
    }
}
