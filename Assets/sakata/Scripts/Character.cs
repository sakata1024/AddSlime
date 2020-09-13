using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Character : StageObject
{
    public float defaultSpeed = 1f;

    public Vector2Int direction = Vector2Int.right;
    // 宇野がパンチするために時間のカウントをおきました。
    private float cnt;

    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void moveTo(Vector2 position)
    {
        isMoving = true;
        var difvec = new Vector2(transform.position.x, this.transform.position.y) - position;
        var transtime = defaultSpeed * difvec.magnitude;
        transform.DOMove(position, transtime);

        //以下は移動したあとの処理
        DOVirtual.DelayedCall(
            transtime,
            () => {
                transform.position = position;
                isMoving = false;
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        cnt += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && cnt > 0.4f)
        {
            cnt = 0;
            transform.DOPunchScale(new Vector3(0.3f, 0.3f), 0.3f);
            stage?.Attack(direction);
        }

        if (isMoving)
        {
            return;
        }

        Vector2Int currentDirection = direction;
        bool keyPressed = false;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            keyPressed = true;
            currentDirection = Vector2Int.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            keyPressed = true;
            currentDirection = Vector2Int.left;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            keyPressed = true;
            currentDirection = Vector2Int.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            keyPressed = true;
            currentDirection = Vector2Int.down;
        }

        if (!keyPressed)
        {
            return;
        }

        if (direction != currentDirection)
        {
            direction = currentDirection;
            if (direction == Vector2Int.right)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (direction == Vector2Int.left)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 180f);
            }
            else if (direction == Vector2Int.up)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 90f);
            }
            else if (direction == Vector2Int.down)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 270f);
            }
        }

        Vector2 newPosition = transform.position + (Vector3)(Vector2)currentDirection * defaultSpeed;
        float clampX = Mathf.Clamp(newPosition.x, stage.ConvertToWorldPosition(new Vector2Int(0, 0)).x, stage.ConvertToWorldPosition(new Vector2Int(stage.stageSizeX - 1, 0)).x);
        float clampY = Mathf.Clamp(newPosition.y, stage.ConvertToWorldPosition(new Vector2Int(0, 0)).y, stage.ConvertToWorldPosition(new Vector2Int(0, stage.stageSizeY - 1)).y);
        newPosition = new Vector2((int)clampX, (int)clampY);
        var gridPos = stage.ConvertToGrid(newPosition);
      
        if (stage.stageCells[gridPos.x, gridPos.y] is Wall)
        {
            return;
        }
        if (stage.stageCells[gridPos.x, gridPos.y] is Slime)
        {
            return;
        }

        moveTo(newPosition);
        stage.characterPoint = gridPos;
    }
}
