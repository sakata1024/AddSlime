using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : StageObject
{
    public float defaultSpeed = 1f;

    public Vector2Int direction = Vector2Int.right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2Int currentDirection = direction;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentDirection = Vector2Int.right;
            transform.position += (Vector3)(Vector2)currentDirection * defaultSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentDirection = Vector2Int.left;
            transform.position += (Vector3)(Vector2)currentDirection * defaultSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentDirection = Vector2Int.up;
            transform.position += (Vector3)(Vector2)currentDirection * defaultSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentDirection = Vector2Int.down;
            transform.position += (Vector3)(Vector2)currentDirection * defaultSpeed * Time.deltaTime;
        }

        float clampX = Mathf.Clamp(transform.position.x, stage.ConvertToWorldPosition(new Vector2Int(0, 0)).x, stage.ConvertToWorldPosition(new Vector2Int(stage.stageSizeX - 1, 0)).x);
        float clampY = Mathf.Clamp(transform.position.y, stage.ConvertToWorldPosition(new Vector2Int(0, 0)).y, stage.ConvertToWorldPosition(new Vector2Int(0, stage.stageSizeY - 1)).y);
        transform.position = new Vector3(clampX, clampY);

        var gridPos = stage.ConvertToGrid(transform.position);
        stage.characterPoint = gridPos;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stage?.Attack(direction);
        }
    }
}
