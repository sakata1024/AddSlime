using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIText : MonoBehaviour
{
    public Text timeText;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = string.Format("Time:{0:0.000}", GameManager.Instance.GetTime());
        scoreText.text = string.Format("Score:{0}", GameManager.Instance.GetScore());
    }
}
