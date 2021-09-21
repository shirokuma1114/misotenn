using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating_Miya : MonoBehaviour
{
	//Public
	public bool		Use			= true;
	public float	Second		= 1;		//�b��
	public float	Amplitude	= 1;        //�U��

	//Member
	const float Tolerance = 0.05f;			//���e�덷
	float StartHeight;
	float Time_Second;

	//������
	void Start()
    {
		//Member
		StartHeight = this.transform.position.y;
		Time_Second = 0;
	}

	//�X�V
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

	//�^��
	void Move()
	{
		//�X�V
		Time_Second += Time.deltaTime;
		if (Time_Second > Second) Time_Second -= Second;

		//�v�Z
		float radian = 2 * Mathf.PI * (Time_Second / Second);

		//���s
		Vector3 position = this.transform.position;
		position.y = StartHeight + Mathf.Sin(radian) * Amplitude;
		this.transform.position = position;
	}


	//Public

	//��ԕύX
	public void Set_Use(bool _use)
	{
		Use = _use;
		if (_use) Time_Second = 0;
	}

	//�X�^�[�g�ʒu���X�V
	public void Set_StartHeight(float _height)
	{
		StartHeight = _height;
	}
}
