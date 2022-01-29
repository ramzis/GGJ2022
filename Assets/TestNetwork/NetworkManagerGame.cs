using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[AddComponentMenu("")]
public class NetworkManagerGame : NetworkManager
{
    GameState gamestate;
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // add player at correct spawn position
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, player);

        //GameObject gamestate2 = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "GameState"));
        //NetworkServer.Spawn(gamestate2);
        //gamestate = gamestate2.GetComponent<GameState>;

        //if (numPlayers == 1)
        //{
        //    Debug.Log("As cia");
        //    gamestate = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        //    gamestate.P1 = player.GetComponent<Player>();
        //    //gamestate.RpcStartGame();

        //}

        //// spawn ball if two players
        //if (numPlayers == 2)
        //{
        //    Debug.Log("As cia2");
        //    gamestate.P2 = player.GetComponent<Player>();
        //    gamestate.RpcStartGame();

        //}
        
    }
}
