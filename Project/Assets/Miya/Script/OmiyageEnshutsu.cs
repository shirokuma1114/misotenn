using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;

public class OmiyageEnshutsu : MonoBehaviour
{
	// 外部で読み込む関数
	public void Use_OmiyageEnshutsu(string _country_name)
	{
		if (CountryNames.Length == OmiyageSprites.Length)
		{
			for (int i = 0; i < CountryNames.Length; i++)
			{
				if (CountryNames[i] == _country_name)
				{
					ImageComponent.sprite = OmiyageSprites[i];
					Set_sequence();
                    _animComplete = false;
                    _particlePlaying = false;

                    break;
				}
			}
		}
		else Debug.Log("Error : OmiyageEnshutsu.cs, ListLength");
	}

    public void Use_OmiyageEnshutsu(Sprite souvenirSprite)
    {
        ImageComponent.sprite = souvenirSprite;
        Set_sequence();
        _animComplete = false;
        _particlePlaying = false;
    }

    // List
    public string[] CountryNames;
	public Sprite[] OmiyageSprites;

	public float Second_Display = 1.5f;

	public GameObject Particle;

	// Variable
	Image ImageComponent;

	// Animation
	Sequence Animation_Sequence;
	Sequence Initial_Sequence;
	RectTransform Rect;

    //
    bool _animComplete = false;
    public bool IsAnimComplete => _animComplete;

    GameObject _particle;
    bool _particlePlaying;

	AudioSource Puni;


	// Start
	void Start()
    {
		// Variable
		ImageComponent = this.GetComponent<Image>();

		// Animation
		Rect = this.GetComponent<RectTransform>();
		// Initialize
		Initial_Sequence = DOTween.Sequence();
		Initial_Sequence.Append(Rect.DOLocalMove(new Vector3(50, -120, 0), 0));
		Initial_Sequence.Join(Rect.DOScale(new Vector3(0, 0, 0), 0));
	}

    // Update
    void Update()
    {
        // Debug
        //if (Input.GetKeyUp(KeyCode.A)) Use_OmiyageEnshutsu("Australia");

        if(_particlePlaying)
        {
            if (_particle == null)
                _animComplete = true;
        }
	}

	// Animation Sequence
	void Set_sequence()
	{
		Animation_Sequence = DOTween.Sequence();

		// 移動・拡大AnimationSequence
		Animation_Sequence.Append(Rect.DOLocalMove(new Vector3(160, 10, 0), 1));
		Animation_Sequence.Join(Rect.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 1));
		Animation_Sequence.Join(Rect.DORotate(new Vector3(0, 0, -15), 1));

		// ぼよよん
		Puni = Control_SE.Get_Instance().Play_SE("Omiyage_Puni");
		Puni.pitch = 1.5f;

		float difference = 0.2f;
		float duration = 0.1f;
		for (int i = 0; i < 3; i++)
		{
			duration = duration + i * 0.1f;
			Animation_Sequence.Append(Rect.DOScaleY(1.25f - difference, duration));
			Animation_Sequence.Join(Rect.DOScaleX(1.25f + difference, duration));
			Animation_Sequence.Append(Rect.DOScaleY(1.25f + difference, duration));
			NewMethod(difference, duration);
		}
		Animation_Sequence.Append(Rect.DOScale(new Vector3(1.25f, 1.25f, 1.25f), duration)
			.OnComplete(Completed_Puni));
		Animation_Sequence.AppendInterval(Second_Display)
			.OnComplete(Do_Shu);

		// 移動・縮小
		Animation_Sequence.Append(Rect.DOLocalMove(new Vector3(20, 0, 0), 1));
		Animation_Sequence.Join(Rect.DOScale(new Vector3(0, 0, 0), 1));
		Animation_Sequence.Join(Rect.DORotate(new Vector3(0, 0, -360), 1));

		// 初期化
		Animation_Sequence.Append(Rect.DOLocalMove(new Vector3(50, -120, 0), 0));
		Animation_Sequence.Join(Rect.DORotate(new Vector3(0, 0, 0), 0)
			.OnComplete(Completed));
	}

	private void NewMethod(float difference, float duration)
	{
		Animation_Sequence.Join(Rect.DOScaleX(1.25f - difference, duration));
	}

	// OnComplete
	private void Completed()
	{
		_particle = Instantiate(Particle);
        _particlePlaying = true;
	}
	private void Completed_Puni()
	{
		Control_SE.Get_Instance().Stop_SE(Puni);
	}
	private void Do_Shu()
	{
		Control_SE.Get_Instance().Play_SE("Omiyage_Shu");
	}
	// OnDisable
	private void OnDisable()
	{
		if (Initial_Sequence != null) Initial_Sequence.Kill();
		if (Animation_Sequence != null) Animation_Sequence.Kill();
	}
}
