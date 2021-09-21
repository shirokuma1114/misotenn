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
	const float Tolerance = 0.1f;          //���e�덷
	RectTransform ThisTransform;
	bool Reached = false;
	Vector2 StartPosition;
	Vector2 TargetPosition;
	Vector2 Direction;

    //������
    void Start()
    {
		ThisTransform = this.GetComponent<RectTransform>();
		StartPosition = ThisTransform.position;
    }

    //�X�V
    void FixedUpdate()
    {
        if ( Use )
		{
			//�͈͓������_���ŖړI�n��T��
			if ( Reached )
			{
				Reached = false;

				TargetPosition = new Vector2(0, 0);
				Direction = TargetPosition - (Vector2)ThisTransform.position;
			}
			//�ړ�
			else
			{
				Vector2 new_position = (Vector2)ThisTransform.position + Direction * Speed * Time.deltaTime;
				ThisTransform.position = new_position;

				//���B����
				if((TargetPosition - (Vector2)ThisTransform.position).magnitude < Tolerance)
				{
					Reached = true;
				}
			}
		}
    }


	//Public

	//�g�p��ԕύX
	public void Set_Use(bool _use)
	{
		Use = _use;
	}
}
