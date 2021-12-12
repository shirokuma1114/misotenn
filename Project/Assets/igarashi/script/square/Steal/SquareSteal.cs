using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(StealEffectManager))]
public class SquareSteal : SquareBase
{
    public enum SquareStealState
    {
        IDLE,

        PAY,
        SLECT_TARGET,

        END,
    }
    private SquareStealState _state;
    public SquareStealState State => _state;


    CharacterBase _character;
    MessageWindow _messageWindow;
    StatusWindow _statusWindow;
    PayUI _payUI;
    private SouvenirWindow _souvenirWindow;

    SelectUI _selectUI;
    List<string> _selectElements;

    private List<CharacterBase> _otherCharacters;

    private StealEffectManager _effect;
    private CameraInterpolation _camera;

    [SerializeField]
    private int _cost;

    MyGameManager _gameManager;

    
    // Start is called before the first frame update
    void Start()
    {
        _messageWindow = FindObjectOfType<MessageWindow>();
        _statusWindow = FindObjectOfType<StatusWindow>();
        _payUI = FindObjectOfType<PayUI>();
        _souvenirWindow = FindObjectOfType<SouvenirWindow>();

        _selectUI = FindObjectOfType<SelectUI>();
        _selectElements = new List<string>();

        _effect = GetComponent<StealEffectManager>();

        _camera = Camera.main.GetComponent<CameraInterpolation>();

        _gameManager = FindObjectOfType<MyGameManager>();


        _squareInfo =
            "刀狩マス\n" +
            "コスト：" + _cost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case SquareStealState.IDLE:
                break;
            case SquareStealState.PAY:
                PayStateProcess();
                break;
            case SquareStealState.SLECT_TARGET:
                SelectTargetStateProcess();
                break;
            case SquareStealState.END:
                EndStateProcess();
                break;
        }
    }

    public override string GetSquareInfo(CharacterBase character)
    {
        int displayCost = CalcCost(character);

        _squareInfo =
           "刀狩マス\n" +
           "コスト：" + displayCost.ToString();

        return _squareInfo;
    }

    private int CalcCost(CharacterBase character)
    {
        // 1位 40000
        // 4位 10000

        return (4 - _gameManager.GetRank(character)) * _cost;
    }

    public override void Stop(CharacterBase character)
    {
        _character = character;

        int cost = CalcCost(_character);

        //お金チェック
        if (!_character.CanPay(cost))
        {
            _messageWindow.SetMessage("お金が足りません", character);
            _state = SquareStealState.END;
            return;
        }

        // 誰もお土産をもっていない
        if(!_gameManager.HasSouvenirByCharacters(character))
        {
            //_messageWindow.SetMessage("誰もお土産を持っていなかった！", character.IsAutomatic);
            //_state = SquareStealState.END;
            //return;
        }


        var message = cost.ToString() + "円を支払ってお土産を奪いますか？";
        _messageWindow.SetMessage(message, character);
        _statusWindow.SetEnable(true);
        _payUI.Open(character);

        _otherCharacters = new List<CharacterBase>();
        _otherCharacters.AddRange(FindObjectsOfType<CharacterBase>());
        _otherCharacters.Remove(_character.GetComponent<CharacterBase>());

        _selectElements.Clear();

        _state = SquareStealState.PAY;

        // 奪うか判断
        if (character.IsAutomatic)
        {
            Invoke("AISelectAutomatic", 1.5f);
        }
    }

    void AISelectAutomatic()
    {
        _payUI.AISelectYes();
    }

    private void PayStateProcess()
    {
        if (_payUI.IsSelectComplete && !_messageWindow.IsDisplayed)
        {
            if (_payUI.IsSelectYes)
            {
                _character.Log.AddUseEventNum(SquareEventType.STEAL);

                for (int i = 0; i < _otherCharacters.Count; i++)
                    _selectElements.Add(_otherCharacters[i].Name);
                _selectElements.Add("やめる");

                _selectUI.Open(_selectElements, _character);
                _messageWindow.SetMessage("誰からお土産を奪いますか？", _character);

                _state = SquareStealState.SLECT_TARGET;

                //AIの選択
                if (_character.IsAutomatic)
                {
                    Invoke("AISelectStealCharacter", 1.5f);
                }
            }
            else
            {
                _state = SquareStealState.END;
            }
        }
    }

    private void AISelectStealCharacter()
    {
        // タイプによって取る処理を変える
        //_character.CharacterType
        //var hasSouvenirCharacters = _otherCharacters.Where(x => x.Souvenirs.Count > 0).ToList();
        //if (hasSouvenirCharacters.Count == 0) return;

        // 相手がリーチの場合阻止する方を選ぶ 該当しない場合ランダム
        for(int i = 0; i < _otherCharacters.Count; i++)
        {
            if (_otherCharacters[i].GetSouvenirTypeNum() + 1 == _gameManager.GetNeedSouvenirType())
            {
                _selectUI.IndexSelect(i);
                return;
            }
        }


        // 検索の最初の位置
        var item = Random.Range(0, _otherCharacters.Count);

        for(int i = 0; i < _otherCharacters.Count; i++)
        {
            if (_otherCharacters[item].Souvenirs.Count > 0)
            {
                _selectUI.IndexSelect(item);
                return;
            }
            item = MathUtils.Wrap(++item, 0, _otherCharacters.Count);
        }
    }

    private void SelectTargetStateProcess()
    {
        
        if(_selectUI.IsComplete && !_messageWindow.IsDisplayed)
        {
            if(_selectUI.SelectIndex == _otherCharacters.Count)
            {
                _state = SquareStealState.END;
                return;
            }

            CharacterBase targetCharacter = _otherCharacters[_selectUI.SelectIndex];

            if (targetCharacter.gameObject.GetComponent<Protector>().IsProtected)
            {
                _messageWindow.SetMessage(targetCharacter.Name + "は身を守られている", _character);
                _selectUI.Open(_selectElements, _character);

                return;
            }
            else if(targetCharacter.Souvenirs.Count == 0)
            {
                _messageWindow.SetMessage(targetCharacter.Name + "お土産を持っていない", _character);
                _selectUI.Open(_selectElements, _character);

                return;
            }
            else
            {
                var targetSouvenirIndex = Random.Range(0, targetCharacter.Souvenirs.Count);
                var targetSouvenir = targetCharacter.Souvenirs[targetSouvenirIndex];

                _character.AddSouvenir(targetSouvenir);
                targetCharacter.RemoveSouvenir(targetSouvenirIndex);

                var message = _character.Name + "は" + targetCharacter.Name + "の" + targetSouvenir.Name + "を奪った！";
                _messageWindow.SetMessage(message, _character);

                _souvenirWindow.SetSouvenirs(_character.Souvenirs);
                _souvenirWindow.SetEnable(true);

                _character.SubMoney(CalcCost(_character));

                //演出
                _effect.EffectStart(targetCharacter, _character,targetSouvenir.Sprite);
                _camera.Enter_Event();
                _camera.Set_NextCamera(0);

            }

            _state = SquareStealState.END;
        }
    }

    private void EndStateProcess()
    {
        if (!_messageWindow.IsDisplayed && !_effect.IsEffectPlaying)
        {
            _character.CompleteStopExec();
            _statusWindow.SetEnable(false);
            _camera.Leave_Event();

            _state = SquareStealState.IDLE;
        }
    }

    public override int GetScore(CharacterBase character, CharacterType characterType)
    {
        // コストが足りない
        if (CalcCost(character) > character.Money) return base.GetScore(character, characterType);

        // 奪うものが無い
        if (_gameManager.GetCharacters(character).Where(x => x.Souvenirs.Count > 0).Count() == 0) return (int)SquareScore.NONE_STEAL + base.GetScore(character, characterType);

        // 持ってないお土産を持っているプレイヤーがいる
        var characters = _gameManager.GetCharacters(character);

        // 持ってないカードリスト
        var dontHaveTypes = new HashSet<SouvenirType>();
        for (int i = 0; i < (int)SouvenirType.MAX_TYPE; i++)
        {
            // カードが無い
            if (character.Souvenirs.Where(x => x.Type == (SouvenirType)i).Count() >= 1)
            {
                dontHaveTypes.Add((SouvenirType)i);
            }
        }

        foreach(var x in characters)
        {
            foreach (var y in dontHaveTypes)
            {
                // 持っていないお土産を持っている
                if (x.Souvenirs.Where(z => z.Type == y).Count() > 0)
                {
                    // 揃えば勝ち
                    if(character.GetSouvenirTypeNum() == 5)
                    {
                        return (int)SquareScore.DONT_HAVE_SOUVENIR_TO_WIN + base.GetScore(character, characterType);
                    }
                    return (int)SquareScore.DONT_HAVE_STEAL + base.GetScore(character, characterType);
                }
            }
        }
        return (int)SquareScore.STEAL + base.GetScore(character, characterType);
    }
}
