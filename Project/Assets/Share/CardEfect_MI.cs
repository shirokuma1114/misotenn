using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardEfect_MI : MonoBehaviour
{
	private int _index;
	private List<Tween> _tweens = new List<Tween>();


	// miya
	CardEfect_Manager_MI Manager;
	public float RotateSpeed_FinishAnimation = -360;


	public void OnClick()
	{
		transform.parent.gameObject.GetComponent<MoveCardManager>().IndexSelect(_index);
	}

	public void SetIndex(int index)
	{
		_index = index;
	}

	public void SetMoveTargetPos(Vector3 targetPos, bool last)
	{
		var rt = GetComponent<RectTransform>();
		
		if (!last)
		{
			_tweens.Add(rt.DOMove(targetPos, 1.0f));
			_tweens.Add(rt.DORotate(new Vector3(0, 0, 0), 1.0f));
		}
		else
		{
			var seq = DOTween.Sequence();
			_tweens.Add(seq.AppendInterval(1.0f));
			_tweens.Add(seq.Append(rt.DOMove(targetPos, 1.0f)));
			_tweens.Add(seq.Join(rt.DORotate(new Vector3(0, 0, RotateSpeed_FinishAnimation), 1.0f, RotateMode.WorldAxisAdd)));

			seq.Play(); 
		}
	}

	// miya
	public void Play_FinishAnimation()
	{
		var rt = GetComponent<RectTransform>();
		
		var seq = DOTween.Sequence();
		_tweens.Add(seq.AppendInterval(1.0f));
		
		_tweens.Add(seq.Append(rt.DOMove(new Vector3(Screen.width / 2, Screen.height / 2, 0), 1.0f)));
		_tweens.Add(seq.Append(rt.DORotate(new Vector3(0, 0, -360), 1.0f, RotateMode.WorldAxisAdd)));
		_tweens.Add(seq.Join(rt.DOScale(new Vector3(0, 0, 0), 2.0f)));

		seq.Play();
		if ( seq.IsComplete() ) Manager.Set_FinishAnimation();
	}

	//================================================

	void Start()
	{
		Manager = FindObjectOfType<CardEfect_Manager_MI>();
	}

	void Update()
	{

	}

	private void OnDisable()
	{
		if (_tweens.Count != 0)
		{
			for (int i = 0; i < _tweens.Count; i++)
				_tweens[i].Kill();
		}
	}
}
