using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class PlayerScript : MonoBehaviour
{
	bool dead;
	AudioSource auSource;
	Rigidbody2D rb;
    Collider2D col;
	public AudioClip[] auClip;
	public GameObject fire;
	public GameManager gameManager;
    public Collider2D finishCol;
    void Start()
	{
		dead = false;
		col = GetComponent<Collider2D>();
		auSource = GetComponent<AudioSource>();
		rb= GetComponent<Rigidbody2D>();
		//GetComponent<AudioSource>().clip = auClip[0];
		if (col.isTrigger == true)
			col.isTrigger = false;
		auSource.clip = auClip[0];
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && !dead)
		{
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if(hit.collider == null)
			{
				Jump();
			}
        }
    }

	void Jump()
	{
		fire.SetActive (true);
		auSource.Play();
		rb.velocity = Vector2.zero;
		rb.AddForce(Vector2.up * 200);
	}

	void OnTriggerEnter2D(Collider2D col) 
	{
		if (!dead)
		{
			if (col.tag == "Score")
			{
				gameManager.Score++;
                Destroy(col.gameObject);
			}
        }
	}
    public void GameFinish()
    {
        dead = true;
		if(col.isTrigger == false)
		col.isTrigger = true;
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
