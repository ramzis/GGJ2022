using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class in_game_control : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ui_setting;
    NetworkManager networkManager;
    // Start is called before the first frame update
    void Start()
    {
        //List <GameObject> temp1 = new List<GameObject>(GameObject.Find("NetworkManager"));
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        Debug.Log(networkManager.mode);
        if(networkManager.mode== "Host") {
            ui_setting[1].SetActive(true);
            Debug.Log(networkManager.mode);
        }
        else if (networkManager.mode == "ClientOnly")
        {
            ui_setting[0].SetActive(true);
            Debug.Log(networkManager.mode);
        }
        else
        {
            Debug.Log(networkManager.mode);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
