using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	// ’†S“_
	[SerializeField] private Vector3 _center = Vector3.zero;

	// ‰ñ“]²
	[SerializeField] private Vector3 _axis = Vector3.up;

	// ‰~‰^“®üŠú
	[SerializeField] private float _period = 2;
	
	// Update is called once per frame
	void FixedUpdate()
    {
		// ’†S“_center‚Ìü‚è‚ğA²axis‚ÅAperiodüŠú‚Å‰~‰^“®
		transform.RotateAround(
			_center,
			_axis,
			360 / _period * Time.deltaTime
		);
	}
}
