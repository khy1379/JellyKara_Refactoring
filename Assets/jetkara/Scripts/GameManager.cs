using System.Collections;
using TMPro;
using UnityEngine;

public enum GameStatus
{
	Playing,
	Pause,
	Finish
}
public class GameManager : MonoBehaviour
{
	public GameObject objects;
	public GameObject changeObj;
	//public TextMesh scoreLabel;
    public TextMeshProUGUI scoreLabel;
    public static int score;
	public GameStatus status;
	public int objCreateMaxCnt;
    public int objCreateCurCnt;
    public int Score
	{
		set
		{
			score = value;

			scoreLabel.text = Score.ToString();
		}
		get
		{
			return score;
		}
	}

	void Awake () 
	{
		Score = 0;
        status = GameStatus.Playing;
        StartCoroutine(CreateObjects());
		objCreateMaxCnt = 0;
		objCreateCurCnt = 0;

    }
    private void Update()
    {
		GameStatusChange();
    }
    IEnumerator CreateObjects()
	{
		yield return new WaitForSeconds(1);
		while (true)
		{
			objCreateMaxCnt = Random.Range(0, 15) + 15;
            for (objCreateCurCnt = 0; objCreateCurCnt < objCreateMaxCnt; objCreateCurCnt++)
			{
				Instantiate(objects, new Vector3(7.5f, Random.Range(-2f, 2.1f), 0), Quaternion.identity);
				yield return new WaitForSeconds(2);
			}
            Instantiate(changeObj, new Vector3(7.5f, 0, 0), Quaternion.identity);
            yield return new WaitForSeconds(2);
        }
	}
	void GameStatusChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
			if (status == GameStatus.Playing)
			{
				GamePause();
            }
			else
			{
				GameResume();
            }
        }
    }
	public void GamePause()
    {
        Time.timeScale = 0;
        status = GameStatus.Pause;
    }
	public void GameResume()
    {
        Time.timeScale = 1;
        status = GameStatus.Playing;
    }
}
