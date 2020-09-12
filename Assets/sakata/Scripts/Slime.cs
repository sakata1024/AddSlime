﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : StageObject
{
    public int number;
    public Vector2Int position;

    //動かせるかどうか(数字による)
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

    // positionへ移動
    public void MoveTo(Vector2 position)
    {
        transform.position = position;
    }

    // addSlimeにぶつかるまで移動する
    public void UnionTo(Vector2 position, Slime addSlime)
    {
        transform.position = position;

        //以下はぶつかったあとの処理
        addSlime.Add(number);
        Destroy(gameObject);
    }

    // 数字が足される
    public void Add(int addNum)
    {
        number += addNum;
        if(number == 10)
        {
            // ここでほかのスライムに爆発処理がかかる
            stage.Bomb(this);
        }
    }

    // 爆発する処理
    public void Bomb()
    {
        Destroy(gameObject);
    }
}