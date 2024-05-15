using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private TMP_Dropdown qualityDropdown;

    [SerializeField]
    private Toggle fullscreenToggle;

    [SerializeField]
    private Slider AimSensitivitySlider;

    private bool isFullscreen = false;
    
    public void setResolution()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
        
        if(PlayerPrefs.GetInt("ResolutionIndex",100) != 100)
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex");
        }
        if (resolutionDropdown.value == 3)
        {
            Screen.SetResolution(720,480, isFullscreen);
        }
        if (resolutionDropdown.value == 2)
        {
            Screen.SetResolution(720, 576, isFullscreen);
        }
        if (resolutionDropdown.value == 1)
        {
            Screen.SetResolution(1280, 720, isFullscreen);
        }
        if (resolutionDropdown.value == 0)
        {
            Screen.SetResolution(1920, 1080, isFullscreen);
        }

    }

    public void GetSettings()
    {
        //----------------   RESOLUTION  --------------------
        int ResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex");
        
        if (ResolutionIndex == 3)
        {
            Screen.SetResolution(720, 480, isFullscreen);
        }
        if (ResolutionIndex == 2)
        {
            Screen.SetResolution(720, 576, isFullscreen);
        }
        if (ResolutionIndex == 1)
        {
            Screen.SetResolution(1280, 720, isFullscreen);
        }
        if (ResolutionIndex == 0)
        {
            Screen.SetResolution(1920, 1080, isFullscreen);
        }

        //----------------   QUALITY  --------------------
        int QualityIndex = PlayerPrefs.GetInt("QualityIndex");
        if (QualityIndex == 0)
        {
            QualitySettings.SetQualityLevel(0,true);
        }
        if (QualityIndex == 1)
        {
            QualitySettings.SetQualityLevel(1, true);
        }
        if (QualityIndex == 2)
        {
            QualitySettings.SetQualityLevel(2, true);
        }
        if (QualityIndex == 3)
        {
            QualitySettings.SetQualityLevel(3, true);
        }
        if (QualityIndex == 4)
        {
            QualitySettings.SetQualityLevel(4, true);
        }
        if (QualityIndex == 5)
        {
            QualitySettings.SetQualityLevel(5, true);
        }


    }

    public void SetAimSensitivity()
    {
        PlayerPrefs.SetFloat("AimSensitivity", AimSensitivitySlider.value);
    }
    public void LoadSettings()
    {
        //----------------   RESOLUTION  --------------------
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex");



        //----------------   QUALITY  --------------------
        qualityDropdown.value = PlayerPrefs.GetInt("QualityIndex");
        


    }
    public void SetFullscreenMode()
    {
        isFullscreen = fullscreenToggle.isOn;
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality()
    {
        PlayerPrefs.SetInt("QualityIndex", qualityDropdown.value);
        if (qualityDropdown.value == 0)
        {
            QualitySettings.SetQualityLevel(0,true);
        }
        if (qualityDropdown.value == 1)
        {
            QualitySettings.SetQualityLevel(1, true);
        }
        if (qualityDropdown.value == 2)
        {
            QualitySettings.SetQualityLevel(2, true);
        }
        if (qualityDropdown.value == 3)
        {
            QualitySettings.SetQualityLevel(3, true);
        }
        if (qualityDropdown.value == 4)
        {
            QualitySettings.SetQualityLevel(4, true);
        }
        if (qualityDropdown.value == 5)
        {
            QualitySettings.SetQualityLevel(5, true);
        }
    }
}
