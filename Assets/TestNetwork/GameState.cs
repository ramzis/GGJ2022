using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameState : NetworkBehaviour
{
    public Player P1, P2;
    int[,,] arena; // 0 - empty, 1 - player, 2 - wall, -2 - bomb, 
    int[,,] arenaTimer; //(1,2,3,4) - ticks till wall, (-1,-2,-3,-4) - tick till bomb;
    public GameObject Player1Pref;
    public GameObject Player2Pref;

    public SpawnBomb Spawner;

    public GameObject Player1Instance, Player2Instance;

    GameObject[,,] SceneObjects;
    // GameObject[,,] SceneObjectsTimer;

    private List<Vector2Int> playerPositions;

    private bool gameReady;

    private enum BlockData : int
    {
        Empty = 0,
        Player = 1,
        Wall = 2,
    }

    private enum PlayerType : int
    {
        Player1,
        Player2
    }

    private enum GameAction : int
    {
        Still,
        Left,
        Up,
        Right,
        Down,
        Wall,
        Bomb
    }

    private void Start()
    {
        gameReady = false;
        playerPositions = new List<Vector2Int>();
        StartCoroutine(WaitForPlayersAndStart());
    }

    private void OnEnable()
    {
        BeatEmmiter.OnBeat += Process;
    }

    private void OnDisable()
    {
        BeatEmmiter.OnBeat -= Process;
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

        playerPositions.Clear();
        playerPositions.Add(new Vector2Int(0, 0));
        playerPositions.Add(new Vector2Int(7, 7));

        SetPlayerPosition(PlayerType.Player1, new Vector2Int(0, 0));
        SetPlayerPosition(PlayerType.Player2, new Vector2Int(7, 7));

        //arena[0, 0, 0] = (int)BlockData.Player;
        //arena[7, 7, 1] = (int)BlockData.Player;

        Player1Instance = Instantiate(Player1Pref, new Vector3(0, 0, 0), Quaternion.identity);
        Player2Instance = Instantiate(Player2Pref, new Vector3(7, 0, 7), Quaternion.Euler(new Vector3(180, 0, 0)));

        gameReady = true;
    }

    private void SetPlayerPosition(PlayerType p, Vector2Int pos)
    {
        playerPositions[(int) p] = pos;
        arena[pos.x, pos.y, (int) p] = (int) BlockData.Player;
    }

    private void ClearPosition(PlayerType p, Vector2Int pos)
    {
        arena[pos.x, pos.y, (int) p] = (int) BlockData.Empty;
    }

    private BlockData CheckPosition(PlayerType p, Vector2Int pos)
    {
        return (BlockData) arena[pos.x, pos.y, (int) p];
    }

    private PlayerType OtherPlayer(PlayerType p)
    {
        return p == PlayerType.Player1 ? PlayerType.Player2 : PlayerType.Player1;
    }

    private void Process()
    {
        if (!gameReady) return;
        //loop arenaTimer and do countdown, then place BlockData.wall on arena
        ProcessTimers();

        ProcessPlayerMove(PlayerType.Player1);
        ProcessPlayerMove(PlayerType.Player2);
    }

    [SerializeField] private Dictionary<PlayerType, Dictionary<GameAction, Vector2Int>> playerToActionDirectionMap =
        new Dictionary<PlayerType, Dictionary<GameAction, Vector2Int>>()
        {
            {
                PlayerType.Player1, new Dictionary<GameAction, Vector2Int>()
                {
                    {GameAction.Left, Vector2Int.right}, // good
                    {GameAction.Right, Vector2Int.left},
                    {GameAction.Up, Vector2Int.up},
                    {GameAction.Down, Vector2Int.down},
                }
            },
            {
                PlayerType.Player2, new Dictionary<GameAction, Vector2Int>()
                {
                    {GameAction.Left, Vector2Int.down},
                    {GameAction.Right, Vector2Int.up},
                    {GameAction.Up, Vector2Int.left},
                    {GameAction.Down, Vector2Int.right},
                }
            },
        };

    private GameAction actionToCheck = 0;
    private Vector2Int dir;

    private void ProcessPlayerMove(PlayerType p)
    {
        actionToCheck = p switch
        {
            PlayerType.Player1 => (GameAction) P1.ActionId,
            PlayerType.Player2 => (GameAction) P2.ActionId,
            _ => actionToCheck
        };

        switch (actionToCheck)
        {
            case GameAction.Still:
                break;
            case GameAction.Left:
            case GameAction.Up:
            case GameAction.Right:
            case GameAction.Down:
                dir = playerToActionDirectionMap[p][actionToCheck];
                if (!CanMoveTo(p, dir)) break;
                switch (p)
                {
                    case PlayerType.Player1:
                        Player1Instance.gameObject.transform.Translate(dir.y, 0, dir.x, Space.World);
                        ClearPosition(p, playerPositions[(int) p]);
                        SetPlayerPosition(PlayerType.Player1, playerPositions[(int) p] + dir);
                        break;
                    case PlayerType.Player2:
                        Player2Instance.gameObject.transform.Translate(dir.y, 0, dir.x, Space.World);
                        ClearPosition(p, playerPositions[(int) p]);
                        SetPlayerPosition(PlayerType.Player2, playerPositions[(int) p] + dir);
                        break;
                }

                break;
            case GameAction.Wall:
                arenaTimer[playerPositions[(int) p].x, playerPositions[(int) p].y, (int) p] = 4;
                arenaTimer[playerPositions[(int) p].x, playerPositions[(int) p].y, (int) OtherPlayer(p)] = -4;
                SceneObjects[playerPositions[(int) p].x, playerPositions[(int) p].y, (int) p] = 
                    Spawner.SpawnBox(new Vector3(playerPositions[(int) p].y, 0, playerPositions[(int) p].x), 2f,p==PlayerType.Player2);
                Spawner.PlantBomb(new Vector3(playerPositions[(int) p].y, 0, playerPositions[(int) p].x), 2f, p==PlayerType.Player1);
                break;
            case GameAction.Bomb:
                arenaTimer[playerPositions[(int) p].x, playerPositions[(int) p].y, (int) p] = -4;
                arenaTimer[playerPositions[(int) p].x, playerPositions[(int) p].y, (int) OtherPlayer(p)] = 4;
                SceneObjects[playerPositions[(int) p].x, playerPositions[(int) p].y, (int) OtherPlayer(p)] = 
                    Spawner.SpawnBox(new Vector3(playerPositions[(int) p].y, 0, playerPositions[(int) p].x), 2f,p==PlayerType.Player1);
                Spawner.PlantBomb(new Vector3(playerPositions[(int) p].y, 0, playerPositions[(int) p].x), 2f, p==PlayerType.Player2);
                break;
            default:
                Debug.Log(actionToCheck);
                throw new ArgumentOutOfRangeException();
        }

        switch (p)
        {
            case PlayerType.Player1:
                P1.ActionId = (int) GameAction.Still;
                break;
            case PlayerType.Player2:
                P2.ActionId = (int) GameAction.Still;
                break;
        }
    }

    private List<(Vector3, Vector3Int)> GetNeighbors(PlayerType p, (Vector3, Vector3Int) pop)
    {
        List<(Vector3, Vector3Int)> neighbors = new List<(Vector3, Vector3Int)>();

        Vector3 nextStep = pop.Item1 + pop.Item2;

        if (nextStep.x >= 0 && nextStep.x < 8 && nextStep.z >= 0 && nextStep.z < 8)
            if (arena[(int) nextStep.z, (int) nextStep.x, (int) p] == (int) BlockData.Empty)
                neighbors.Add((nextStep, pop.Item2));

        return neighbors;
    }

    private (Vector3, Vector3Int) pop;

    private List<Vector3> FindLocationsDFS(PlayerType p, Vector3 start)
    {
        var results = new List<Vector3>();
        var stack = new Stack<(Vector3, Vector3Int)>();
        stack.Push((start, new Vector3Int(1, 0, 0)));
        stack.Push((start, new Vector3Int(-1, 0, 0)));
        stack.Push((start, new Vector3Int(0, 0, -1)));
        stack.Push((start, new Vector3Int(0, 0, 1)));
  

        while (stack.Count > 0)
        {
            pop = stack.Pop();
            var neighbors = GetNeighbors(p, pop);
            if (neighbors.Count > 0)
                foreach (var neighbor in neighbors)
                    stack.Push(neighbor);
            else
            {
                Vector3 nextStep = pop.Item1 + pop.Item2;
                 if (nextStep.x >= 0 && nextStep.x < 8 && nextStep.z >= 0 && nextStep.z < 8)
                                results.Add(pop.Item1+ pop.Item2);
                 else
                 {
                     results.Add(pop.Item1);
                 }
            }
           
        }

        return results;
    }

    private void ProcessTimers()
    {
        for (int z = 0; z < 8; z++)
        for (int x = 0; x < 8; x++)
        for (int i = 0; i < 2; i++)
        {
            if (arenaTimer[z, x, i] > 1)
                arenaTimer[z, x, i]--;
            else if (arenaTimer[z, x, i] < -1)
                arenaTimer[z, x, i]++;

            if (arenaTimer[z, x, i] == 1)
            {
                arenaTimer[z, x, i] = 0;
                arena[z, x, i] = (int) BlockData.Wall;
            }


            if (arenaTimer[z, x, i] == -1)
            {
                arenaTimer[z, x, i] = 0;
                var locations = FindLocationsDFS((PlayerType)i, new Vector3(x, 0, z));

                foreach (var loc in locations)
                {
                    if (arena[(int) loc.z, (int) loc.x, i] == 2)
                    {
                        SceneObjects[(int) loc.z, (int) loc.x, i].GetComponent<BoxHolder>().naikinti();
                        arena[(int) loc.z, (int) loc.x, i] = 0;
                    }

                    if (arena[(int) loc.z, (int) loc.x, i] == 1)
                    {
                        SceneManager.LoadScene(i == 1 ? "P1Win" : "P2Win");
                    }
                }
                
                Spawner.DetonateBomb(new Vector3(x, 0, z), locations,i==(int) PlayerType.Player2);
                arenaTimer[z, x, i] = 0;
            }
        }
    }

    private Vector2Int targetPos;

    private bool CanMoveTo(PlayerType p, Vector2Int dir)
    {
        targetPos = playerPositions[(int) p] + dir;
        if (targetPos.y >= 0 && targetPos.y < 8 && targetPos.x >= 0 && targetPos.x < 8)
        {
            return CheckPosition(p, targetPos) == BlockData.Empty;
        }

        return false;
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
                Debug.Log(arena[z + 1, x, 0]);
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
                if (z > 0 && arena[z - 1, x, 0] != (int) BlockData.Wall)
                {
                    arena[z, x, 0] = 0;
                    arena[z - 1, x, 0] = 1;
                    P1.ActionId = 0;
                    Player1Instance.gameObject.transform.Translate(0, 0, -1, Space.World);
                }
            }

            if (P1.ActionId == 4) // S - go left, -X
            {
                if (x > 0 && arena[z, x - 1, 0] != (int) BlockData.Wall)
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
                SceneObjects[z, x, 0] = Spawner.SpawnBox(new Vector3(x, 0, z), 2f);
                Spawner.PlantBomb(new Vector3(x, 0, z), 2f, true);
                //   Instantiate(WallPref, new Vector3(x, 0, z), Quaternion.Euler(new Vector3()));
                //   Instantiate(BombPref, new Vector3(x, 0, z), Quaternion.Euler(new Vector3()));
                // instantiate wallWait prefab
                // instantiate waitBomb prefab
            }

            if (P1.ActionId == 6) // E Build Bomb
            {
                //    Instantiate(BombPref, new Vector3(x, 0, z), Quaternion.Euler(new Vector3()));
                //    Instantiate(WallPref, new Vector3(x, 0, z), Quaternion.Euler(new Vector3()));

                arenaTimer[z, x, 0] = -4;
                arenaTimer[z, x, 1] = 4;
                SceneObjects[z, x, 0] = Spawner.SpawnBox(new Vector3(x, 0, z), 2f, true);
                Spawner.PlantBomb(new Vector3(x, 0, z), 2f);
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
                    if (x > 0 && arena[z, x - 1, 1] != (int) BlockData.Wall)
                    {
                        Debug.Log("-x");
                        arena[z, x, 1] = 0;
                        arena[z, x - 1, 1] = 1;
                        P2.ActionId = 0;
                        Player2Instance.gameObject.transform.Translate(-1, 0, 0, Space.World);
                    }
                }

                else if (P2.ActionId == 2) // W - go left, -Z
                {
                    if (z > 0 && arena[z - 1, x, 1] != (int) BlockData.Wall)
                    {
                        Debug.Log("-z");
                        arena[z, x, 1] = 0;
                        arena[z - 1, x, 1] = 1;
                        P2.ActionId = 0;
                        Player2Instance.gameObject.transform.Translate(0, 0, -1, Space.World);
                    }
                }

                else if (P2.ActionId == 3) // D - go left, +X
                {
                    if (x < 7 && arena[z, x + 1, 1] != (int) BlockData.Wall)
                    {
                        Debug.Log("+X");
                        arena[z, x, 1] = 0;
                        arena[z, x + 1, 1] = 1;
                        P2.ActionId = 0;
                        Player2Instance.gameObject.transform.Translate(1, 0, 0, Space.World);
                    }
                }

                else if (P2.ActionId == 4) // S - go left, +Z
                {
                    if (z < 7 && arena[z + 1, x, 1] != (int) BlockData.Wall)
                    {
                        Debug.Log("+Z");
                        arena[z, x, 1] = 0;
                        arena[z + 1, x, 1] = 1;
                        P2.ActionId = 0;
                        Player2Instance.gameObject.transform.Translate(0, 0, 1, Space.World);
                    }
                }
                else if (P2.ActionId == 5) // Q Build wall
                {
                    // Debug.Log(Z + " " + X + " " + SceneObjects[Z, X, 0].gameObject.name);
                    arenaTimer[z, x, 0] = -4;
                    arenaTimer[z, x, 1] = 4;

                    SceneObjects[z, x, 1] = Spawner.SpawnBox(new Vector3(x, 0, z), 2f, true);
                    Spawner.PlantBomb(new Vector3(x, 0, z), 2f, false);
                    //Spawner.PlantBomb(new Vector3(z, x, -1), 2f);
                    // instantiate wallWait prefab
                    // instantiate waitBomb prefab
                }

                else if (P2.ActionId == 6) // Q Build bomb
                {
                    arenaTimer[z, x, 0] = 4;
                    arenaTimer[z, x, 1] = -4;
                    SceneObjects[z, x, 1] = Spawner.SpawnBox(new Vector3(x, 0, z), 2f, false);
                    Spawner.PlantBomb(new Vector3(x, 0, z), 2f);
                    //Spawner.PlantBomb(new Vector3(z, x, 0), 2f);
                    // instantiate waitBomb prefab
                    // instantiate wallWait prefab
                }
            }
        }

        // Process timer
        for (int z = 0; z < 8; z++)
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


                    //   SceneObjects[z, x, 1] = Instantiate(WallPref, new Vector3(x, z, 0), Quaternion.identity);
                }


                for (int i = 0; i < 2; i++)
                {
                    if (arenaTimer[z, x, i] > 1)
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