using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : ButtonEventFrame
{
    public ButtonImageChange bic;
    private void Awake()
    {
        if (PlayerPrefs.GetInt("Mute", 0) == 1)
        {
            SoundOn(true);
        }
        else
        {
            SoundOff(true);
        }
    }
    protected override void EventActive()
    {
        if (PlayerPrefs.GetInt("Mute", 0) == 0)
        {
            SoundOn();
        }
        else
        {
            SoundOff();
        }
    }
    void SoundOff(bool isAwake = false)
    {
        AudioListener.volume = 0;
        if (!isAwake)
            PlayerPrefs.SetInt("Mute", 0);
        bic.ButtonImgOff();
    }
    void SoundOn(bool isAwake = false)
    {
        AudioListener.volume = 0.1f;
        if (!isAwake)
            PlayerPrefs.SetInt("Mute", 1);
        bic.ButtonImgOn();
    }
}
