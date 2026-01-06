using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class ButtonImageChange
{
    public Image img;
    public Sprite imgOn, imgOff;
    void Awake()
    {
        ButtonImgOn();
    }
    public void ButtonImgOn()
    {
        img.sprite = imgOn;
    }
    public void ButtonImgOff()
    {
        img.sprite = imgOff;
    }
}
public abstract class ButtonEventFrame : MonoBehaviour
{
    public void PointerDown()
    {
        transform.localScale = new Vector3(0.9f, 0.9f, 1);
    }

    public void PointerUp()
    {
        transform.localScale = new Vector3(1, 1, 1);
        EventActive();
    }
    protected abstract void EventActive();
}
