using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{

    public int ActionId = 0;

    [Client]
    void Start()
    {
        if (!hasAuthority) return;
        Debug.Log(GetComponent<NetworkIdentity>().netId);
        if (GetComponent<NetworkIdentity>().netId == 2)
        {
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(0, 2.9f, 0);
            GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles = new Vector3(30, 45, 0);
        }
        else
        {

            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(7, -2.9f, 7);
            GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles = new Vector3(-30, -135, 180);
            
        }
    }

    [Client]
    void Update()
    {
        if (!hasAuthority) return;
        if (Input.GetKeyDown(KeyCode.Q)) // build wall
        {
            CmdAction(5);
        }
        else if (Input.GetKeyDown(KeyCode.E)) // build bomb
        {
            CmdAction(6);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            CmdAction(1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CmdAction(3);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CmdAction(4);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            CmdAction(2);
        }
        
    }
    [Command]
    void CmdAction(int a)
    {
        RpcAction(a);
    }
    [ClientRpc]
    void RpcAction(int a)
    {
        ActionId = a;
    }

}
