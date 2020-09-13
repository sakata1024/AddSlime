using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeFactory : MonoBehaviour
{
    public float fallSlimeRate = 5f;
    public Stage stage;
    public Slime slimePrefab;
    float currentTime = 0f;
    public GameObject warningPrefab;
    Vector2Int fallSlimePos;
    GameObject warningInstance;

    // Update is called once per frame
    void Update()
    {
        if(currentTime > fallSlimeRate)
        {
            currentTime = 0f;
            fallSlimePos = stage.GetRandomGrid();
            var worldPos = stage.ConvertToWorldPosition(fallSlimePos);
            worldPos.z = -3;
            warningInstance = Instantiate(warningPrefab, worldPos, Quaternion.identity);
            StartCoroutine(PutSlime());
        }
        currentTime += Time.deltaTime;
    }


    IEnumerator PutSlime()
    {
        yield return new WaitForSeconds(1.1f);
        Destroy(warningInstance);
        warningInstance = null;
        var instance = Instantiate(slimePrefab, stage.ConvertToWorldPosition(fallSlimePos), Quaternion.identity);
        stage.PutSlime(instance, fallSlimePos.x, fallSlimePos.y);
        instance.number = Random.Range(1, 10);
    }
}
