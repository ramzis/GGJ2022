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

        if (numPlayers == 1)
        {
            Debug.Log("As cia");
            gamestate = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
            gamestate.P1 = player.GetComponent<Player>();

        }

        // spawn ball if two players
        if (numPlayers == 2)
        {
            Debug.Log("As cia2");
            gamestate.P2 = player.GetComponent<Player>();
            gamestate.StartGame();

        }
    }
}
