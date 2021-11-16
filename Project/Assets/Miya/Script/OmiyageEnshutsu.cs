using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;

public class OmiyageEnshutsu : MonoBehaviour
{
	// äOïîÇ≈ì«Ç›çûÇﬁä÷êî
	public void Use_OmiyageEnshutsu(string _country_name)
	{
		if (CountryNames.Length == OmiyageSprites.Length)
		{
			for (int i = 0; i < CountryNames.Length; i++)
			{
				if (CountryNames[i] == _country_name)
				{
					ImageComponent.sprite = OmiyageSprites[i];
					Set_AnimationSequence();
					break;
				}
			}
		}
		else Debug.Log("Error : OmiyageEnshutsu.cs, ListLength");
	}

	// List
	public string[] CountryNames;
	public Sprite[] OmiyageSprites;

	public float Second_Display = 1.5f;

	public GameObject Particle;

	// Variable
	Image ImageComponent;

	// Animation
	RectTransform Rect;

	// Start
	void Start()
    {
		// Variable
		ImageComponent = this.GetComponent<Image>();

		// Animation
		Rect = this.GetComponent<RectTransform>();
		// Initialize
		Sequence AnimationSequence = DOTween.Sequence();
		AnimationSequence.Append(Rect.DOLocalMove(new Vector3(50, -120, 0), 0));
		AnimationSequence.Join(Rect.DOScale(new Vector3(0, 0, 0), 0));
	}

    // Update
    void Update()
    {
		// Debug
		if (Input.GetKeyUp(KeyCode.A)) Use_OmiyageEnshutsu("Australia");
	}

	// Animation Sequence
	void Set_AnimationSequence()
	{
		Sequence AnimationSequence = DOTween.Sequence();

		// à⁄ìÆÅEägëÂ
		AnimationSequence.Append(Rect.DOLocalMove(new Vector3(160, 10, 0), 1));
		AnimationSequence.Join(Rect.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 1));
		AnimationSequence.Join(Rect.DORotate(new Vector3(0, 0, -15), 1));

		// Ç⁄ÇÊÇÊÇÒ
		float difference = 0.2f;
		float duration = 0.1f;
		for (int i = 0; i < 3; i++)
		{
			duration = duration + i * 0.1f;
			AnimationSequence.Append(Rect.DOScaleY(1.25f - difference, duration));
			AnimationSequence.Join(Rect.DOScaleX(1.25f + difference, duration));
			AnimationSequence.Append(Rect.DOScaleY(1.25f + difference, duration));
			AnimationSequence.Join(Rect.DOScaleX(1.25f - difference, duration));
		}
		AnimationSequence.Append(Rect.DOScale(new Vector3(1.25f, 1.25f, 1.25f), duration));
		AnimationSequence.AppendInterval(Second_Display);

		// à⁄ìÆÅEèkè¨
		AnimationSequence.Append(Rect.DOLocalMove(new Vector3(20, 0, 0), 1));
		AnimationSequence.Join(Rect.DOScale(new Vector3(0, 0, 0), 1));
		AnimationSequence.Join(Rect.DORotate(new Vector3(0, 0, -360), 1));

		// èâä˙âª
		AnimationSequence.Append(Rect.DOLocalMove(new Vector3(50, -120, 0), 0));
		AnimationSequence.Join(Rect.DORotate(new Vector3(0, 0, 0), 0)
			.OnComplete(Completed));
	}
	
	// OnComplete
	private void Completed()
	{
		Instantiate(Particle);
	}
	// OnDisable
	private void OnDisable()
	{
		//if (AnimationSequence != null) AnimationSequence.Kill();
	}
}
