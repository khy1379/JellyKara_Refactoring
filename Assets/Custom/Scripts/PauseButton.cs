using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : ButtonEventFrame
{
    public GameManager gm;
    public ButtonImageChange bic;
    static bool isPause;
    private void OnEnable()
    {
        gm.GameResume();
        isPause = false;
    }
    protected override void EventActive()
    {
        if (!isPause)
        {
            gm.GamePause();
            isPause = true;
        }
        else
        {
            gm.GameResume();
            isPause = false;
        }
    }
}
