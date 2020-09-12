using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFactory : MonoBehaviour
{
    public Stage stage;
    public int stageSizeX;
    public int stageSizeY;
    public GameObject iceBlock;

    // Start is called before the first frame update
    void Start()
    {
        for (var x = 0; x < stageSizeX; x++)
        {
            for (var y = 0; y < stageSizeY; y++)
            {
                var instance = Instantiate(iceBlock,transform);
                instance.transform.localPosition = new Vector3(x - stageSizeX / 2, y - stageSizeY / 2, 0);
                instance.transform.localRotation = Quaternion.identity;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
