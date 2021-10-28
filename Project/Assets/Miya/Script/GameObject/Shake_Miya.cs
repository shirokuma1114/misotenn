using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake_Miya : MonoBehaviour
{
	//Public
	public bool Use = true;
	public float Second = 1;        //秒数
	public float Degree = 5;        //角度
	public bool Rotate_Z = true;
	public bool Rotate_X = false;
	public bool Rotate_Y = false;

	//Member
	const float Tolerance = Mathf.PI / 1800;//許容誤差
	float Time_Second;

	//初期化
	void Start()
	{
		//Member
		Time_Second = 0;
	}

	//更新
	void FixedUpdate()
	{
		if (Use)
		{
			Move();
		}
		else
		{
			if                                                                      //現状停止してRotateが0の状態にしてからしないとバグる
				(                                                                   //ここ、もう少し作りこめる
					Mathf.Abs(this.transform.rotation.eulerAngles.z) > Tolerance ||
					Mathf.Abs(this.transform.rotation.eulerAngles.x) > Tolerance ||
					Mathf.Abs(this.transform.rotation.eulerAngles.y) > Tolerance
				)
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
		Vector3 rotation = this.transform.rotation.eulerAngles;
		if (Rotate_Z) rotation.z = Mathf.Sin(radian) * Degree;
		if (Rotate_X) rotation.x = Mathf.Sin(radian) * Degree;
		if (Rotate_Y) rotation.y = Mathf.Sin(radian) * Degree;
		this.transform.rotation = Quaternion.Euler(rotation);
	}


	//Public

	//状態変更
	public void Set_Use(bool _use)
	{
		Use = _use;
		if (_use) Time_Second = 0;
	}
}
