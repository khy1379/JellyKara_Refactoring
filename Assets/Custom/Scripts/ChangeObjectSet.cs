using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObjectSet : MonoBehaviour
{
    public GameObject[] changeJellys;
    private void OnEnable()
    {
        ChangeColliderSet();
    }
    void ChangeColliderSet()
    {
        int[] targetJellyNum = new int[2];
        switch ((PlayerType)PlayerPrefs.GetInt("Jelly", 0))
        {
            case PlayerType.Slime:
                targetJellyNum[0] = 1;
                targetJellyNum[1] = 2;
                break;
            case PlayerType.Bear:
                targetJellyNum[0] = 0;
                targetJellyNum[1] = 2;
                break;
            case PlayerType.Earth:
                targetJellyNum[0] = 0;
                targetJellyNum[1] = 1;
                break;
        }
        int index = Random.Range(0, 2);
        changeJellys[targetJellyNum[index]].SetActive(true);
    }
}
