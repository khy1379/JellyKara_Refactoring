using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class JellyKara : MonoBehaviour
{
    AudioSource auSource;
    BoxCollider2D thisCol;
    public Rigidbody2D rb;
    public AudioClip[] auClip;
    public GameManager gm;
    public BoxCollider2D finishCol;
    public JellySpriteControl jellySprite;
    public float upMoveSpeed;
    public float speed;
    public float maxUpMoveSpeed;
    void Awake()
    {
        thisCol = GetComponent<BoxCollider2D>();
        auSource = GetComponent<AudioSource>();
        if (thisCol.isTrigger == true)
            thisCol.isTrigger = false;
        auSource.clip = auClip[0];
        upMoveSpeed = 0;
    }
    private void FixedUpdate()
    {
        if (gm.status == GameStatus.Playing && PlayerTypeChanger.pt == PlayerType.Bear) 
            BearUpDownMove();
    }
    void Update()
    {
        if (gm.status == GameStatus.Playing)
        {
            switch (PlayerTypeChanger.pt)
            {
                case PlayerType.Slime:
                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                        if (EventSystem.current.IsPointerOverGameObject() == false && hit.collider == null)
                        {
                            SlimeJump();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        SlimeJump();
                    }
                    break;
                case PlayerType.Bear:
                    if (Input.GetMouseButton(0))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                        if (EventSystem.current.IsPointerOverGameObject() == false && hit.collider == null)
                        {
                            PlayerTypeChanger.isUpMove = true;
                            speed = 10;
                        }
                    }
                    else if (Input.GetKey(KeyCode.Space))
                    {
                        PlayerTypeChanger.isUpMove = true;
                        speed = 10;
                    }
                    else if (Input.GetMouseButtonUp(0))
                    {
                        PlayerTypeChanger.isUpMove = false;
                        speed = 10;
                    }
                    else if (Input.GetKeyUp(KeyCode.Space))
                    {
                        PlayerTypeChanger.isUpMove = false;
                        speed = 10;
                    }

                    break;
                case PlayerType.Earth:
                    if (Input.GetMouseButtonDown(0))
                    {
                        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                        if (EventSystem.current.IsPointerOverGameObject() == false && hit.collider == null)
                        {
                            EarthGravityReverse();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        EarthGravityReverse();
                    }
                    break;
            }
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
            //if (col.tag == "Score")
            //{
            //    gm.Score++;
            //    Destroy(col.gameObject);
            //}
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
                    speed = 10;
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
    void PlayerTypeChangeInit()
    {
        if (PlayerTypeChanger.isUpMove)
        {
            PlayerTypeChanger.isUpMove = false;
            rb.gravityScale = 1;
            rb.velocity = Vector2.zero;
        }
    }
    void PlayerTypeChangeSlime()
    {
        PlayerTypeChangeInit();
        PlayerTypeChanger.TypeSetSlime();
        jellySprite.SpriteChangeSlime();
    }
    void PlayerTypeChangeBear()
    {
        PlayerTypeChangeInit();
        PlayerTypeChanger.TypeSetBear();
        jellySprite.SpriteChangeBear();
    }
    void PlayerTypeChangeEarth()
    {
        PlayerTypeChangeInit();
        PlayerTypeChanger.TypeSetEarth();
        jellySprite.SpriteChangeEarth();
    }
    void SlimeJump()
    {
        auSource.Play();
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * 200);
    }
    void BearUpDownMove()
    {
        if (PlayerTypeChanger.isUpMove)
        {
            if (maxUpMoveSpeed < upMoveSpeed)
                upMoveSpeed = maxUpMoveSpeed;
            else
                upMoveSpeed += Time.fixedDeltaTime * speed;
            rb.velocity = new Vector2(0, upMoveSpeed);
        }
        else
        {
            if (-maxUpMoveSpeed > upMoveSpeed)
                upMoveSpeed = -maxUpMoveSpeed;
            else
                upMoveSpeed -= Time.fixedDeltaTime * speed;
            rb.velocity = new Vector2(0, upMoveSpeed);
        }
    }
    void EarthGravityReverse()
    {
        /*RaycastHit2D jumpHit;
        if(rb.gravityScale > 0)
            jumpHit = Physics2D.BoxCast(transform.position,
                new Vector2(0.8f, 0.05f), 0, Vector2.down, 0.405f);
        else
            jumpHit = Physics2D.BoxCast(transform.position,
                new Vector2(0.8f, 0.05f), 0, Vector2.up, 0.405f);
        if (jumpHit.collider.tag != "Player")
        {
            auSource.Play();
            rb.gravityScale = -rb.gravityScale;
        }*/
        auSource.Play();
        if (!PlayerTypeChanger.isUpMove)
        {
            PlayerTypeChanger.isUpMove = true;
            rb.gravityScale = -1;
        }
        else
        {
            PlayerTypeChanger.isUpMove = false;
            rb.gravityScale = 1;
        }
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
        if (PlayerTypeChanger.isUpMove)
        {
            PlayerTypeChanger.isUpMove = false;
            rb.gravityScale = 1;
        }
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
