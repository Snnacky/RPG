using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle healthBarToggle;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mixerMultiplier = 25;

    [Header("BGM Volume Settings")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private string bgmParameter;

    [Header("SFX Volume Settings")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private string sfxParameter;

    

    private void Start()
    {
        player=FindFirstObjectByType<Player>();

        healthBarToggle.onValueChanged.AddListener(OnHealthBarToggleChanged);
    }

    public void BGMSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmParameter, newValue);
    }

    public void SFXSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxParameter, newValue);
    }

    public void OnHealthBarToggleChanged(bool isOn)
    {   if (player == null) player = Player.instance;
        if(player)
        {
            player.health.EnableHealthBar(isOn);
        }
    }

    public void GoMainMenuBTN()=>GameManager.Instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);

    private void OnEnable()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .6f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .6f);
        healthBarToggle.isOn = PlayerPrefs.GetInt("HealthBarEnabled", 1) == 1;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
        PlayerPrefs.SetInt("HealthBarEnabled", healthBarToggle.isOn ? 1 : 0);
    }

    public void LoadUpVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .6f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, .6f);
    }
}
