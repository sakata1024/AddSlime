using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Stage stage;
    public StageFactory stageFactory;
    public SlimeFactory slimeFactory;
    public StageLevelData stageLevelData;

    int score;
    float timerCount = 0f;
    bool isGameStarted = false;

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
            stageFactory.stageSizeX = stageLevelData.stageSize.x;
            stageFactory.stageSizeY = stageLevelData.stageSize.y;
            slimeFactory.fallSlimeRate = stageLevelData.slimeCreateTiming;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    void Update()
    {
        if (isGameStarted)
        {
            timerCount += Time.deltaTime;
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
        return timerCount;
    }

    public void GameStart()
    {
        isGameStarted = true;
    }

    public void GameOver()
    {
        isGameStarted = false;
    }

}
