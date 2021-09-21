using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RandomMove_Miya : MonoBehaviour
{
	//Public
	public bool	 Use	= true;
	public float Range	= 50;
	public float Speed	= 1;

	//Member
	const float Tolerance = 0.1f;          //許容誤差
	RectTransform ThisTransform;
	bool Reached = false;
	Vector2 StartPosition;
	Vector2 TargetPosition;
	Vector2 Direction;

    //初期化
    void Start()
    {
		ThisTransform = this.GetComponent<RectTransform>();
		StartPosition = ThisTransform.position;
    }

    //更新
    void FixedUpdate()
    {
        if ( Use )
		{
			//範囲内ランダムで目的地を探す
			if ( Reached )
			{
				Reached = false;

				TargetPosition = new Vector2(0, 0);
				Direction = TargetPosition - (Vector2)ThisTransform.position;
			}
			//移動
			else
			{
				Vector2 new_position = (Vector2)ThisTransform.position + Direction * Speed * Time.deltaTime;
				ThisTransform.position = new_position;

				//到達判定
				if((TargetPosition - (Vector2)ThisTransform.position).magnitude < Tolerance)
				{
					Reached = true;
				}
			}
		}
    }


	//Public

	//使用状態変更
	public void Set_Use(bool _use)
	{
		Use = _use;
	}
}
