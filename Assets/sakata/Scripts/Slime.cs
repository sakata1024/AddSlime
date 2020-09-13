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
        //登場時にぷよぷよします
        transform.DOPunchScale(
    new Vector3(0.3f, 0.3f),    // scale1.5倍指定
    0.3f                        // アニメーション時間
);
        SoundPlayer.Instance.PlaySE("Slime");
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
        var difvec = new Vector2(transform.position.x, this.transform.position.y) - position;
        var transtime = 0.5f * difvec.magnitude;
        transform.DOMove(position, transtime);

        //以下はぶつかったあとの処理
        DOVirtual.DelayedCall(
            transtime,   // 遅延させる（待機する）時間
            () => {
                addSlime.Add(number);
                Destroy(gameObject);
                SoundPlayer.Instance.PlaySE("PluSlime");
            }
        );
    }

    // 数字が足される
    public void Add(int addNum)
    {
        number += addNum;
        transform.DOShakeScale(0.5f);
        if (number == 10)
        {
            // ここでほかのスライムに爆発処理がかかる
            stage.Bomb(this);
        }
        else if(number > 10)
        {
            gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
        }
    }

    // 爆発する処理
    public void Bomb()
    {
        ParticleSystem expl = Instantiate(particle, transform.position, transform.rotation) as ParticleSystem;
        expl.Play();
        SoundPlayer.Instance.PlaySE("bomb");
        Destroy(gameObject, 0.5f);
        Destroy(expl.gameObject, 0.5f);
    }
}
