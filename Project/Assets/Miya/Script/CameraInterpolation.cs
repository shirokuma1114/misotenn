using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInterpolation : MonoBehaviour
{
	// �ϐ�
	public GameObject[] ObjectArray;    // �G�f�B�^�����i�[
	public float Second = 1.0f;         // �ړ��ɂ����鎞��

	uint CurrentCamera	= 0;            // ���݃J����
	uint NextCamera		= 0;			// ���J����
	bool Moving			= false;        // �ړ�������

	float Distance = 0;
	float StartTime = 0;
	float CurrentTime = 0;


	// �ړ���J�����w��
	public void Set_Second(float _second)
	{
		Second = _second;
	}
	public bool Set_NextCamera(uint _witch)
	{
		// �͈͊O
		if
			(
			_witch > ObjectArray.Length ||
			_witch == CurrentCamera
			) return false;

		// �ړ��J�n
		NextCamera = _witch;
		Distance = Vector3.Distance
			(
			ObjectArray[CurrentCamera].transform.position,
			ObjectArray[NextCamera].transform.position
			);
		StartTime = Time.time;
		Moving = true;
		return true;
	}

	// ������
	void Start()
    {
		this.transform.position = ObjectArray[CurrentCamera].transform.position;
		this.transform.rotation = ObjectArray[CurrentCamera].transform.rotation;
	}

    // �X�V(�f�����ł��邾���Ȃ߂炩�ɂȂ��Ăق�������Fiexed���g��Ȃ��ō���Ă݂�)
    void Update()
    {
		// Debug
		//if (Input.GetKeyUp(KeyCode.A)) Set_NextCamera(1);

		// Move
		if ( Moving )
		{
			CurrentTime = Time.time;

			float present = (CurrentTime - StartTime) / Second;
			if (CurrentTime - StartTime > Second) Moving = false;

			this.transform.position = Vector3.Lerp
				(
				ObjectArray[CurrentCamera].transform.position, 
				ObjectArray[NextCamera].transform.position,
				present
				);

			this.transform.rotation = Quaternion.Lerp
				 (
				 ObjectArray[CurrentCamera].transform.rotation,
				 ObjectArray[NextCamera].transform.rotation,
				 present
				 );
		}
    }
}
