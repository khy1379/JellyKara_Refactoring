using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    Slime,
    Bear,
    Earth,

}
public class PlayerTypeChanger : MonoBehaviour
{
    public static PlayerType pt;
    public static bool isUpMove;
    private void Awake()
    {
        switch (PlayerPrefs.GetInt("Jelly", 0))
        {
            case 1:
                TypeSetBear();
                break;
            case 2:
                TypeSetEarth();
                break;
            case 0:
            default:
                TypeSetSlime();
                break;
        }
    }
    public static void TypeSetSlime()
    {
        /*if (pt != PlayerType.Slime)
            pt = PlayerType.Slime;
        if (PlayerPrefs.GetInt("Jelly", 0) != 0)
            PlayerPrefs.SetInt("Jelly", 0);
        if (!isUpMove)
            isUpMove = false;*/
            PlayerPrefs.SetInt("Jelly", 0);
            pt = PlayerType.Slime;
            isUpMove = false;
    }
    public static void TypeSetBear()
    {
        /*if (pt != PlayerType.Bear)
            pt = PlayerType.Bear;
        if (PlayerPrefs.GetInt("Jelly", 0) != 1)
            PlayerPrefs.SetInt("Jelly", 1);
        if (!isUpMove)
            isUpMove = false;*/ 
            PlayerPrefs.SetInt("Jelly", 1);
            pt = PlayerType.Bear;
            isUpMove = false;
    }
    public static void TypeSetEarth()
    {
        /*if (pt != PlayerType.Earth)
            pt = PlayerType.Earth;
        if (PlayerPrefs.GetInt("Jelly", 0) != 2)
            PlayerPrefs.SetInt("Jelly", 2);
        if (!isUpMove)
            isUpMove = false;*/
            PlayerPrefs.SetInt("Jelly", 2);
            pt = PlayerType.Earth;
            isUpMove = false;
    }
}
