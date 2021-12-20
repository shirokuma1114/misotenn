using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;

public class SelectSelectWindow : MonoBehaviour
{
    [SerializeField]
    Animator _fadeAnimation;

    [SerializeField]
    List<RectTransform> _selectRts;

    int _selectedIndex;

    Vector2[] _initPositions;

    [SerializeField]
    List<Vector2> _movePositions;

    [SerializeField]
    Ease _inEase;

    [SerializeField]
    Ease _outEase;

    [SerializeField]
    SelectPlanet _selectPlanet;

    bool _isFade;

    float _beforeTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _fadeAnimation.Play("FadeIn");

        // -280 -120 

        _initPositions = new Vector2[_selectRts.Count];

        for(int i = 0; i < _selectRts.Count; i++)
        {
            _initPositions[i] = _selectRts[i].anchoredPosition;
        }

        _selectRts[_selectedIndex].DOAnchorPos(_initPositions[_selectedIndex] + _movePositions[_selectedIndex], 0.5f).SetEase(Ease.InBounce);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFade && _fadeAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FadeOut" && _fadeAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            SceneManager.LoadScene("NewTincleScene");
        }

        if (_isFade) return;

        float viewButton = Input.GetAxis("Vertical");
        if (_beforeTrigger == 0.0f)
        {
            if (viewButton > 0)
            {
                var newIndex = _selectedIndex - 1;
                if (newIndex < 0) newIndex = _selectRts.Count - 1;
                ChangeSelect(newIndex);

                if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
            }
            if (viewButton < 0)
            {
                var newIndex = _selectedIndex + 1;
                if (newIndex == _selectRts.Count) newIndex = 0;
                ChangeSelect(newIndex);

                if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Select");
            }
        }
        _beforeTrigger = viewButton;

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("A") || Input.GetButtonDown("Start"))
        {
            Apply();
            if (Control_SE.Get_Instance()) Control_SE.Get_Instance().Play_SE("UI_Correct");
        }
    }


    private void Apply()
    {
        AllocateEntryCharacters();
        _isFade = true;
        _fadeAnimation.Play("FadeOut");
    }

    private void AllocateEntryCharacters()
    {
        CharacterType[] characterTypes = new CharacterType[4];
        characterTypes[0] = CharacterType.PLAYER1;
        if(_selectedIndex == 0)
        {
            characterTypes[1] = CharacterType.COM2;
            characterTypes[2] = CharacterType.COM3;
            characterTypes[3] = CharacterType.COM4;
        }
        if(_selectedIndex == 1)
        {
            characterTypes[1] = CharacterType.PLAYER2;
            characterTypes[2] = CharacterType.COM3;
            characterTypes[3] = CharacterType.COM4;
        }
        if(_selectedIndex == 2)
        {
            characterTypes[1] = CharacterType.PLAYER2;
            characterTypes[2] = CharacterType.PLAYER3;
            characterTypes[3] = CharacterType.COM4;
        }
        if(_selectedIndex == 3)
        {
            characterTypes[1] = CharacterType.PLAYER2;
            characterTypes[2] = CharacterType.PLAYER3;
            characterTypes[3] = CharacterType.PLAYER4;
        }
        var random = new System.Random();
        characterTypes = characterTypes.OrderBy(x => random.Next()).ToArray();
        FindObjectOfType<DontDestroyManager>().SetInitCharacterTypes(characterTypes);
    }

    void InitSelect()
    {
        _selectRts[_selectedIndex].DOAnchorPos(_initPositions[_selectedIndex] + _movePositions[_selectedIndex], 0.5f).SetEase(Ease.InBounce);
    }

    void ChangeSelect(int newIndex)
    {
        // Œ³‚ÌˆÊ’u‚É–ß‚·
        _selectRts[_selectedIndex].DOAnchorPos(_initPositions[_selectedIndex], 0.2f).SetEase(_outEase);
        
        _selectRts[newIndex].DOAnchorPos(_initPositions[newIndex] + _movePositions[newIndex], 0.2f).SetEase(_inEase).SetDelay(0.1f);

        _selectedIndex = newIndex;

        _selectPlanet.SetIndex(_selectedIndex);
    }
}
