using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CharacterControllerBase
{


    bool _isMoved = false;

    SquareBase[] _roots;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 移動カードを選ぶ
    public override void Move()
    {
        var movingCount = _character.MovingCards[0];


        // とりあえず０番目を選ぶ
        _character.RemoveMovingCard(0);

        // UIに通知
        NotifyMovingCount(movingCount);

        _isMoved = true;
        
        // ルート生成
        SetSquareRoots();
        NotifyArrow();
    }

    private void NotifyArrow()
    {

    }

    public void SetSquareRoots()
    {
        // ４方向の選択肢を生成
        
        _roots = new SquareBase[4];

        // 左0 上 1 右 2 下 3 の順に向きが適切なマスを入れる
        var squarePos = _character.CurrentSquare.GetPosition();
        
        foreach(var x in _character.GetInConnects())
        {
            var pos = (x.GetPosition() - squarePos);
            pos.y = 0.0f;
            if (Vector3.Dot(new Vector3(-1, 0, 0), pos.normalized) > 0.8f){
                _roots[0] = x;
            }
            if (Vector3.Dot(new Vector3(0, 1, 0), pos.normalized) > 0.8f){
                _roots[1] = x;
            }
            if (Vector3.Dot(new Vector3(1, 0, 0), pos.normalized) > 0.8f){
                _roots[2] = x;
            }
            if (Vector3.Dot(new Vector3(0, -1, 0), pos.normalized) > 0.8f){
                _roots[3] = x;
            }
        }

        foreach(var x in _character.GetOutConnects())
        {
            var pos = (x.GetPosition() - squarePos);
            pos.y = 0.0f;
            if (Vector3.Dot(new Vector3(-1, 0, 0), pos.normalized) > 0.8f){
                _roots[0] = x;
            }
            if (Vector3.Dot(new Vector3(0, 1, 0), pos.normalized) > 0.8f){
                _roots[1] = x;
            }
            if (Vector3.Dot(new Vector3(1, 0, 0), pos.normalized) > 0.8f){
                _roots[2] = x;
            }
            if (Vector3.Dot(new Vector3(0, -1, 0), pos.normalized) > 0.8f){
                _roots[3] = x;
            }
        }

        // 選択肢の表示
    }

    private void UpdateMove()
    {
        if (!_isMoved) return;

        if (_character.IsMoved) return;

        var inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(Vector2.Dot(inputDir, new Vector2(-1.0f, 0.0f)) < 0.8f)
        {
            _character.StartMove(_roots[0]);
        }

    }

    void NotifyMovingCount(int count)
    {
        GameObject.Find("Text").GetComponent<Text>().text = count.ToString();
    }
}
