using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    GameState gamestate;
    bool playerID = false;//false = host, true = client.

    [Client]
    private void Start()
    {
        if (!hasAuthority) return;
        gamestate = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        
        if(gameObject.name== "Player [connId=0]")
        {
            playerID = false;
        }
        else
        {
            playerID = true;
        }
    }
    [Client]
    void Update()
    {
        if (!hasAuthority) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerAction(1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerAction(2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerAction(3);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerAction(2);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerAction(3);
        }
    }

    [Command]
    void PlayerAction(int a)
    {
        DoAction(a);
    }

    [ClientRpc]

    private void DoAction(int a)
    {
        if (playerID) // client p2
        {
            gamestate.P2Action = a;

        }
        else // host p1
        {
            gamestate.P1Action = a;
        }
    }
}
