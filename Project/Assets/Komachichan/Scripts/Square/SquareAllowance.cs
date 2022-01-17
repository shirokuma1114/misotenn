using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAllowance : SquareBase
{
    enum Phase
    {
        NONE,
        INIT_MESSAGE,
        ROULETTE,
        GET_MESSAGE
    }


    CharacterBase _character;

    MessageWindow _messageWindow;

    StatusWindow _statusWindow;

    RouletteUI _rouletteUI;

    Phase _phase;

    OkozukaiEnshutsu _effect;

    [SerializeField]
    List<int> _rouletteValues = new List<int>() { 1000,2000,3000,4000,5000,6000,10000 };

    private void Start()
    {
        _rouletteUI = FindObjectOfType<RouletteUI>();
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _phase = Phase.NONE;

       //�����Ƀ\�[�g
        _rouletteValues.Sort();

        _effect = FindObjectOfType<OkozukaiEnshutsu>();

        _squareInfo = "���������}�X";
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;

        // �C���X�^���X����
        var message =  "�����������[���b�g�X�^�[�g�I";

        _messageWindow.SetMessage(message, character);
        _statusWindow.SetEnable(true);

        _phase = Phase.INIT_MESSAGE;
    }

    private void Update()
    {
        if (_phase == Phase.NONE) return;

        if (_phase == Phase.INIT_MESSAGE)
        {
            if (!_messageWindow.IsDisplayed)
            {
                _phase = Phase.ROULETTE;
                // �����Â������[���b�g
                BeginRoulette();
            }
        }
        if(_phase == Phase.ROULETTE)
        {
            if (_rouletteUI.GetSelectedItem() != null)
            {
                // ���������l������ �� ���b�Z�[�W ���@���o
                _rouletteUI.GetSelectedItem().Select(_character);
                _phase = Phase.GET_MESSAGE;


                var selectedItem = (RouletteItemAllowance) _rouletteUI.GetSelectedItem();
                int selectedItemIndex = _rouletteValues.FindIndex(m => m == selectedItem.GetValue );
                int effectLevel = selectedItemIndex * 10 / _rouletteValues.Count; //���o�̃��x���v�Z�@10�̓��x���̍ő�

                _effect.Start_OkozukaiEnshutsu(effectLevel);
                Control_SE.Get_Instance().Play_SE("Money");
            }
        }

        if(_phase == Phase.GET_MESSAGE)
        {
            if (!_messageWindow.IsDisplayed)
            {
                // �~�܂鏈���I��
                _character.CompleteStopExec();
                _phase = Phase.NONE;
                _rouletteUI.End();
                _statusWindow.SetEnable(false);
                _effect.Kill();
            }
        }
    }

    private void BeginRoulette()
    {
        //_rouletteUI.AddItem(new RouletteItemAllowance(1000)).AddItem(new RouletteItemAllowance(2000)).AddItem(new RouletteItemAllowance(3000)).AddItem(new RouletteItemAllowance(4000))
        //    .AddItem(new RouletteItemAllowance(5000)).AddItem(new RouletteItemAllowance(6000)).AddItem(new RouletteItemAllowance(10000))
        //    .Begin(_character);

        foreach(var values in _rouletteValues)
        {
            _rouletteUI.AddItem(new RouletteItemAllowance(values));
        }
        _rouletteUI.Begin(_character);
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        if(character.Money < 50000)
        {
            return (int)SquareScore.ALLOWANCE_50000_OR_LESS + base.GetScore(character, characterType);
        }
        return (int)SquareScore.ALLOWANCE_50000_OR_MORE + base.GetScore(character, characterType);
    }
}
