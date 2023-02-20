using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audio_clips;

    AudioSource bgm_player;
    AudioSource sfx_player;

    public static SoundManager instance;

    public Slider bgm_slider;
    public Slider sfx_slider;


    void Awake()
    {
        instance = this;

        bgm_player = GameObject.Find("BGM Player").GetComponent<AudioSource>();
        sfx_player = GameObject.Find("Sfx Player").GetComponent<AudioSource>();

        bgm_slider = bgm_slider.GetComponent<Slider>();
        sfx_slider = sfx_slider.GetComponent<Slider>();

        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
        sfx_slider.onValueChanged.AddListener(ChangeSfxSound);
    }

    public void PlaySound(string type)
    {
        int index = 0;

        switch (type)
        {
            case "Touch": index = 0; break;
            case "Grow": index = 1; break;
            case "Sell": index = 2; break;
            case "Buy": index = 3; break;
            case "Unlock": index = 4; break;
            case "Fail": index = 5; break;
            case "Button": index = 6; break;
            case "Pause In": index = 7; break;
            case "Pause Out": index = 8; break;
            case "Clear": index = 9; break;
        }

        sfx_player.clip = audio_clips[index];
        sfx_player.Play();
    }

    void ChangeBgmSound(float value)
    {
        bgm_player.volume = value;
    }

    void ChangeSfxSound(float value)
    {
        sfx_player.volume = value;
    }

}
