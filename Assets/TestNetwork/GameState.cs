using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameState : NetworkBehaviour
{
    public Player P1, P2;
    int[,,] arena; // 0 - empty, 1 - player, 2 - wall, -2 - bomb, 
    int[,,] arenaTimer; //(1,2,3,4) - ticks till wall, (-1,-2,-3,-4) - tick till bomb;
    public GameObject Player1Pref;
    public GameObject Player2Pref;
    public GameObject BombPref;
    public GameObject BombWaitPref;
    public GameObject WallPref;
    public GameObject WallWaitPref;

    public GameObject Player1Instance,Player2Instance;

    GameObject[,,] SceneObjects;
   // GameObject[,,] SceneObjectsTimer;


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
        arenaTimer = new int[8, 8, 2];
        arena = new int[8, 8, 2]; //x , y, z - dimention;
        SceneObjects = new GameObject[8, 8, 2];
        //SceneObjectsTimer = new GameObject[8, 8, 2];
        for (int i =0;i<8;i++)
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
        Player1Instance = (GameObject)Instantiate(Player1Pref, new Vector3(0, 0, 0), Quaternion.identity);
        Player2Instance = (GameObject)Instantiate(Player2Pref, new Vector3(7, 0, 7), Quaternion.Euler(new Vector3(180,0,0)));
        //Debug.Log(SceneObjects[0, 0, 0].gameObject.name);
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
                            if (Z < 7 && Z+1 !=2)
                            {
                                Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                                arena[Z, X, 0] = 0;
                                arena[Z + 1, X, 0] = 1;
                                P1.ActionId = 0;
                                Player1Instance.gameObject.transform.Translate(0, 0, 1, Space.World);
                  

                            }
                        }
                        if (P1.ActionId == 2) // W - go left, +X
                        {
                            if (X < 7 && X+1 != 2)
                            {
                                Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                                arena[Z, X, 0] = 0;
                                arena[Z, X + 1, 0] = 1;
                                P1.ActionId = 0;
                                Player1Instance.gameObject.transform.Translate(1, 0, 0, Space.World);
                          

                            }
                        }
                        if (P1.ActionId == 3) // D - go left, -Z
                        {
                            if (Z > 0 && Z-1!=2)
                            {
                                Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                                arena[Z, X, 0] = 0;
                                arena[Z - 1, X, 0] = 1;
                                P1.ActionId = 0;
                                Player1Instance.gameObject.transform.Translate(0, 0, -1, Space.World);
                              

                            }
                        }
                        if (P1.ActionId == 4 && X-1 !=2) // S - go left, -X
                        {
                            if (X > 0)
                            {
                                Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                                arena[Z, X, 0] = 0;
                                arena[Z, X - 1, 0] = 1;
                                P1.ActionId = 0;
                                Player1Instance.gameObject.transform.Translate(-1, 0, 0, Space.World);
                              

                            }
                        }
                        if (P1.ActionId == 5) // Q Build wall
                        {
                            
                            
                                // Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                                arenaTimer[Z, X, 0] = 4;
                                arenaTimer[Z, X, 1] = -4;

                            Instantiate(WallPref, new Vector3(X, 0, Z), Quaternion.Euler(new Vector3()));
                            Instantiate(BombPref, new Vector3(X, 0, Z), Quaternion.Euler(new Vector3()));
                            // instantiate wallWait prefab
                            // instantiate waitBomb prefab


                        }
                        if (P1.ActionId == 6) // Q Build wall
                        {

                            
                            Instantiate(BombPref, new Vector3(X, 0, Z), Quaternion.Euler(new Vector3()));
                            Instantiate(WallPref, new Vector3(X, 0, Z), Quaternion.Euler(new Vector3()));

                            arenaTimer[Z, X, 0] = -4;
                                arenaTimer[Z, X, 1] = 4;

                            // instantiate waitBomb prefab
                            // instantiate wallWait prefab



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
                                Player2Instance.gameObject.transform.Translate(-1, 0, 0, Space.World);
                   

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
                                Player2Instance.gameObject.transform.Translate(0, 0, -1, Space.World);
                              

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
                                Player2Instance.gameObject.transform.Translate(1, 0, 0, Space.World);
                               

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
                                Player2Instance.gameObject.transform.Translate(0, 0, 1, Space.World);
                               

                            }
                        }
                    }
                    if (P2.ActionId == 5) // Q Build wall
                    {


                        // Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                        arenaTimer[Z, X, 0] = 4;
                        arenaTimer[Z, X, 1] = -4;

                        // instantiate wallWait prefab
                        // instantiate waitBomb prefab


                    }
                    if (P2.ActionId == 6) // Q Build bomb
                    {


                        arenaTimer[Z, X, 0] = -4;
                        arenaTimer[Z, X, 1] = 4;

                        // instantiate waitBomb prefab
                        // instantiate wallWait prefab



                    }
                }
                }
            }
       //
       //process timer

        for(int Z;Z<8;Z++)
        {
            for (int X; X < 8; X++)
            {
                //loop arenaTimer
                if (arenaTimer[Z, X, 0] == 1)
                {
                    
                    
                        //Instantiate wall here, if player here - KILL
                        if (arena[Z, X, 0] == 1)
                        {
                            //Kill player, end game !i player won
                            // change scene to white win
                        }
                        arena[Z, X, 0] = 2;
                        
                        

                }
                if (arenaTimer[Z, X, 1] == 1)
                {


                    //Instantiate wall here, if player here - KILL
                    if (arena[Z, X, 1] == 1)
                    {
                        //Kill player, end game !i player won
                        // change scene to white win
                    }
                    arena[Z, X, 1] = 2;
                    SceneObjects[Z, X, 1] = Instantiate(WallPref, new Vector3(X, Z, 0), Quaternion.identity);


                }
                for (int i; i <2; i++)
                {

                    if (arenaTimer[Z, X, i] == -1)
                    {
                        //Instantiate Bomb here, if player here - KILL
                    }

                    if (arenaTimer[Z,X,i]>1)
                    {
                        arenaTimer[Z, X, i]--;
                    }
                    if (arenaTimer[Z, X, i] < -1)
                    {
                        arenaTimer[Z, X, i]++;
                    }
                }
            }
        }
        }
    } 

