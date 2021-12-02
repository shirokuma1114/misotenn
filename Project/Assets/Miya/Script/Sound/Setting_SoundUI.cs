using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Setting_SoundUI : WindowBase
{
	// äOïîÇ©ÇÁåƒÇ—èoÇ∑ä÷êî
	public override void SetEnable(bool enable)
	{
		Window.SetActive(true);
		Start();
	}

	// static
	static public float Magnification_BGM = 0.5f;
	static public float Magnification_SE  = 0.5f;

	// éQè∆
	public GameObject Window;
	public Image	Background_Select;
	public Slider	Slider_BGM;
	public Slider	Slider_SE;
	public Slider	_sliderTextSpeed;
	public Text		Image_BACK;
	public AudioSource SE_Test;

    [SerializeField]
    WindowBase _backToWindow;

    // enum
    enum SELECT
	{
		BGM,
		SE,
        TEXT_SPEED,
		BACK,
		SELECT_MAX
	}
	int Select = (int)SELECT.BGM;

	// Grid
	float GridValue = 0.1f;

	// Event
	public delegate void EventHandler_Sound(bool changed);
	public static event EventHandler_Sound Event_Sound;

    [SerializeField]
    private MessageWindow _messageWindow;

	// Start
	void Start()
	{
		Select = (int)SELECT.BGM;
		Update_SelectBackground();
        _sliderTextSpeed.minValue = MessageWindow.TEXT_SPEED_MIN;
        _sliderTextSpeed.maxValue = MessageWindow.TEXT_SPEED_MAX;
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

                    case (int)SELECT.TEXT_SPEED:
						_sliderTextSpeed.value = Mathf.Max(_sliderTextSpeed.value - MessageWindow.TEXT_SPEED_MAX / MessageWindow.TEXT_SPEED_MIN, _sliderTextSpeed.minValue);
                        _messageWindow.SetTextSpeed(_sliderTextSpeed.value);
						
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

					case (int)SELECT.TEXT_SPEED:
                        _sliderTextSpeed.value = Mathf.Min(_sliderTextSpeed.value + MessageWindow.TEXT_SPEED_MAX / MessageWindow.TEXT_SPEED_MIN, _sliderTextSpeed.maxValue);
                        _messageWindow.SetTextSpeed(_sliderTextSpeed.value);

						break;

				}
				Event_Sound(true);
			}

			// BACK
			if (Input.GetKeyUp(KeyCode.Return) && Select == (int)SELECT.BACK)
			{
				Window.SetActive(false);
				Start();
                _backToWindow.SetEnable(true);
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
                case (int)SELECT.TEXT_SPEED:
				position.y = _sliderTextSpeed.GetComponent<RectTransform>().position.y;
				break;
			case (int)SELECT.BACK:
				position.y = Image_BACK.GetComponent<RectTransform>().position.y;
				break;
		}
		Background_Select.GetComponent<RectTransform>().position = position;
	}
}
