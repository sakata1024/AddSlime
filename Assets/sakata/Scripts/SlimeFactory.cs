using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFactory : MonoBehaviour
{
    public float fallSlimeRate = 5f;
    public Stage stage;
    public Slime slimePrefab;
    float currentTime = 0f;

    // Update is called once per frame
    void Update()
    {
        if(currentTime > fallSlimeRate)
        {
            currentTime = 0f;
            var pos = stage.GetRandomGrid();
            var instance = Instantiate(slimePrefab, stage.ConvertToWorldPosition(pos), Quaternion.identity);
            stage.PutSlime(instance, pos.x, pos.y);
            instance.number = Random.Range(1, 10);
        }
        currentTime += Time.deltaTime;
    }
}
