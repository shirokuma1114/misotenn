using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInterpolation : MonoBehaviour
{
	// 変数
	public GameObject[] ObjectArray;    // エディタより情報格納
	public float Second = 1.0f;         // 移動にかかる時間

	uint CurrentCamera	= 0;            // 現在カメラ
	uint NextCamera		= 0;			// 次カメラ
	bool Moving			= false;        // 移動中判定

	float Distance = 0;
	float StartTime = 0;
	float CurrentTime = 0;


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
		StartTime = Time.time;
		Moving = true;
		return true;
	}

	// 初期化
	void Start()
    {
		this.transform.position = ObjectArray[CurrentCamera].transform.position;
		this.transform.rotation = ObjectArray[CurrentCamera].transform.rotation;
	}

    // 更新(映像ができるだけなめらかになってほしいためFiexedを使わないで作ってみる)
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
