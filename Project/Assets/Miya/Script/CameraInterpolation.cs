using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;

public class CameraInterpolation : MonoBehaviour
{
	// �ϐ�
	public GameObject[] ObjectArray;    // �G�f�B�^�����i�[
	public float Second = 1.0f;         // �ړ��ɂ����鎞��

	uint CurrentCamera	= 0;            // ���݃J����
	uint NextCamera		= 0;			// ���J����
	bool Moving			= false;        // �ړ�������

	float Distance = 0;
	//float StartTime = 0;
	//float CurrentTime = 0;
	float Timer_Move = 0;
	
	public PostProcessVolume PostEffect;
	PostProcessProfile PostEffect_Profile;
	DepthOfField Depth;
	void OnDestroy()
	{
		RuntimeUtilities.DestroyProfile(PostEffect_Profile, true);
	}

	// �C�x���g
	int Event_State = 0;        // 0:NONE, 1:Enter, 2:Event, 3:Leave
	float Timer_Event = 0;
	public float Second_Event = 2;
	public void Enter_Event()
	{
		if (Event_State == 0)
		{
			Event_State = 1;
			Timer_Event = 0;
		}
	}
	public void Leave_Event()
	{
		if (Event_State == 2)
		{
			Event_State = 3;
			Timer_Event = 0;
		}
	}


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
		//StartTime = Time.time;
		Timer_Move = 0;
		Moving = true;

		return true;
	}

	// ������
	void Start()
    {
		this.transform.position = ObjectArray[CurrentCamera].transform.position;
		this.transform.rotation = ObjectArray[CurrentCamera].transform.rotation;


		// �|�X�g�G�t�F�N�g
		PostEffect_Profile = PostEffect.profile;
		Depth = PostEffect_Profile.GetSetting<DepthOfField>();
	}

    // �X�V(�f�����ł��邾���Ȃ߂炩�ɂȂ��Ăق�������Fiexed���g��Ȃ��ō���Ă݂�)
    void Update()
    {
		float present = 0;
		// �C�x���g
		switch ( Event_State )
		{
			case 0: // NONE
				break;

			case 1: // Enter
				Timer_Event += Time.deltaTime;
				present = Timer_Event / Second_Event;
				Depth.focusDistance.Override(Mathf.Lerp(1.5f, 3.0f, present));
				if (Timer_Event > Second_Event)
				{
					Event_State = 2;
					Depth.active = false;
				}

				break;

			case 2: // Event
				break;

			case 3: // Leave
				Depth.active = true;
				Timer_Event += Time.deltaTime;
				present = Timer_Event / Second_Event;
				Depth.focusDistance.Override(Mathf.Lerp(3.0f, 1.5f, present));
				if (Timer_Event > Second_Event)
				{
					Event_State = 0;
				}

				break;
		}
		
		// Move
		if ( Moving/* && (Event_State == 0 || Event_State == 2)*/)
		{
			//CurrentTime = Time.time;
			Timer_Move += Time.deltaTime;

			present = Timer_Move / Second;
			if (Timer_Move > Second)
			{
				Moving = false;
				CurrentCamera = NextCamera;
			}

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

			// Depth
			if		(NextCamera == 1) Depth.focusDistance.Override(Mathf.Lerp(1.5f, 1.1f, present));
			else if (NextCamera == 0) Depth.focusDistance.Override(Mathf.Lerp(1.1f, 1.5f, present));
		}


		// Debug
		//if (Input.GetKeyUp(KeyCode.A))
		//{
		//	if (Event_State == 0) Enter_Event();
		//	else if (Event_State == 2) Leave_Event();
		//}
	}
}
