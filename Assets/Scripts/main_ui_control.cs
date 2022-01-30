using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using Mirror;

public class main_ui_control : MonoBehaviour
{
    public static main_ui_control ins;
    [SerializeField]
    GameObject start_, change_, settings_, credits_;
    public Text nick_holder;
    public InputField ip_holder;
    public Dropdown m_Dropdown;
    [SerializeField]
    string  enemy,ip_address; 
    [SerializeField] private string nick_txt;

    [SerializeField]
    NetworkManager networkManager;
    [SerializeField]
     Mirror.Discovery.NetworkDiscovery ndHud;
     
    public List<string> ip_meniu;
   
    private Scene scene; 
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (PlayerPrefs.HasKey("nick_"))
        {
            nick_holder.text = "nick: " + getNick(); 
            nick_txt= getNick();
        }
        ip_holder.text = GetLocalIPAddress();
        ip_address = GetLocalIPAddress();
        ins = this;
    } 
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
         
    }
    public void addIp(Mirror.Discovery.ServerResponse a )
    {
        Debug.Log(a.EndPoint.ToString());
       // if (ip_meniu.Count == 0)
       // {
            ip_meniu.Add(a.EndPoint.ToString());
      //  }
       // else
       // {
          //  for(int i = 0; i < ip_meniu.Count; i++)
        //   {
        //       if(ip_meniu[i]!= a.EndPoint.ToString())
       //      {
       //           ip_meniu.Add(a.EndPoint.ToString());
       //       }
      //  }
      //  }
   //  m_Dropdown.ClearOptions();
    //  m_Dropdown.AddOptions(ip_meniu);
    }
    public void open_settings()
    {
        settings_.SetActive(true);
    }
    public void setNick(string a)
    {
        
        PlayerPrefs.SetString("nick_", a);
        nick_holder.text = "nick: " + a;
        nick_txt = a;
    }
    public string getNick()
    { 
        return PlayerPrefs.GetString("nick_");
    }
    public void host_game()
    {
        networkManager.networkAddress = ip_address;
        networkManager.StartHost();
    }
    public void setIp(string _ip)
    {
        ip_address = _ip;
    }
    public void JoinGame()
    {
        networkManager.networkAddress = ip_address;
        networkManager.StartClient();
    }
    public void exsit()
    {
        
            Application.Quit();
      
    }
    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("localhost");
    }
    public void leftClient()
    {
        networkManager.StopClient();
    }
    public void leftHost()
    {
        networkManager.StopHost();
    }
}