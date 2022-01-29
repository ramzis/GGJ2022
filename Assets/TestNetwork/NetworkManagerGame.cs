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

    }
}
