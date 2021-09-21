using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating_Miya : MonoBehaviour
{
	//Public
	public bool		Use			= true;
	public float	Second		= 1;		//秒数
	public float	Amplitude	= 1;        //振幅

	//Member
	const float Tolerance = 0.05f;			//許容誤差
	float StartHeight;
	float Time_Second;

	//初期化
	void Start()
    {
		//Member
		StartHeight = this.transform.position.y;
		Time_Second = 0;
	}

	//更新
	void FixedUpdate()
	{
		if ( Use )
		{
			Move();
		}
		else
		{
			if (Mathf.Abs(this.transform.position.y - StartHeight) > Tolerance)
			{
				Move();
			}
		}
	}

	//運動
	void Move()
	{
		//更新
		Time_Second += Time.deltaTime;
		if (Time_Second > Second) Time_Second -= Second;

		//計算
		float radian = 2 * Mathf.PI * (Time_Second / Second);

		//実行
		Vector3 position = this.transform.position;
		position.y = StartHeight + Mathf.Sin(radian) * Amplitude;
		this.transform.position = position;
	}


	//Public

	//状態変更
	public void Set_Use(bool _use)
	{
		Use = _use;
		if (_use) Time_Second = 0;
	}

	//スタート位置を更新
	public void Set_StartHeight(float _height)
	{
		StartHeight = _height;
	}
}
