using UnityEngine;

public class MainSceneControl : MonoBehaviour
{
    public GameObject jellySelectUI;
    public SpriteRenderer mainJelly;
    public Sprite[] jellys;
    int CurTypeNum => PlayerPrefs.GetInt("Jelly", 0);
    public void Start()
    {
        MainJellyChange();
    }
    private void FixedUpdate()
    {
        SpriteScaleSet();
    }
    public void ShowJellySelectUI()
    {
        if(!jellySelectUI.activeSelf)
            jellySelectUI.SetActive(true);
    }
    public void CloseJellySelectUI()
    {
        if (jellySelectUI.activeSelf)
        {
            jellySelectUI.SetActive(false);
            MainJellyChange();
        }
    }
    void MainJellyChange()
    {
        mainJelly.sprite = jellys[CurTypeNum];
    }
    void SpriteScaleSet()
    {
        int index = CurTypeNum;
        // Sprite의 픽셀 크기 (Rect)
        float spritePixelWidth = jellys[index].rect.width;
        float spritePixelHeight = jellys[index].rect.height;

        // Sprite의 Pixels Per Unit (PPU)
        float ppu = jellys[index].pixelsPerUnit;

        // 3. Sprite의 현재 월드 유닛 크기 계산
        float spriteWorldWidth = spritePixelWidth / ppu;
        float spriteWorldHeight = spritePixelHeight / ppu;

        // 4. 필요한 X, Y 스케일 배율 계산 (새로운 스케일 / 현재 스케일)
        // 현재 스케일은 이미 부모 Transform에 적용되어 있으므로, 
        // 최종적으로 원하는 월드 크기(colliderWorldSize)를 현재 월드 유닛 크기(spriteWorldWidth/Height)로 나눠서
        // localScale 값을 직접 계산하여 대입합니다.

        float scaleX = 16 / spriteWorldWidth;
        float scaleY = 16 / spriteWorldHeight;

        // 5. Transform.localScale에 적용
        // Z축은 2D에서 보통 1을 유지합니다.
        transform.localScale = new Vector2(scaleX, scaleY);
    }
}
