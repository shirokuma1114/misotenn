using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILevelEasy : AILevelBase
{

    protected override int JudgeSquare(CharacterBase character, List<SquareBase> squares)
    {
        // �ł��ǂ��}�X�̑I��
        int maxScore = -1;
        int index = -1;
        //Debug.Log(_character.Name + "�̈ړ��\�}�X�̃X�R�A");
        for (int i = 0; i < squares.Count; i++)
        {
            var score = squares[i].GetScore(character, CharacterType.COM1);
            //Debug.Log(squares[i].name + ":" + score);
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
