using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class game_settings : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Slider mSlider;
    public Dropdown m_Dropdown,m_Dropdown_2; 
    Resolution[] resolutions; 
    public void SetVolume(float volumea)
    {
        audioMixer.SetFloat("volume",volumea); PlayerPrefs.SetFloat("volume_lvl", volumea);
     
    }
    public void out_setting(){
        check_save_setting();
       
    }
    public void setQuality(int id){
        QualitySettings.SetQualityLevel(id);
        PlayerPrefs.SetInt("gpu_lvl", id);
    }
    public void setResuliution(int id){
        Resolution resolution= resolutions[id];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("rez_lvl",id);
    }
    
    void Start()
    {
        check_save_setting();
    }
    public void check_save_setting()
    {
        float temp_audio;
        if (PlayerPrefs.HasKey("gpu_lvl"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("gpu_lvl"));
            m_Dropdown.value = PlayerPrefs.GetInt("gpu_lvl");
        }
        else
        {
            int qualityLevel = QualitySettings.GetQualityLevel();
            PlayerPrefs.SetInt("gpu_lvl", qualityLevel);
            m_Dropdown.value = qualityLevel;
        }

        if (PlayerPrefs.HasKey("volume_lvl"))
        {

            temp_audio = PlayerPrefs.GetFloat("volume_lvl");
            SetVolume(temp_audio); mSlider.value = temp_audio;
        }
        else
        {
            temp_audio = 0f;
            SetVolume(temp_audio); mSlider.value = temp_audio;
        }
        mSlider.value = temp_audio;
        if (PlayerPrefs.HasKey("rez_lvl"))
        {
            resolutions = Screen.resolutions;
            m_Dropdown_2.ClearOptions();
            List<string> options = new List<string>();
            int currentresolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);


            }
            m_Dropdown_2.AddOptions(options);
            setResuliution(PlayerPrefs.GetInt("rez_lvl"));
            m_Dropdown_2.value = PlayerPrefs.GetInt("rez_lvl");
        }
        else
        {
            resolutions = Screen.resolutions;
            m_Dropdown_2.ClearOptions();
            List<string> options = new List<string>();
            int currentresolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentresolutionIndex = i;
                }

            }
            m_Dropdown_2.AddOptions(options);
            m_Dropdown_2.value = currentresolutionIndex;
            PlayerPrefs.SetInt("rez_lvl", currentresolutionIndex);
        }
        

    }
}
