using TMPro;
using UnityEngine;

public class SetScore : MonoBehaviour 
{
	//public TextMesh bestScoreLabel;
	//public TextMesh scoreLabel;
    public TextMeshProUGUI bestScoreLabel;
    public TextMeshProUGUI scoreLabel;

    void Start () 
	{
		scoreLabel.text = "Score: " + GameManager.score.ToString();

		if (GameManager.score > 0)
		{
			if (PlayerPrefs.GetInt("Score", 0) < GameManager.score)
			{
				PlayerPrefs.SetInt("Score", GameManager.score);
				PlayerPrefs.Save();
			}
		}

		bestScoreLabel.text = "HighScore: " + PlayerPrefs.GetInt("Score", 0).ToString();
		GameManager.score = 0;
	}
}