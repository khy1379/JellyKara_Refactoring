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
    int CurTypeNum => PlayerPrefs.GetInt("Jelly", 0);
    private void OnEnable()
    {
        SelectJelly(CurTypeNum);
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
        //PlayerTypeChanger.TypeChange((PlayerType)index);
        PlayerPrefs.SetInt("Jelly", index);
        ColorSet(index, notIndex);
        tmp.text = keyGuide + "\n" + guideTexts[index];
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
        if ((PlayerType)CurTypeNum != PlayerType.Slime)
        {
            SelectJelly(0);
        }
    }
    public void SelectJellyBear()
    {
        if ((PlayerType)CurTypeNum != PlayerType.Bear)
        {
            SelectJelly(1);
        }
    }
    public void SelectJellyEarth()
    {
        if ((PlayerType)CurTypeNum != PlayerType.Earth)
        {
            SelectJelly(2);
        }
    }
}
