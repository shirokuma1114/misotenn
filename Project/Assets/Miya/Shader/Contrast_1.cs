using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contrast_1 : MonoBehaviour
{
	public Shader shader;
	public Material mat;

	void Start()
	{
		mat = new Material(shader);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, mat);
	}
}
