using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Stage stage;
    public StageFactory stageFactory;
    public SlimeFactory slimeFactory;
    public static StageLevelData stageLevelData;

    public float finishTime = 10f;

    int score;
    float timerCount = 0f;
    bool isGameStarted = false;
    string gameFinishMessage;

    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) Debug.LogAssertion("GameManager instance is null");
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        if(stageLevelData != null)
        {
            stage.stageSizeX = stageLevelData.stageSize.x;
            stage.stageSizeY = stageLevelData.stageSize.y;
            stage.stageData = stageLevelData.stageObjectData;
            slimeFactory.fallSlimeRate = stageLevelData.slimeCreateTiming;
            finishTime = stageLevelData.gameTimeSeconds;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        GameStart();
    }

    void Update()
    {
        if (!isGameStarted) return;

        timerCount += Time.deltaTime;

        if (timerCount >= finishTime)
        {
            timerCount = finishTime;
            TimeOver();
        }

    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int addScore)
    {
        score += addScore;
    }

    public float GetTime()
    {
        return finishTime - timerCount;
    }

    public void GameStart()
    {
        isGameStarted = true;
    }

    public void GameFinish()
    {
        SceneManager.LoadScene("ResultScene");

        void SetResult(Scene next, LoadSceneMode mode)
        {
            GameObject.Find("ResultLoader")?.GetComponent<ResultLoader>().SetResult(gameFinishMessage, score);
            SceneManager.sceneLoaded -= SetResult;
        }

        SceneManager.sceneLoaded += SetResult;
    }

    public void TimeOver()
    {
        isGameStarted = false;
        gameFinishMessage = "GAME FINISH!";
        GameFinish();
    }

    public void GameOver()
    {
        isGameStarted = false;
        gameFinishMessage = "GAME OVER...";
        GameFinish();
    }

}
