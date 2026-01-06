using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadButton : ButtonEventFrame
{
    public string levelName;
    protected override void EventActive()
    {
        SceneManager.LoadScene(levelName);
    }
}
