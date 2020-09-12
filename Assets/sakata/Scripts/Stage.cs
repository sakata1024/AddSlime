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
    public Slime debugSlime2;
    public Slime debugSlime3;
    public Wall debugWall;
    public Vector3 stageOriginPosition = new Vector3(-2.5f, -2.5f);
    public float gridScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        stageCells = new StageObject[stageSizeX,stageSizeY];
        characterPoint = new Vector2Int(4, 4);
        character.stage = this;
        stageCells[2, 2] = debugSlime;
        debugSlime.position = new Vector2Int(2, 2);
        debugSlime.stage = this;
        if(debugSlime2 != null)
        {
            stageCells[3, 3] = debugSlime2;
            debugSlime2.position = new Vector2Int(3, 3);
            debugSlime2.stage = this;
        }
        if(debugSlime3 != null)
        {
            stageCells[1, 4] = debugSlime3;
            debugSlime3.position = new Vector2Int(1, 4);
            debugSlime3.stage = this;
        }
        if (debugWall != null)
        {
            stageCells[0, 2] = debugWall;
        }
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
                    targetSlime.UnionTo(stageOriginPosition + (Vector3)((Vector2)finishPosition * gridScale) + new Vector3(gridScale / 2, gridScale / 2, 0), (Slime)stageCells[finishPosition.x, finishPosition.y]);
                    stageCells[attackPosition.x, attackPosition.y] = null;
                }
                else
                {
                    targetSlime.position = finishPosition;
                    targetSlime.MoveTo(stageOriginPosition + (Vector3)((Vector2)finishPosition * gridScale) + new Vector3(gridScale / 2, gridScale / 2, 0));
                    stageCells[finishPosition.x, finishPosition.y] = targetSlime;
                    stageCells[attackPosition.x, attackPosition.y] = null;
                }
            }
        }
    }

    public void Bomb(Slime bombSlime)
    {
        var bombPos = bombSlime.position;

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

    bool isInStage(Vector2Int position)
    {
        return position.x >= 0 && position.x < stageCells.GetLength(0) && position.y >= 0 && position.y < stageCells.GetLength(1);
    }

    public Vector2Int ConvertToGrid(Vector3 position)
    {
        Vector2 pos = new Vector2((position.x - stageOriginPosition.x) / gridScale, (position.y - stageOriginPosition.y) / gridScale);
        return Vector2Int.FloorToInt(pos);
    }
}
