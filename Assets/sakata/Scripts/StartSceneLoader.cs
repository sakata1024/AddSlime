using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneLoader : MonoBehaviour
{
    public StageLevelData easyLevelData;
    public StageLevelData normalLevelData;
    public StageLevelData hardLevelData;

    public void OnEasyButton()
    {
        GameManager.stageLevelData = easyLevelData;
        SceneManager.LoadScene("MainScene");

    }

    public void OnNormalButton()
    {
        GameManager.stageLevelData = normalLevelData;
        SceneManager.LoadScene("MainScene");
    }

    public void OnHardButton()
    {
        GameManager.stageLevelData = hardLevelData;
        SceneManager.LoadScene("MainScene");
    }
}
