using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_Blur : MonoBehaviour
{
	//Public
	public float Time_Stopping = 1;

	// Member
	Material Blur;
	const float Tolerance	= 0.01f;
	bool  Moving			= false;
	float Value				= 0;
	float Timer_Moving		= 0;
	float Timer_Stopping	= 0;

	// èâä˙âª
	void Start()
    {
		Blur = this.GetComponent<Renderer>().material;
	}

    // çXêV
    void FixedUpdate()
    {
		if ( Moving )
		{
			Timer_Moving += Time.deltaTime;
			Value = Mathf.Sin(Timer_Moving);
			if (Mathf.Abs(Value) < Tolerance)
			{
				Moving = false;
				Timer_Moving = 0;
			}

			Blur.SetFloat("_Blur", Value);
			Blur.SetFloat("_Time_Fade", (Value + 0.01f) * 0.2f);
		}
		else
		{
			Timer_Stopping += Time.deltaTime;
			if (Timer_Stopping > Time_Stopping)
			{
				Moving = true;
				Timer_Stopping = 0;
			}
		}
		
	}
}
