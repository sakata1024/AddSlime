using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/InitalStageData")]
public class InitialStageData : ScriptableObject
{
    public List<StageObjectData> stageObjectList = new List<StageObjectData>();
}

[System.Serializable]
public class StageObjectData
{
    public StageObject stageObject;
    public Vector2Int position;
    public int initialSlimeNum;
}
