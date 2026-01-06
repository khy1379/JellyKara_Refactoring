using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySpriteControl : MonoBehaviour, IPlayerTypeChangeable, IKaraEventable
{
    public Sprite[] jellys;
    public GameManager gm;
    JellyKara kara;
    SpriteRenderer spriteRenderer;
    public BoxCollider2D col;
    public bool isSpriteIdle;
    bool isSpriteDecrease;
    Vector2 spriteScale;
    public void SpriteInit()
    {
        kara = JellyKara.instanse;
        kara.AddKaraEventObserver(this);
        kara.ptc.AddTypeChangeObserver(this);
        spriteRenderer = GetComponent<SpriteRenderer>();
        isSpriteIdle = true;
        isSpriteDecrease = true;
        switch (kara.ptc.pt)
        {
            default:
            case PlayerType.Slime:
                spriteRenderer.sprite = jellys[0];
                ScaleSpriteToCollider(0);
                break;
            case PlayerType.Bear:
                spriteRenderer.sprite = jellys[1];
                ScaleSpriteToCollider(1);
                break;
            case PlayerType.Earth:
                spriteRenderer.sprite = jellys[2];
                ScaleSpriteToCollider(2);
                break;
        }
        spriteRenderer.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    private void LateUpdate()
    {
        SpriteChange();
    }
    public void IdleSpriteExe()
    {
        if (isSpriteIdle)
        {
            if (kara.ptc.pt == PlayerType.Earth)
            {
                SpriteRoll();
            }
            else
            {
                SpriteBiggerAndSmaller();
            }
        }
    }
    void SpriteRoll()
    {
        if (kara.rb.gravityScale > 0)
            spriteRenderer.transform.Rotate(Vector3.forward * -200 * Time.deltaTime, Space.Self);
        else
            spriteRenderer.transform.Rotate(Vector3.forward * 200 * Time.deltaTime, Space.Self);
    }
    void SpriteBiggerAndSmaller()
    {
        if (isSpriteDecrease)
        {
            spriteRenderer.transform.localScale = new Vector2(spriteRenderer.transform.localScale.x - Time.fixedDeltaTime / 2f, spriteRenderer.transform.localScale.y - Time.fixedDeltaTime / 2f);
            if (spriteRenderer.transform.localScale.x < spriteScale.x * 0.8f)
            {
                isSpriteDecrease = false;
                spriteRenderer.transform.localScale = spriteScale * 0.8f;

            }
        }
        else
        {
            spriteRenderer.transform.localScale = new Vector2(spriteRenderer.transform.localScale.x + Time.fixedDeltaTime / 2f, spriteRenderer.transform.localScale.y + Time.fixedDeltaTime / 2f);
            if (spriteRenderer.transform.localScale.x > spriteScale.x)
            {
                isSpriteDecrease = true;
                spriteRenderer.transform.localScale = spriteScale;

            }
        }
    }
    public void TypeChange(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Slime:
                SpriteChangeSlime();
                break;
            case PlayerType.Bear:
                SpriteChangeBear();
                break;
            case PlayerType.Earth:
                SpriteChangeEarth();
                break;
        }
    }
    void SpriteChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && kara.ptc.pt == PlayerType.Slime)
            SpriteChangeSlime();
        else if (Input.GetKeyDown(KeyCode.Alpha2) && kara.ptc.pt == PlayerType.Bear)
            SpriteChangeBear();
        else if (Input.GetKeyDown(KeyCode.Alpha3) && kara.ptc.pt == PlayerType.Earth)
            SpriteChangeEarth();
    }
    public void JellySpriteReset()
    {
        isSpriteIdle = false;
        spriteRenderer.transform.localScale = spriteScale;
        spriteRenderer.color = Color.red;
    }
    public void SpriteChangeSlime()
    {
        if (spriteRenderer.sprite != jellys[0])
        {
            if (spriteRenderer.sprite == jellys[2])
                spriteRenderer.transform.rotation = Quaternion.Euler(Vector3.zero);
            spriteRenderer.sprite = jellys[0];
            ScaleSpriteToCollider(0);
        }
    }
    public void SpriteChangeBear()
    {
        if (spriteRenderer.sprite != jellys[1])
        {
            if (spriteRenderer.sprite == jellys[2])
                spriteRenderer.transform.rotation = Quaternion.Euler(Vector3.zero);
            spriteRenderer.sprite = jellys[1];
            ScaleSpriteToCollider(1);
        }
    }
    public void SpriteChangeEarth()
    {
        if (spriteRenderer.sprite != jellys[2])
        {
            spriteRenderer.sprite = jellys[2];
            ScaleSpriteToCollider(2);
        }
    }
    private void ScaleSpriteToCollider(int index)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer가 존재하지 않습니다. 스케일 조정 불가.");
            return;
        }
        else if (col == null)
        {
            Debug.LogError("Collider2D가 존재하지 않습니다. 스케일 조정 불가.");
            return;
        }
        else if (spriteRenderer.sprite == null)
        {
            Debug.LogError("Sprite가 존재하지 않습니다. 스케일 조정 불가.");
            return;
        }

        // 1. 콜라이더의 월드 크기 가져오기 : bounds.size
        Vector3 colliderWorldSize = col.bounds.size;


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

        float scaleX = colliderWorldSize.x / spriteWorldWidth;
        float scaleY = colliderWorldSize.y / spriteWorldHeight;

        // 5. Transform.localScale에 적용
        // Z축은 2D에서 보통 1을 유지합니다.
        spriteScale = transform.localScale = new Vector2(scaleX, scaleY);

    }


    public void KaraDie()
    {
        JellySpriteReset();
    }
}
