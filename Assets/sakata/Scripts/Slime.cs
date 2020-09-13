using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Slime : StageObject
{
    public int number;
    public Vector2Int position;

    public ParticleSystem particle;
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
        //速度を一定(0.5f/s)にするために、移動時間に距離をかけてます。
        var difvec = new Vector2(transform.position.x, this.transform.position.y) - position;
        transform.DOMove(position, 0.5f * difvec.magnitude);
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
        else if(number > 11)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
        }
    }

    // 爆発する処理
    public void Bomb()
    {
        ParticleSystem expl = Instantiate(particle, transform.position, transform.rotation) as ParticleSystem;
        expl.Play();
        Destroy(gameObject);//, expl.duration);
    }
}
