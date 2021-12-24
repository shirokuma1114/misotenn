using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutotsuEnshutsu : MonoBehaviour
{
	GameObject Test;
	float Count_ParticleFinish = 0;

	// äOïîÇ≈ì«Ç›çûÇﬁä÷êî--------------------------------------------------------------------------
	public void Start_ShoutotsuEnshutsu()
	{
		Test = Instantiate(Particle);
		Count_ParticleFinish = Timer_ParticleFinish;

		Control_SE.Get_Instance().Play_SE("Shoutotsu");
	}
	public bool Get_Completed()
	{
		return Completed_Enshutsu;
	}

	// Variable------------------------------------------------------------------------------------
	public GameObject Particle;
	ParticleSystem ParticleSystem_m;

	bool Completed_Enshutsu = false;
	float Timer_ParticleFinish = -1;

	// Start
	void Start()
	{
		ParticleSystem_m = Particle.GetComponent<ParticleSystem>();
		var main = ParticleSystem_m.main;
		Timer_ParticleFinish = 3.00f + 1;// main.duration + main.startLifetime.constant;

		Count_ParticleFinish = Timer_ParticleFinish;
	}

	// Update
	void Update()
	{
		// Debug
		//if (Input.GetKeyUp(KeyCode.A)) Start_ShoutotsuEnshutsu();
	}

	// Update
	void FixedUpdate()
	{
		Count_ParticleFinish -= Time.deltaTime;
		if (Count_ParticleFinish < 0)
		{
			Completed_Enshutsu = true;
			Destroy(Test);
		}
	}
}
