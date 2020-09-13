
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForBGM_title : MonoBehaviour
{
    void Start()
    {
        SoundPlayer.Instance.StopBGM();
        SoundPlayer.Instance.PlayBGM("title");
    }

}
