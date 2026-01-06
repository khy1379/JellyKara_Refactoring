using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public interface IKaraEventable
{
    void KaraDie();
}
public class KaraEventObserver
{
    List<IKaraEventable> observerList = new List<IKaraEventable>();
    public void AddkaraEventObserver(IKaraEventable karaDieClass) => observerList.Add(karaDieClass);
    public void RemovekaraEventObserver(IKaraEventable karaDieClass) => observerList.Remove(karaDieClass);
    public void ClaerAllkaraEventObserver() => observerList.Clear();
    public void DieAction()
    {
        foreach (IKaraEventable observer in observerList)
        {
            observer.KaraDie();
        }
    }
}
public class JellyKara : MonoBehaviour
{
    public static JellyKara instanse;
    public AudioSource auSource;
    BoxCollider2D thisCol;
    public Rigidbody2D rb;
    public AudioClip[] auClip;
    public GameManager gm;
    public PlayerTypeChanger ptc;
    public JellySpriteControl jellySprite;
    public float upMoveSpeed;
    public float speed;
    public float baseSpeed;
    public float maxUpMoveSpeed;
    KaraEventObserver observer;
    void Awake()
    {
        InitJellyKara();
    }
    void InitJellyKara()
    {
        if (instanse == null)
        {
            instanse = this;
            thisCol = GetComponent<BoxCollider2D>();
            auSource = GetComponent<AudioSource>();
            if (thisCol.isTrigger == true)
                thisCol.isTrigger = false;
            auSource.clip = auClip[0];
            upMoveSpeed = 0;
            speed = baseSpeed;
            observer = new KaraEventObserver();
            ptc.JellyTypeInit();
            jellySprite.SpriteInit();
        }
        else
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        RemoveStaticJellyKara();
    }
    void RemoveStaticJellyKara()
    {
        if (instanse != null) instanse = null;
    }
    private void FixedUpdate()
    {
        if (gm.status == GameStatus.Playing)
        {
            ptc.JellyMoving();
            jellySprite.IdleSpriteExe();
        }
    }
    void Update()
    {
        if (gm.status == GameStatus.Playing)
        {
            ptc.InputKey();
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
                    break;
                case "Slime":
                    PlayerTypeChangeSlime();
                    break;
                case "Bear":
                    PlayerTypeChangeBear();
                    break;
                case "Earth":
                    PlayerTypeChangeEarth();
                    break;
            }
            col.gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gm.status == GameStatus.Playing)
        {
            switch (ptc.pt)
            {
                case PlayerType.Bear:
                case PlayerType.Earth:
                    ptc.CollisionEnterSetting();
                    break;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (gm.status == GameStatus.Playing)
        {
            switch (ptc.pt)
            {
                case PlayerType.Bear:
                case PlayerType.Earth:
                    ptc.CollisionExitSetting();
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
        ptc.TypeChange(PlayerType.Slime);
    }
    void PlayerTypeChangeBear()
    {
        ptc.TypeChange(PlayerType.Bear);
    }
    void PlayerTypeChangeEarth()
    {
        ptc.TypeChange(PlayerType.Earth);
    }
    public void GameFinish()
    {
        gm.status = GameStatus.Finish;
        if (thisCol.isTrigger == false)
            thisCol.isTrigger = true;
        rb.velocity = Vector2.zero;
        observer.DieAction();
        auSource.clip = auClip[1];
        auSource.Play();
        StartCoroutine(BackToMain());
    }
    IEnumerator BackToMain()
    {
        yield return new WaitForSeconds(1.5f);
        observer.ClaerAllkaraEventObserver();
        SceneManager.LoadScene("MainMenu");
    }
    public void AddKaraEventObserver(IKaraEventable obsClass) => observer.AddkaraEventObserver(obsClass);
    public void RemoveKaraEventObserver(IKaraEventable obsClass) => observer.RemovekaraEventObserver(obsClass);
}
