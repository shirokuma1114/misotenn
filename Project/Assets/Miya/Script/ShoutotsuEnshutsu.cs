using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutotsuEnshutsu : MonoBehaviour
{
	// äOïîÇ≈ì«Ç›çûÇﬁä÷êî--------------------------------------------------------------------------
	public void Start_ShoutotsuEnshutsu()
	{
		Instantiate(Particle);
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
		Timer_ParticleFinish = main.duration + main.startLifetime.constant;
	}

	// Update
	void Update()
	{
		// Debug
		//if (Input.GetKeyUp(KeyCode.A)) Start_OkozukaiEnshutsu(Level + 1);
	}

	// Update
	void FixedUpdate()
	{
		Timer_ParticleFinish -= Time.deltaTime;
		if (Timer_ParticleFinish < 0) Completed_Enshutsu = true;
	}
}
