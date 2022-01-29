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
    [SerializeField]
    string  enemy,ip_address;
    [SerializeField]
    NetworkManager networkManager;
    private Scene scene; 
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        if (PlayerPrefs.HasKey("nick_"))
        {
            nick_holder.text = "nick: " + getNick(); 
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
    public void open_settings()
    {
        settings_.SetActive(true);
    }
    public void setNick(string a)
    {
        
        PlayerPrefs.SetString("nick_", a);
        nick_holder.text = "nick: " + a;
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
}