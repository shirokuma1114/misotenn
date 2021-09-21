using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake_Miya : MonoBehaviour
{
	//Public
	public bool Use = true;
	public float Second = 1;        //�b��
	public float Degree = 5;        //�p�x
	public bool Rotate_Z = true;
	public bool Rotate_X = false;
	public bool Rotate_Y = false;

	//Member
	const float Tolerance = Mathf.PI / 1800;//���e�덷
	float Time_Second;

	//������
	void Start()
	{
		//Member
		Time_Second = 0;
	}

	//�X�V
	void FixedUpdate()
	{
		if (Use)
		{
			Move();
		}
		else
		{
			if                                                                      //�����~����Rotate��0�̏�Ԃɂ��Ă��炵�Ȃ��ƃo�O��
				(                                                                   //�����A����������肱�߂�
					Mathf.Abs(this.transform.rotation.eulerAngles.z) > Tolerance ||
					Mathf.Abs(this.transform.rotation.eulerAngles.x) > Tolerance ||
					Mathf.Abs(this.transform.rotation.eulerAngles.y) > Tolerance
				)
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
		Vector3 rotation = this.transform.rotation.eulerAngles;
		if (Rotate_Z) rotation.z = Mathf.Sin(radian) * Degree;
		if (Rotate_X) rotation.x = Mathf.Sin(radian) * Degree;
		if (Rotate_Y) rotation.y = Mathf.Sin(radian) * Degree;
		this.transform.rotation = Quaternion.Euler(rotation);
	}


	//Public

	//��ԕύX
	public void Set_Use(bool _use)
	{
		Use = _use;
		if (_use) Time_Second = 0;
	}
}
