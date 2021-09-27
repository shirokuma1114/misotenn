using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CharacterControllerBase
{


    bool _isMoved = false;

    SquareBase[] _directionRoots;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �ړ��J�[�h��I��
    public override void Move()
    {
        var movingCount = _character.MovingCards[0];


        // �Ƃ肠�����O�Ԗڂ�I��
        _character.RemoveMovingCard(0);

        // UI�ɒʒm
        NotifyMovingCount(movingCount);

        _isMoved = true;
        
        // ���[�g����
        SetSquareRoots();
        NotifyArrow();
    }

    private void NotifyArrow()
    {

    }

    public void SetSquareRoots()
    {
        // �S�����̑I�����𐶐�

        //TODO �O�i�H��ށH

        _directionRoots = new SquareBase[4];

        // ��0 �� 1 �E 2 �� 3 �̏��Ɍ������K�؂ȃ}�X������
        var squarePos = _character.CurrentSquare.GetPosition();
        
        foreach(var x in _character.GetInConnects())
        {
            var pos = (x.GetPosition() - squarePos);
            pos.y = 0.0f;
            if (Vector3.Dot(new Vector3(-1, 0, 0), pos.normalized) > 0.8f){
                _directionRoots[0] = x;
            }
            if (Vector3.Dot(new Vector3(0, 1, 0), pos.normalized) > 0.8f){
                _directionRoots[1] = x;
            }
            if (Vector3.Dot(new Vector3(1, 0, 0), pos.normalized) > 0.8f){
                _directionRoots[2] = x;
            }
            if (Vector3.Dot(new Vector3(0, -1, 0), pos.normalized) > 0.8f){
                _directionRoots[3] = x;
            }
        }

        foreach(var x in _character.GetOutConnects())
        {
            var pos = (x.GetPosition() - squarePos);
            pos.y = 0.0f;
            if (Vector3.Dot(new Vector3(-1, 0, 0), pos.normalized) > 0.8f){
                _directionRoots[0] = x;
            }
            if (Vector3.Dot(new Vector3(0, -1, 0), pos.normalized) > 0.8f){
                _directionRoots[1] = x;
            }
            if (Vector3.Dot(new Vector3(1, 0, 0), pos.normalized) > 0.8f){
                _directionRoots[2] = x;
            }
            if (Vector3.Dot(new Vector3(0, 1, 0), pos.normalized) > 0.8f){
                _directionRoots[3] = x;
            }
        }

        // �I�����̕\��
    }

    private void UpdateMove()
    {
        if (!_isMoved) return;

        if (_character.IsMoved) return;

        var inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(_directionRoots[0] && Vector2.Dot(inputDir, new Vector2(-1.0f, 0.0f)) < 0.8f)
        {
            _character.StartMove(_directionRoots[0]);
        }
        if(_directionRoots[1] && Vector2.Dot(inputDir, new Vector2(0.0f, -1.0f)) < 0.8f)
        {
            _character.StartMove(_directionRoots[1]);
        }
        if(_directionRoots[2] && Vector2.Dot(inputDir, new Vector2(1.0f, 0.0f)) < 0.8f)
        {
            _character.StartMove(_directionRoots[2]);
        }
        if(_directionRoots[3] && Vector2.Dot(inputDir, new Vector2(0.0f, 1.0f)) < 0.8f)
        {
            _character.StartMove(_directionRoots[3]);
        }

    }

    void NotifyMovingCount(int count)
    {
        GameObject.Find("Text").GetComponent<Text>().text = count.ToString();
    }
}
