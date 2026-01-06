using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JellySelect : MonoBehaviour
{
    public Image[] imgArr;
    public Image showImg;
    public Image curImg;
    public TextMeshProUGUI tmp;
    public string keyGuide;
    public string[] guideTexts;
    public Material grayMat;
    private void OnEnable()
    {
        switch (PlayerTypeChanger.pt)
        {
            case PlayerType.Slime:
                SelectJelly(0);
                break;
            case PlayerType.Bear:
                SelectJelly(1);
                break;
            case PlayerType.Earth:
                SelectJelly(2);
                break;
        }

    }
    void SelectJelly(int index)
    {
        curImg = imgArr[index];
        showImg.sprite = imgArr[index].sprite;
        int[] notIndex = new int[2];
        switch (index)
        {
            case 0:
                notIndex[0] = 1;
                notIndex[1] = 2;
                PlayerTypeChanger.TypeSetSlime();
                break;
            case 1:
                notIndex[0] = 0;
                notIndex[1] = 2;
                    PlayerTypeChanger.TypeSetBear();
                break;
            case 2:
                notIndex[0] = 0;
                notIndex[1] = 1;
                    PlayerTypeChanger.TypeSetEarth();
                break;
        }
        ColorSet(index, notIndex);
        tmp.text = keyGuide + "\n" + guideTexts[index];
    }
    bool IsImageChangeCheck(int index)
    {
        if(curImg == imgArr[index])
        {
            Debug.Log("같은 이미지로 변경 불가");
            return false;
        }

        Image temp = curImg;

        curImg = imgArr[index];
        if(curImg == temp)
        {
            Debug.Log("Button 대상 변경 안 됨");
            return false;
        }

        showImg.sprite = imgArr[index].sprite;
        if (showImg.sprite == temp.sprite)
        {
            Debug.Log("Show 이미지 변경 안 됨");
            return false;
        }

        int[] notIndex = new int[2];
        switch (index)
        {
            case 0:
                notIndex[0] = 1;
                notIndex[1] = 2;
                break;
            case 1:
                notIndex[0] = 0;
                notIndex[1] = 2;
                break;
            case 2:
                notIndex[0] = 0;
                notIndex[1] = 1;
                break;
        }
        Image[] tempImgs = new Image[3];
        tempImgs[2] = curImg;
        for(int i = 0; i < 2; i++)
        {
            tempImgs[i] = imgArr[notIndex[i]];
        }
        ColorSet(index, notIndex);

        if (tempImgs[2].material == curImg.material)
        {
            Debug.Log("선택된 이미지의 material 변경 안 됨");
            return false;
        }
        for (int i = 0; i < 2; i++)
        {
            if (tempImgs[i].material == imgArr[notIndex[i]].material)
            {
                Debug.Log($"{i}번째 이미지의 material 변경 안 됨");
                return false;
            }
        }
        Debug.Log("이미지 변경 및 material 변경 완료");
        return true;
    }
    void ColorSet(int index, int[] notInd)
    {
        ColorReset(index);
        foreach(int i in notInd)
        {
            ColorGraySet(i);
        }
    }
    void ColorGraySet(int index)
    {
        if (imgArr[index].material != grayMat)
            imgArr[index].material = grayMat;
        //else Debug.Log($"이미 {index}번째 이미지는 회색 material");
    }
    void ColorReset(int index)
    {
        if (imgArr[index].material == grayMat)
            imgArr[index].material = null;
        //else Debug.Log($"이미 {index}번째 이미지는 기존 material");
    }
    public void SelectJellySlime()
    {
        if (PlayerTypeChanger.pt != PlayerType.Slime)
        {
            SelectJelly(0);
            //if(IsImageChangeCheck(0)) PlayerTypeChanger.TypeSetSlime();
        }
    }
    public void SelectJellyBear()
    {
        if (PlayerTypeChanger.pt != PlayerType.Bear)
        {
            SelectJelly(1);
            //if (IsImageChangeCheck(1)) PlayerTypeChanger.TypeSetBear();
        }
    }
    public void SelectJellyEarth()
    {
        if (PlayerTypeChanger.pt != PlayerType.Earth)
        {
            SelectJelly(2);
            //if (IsImageChangeCheck(2)) PlayerTypeChanger.TypeSetEarth();
        }
    }
}
