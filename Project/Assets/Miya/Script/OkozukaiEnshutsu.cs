using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkozukaiEnshutsu : MonoBehaviour
{
	// äOïîÇ≈ì«Ç›çûÇﬁä÷êî--------------------------------------------------------------------------
	public void Start_OkozukaiEnshutsu	(int _level)	// 1Å`10íiäK
	{
		Level = _level;
		if (Level < 1 ) Level = 1;
		if (Level > 10) Level = 10;

		var emission = ParticleSystem_m.emission;
		emission.rateOverTime = BaseNumber + Level * Level * Auxiliary;

        Particle_Instance = Instantiate(Particle);

        Completed_Enshutsu = false;

    }
	public bool Get_Completed()
	{
		return Completed_Enshutsu;
	}
	
	// Variable------------------------------------------------------------------------------------
	public GameObject Particle;
	GameObject Particle_Instance;
	ParticleSystem ParticleSystem_m;
	
	bool Completed_Enshutsu = false;
	float Timer_ParticleFinish = -1;

	int Level = 0;
	int BaseNumber = 10;
	int Auxiliary = 2;

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
        //Timer_ParticleFinish -= Time.deltaTime;
        //if (Timer_ParticleFinish < 0) Completed_Enshutsu = true;

        if (Completed_Enshutsu)
            return;

        if (!Particle_Instance)
            Completed_Enshutsu = true;
	}
}
