using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameState : NetworkBehaviour
{
    public Player P1, P2;

    private void Start()
    {
        InvokeRepeating("Update2", 0.5f, 0.5f);
    }
    private void Update2()
    {
        if(GameObject.FindGameObjectsWithTag("Player").Length == 2)
        {
            CancelInvoke("Update2");
            RpcStartGame();
        }   
    }

    public void RpcStartGame()
    {
        Debug.Log("As cia3");
        if(GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<NetworkIdentity>().netId == 1)
        {
            P1 = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
            P2 = GameObject.FindGameObjectsWithTag("Player")[1].GetComponent<Player>();
        }
        else
        {
            P2 = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
            P1 = GameObject.FindGameObjectsWithTag("Player")[1].GetComponent<Player>();
        }
        InvokeRepeating("Beat", 0.5f, 0.5f);
        
    }




    void Beat()
    {
        Debug.Log(P1.ActionId + " " + P2.ActionId);
    }
}
