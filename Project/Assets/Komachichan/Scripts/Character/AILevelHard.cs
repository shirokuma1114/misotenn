using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILevelHard : AILevelBase
{


    protected override int JudgeSquare(CharacterBase character, List<SquareBase> squares)
    {
        // �ł��ǂ��}�X�̑I��
        int maxScore = -1;
        int index = -1;
        Debug.Log(character.Name + "�̈ړ��\�}�X�̃X�R�A");
        for (int i = 0; i < squares.Count; i++)
        {
            var score = squares[i].GetScore(character, CharacterType.COM1);
            Debug.Log(squares[i].name + ":" + squares[i].GetType() + ":" + score);
            if (maxScore < score)
            {
                maxScore = score;
                index = i;
            }
        }

        if (index < 0) return -1;
        return index;
    }
}
