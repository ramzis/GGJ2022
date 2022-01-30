using System.Collections;
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

   private bool gameReady;

   private enum BlockData : int
   {
       Empty = 0,
       Player = 1,
       Wall = 2,
   }
   
    private void Start()
    {
        gameReady = false;
        StartCoroutine(WaitForPlayersAndStart());
    }

    private void OnEnable()
    {
        BeatEmmiter.OnBeat += Beat;
    }

    private void OnDisable()
    {
        BeatEmmiter.OnBeat -= Beat;
        StopCoroutine(nameof(WaitForPlayersAndStart));
    }

    private IEnumerator WaitForPlayersAndStart()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == 2);
        
        StartGame();
    }

    private void StartGame()
    {
        arenaTimer = new int[8, 8, 2]; // timer for spawn: bomb is negative, wall is positive countdown
        arena = new int[8, 8, 2]; // x , y, z - dimension
        SceneObjects = new GameObject[8, 8, 2];

        P1 = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
        P2 = GameObject.FindGameObjectsWithTag("Player")[1].GetComponent<Player>();

        arena[0, 0, 0] = (int) BlockData.Player;
        arena[7, 7, 1] = (int) BlockData.Player;
        
        Player1Instance = Instantiate(Player1Pref, new Vector3(0, 0, 0), Quaternion.identity);
        Player2Instance = Instantiate(Player2Pref, new Vector3(7, 0, 7), Quaternion.Euler(new Vector3(180,0,0)));

        gameReady = true;
    }

    private void Beat()
    {
        if (!gameReady) return;

        // 1. Handle Player 1 Actions
        // 2. Handle Player 2 Actions
        // 
        // Can either move or place wall or bomb
        
        for (int z = 0; z < 8; z++)
            for (int x = 0; x < 8; x++)
        {
            if (arena[z, x, 0] != (int) BlockData.Player) continue;

            if (P1.ActionId == 1) // A - go left, +Z
            {
                if (z < 7 && arena[z + 1, x, 0] != (int) BlockData.Wall)
                {
                    arena[z, x, 0] = 0;
                    arena[z + 1, x, 0] = 1;
                    P1.ActionId = 0;
                    Player1Instance.gameObject.transform.Translate(0, 0, 1, Space.World);
                }
            }

            if (P1.ActionId == 2) // W - go left, +X
            {
                if (x < 7 && arena[z, x + 1, 0] != (int) BlockData.Wall)
                {
                    arena[z, x, 0] = 0;
                    arena[z, x + 1, 0] = 1;
                    P1.ActionId = 0;
                    Player1Instance.gameObject.transform.Translate(1, 0, 0, Space.World);
                }
            }

            if (P1.ActionId == 3) // D - go left, -Z
            {
                if (z > 0 && z - 1 != 2)
                {
                    arena[z, x, 0] = 0;
                    arena[z - 1, x, 0] = 1;
                    P1.ActionId = 0;
                    Player1Instance.gameObject.transform.Translate(0, 0, -1, Space.World);
                }
            }

            if (P1.ActionId == 4 && x - 1 != 2) // S - go left, -X
            {
                if (x > 0)
                {
                    arena[z, x, 0] = 0;
                    arena[z, x - 1, 0] = 1;
                    P1.ActionId = 0;
                    Player1Instance.gameObject.transform.Translate(-1, 0, 0, Space.World);
                }
            }

            if (P1.ActionId == 5) // Q Build wall
            {
                arenaTimer[z, x, 0] = 4;
                arenaTimer[z, x, 1] = -4;

                Instantiate(WallPref, new Vector3(x, 0, z), Quaternion.Euler(new Vector3()));
                Instantiate(BombPref, new Vector3(x, 0, z), Quaternion.Euler(new Vector3()));
                // instantiate wallWait prefab
                // instantiate waitBomb prefab
            }

            if (P1.ActionId == 6) // Q Build wall
            {
                Instantiate(BombPref, new Vector3(x, 0, z), Quaternion.Euler(new Vector3()));
                Instantiate(WallPref, new Vector3(x, 0, z), Quaternion.Euler(new Vector3()));

                arenaTimer[z, x, 0] = -4;
                arenaTimer[z, x, 1] = 4;

                // instantiate waitBomb prefab
                // instantiate wallWait prefab
            }
        }

        for (int z = 0; z < 8; z++)
            for (int x = 0; x < 8; x++)
        {
            if (arena[z, x, 1] == 1) // if player
            {
                if (P2.ActionId == 1) // A - go left, -X
                {
                    if (x > 0)
                    {
                        Debug.Log("-x");
                        arena[z, x, 1] = 0;
                        arena[z, x - 1, 1] = 1;
                        P2.ActionId = 0;
                        Player2Instance.gameObject.transform.Translate(-1, 0, 0, Space.World);


                    }

                }

                if (P2.ActionId == 2) // W - go left, -Z
                {
                    if (z > 0)
                    {
                        Debug.Log("-z");
                        arena[z, x, 1] = 0;
                        arena[z - 1, x, 1] = 1;
                        P2.ActionId = 0;
                        Player2Instance.gameObject.transform.Translate(0, 0, -1, Space.World);


                    }

                }

                if (P2.ActionId == 3) // D - go left, +X
                {
                    if (x < 7)
                    {
                        Debug.Log("+X");
                        arena[z, x, 1] = 0;
                        arena[z, x + 1, 1] = 1;
                        P2.ActionId = 0;
                        Player2Instance.gameObject.transform.Translate(1, 0, 0, Space.World);


                    }
                }

                if (P2.ActionId == 4) // S - go left, +Z
                {
                    if (z < 7)
                    {
                        Debug.Log("+Z");
                        arena[z, x, 1] = 0;
                        arena[z + 1, x, 1] = 1;
                        P2.ActionId = 0;
                        Player2Instance.gameObject.transform.Translate(0, 0, 1, Space.World);


                    }
                }
            }

            if (P2.ActionId == 5) // Q Build wall
            {


                // Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                arenaTimer[z, x, 0] = 4;
                arenaTimer[z, x, 1] = -4;

                // instantiate wallWait prefab
                // instantiate waitBomb prefab


            }

            if (P2.ActionId == 6) // Q Build bomb
            {


                arenaTimer[z, x, 0] = -4;
                arenaTimer[z, x, 1] = 4;

                // instantiate waitBomb prefab
                // instantiate wallWait prefab



            }
        }

        // Process timer
        for(int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                //loop arenaTimer
                if (arenaTimer[z, x, 0] == 1)
                {
                    //Instantiate wall here, if player here - KILL
                    if (arena[z, x, 0] == 1)
                    {
                        //Kill player, end game !i player won
                        // change scene to white win
                    }
                    arena[z, x, 0] = 2;
                }
                if (arenaTimer[z, x, 1] == 1)
                {
                    //Instantiate wall here, if player here - KILL
                    if (arena[z, x, 1] == 1)
                    {
                        // Kill player, end game !i player won
                        // change scene to white win
                    }
                    arena[z, x, 1] = 2;
                    SceneObjects[z, x, 1] = Instantiate(WallPref, new Vector3(x, z, 0), Quaternion.identity);
                }
                for (int i = 0; i <2; i++)
                {
                    if (arenaTimer[z, x, i] == -1)
                    {
                        //Instantiate Bomb here, if player here - KILL
                    }

                    if (arenaTimer[z,x,i]>1)
                    {
                        arenaTimer[z, x, i]--;
                    }
                    if (arenaTimer[z, x, i] < -1)
                    {
                        arenaTimer[z, x, i]++;
                    }
                }
            }
        }
    }
}
