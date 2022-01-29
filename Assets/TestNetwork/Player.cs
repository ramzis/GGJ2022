using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    GameState gamestate;
    bool playerID = false;//false = host, true = client.
    int ActionId = 0;
    [Client]
    private void Start()
    {
        if (!hasAuthority) return;
        gamestate = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        
        if(GetComponent<NetworkIdentity>().netId==1)
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
            CmdPlayerAction(0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CmdPlayerAction(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            CmdPlayerAction(3);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            CmdPlayerAction(4);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CmdPlayerAction(2);
        }
    }

    [Command]
    void CmdPlayerAction(int a)
    {
        RpcDoAction(a);

    }

    [ClientRpc]

    private void RpcDoAction(int a)
    {
        if(playerID)
        {
            gamestate.P2Action = a;
        }
        else
        {
            gamestate.P1Action = a;
        }
        //ActionId = a;
    }
}
