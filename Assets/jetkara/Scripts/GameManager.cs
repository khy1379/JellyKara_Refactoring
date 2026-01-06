using System.Collections;
using System.Collections.Generic;
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
    Queue<GameObject> objectsPool;
    GameObject changeObj;
    public GameObject objectsPrefab;
    public GameObject changeObjPrefab;
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

    void Awake()
    {
        objectsPool = new Queue<GameObject>();
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
        ObjectSet(new Vector3(5.7f, 0, 0));
        yield return new WaitForSeconds(1);
        while (true)
        {
            objCreateMaxCnt = Random.Range(0, 5) + 15;
            for (objCreateCurCnt = 0; objCreateCurCnt < objCreateMaxCnt; objCreateCurCnt++)
            {
                ObjectSet(new Vector3(7.5f, Random.Range(-2f, 2.1f), 0));
                yield return new WaitForSeconds(2);
            }
            ChangeObjectSet();
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
    public void EnqueueScoreObjects(ObjectMove objMove)
    {
        //objMove.gameObject.SetActive(false);
        objectsPool.Enqueue(objMove.gameObject);
    }
    public void DequeueScoreObjects(Vector3 startPos)
    {
        GameObject obj = objectsPool.Dequeue();
        obj.transform.position = startPos;
        obj.SetActive(true);
    }
    public void ObjectSet(Vector3 startPos)
    {
        if (objectsPool.Count == 0)
        {
            GameObject obj = Instantiate(objectsPrefab.gameObject, startPos, Quaternion.identity, transform);
            if (obj.GetComponent<ObjectMove>() is ObjectMove objMove)
            {
                objMove.GameManagerSet(this);
            }
        }
        else
        {
            DequeueScoreObjects(startPos);
        }
    }
    public void ChangeObjectSet()
    {
        if(changeObj == null)
            changeObj = Instantiate(changeObjPrefab, new Vector3(7.5f, 0, 0), Quaternion.identity, transform);
        else
        {
            changeObj.transform.position = new Vector3(7.5f, 0, 0);
            changeObj.gameObject.SetActive(true);
        }
    }
}
