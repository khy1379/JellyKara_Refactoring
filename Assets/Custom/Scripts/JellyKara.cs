using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class JellyKara : MonoBehaviour
{
    public AudioSource auSource;
    BoxCollider2D thisCol;
    public Rigidbody2D rb;
    public AudioClip[] auClip;
    public GameManager gm;
    public JellySpriteControl jellySprite;
    public float upMoveSpeed;
    public float speed;
    public float baseSpeed;
    public float maxUpMoveSpeed;
    void Awake()
    {
        PlayerTypeChanger.kara = this;
        thisCol = GetComponent<BoxCollider2D>();
        auSource = GetComponent<AudioSource>();
        if (thisCol.isTrigger == true)
            thisCol.isTrigger = false;
        auSource.clip = auClip[0];
        upMoveSpeed = 0;
        speed = baseSpeed;
    }
    private void OnDestroy()
    {
        PlayerTypeChanger.kara = null;
    }
    private void FixedUpdate()
    {
        if (gm.status == GameStatus.Playing )
        {
            PlayerTypeChanger.JellyMoving();
        }
    }
    void Update()
    {
        if (gm.status == GameStatus.Playing)
        {
            PlayerTypeChanger.InputKey();
            PlayerTypeChange();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (gm.status == GameStatus.Playing)
        {
            switch (col.tag)
            {
                case "Score":
                    gm.Score++;
                    Destroy(col.gameObject);
                    break;
                case "Slime":
                    PlayerTypeChangeSlime();
                    Destroy(col.gameObject);
                    break;
                case "Bear":
                    PlayerTypeChangeBear();
                    Destroy(col.gameObject);
                    break;
                case "Earth":
                    PlayerTypeChangeEarth();
                    Destroy(col.gameObject);
                    break;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gm.status == GameStatus.Playing)
        {
            switch (PlayerTypeChanger.pt)
            {
                case PlayerType.Bear:
                    upMoveSpeed = 0;
                    speed = 0;
                    break;
                case PlayerType.Earth:
                    rb.velocity = Vector2.zero;
                    break;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gm.status == GameStatus.Playing)
        {
            switch (PlayerTypeChanger.pt)
            {
                case PlayerType.Bear:
                    speed = baseSpeed;
                    break;
                case PlayerType.Earth:
                    rb.velocity = Vector2.zero;
                    break;
            }
        }
    }
    void PlayerTypeChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerTypeChangeSlime();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerTypeChangeBear();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerTypeChangeEarth();
        }
    }
    void PlayerTypeChangeSlime()
    {
        //PlayerTypeChangeInit();
        PlayerTypeChanger.InputStateReset();
        PlayerTypeChanger.TypeSetSlime();
        jellySprite.SpriteChangeSlime();
    }
    void PlayerTypeChangeBear()
    {
        //PlayerTypeChangeInit();
        PlayerTypeChanger.InputStateReset();
        PlayerTypeChanger.TypeSetBear();
        jellySprite.SpriteChangeBear();
    }
    void PlayerTypeChangeEarth()
    {
        //PlayerTypeChangeInit();
        PlayerTypeChanger.InputStateReset();
        PlayerTypeChanger.TypeSetEarth();
        jellySprite.SpriteChangeEarth();
    }
    public void GameFinish()
    {
        gm.status = GameStatus.Finish;
        if (thisCol.isTrigger == false)
            thisCol.isTrigger = true;
        if (jellySprite.isSpriteIdle == true)
        {
            jellySprite.JellySpriteReset();
        }
        rb.velocity = Vector2.zero;
        PlayerTypeChanger.InputStateReset();
        auSource.clip = auClip[1];
        auSource.Play();
        StartCoroutine(BackToMain());
    }
    IEnumerator BackToMain()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
