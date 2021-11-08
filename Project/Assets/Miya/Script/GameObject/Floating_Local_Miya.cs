using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating_Local_Miya : MonoBehaviour
{
	// •Ï”
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

	// ‰Šú‰»
    void Start()
    {

	}

	// “ü—Í
	private void Update()
	{
		// Debug
		//if (Input.GetKeyUp(KeyCode.B)) Set_Using(!Using);
	}

	// XV
	void FixedUpdate()
    {
		if ( Using )
		{
			// ŒvŽZ
			Angle += 360 / Second * Time.deltaTime;
			if (Angle > 360) Angle -= 360;

			// •ÏŠ·
			Vector3 position = this.transform.position;
			position += this.transform.up * Mathf.Sin(Angle * Mathf.Deg2Rad) * Amplitude;

			this.transform.position = position;
		}
	}
}
