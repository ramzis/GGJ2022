using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class GameState : NetworkBehaviour
{
    public Player P1, P2;
    int[,,] arena; // 0 - empty, 1 - player, 2 - wall, -2 - bomb, 
    int[,,] arenaTimer; //(1,2,3,4) - ticks till wall, (-1,-2,-3,-4) - tick till bomb;
    public GameObject Player1Pref;
    public GameObject Player2Pref;
    public Camera mainCam;
    public SpawnBomb Spawner;
    public Text P1Name;
    public Text P2Name;

    public GameObject Player1Instance, Player2Instance;

    GameObject[,,] SceneObjects;
    // GameObject[,,] SceneObjectsTimer;

    [SerializeField]
    private AudioSequencer audio;

    private List<Vector2Int> playerPositions;

    private bool gameReady;

    private enum BlockData : int
    {
        Empty = 0,
        Player = 1,
        Wall = 2,
    }

    public enum PlayerType : int
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

        P1Name.text = P1.MyNick;
        P2Name.text = P2.MyNick;

        playerPositions.Clear();
        playerPositions.Add(new Vector2Int(0, 0));
        playerPositions.Add(new Vector2Int(7, 7));

        SetPlayerPosition(PlayerType.Player1, new Vector2Int(0, 0));
        SetPlayerPosition(PlayerType.Player2, new Vector2Int(7, 7));

        Player1Instance = Instantiate(Player1Pref, new Vector3(0, 0, 0), Quaternion.identity);
        Player2Instance = Instantiate(Player2Pref, new Vector3(7, 0, 7), Quaternion.Euler(new Vector3(180, 0, 0)));

        gameReady = true;

        audio.CreateTileBoardRhythm(0, 0);
    }

    private void SetPlayerPosition(PlayerType p, Vector2Int pos)
    {
        playerPositions[(int)p] = pos;
        arena[pos.x, pos.y, (int)p] = (int)BlockData.Player;
    }

    private void ClearPosition(PlayerType p, Vector2Int pos)
    {
        arena[pos.x, pos.y, (int)p] = (int)BlockData.Empty;
    }

    private BlockData CheckPosition(PlayerType p, Vector2Int pos)
    {
        return (BlockData)arena[pos.x, pos.y, (int)p];
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

    [SerializeField]
    private Dictionary<PlayerType, Dictionary<GameAction, Vector2Int>> playerToActionDirectionMap =
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
            PlayerType.Player1 => (GameAction)P1.ActionId,
            PlayerType.Player2 => (GameAction)P2.ActionId,
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
                        ClearPosition(p, playerPositions[(int)p]);
                        SetPlayerPosition(PlayerType.Player1, playerPositions[(int)p] + dir);
                        break;
                    case PlayerType.Player2:
                        Player2Instance.gameObject.transform.Translate(dir.y, 0, dir.x, Space.World);
                        ClearPosition(p, playerPositions[(int)p]);
                        SetPlayerPosition(PlayerType.Player2, playerPositions[(int)p] + dir);
                        break;
                }

                break;
            case GameAction.Wall:
                if (arena[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)OtherPlayer(p)] == (int)BlockData.Wall) break;
                if (arenaTimer[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)OtherPlayer(p)] != 0) break;
                arenaTimer[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)p] = 5;
                arenaTimer[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)OtherPlayer(p)] = -5;
                SceneObjects[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)p] =
                    Spawner.SpawnBox(new Vector3(playerPositions[(int)p].y, 0, playerPositions[(int)p].x), 2f, p == PlayerType.Player2, p);
                Spawner.PlantBomb(new Vector3(playerPositions[(int)p].y, 0, playerPositions[(int)p].x), 2f, p == PlayerType.Player1, OtherPlayer(p));
                audio.CreateTileBoardRhythm(playerPositions[(int)p].y, playerPositions[(int)p].x);
                mainCam.GetComponent<CameraEffects>().Shake(UnityEngine.Random.value * 0.5f);
                break;
            case GameAction.Bomb:
                if (arena[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)OtherPlayer(p)] == (int)BlockData.Wall) break;
                if (arenaTimer[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)OtherPlayer(p)] != 0) break;
                arenaTimer[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)p] = -5;
                arenaTimer[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)OtherPlayer(p)] = 5;
                SceneObjects[playerPositions[(int)p].x, playerPositions[(int)p].y, (int)OtherPlayer(p)] =
                    Spawner.SpawnBox(new Vector3(playerPositions[(int)p].y, 0, playerPositions[(int)p].x), 2f, p == PlayerType.Player1, OtherPlayer(p));
                Spawner.PlantBomb(new Vector3(playerPositions[(int)p].y, 0, playerPositions[(int)p].x), 2f, p == PlayerType.Player2, p);
                StartCoroutine(StartShortMelody(playerPositions[(int)p].y, playerPositions[(int)p].x));
                mainCam.GetComponent<CameraEffects>().Shake(UnityEngine.Random.value * 0.5f);
                break;
            default:
                Debug.Log(actionToCheck);
                throw new ArgumentOutOfRangeException();
        }

        switch (p)
        {
            case PlayerType.Player1:
                P1.ActionId = (int)GameAction.Still;
                break;
            case PlayerType.Player2:
                P2.ActionId = (int)GameAction.Still;
                break;
        }
    }

    private IEnumerator StartShortMelody(int x, int y)
    {
        audio.CreateTileBoardMelody(x, y);
        yield return new WaitForSeconds(5f);
        audio.DestroyTileBoardMelody(x, y);
    }

    private List<(Vector3, Vector3Int)> GetNeighbors(PlayerType p, (Vector3, Vector3Int) pop)
    {
        List<(Vector3, Vector3Int)> neighbors = new List<(Vector3, Vector3Int)>();

        Vector3 nextStep = pop.Item1 + pop.Item2;

        if (nextStep.x >= 0 && nextStep.x < 8 && nextStep.z >= 0 && nextStep.z < 8)
            if (arena[(int)nextStep.z, (int)nextStep.x, (int)p] == (int)BlockData.Empty)
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
                    results.Add(pop.Item1 + pop.Item2);
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
                        arena[z, x, i] = (int)BlockData.Wall;
                    }


                    if (arenaTimer[z, x, i] == -1)
                    {
                        arenaTimer[z, x, i] = 0;
                        var locations = FindLocationsDFS((PlayerType)i, new Vector3(x, 0, z));

                        foreach (var loc in locations)
                        {
                            if (arena[(int)loc.z, (int)loc.x, i] == (int)BlockData.Wall || arena[z, x, i] == (int)BlockData.Wall)
                            {
                                SceneObjects[(int)loc.z, (int)loc.x, i].GetComponent<BoxHolder>().naikinti();
                                arena[(int)loc.z, (int)loc.x, i] = 0;
                            }

                            if (arena[(int)loc.z, (int)loc.x, i] == 1)
                            {
                                gameReady = false;
                                StartCoroutine(EndGame(i));
                            }
                        }

                        Spawner.DetonateBomb(new Vector3(x, 0, z), locations, i == (int)PlayerType.Player2, (PlayerType)i);
                        arenaTimer[z, x, i] = 0;
                    }
                }
    }

    private IEnumerator EndGame(int i)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(i == 1 ? "P1Win" : "P2Win");
    }

    private Vector2Int targetPos;
    private bool CanMoveTo(PlayerType p, Vector2Int dir)
    {
        targetPos = playerPositions[(int)p] + dir;
        if (targetPos.y >= 0 && targetPos.y < 8 && targetPos.x >= 0 && targetPos.x < 8)
        {
            return CheckPosition(p, targetPos) == BlockData.Empty;
        }

        return false;
    }
}