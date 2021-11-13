using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating_Local_Miya : MonoBehaviour
{
	// �ϐ�
	public float Amplitude = 0.005f;
	public float Second = 0.3f;

	Transform Start_Position;

	bool Using = true;
	public void Set_Using(bool _using)
	{
		//if (!Using) Start_Position = this.transform;
		//else this.transform.position = Start_Position.position;

		Using = _using;
	}

	float Angle = 0;

	// ������
    void Start()
    {

	}

	// ����
	private void Update()
	{
		// Debug
		//if (Input.GetKeyUp(KeyCode.B)) Set_Using(!Using);
	}

	// �X�V
	void FixedUpdate()
    {
		if ( Using )
		{
			// �v�Z
			Angle += 360 / Second * Time.deltaTime;
			if (Angle > 360) Angle -= 360;

			// �ϊ�
			Vector3 position = this.transform.position;
			position += this.transform.up * Mathf.Sin(Angle * Mathf.Deg2Rad) * Amplitude;

			this.transform.position = position;
		}
	}
}
