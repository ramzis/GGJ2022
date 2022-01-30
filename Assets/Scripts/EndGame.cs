using Mirror;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            var manager = FindObjectOfType<NetworkManager>();
            if (manager != null)
            {
                if (manager.mode == NetworkManagerMode.Host)
                {
                    manager.StopHost();
                }

                if (manager.mode == NetworkManagerMode.ClientOnly)
                {
                    manager.StopClient();
                }
            }
        }
    }
}
