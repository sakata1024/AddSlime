using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Stage : MonoBehaviour
{
    public int stageSizeX;
    public int stageSizeY;

    public StageObject[,] stageCells;
    public Character character;
    public Vector2Int characterPoint;
    public Slime debugSlime;
    public Vector3 stageOriginPosition;
    public float gridScale = 1f;
    public InitialStageData stageData;

    // Start is called before the first frame update
    void Start()
    {
        stageOriginPosition = new Vector2(-stageSizeX / 2.0f * gridScale, -stageSizeY / 2.0f * gridScale);
        stageCells = new StageObject[stageSizeX,stageSizeY];
        if(stageData != null)
        {
            CreateStageObject();
        }
        else
        {
            characterPoint = ConvertToGrid(character.transform.position);
            character.stage = this;
            var debugSlimeGridPos = ConvertToGrid(debugSlime.transform.position);
            stageCells[debugSlimeGridPos.x, debugSlimeGridPos.y] = debugSlime;
            debugSlime.stage = this;
        }
    }

    void CreateStageObject()
    {
        foreach(var stageObject in FindObjectsOfType<StageObject>())
        {
            Destroy(stageObject.gameObject);
        }

        foreach (var stageObjectData in stageData.stageObjectList)
        {
            var instance = Instantiate(stageObjectData.stageObject, ConvertToWorldPosition(stageObjectData.position), Quaternion.identity);
            instance.stage = this;
            if(instance is Slime)
            {
                ((Slime)instance).number = stageObjectData.initialSlimeNum;
            }

            if(instance is Character)
            {
                characterPoint = stageObjectData.position;
            }
            else
            {
                stageCells[stageObjectData.position.x, stageObjectData.position.y] = instance;
            }
        }
    }
    
    public void PutSlime(Slime slime, int x, int y)
    {
        stageCells[x, y] = slime;
        slime.stage = this;
    }

    public void Attack(Vector2Int direction)
    {
        Vector2Int attackPosition = characterPoint + direction;

        if (isInStage(attackPosition) && stageCells[attackPosition.x, attackPosition.y] is Slime && ((Slime)stageCells[attackPosition.x, attackPosition.y]).canMove)
        {
            Slime targetSlime = (Slime)stageCells[attackPosition.x, attackPosition.y];
            Vector2Int finishPosition = attackPosition;
            bool canUnion = false;
            for (Vector2Int searchPosition = attackPosition + direction; isInStage(searchPosition); searchPosition += direction)
            {
                // TODO:壁があればbreak
                if(stageCells[searchPosition.x, searchPosition.y] is Wall)
                {
                    break;
                }
                else if(stageCells[searchPosition.x, searchPosition.y] is Slime)
                {
                    // 9以下ならそこまで行く
                    if(((Slime)stageCells[searchPosition.x, searchPosition.y]).canMove)
                    {
                        finishPosition = searchPosition;
                        canUnion = true;
                        break;
                    }
                    else //11以上なら手前で終了
                    {
                        break;
                    }
                }
                else
                {
                    finishPosition = searchPosition;
                }
            }
            if (finishPosition != attackPosition)
            {
                if (canUnion)
                {
                    // スライムが加算するときの処理
                    targetSlime.UnionTo(ConvertToWorldPosition(finishPosition), (Slime)stageCells[finishPosition.x, finishPosition.y]);
                    stageCells[attackPosition.x, attackPosition.y] = null;
                }
                else
                {
                    targetSlime.MoveTo(ConvertToWorldPosition(finishPosition));
                    stageCells[finishPosition.x, finishPosition.y] = targetSlime;
                    stageCells[attackPosition.x, attackPosition.y] = null;
                }
            }
        }
    }

    public void Bomb(Slime bombSlime)
    {
        var bombPos = ConvertToGrid(bombSlime.transform.position);
        int totalAddScore = 0;

        for(int y = -1; y <= 1; y++)
        {
            for(int x = -1; x <= 1; x++)
            {
                var massPos = bombPos + new Vector2Int(x, y);
                if (!isInStage(massPos))
                {
                    continue;
                }
                if(stageCells[massPos.x, massPos.y] is Slime)
                {
                    totalAddScore += ((Slime)stageCells[massPos.x, massPos.y]).number;
                    ((Slime)stageCells[massPos.x, massPos.y]).Bomb();
                }
            }
        }
        GameManager.Instance.AddScore(totalAddScore);
    }

    public Vector2Int GetRandomGrid()
    {
        List<Vector2Int> posList = new List<Vector2Int>();

        for(int y = 0; y < stageSizeY; y++)
        {
            for(int x = 0; x < stageSizeX; x++)
            {
                if(stageCells[x,y] == null)
                {
                    posList.Add(new Vector2Int(x, y));
                }
            }
        }
        if(posList.Count != 0)
        {
            return posList[Random.Range(0, posList.Count)];
        }
        else
        {
            return new Vector2Int(-1, -1);
        }
    }

    bool isInStage(Vector2Int position)
    {
        return position.x >= 0 && position.x < stageCells.GetLength(0) && position.y >= 0 && position.y < stageCells.GetLength(1);
    }

    public Vector2Int ConvertToGrid(Vector3 position)
    {
        Vector2 pos = new Vector2((position.x - stageOriginPosition.x) / gridScale, (position.y - stageOriginPosition.y) / gridScale);
        return Vector2Int.FloorToInt(pos);
    }

    public Vector3 ConvertToWorldPosition(Vector2Int position)
    {
        return stageOriginPosition + (Vector3)((Vector2)position * gridScale) + new Vector3(gridScale / 2, gridScale / 2, 0);
    }
}
