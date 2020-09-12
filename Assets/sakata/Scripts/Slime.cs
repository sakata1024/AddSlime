using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : StageObject
{
    public int number;
    public Vector2Int position;

    public bool canMove
    {
        get
        {
            return number <= 10;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(Vector2 position)
    {
        transform.position = position;
    }

    public void UnionTo(Vector2 position, Slime addSlime)
    {
        transform.position = position;
        addSlime.Add(number);
        Destroy(gameObject);
    }

    public void Add(int addNum)
    {
        number += addNum;
        if(number == 10)
        {
            stage.Bomb(this);
        }
    }

    public void Bomb()
    {
        Destroy(gameObject);
    }
}
