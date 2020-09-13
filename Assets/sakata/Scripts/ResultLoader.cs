using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultLoader : MonoBehaviour
{
    public Text resultText;
    public Text scoreText;

    public string result;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        resultText.text = result;
        scoreText.text = score.ToString();
    }

    public void MoveToStartScene()
    {
        SceneManager.LoadScene("start");
    }

    public void ShowRanking()
    {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);
    }

    public void SetResult(string msg, int score)
    {
        result = msg;
        this.score = score;
    }
}
