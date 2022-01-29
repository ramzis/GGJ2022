using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
public class in_game_control : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> ui_setting;
    [SerializeField]
    NetworkManager networkManager;

    private void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Main") return;

        if (ui_setting[1].activeSelf || ui_setting[0].activeSelf) return;
        
        switch (networkManager.mode.ToString())
        {
            case "Host":
                ui_setting[1].SetActive(true);
                Debug.Log(networkManager.mode);
                break;
            case "ClientOnly":
                ui_setting[0].SetActive(true);
                Debug.Log(networkManager.mode);
                break;
            default:
                Debug.Log(networkManager.mode);
                break;
        }
    } 
    public void LeftHost()
    { 
        networkManager.StopHost(); 
    }
    public void LeftClient()
    {
       networkManager.StopClient();
    }
}
