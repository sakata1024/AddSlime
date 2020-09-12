using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public int stageSizeX;
    public int stageSizeY;

    public StageObject[,] stageCells;
    public Character character;
    public Vector2Int characterPoint;
    public Slime debugSlime;
    public Vector3 stageOriginPosition = new Vector3(-2.5f, -2.5f);
    public float gridScale = 1f;
    public InitialStageData stageData;

    // Start is called before the first frame update
    void Start()
    {
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
        foreach (var stageObjectData in stageData.stageObjectList)
        {
            var instance = Instantiate(stageObjectData.stageObject, ConvertToWorldPosition(stageObjectData.position), Quaternion.identity);
            stageCells[stageObjectData.position.x, stageObjectData.position.y] = instance;
            instance.stage = this;
            if(instance is Slime)
            {
                ((Slime)instance).number = stageObjectData.initialSlimeNum;
            }
            else if(instance is Character)
            {
                characterPoint = stageObjectData.position;
            }
        }
    }
    
    public void PutSlime(Slime slime, int x, int y)
    {
        stageCells[x, y] = slime;
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
                    ((Slime)stageCells[massPos.x, massPos.y]).Bomb();
                }
            }
        }
    }

    public Vector2Int GetRandomGrid()
    {
        Vector2Int randPos;
        do
        {
            randPos = new Vector2Int(Random.Range(0, stageSizeX), Random.Range(0, stageSizeY));
        } while (stageCells[randPos.x,randPos.y] != null);

        return randPos;
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
