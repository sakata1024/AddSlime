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
    public Vector3 stageOriginPosition = new Vector3(-2f, -2f);
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
    }

    public void Attack(Vector2Int direction)
    {
        Vector2Int attackPosition = characterPoint + direction;

        if (isInStage(attackPosition) && stageCells[attackPosition.x, attackPosition.y]!= null && stageCells[attackPosition.x, attackPosition.y].GetType() == typeof(Slime))
        {
            Slime targetSlime = (Slime)stageCells[attackPosition.x, attackPosition.y];
            Vector2Int finishPosition = attackPosition + direction;
            bool canUnion = false;
            for (Vector2Int searchPosition = attackPosition + direction; isInStage(searchPosition); searchPosition += direction)
            {
                // TODO:壁があればbreak
                // TODO:スライムがあれば、数字によって変える
                finishPosition = searchPosition;
            }
            if (finishPosition != attackPosition)
            {
                if (canUnion)
                {
                    // スライムが加算するときの処理
                }
                else
                {
                    targetSlime.MoveTo(stageOriginPosition + (Vector3)((Vector2)finishPosition * gridScale));
                    stageCells[finishPosition.x, finishPosition.y] = targetSlime;
                    stageCells[attackPosition.x, attackPosition.y] = null;
                }
            }
        }
    }

    public void Bomb(Slime bombSlime)
    {

    }

    bool isInStage(Vector2Int position)
    {
        return position.x >= 0 && position.x < stageCells.GetLength(0) && position.y >= 0 && position.y < stageCells.GetLength(1);
    }

    public Vector2Int ConvertToGrid(Vector3 position)
    {
        Vector2 pos = new Vector2((position.x - stageOriginPosition.x) / gridScale, (position.y - stageOriginPosition.y) / gridScale);
        return Vector2Int.CeilToInt(pos);
    }
}
