using UnityEngine;


public class ObjectMove : MonoBehaviour
{
    GameManager gm;
    public GameObject scoreTrigger;
    private void OnDisable()
    {
        ScoreObjectSet(true);
        if(gm != null)
            gm.EnqueueScoreObjects(this);
    }
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - 0.03f, transform.position.y, 0);

        if (transform.position.x <= -7.5f)
        {
            gameObject.SetActive(false);
        }
    }
    public void GameManagerSet(GameManager gm) => this.gm = gm;
    public void ScoreObjectSet(bool isSetActive)
    {
        if (scoreTrigger == null) return;
        scoreTrigger.SetActive(isSetActive);
    }
}
