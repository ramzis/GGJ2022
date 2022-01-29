using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameState : NetworkBehaviour
{
    public Player P1, P2;
    int[,,] arena; // 0 - empty, 1 - player, 2 - wall, -2 - bomb, (3,4,5,6) - ticks till wall, (-3,-4,-5,-6) - tick till bomb;

    public GameObject Player1Pref;
    public GameObject Player2Pref;
    public GameObject BombPref;
    public GameObject BombWaitPref;
    public GameObject WallPref;
    public GameObject WallWaitPref;

    GameObject[,,] SceneObjects;

    private void Start()
    {
        InvokeRepeating("Update2", 0.5f, 0.1f);
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
        arena = new int[8, 8, 2]; //x , y, z - dimention;
        SceneObjects = new GameObject[8, 8, 2];
        for(int i =0;i<8;i++)
        {
            for (int i1 = 0; i1 < 8; i1++)
            {
                for (int i2 = 0; i2 < 2; i2++)
                {
                    SceneObjects[i, i1, i2] = null;
                }
            }
        }
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

        arena[0, 0, 0] = 1;
        arena[7, 7, 1] = 1;
        SceneObjects[0, 0, 0] = (GameObject)Instantiate(Player1Pref, new Vector3(0, 0, 0), Quaternion.identity);
        SceneObjects[7, 7, 1] = (GameObject)Instantiate(Player2Pref, new Vector3(7, 0, 7), Quaternion.Euler(new Vector3(180,0,0)));
        Debug.Log(SceneObjects[0, 0, 0].gameObject.name);
        StartCoroutine("Beat");
    }




    private IEnumerator Beat()
    {

        while (true)
        {

            yield return new WaitForSeconds(0.5f);

            for (int Z = 0; Z < 8; Z++)
            {
                for (int X = 0; X < 8; X++)
                {
                    if (arena[Z, X, 0] == 1)// if player
                    {
                        if (P1.ActionId == 1) // A - go left, +Z
                        {
                            if (Z < 7)
                            {
                                Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                                arena[Z, X, 0] = 0;
                                arena[Z + 1, X, 0] = 1;
                                P1.ActionId = 0;
                                SceneObjects[Z, X, 0].gameObject.transform.Translate(0, 0, 1, Space.World);
                                SceneObjects[Z + 1, X, 0] = SceneObjects[Z, X, 0];
                                SceneObjects[Z, X, 0] = null;

                            }
                        }
                        if (P1.ActionId == 2) // W - go left, +X
                        {
                            if (X < 7)
                            {
                                Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                                arena[Z, X, 0] = 0;
                                arena[Z, X + 1, 0] = 1;
                                P1.ActionId = 0;
                                SceneObjects[Z, X, 0].gameObject.transform.Translate(1, 0, 0, Space.World);
                                SceneObjects[Z, X + 1, 0] = SceneObjects[Z, X, 0];
                                SceneObjects[Z, X, 0] = null;

                            }
                        }
                        if (P1.ActionId == 3) // D - go left, -Z
                        {
                            if (Z > 0)
                            {
                                Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                                arena[Z, X, 0] = 0;
                                arena[Z - 1, X, 0] = 1;
                                P1.ActionId = 0;
                                SceneObjects[Z, X, 0].gameObject.transform.Translate(0, 0, -1, Space.World);
                                SceneObjects[Z - 1, X, 0] = SceneObjects[Z, X, 0];
                                SceneObjects[Z, X, 0] = null;

                            }
                        }
                        if (P1.ActionId == 4) // S - go left, -X
                        {
                            if (X > 0)
                            {
                                Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                                arena[Z, X, 0] = 0;
                                arena[Z, X - 1, 0] = 1;
                                P1.ActionId = 0;
                                SceneObjects[Z, X, 0].gameObject.transform.Translate(-1, 0, 0, Space.World);
                                SceneObjects[Z, X - 1, 0] = SceneObjects[Z, X, 0];
                                SceneObjects[Z, X, 0] = null;

                            }
                        }
                    }
                }
            }

            for (int Z = 0; Z < 8; Z++)
            {
                for (int X = 0; X < 8; X++)
                {
                    if (arena[Z, X, 1] == 1)// if player
                    {
                        if (P2.ActionId == 1) // A - go left, -X
                        {
                            if (X > 0)
                            {
                                Debug.Log("-x");
                                arena[Z, X, 1] = 0;
                                arena[Z, X - 1, 1] = 1;
                                P2.ActionId = 0;
                                SceneObjects[Z, X, 1].gameObject.transform.Translate(-1, 0, 0, Space.World);
                                SceneObjects[Z, X - 1, 1] = SceneObjects[Z, X, 1];
                                SceneObjects[Z, X, 1] = null;

                            }

                        }
                        if (P2.ActionId == 2) // W - go left, -Z
                        {
                            if (Z > 0)
                            {
                                Debug.Log("-z");
                                arena[Z, X, 1] = 0;
                                arena[Z - 1, X, 1] = 1;
                                P2.ActionId = 0;
                                SceneObjects[Z, X, 1].gameObject.transform.Translate(0, 0, -1, Space.World);
                                SceneObjects[Z - 1, X, 1] = SceneObjects[Z, X, 1];
                                SceneObjects[Z, X, 1] = null;

                            }

                        }
                        if (P2.ActionId == 3) // D - go left, +X
                        {
                            if (X <7 )
                            {
                                Debug.Log("+X");
                                arena[Z, X, 1] = 0;
                                arena[Z, X + 1, 1] = 1;
                                P2.ActionId = 0;
                                SceneObjects[Z, X, 1].gameObject.transform.Translate(1, 0, 0, Space.World);
                                SceneObjects[Z, X + 1, 1] = SceneObjects[Z, X, 1];
                                SceneObjects[Z, X, 1] = null;

                            }
                        }
                        if (P2.ActionId == 4) // S - go left, +Z
                        {
                            if (Z < 7)
                            {
                                Debug.Log("+Z");
                                arena[Z, X, 1] = 0;
                                arena[Z + 1, X, 1] = 1;
                                P2.ActionId = 0;
                                SceneObjects[Z, X, 1].gameObject.transform.Translate(0, 0, 1, Space.World);
                                SceneObjects[Z + 1, X, 1] = SceneObjects[Z, X, 1];
                                SceneObjects[Z, X, 1] = null;

                            }
                        }
                    }
                }
            }
        }
    } 
}

