using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Setting_SoundUI : MonoBehaviour
{
	// static
	static public float Magnification_BGM = 0.5f;
	static public float Magnification_SE  = 0.5f;

	// éQè∆
	public GameObject Window;
	public Image	Background_Select;
	public Slider	Slider_BGM;
	public Slider	Slider_SE;
	public Text		Image_BACK;
	public AudioSource SE_Test;

	// enum
	enum SELECT
	{
		BGM,
		SE,
		BACK,
		SELECT_MAX
	}
	int Select = (int)SELECT.BGM;

	// Grid
	float GridValue = 0.1f;

	// Event
	public delegate void EventHandler_Sound(bool changed);
	public event EventHandler_Sound Event_Sound;

	// Start
	void Start()
	{
		Select = (int)SELECT.BGM;
		Update_SelectBackground();
	}

	// FixedUpdate
	void FixedUpdate()
	{

	}

	// Input
	void Update()
    {
		if ( Window.activeSelf )
		{
			// Select
			if (Input.GetKeyUp(KeyCode.W))
			{
				Select--;
				if (Select < 0) Select = 0;
				Update_SelectBackground();
			}
			if (Input.GetKeyUp(KeyCode.S))
			{
				Select++;
				if (Select > (int)SELECT.SELECT_MAX - 1) Select = (int)SELECT.SELECT_MAX - 1;
				Update_SelectBackground();
			}

			// Volume
			if (Input.GetKeyUp(KeyCode.A))
			{
				switch( Select )
				{
					case (int)SELECT.BGM:
						Slider_BGM.value -= GridValue;
						if (Slider_BGM.value < 0) Slider_BGM.value = 0;
						Magnification_BGM = Slider_BGM.value;
						break;

					case (int)SELECT.SE:
						Slider_SE.value -= GridValue;
						if (Slider_SE.value < 0) Slider_SE.value = 0;
						Magnification_SE = Slider_SE.value;
						SE_Test.Play();
						break;
				}
				Event_Sound(true);
			}
			if (Input.GetKeyUp(KeyCode.D))
			{
				switch ( Select )
				{
					case (int)SELECT.BGM:
						Slider_BGM.value += GridValue;
						if (Slider_BGM.value > 1) Slider_BGM.value = 1;
						Magnification_BGM = Slider_BGM.value;
						break;

					case (int)SELECT.SE:
						Slider_SE.value += GridValue;
						if (Slider_SE.value > 1) Slider_SE.value = 1;
						Magnification_SE = Slider_SE.value;
						SE_Test.Play();
						break;
				}
				Event_Sound(true);
			}

			// BACK
			if (Input.GetKeyUp(KeyCode.Return) && Select == (int)SELECT.BACK)
			{
				Window.SetActive(false);
				Start();
			}
		}

		// Debug
		else if (Input.GetKeyUp(KeyCode.Return) && !Window.activeSelf)
		{
			//Window.SetActive(true);
		}
	}

	// Local Function
	void Update_SelectBackground()
	{
		Vector2 position = Background_Select.GetComponent<RectTransform>().position;

		switch( Select )
		{
			case (int)SELECT.BGM:
				position.y = Slider_BGM.GetComponent<RectTransform>().position.y;
				break;
			case (int)SELECT.SE:
				position.y = Slider_SE.GetComponent<RectTransform>().position.y;
				break;
			case (int)SELECT.BACK:
				position.y = Image_BACK.GetComponent<RectTransform>().position.y;
				break;
		}
		Background_Select.GetComponent<RectTransform>().position = position;
	}
}
