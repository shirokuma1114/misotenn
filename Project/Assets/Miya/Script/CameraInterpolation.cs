using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering.PostProcessing;

public class CameraInterpolation : MonoBehaviour
{
	// 変数
	public GameObject[] ObjectArray;    // エディタより情報格納
	public float Second = 1.0f;         // 移動にかかる時間

	uint CurrentCamera	= 0;            // 現在カメラ
	uint NextCamera		= 0;			// 次カメラ
	bool Moving			= false;        // 移動中判定

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

	// イベント
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


	// 移動先カメラ指定
	public void Set_Second(float _second)
	{
		Second = _second;
	}
	public bool Set_NextCamera(uint _witch)
	{
		// 範囲外
		if
			(
			_witch > ObjectArray.Length ||
			_witch == CurrentCamera
			) return false;

		// 移動開始
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

	// 初期化
	void Start()
    {
		this.transform.position = ObjectArray[CurrentCamera].transform.position;
		this.transform.rotation = ObjectArray[CurrentCamera].transform.rotation;


		// ポストエフェクト
		PostEffect_Profile = PostEffect.profile;
		Depth = PostEffect_Profile.GetSetting<DepthOfField>();
	}

    // 更新(映像ができるだけなめらかになってほしいためFiexedを使わないで作ってみる)
    void Update()
    {
		float present = 0;
		// イベント
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
