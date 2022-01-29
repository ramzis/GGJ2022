using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_ui_control : MonoBehaviour
{
    public static main_ui_control ins;
    [SerializeField]
    GameObject start_, change_, settings_, credits_;
    /// <summary>
    ///  laikinas nick  
    ///  
    /// </summary>
    [SerializeField]
    string nick, enemy;


    // Start is called before the first frame update
    void Start()
    {
        ins = this;
    } 
    // Update is called once per frame
    void Update()
    {
        
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
}
