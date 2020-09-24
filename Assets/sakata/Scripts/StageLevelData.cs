using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/StageLevelData")]
public class StageLevelData : ScriptableObject
{
    public int level;
    public Vector2Int stageSize;
    public InitialStageData stageObjectData;
    public float slimeCreateTiming;
    public float gameTimeSeconds;
}
