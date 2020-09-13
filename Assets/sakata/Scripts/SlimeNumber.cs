using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeNumber : MonoBehaviour
{
    public Slime slime;
    public Text numberText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var num = slime.number;
        numberText.text = num.ToString();
        if(num >= 11)
        {
            numberText.color = Color.white;
        }
    }
}
