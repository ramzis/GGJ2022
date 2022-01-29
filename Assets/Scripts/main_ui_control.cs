using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// laikinas
/// </summary>
using Mirror;

public class main_ui_control : MonoBehaviour
{
    public static main_ui_control ins;
    [SerializeField]
    GameObject start_, change_, settings_, credits_;
    /// <summary>
    ///  laikini  
    ///  
    /// </summary>
    [SerializeField]
    string nick, enemy,ip_adress;
    [SerializeField]
    NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        ins = this;
    } 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    
    public void open_settings()
    {
        settings_.SetActive(true);
    }

    public void setNick(string a)
    {
        nick = a;
        PlayerPrefs.SetString("nick_", a);
    }
    public string getNick()
    {
        return PlayerPrefs.GetString("nick_");
    }
    public void host_game()
    {
     
    }
    public void setIp(string _ip)
    {
        ip_adress = _ip;
    }
    public void JoinGame()
    {
        networkManager.networkAddress = ip_adress;
        networkManager.StartClient();
    }
    public void exsit()
    {
        
            Application.Quit();
      
    }
}
