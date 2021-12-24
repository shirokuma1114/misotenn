using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartOrderWindow : WindowBase
{
    [SerializeField]
    private MyGameManager _gameManager;

    [SerializeField]
    private List<Image> _cakeImage;

    [SerializeField]
    private List<GameObject> _cakeTransforms;

    [SerializeField]
    private List<CharacterIcon> _characterIcons;



    public void SetUpCakes()
    {
        var characterList = _gameManager.GetCharacters();

        for (int i = 0; i < characterList.Count; i++)
        {
            var character = characterList[i];

            foreach (var icon in _characterIcons)
            {
                if (icon.Name == character.Name)
                {
                    _cakeImage[i].sprite = icon.Sprite;
                    break;
                }
            }
        }
    }

    public override void SetEnable(bool enable)
    {
        gameObject.SetActive(enable);

        if (enable)
        {
            SetUpCakes();
        }
    }

    //-------------------------

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
