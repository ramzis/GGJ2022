using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameState : NetworkBehaviour
{
    public Player P1, P2;
    int[,,] arena; // 0 - empty, 1 - player, 2 - wall, -2 - bomb, (3,4,5,6) - ticks till wall, (-3,-4,-5,-6) - tick till bomb;

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

            arena = new int[8, 8, 2]; //x , y, z - dimention;
            


        }   
    }

    public void RpcStartGame()
    {
        Debug.Log("As cia3");
        //if (GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<NetworkIdentity>().netId == 1)
        //{
            P1 = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
            P2 = GameObject.FindGameObjectsWithTag("Player")[1].GetComponent<Player>();
        //}

        //else
        //{
        //    P2 = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        //    P1 = GameObject.FindGameObjectsWithTag("Player")[1].GetComponent<Player>();
        //}

        StartCoroutine("Beat");
    }




    private IEnumerator Beat()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);


        } 
    }
}
