using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class in_game_control : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ui_setting;
    [SerializeField]
    NetworkManager networkManager;
    private Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        //List <GameObject> temp1 = new List<GameObject>(GameObject.Find("NetworkManager"));
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();


    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(networkManager.mode);
        Debug.Log("ping "+ NetworkTime.rtt);
        scene = SceneManager.GetActiveScene();
        if ( scene.name == "Main")
        {
            if (ui_setting[1].activeSelf == false && ui_setting[0].activeSelf == false)
            {
                Debug.Log(networkManager.mode);
                if (networkManager.mode.ToString() == "Host")
                {
                    ui_setting[1].SetActive(true);
                    Debug.Log(networkManager.mode);
                }
                else if (networkManager.mode.ToString() == "ClientOnly")
                {
                    ui_setting[0].SetActive(true);
                    Debug.Log(networkManager.mode);
                }
                else
                {
                    Debug.Log(networkManager.mode);
                }
            }
        }
    } 
    public void leftHost()
    { 
        networkManager.StopHost(); 
        //NetworkServer.Shutdown();
    }
    public void leftClient()
    {
       networkManager.StopClient();
    }
     
}
