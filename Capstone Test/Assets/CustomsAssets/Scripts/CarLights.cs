using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour
{
	public string HeadlightInput;
	public string BrakelightInput;
	public string RightTurnSignalInput;
	public string LeftTurnSignalInput;

	public GameObject[] BrakeLights;
	public GameObject[] RightTurnSignals;
	public GameObject[] LeftTurnSignals;
	public GameObject[] HeadLights;
	public Light[] HeadlightsLight;
	public Light[] BrakelightsLight;
	public Light[] RightTurnSignalLight;
	public Light[] LeftTurnSignalLight;

	public Material BrakeOn;
	public Material BrakeOff;
	public Material TurnSignalOn;
	public Material TurnSignalOff;
	public Material HeadlightOn;
	public Material HeadlightOff;

	public float HeadlightIntensity;
	public float HeadlightRange;

	private bool leftTurn;
	private bool rightTurn;
	private bool headlightToggle;
	private float blinker;

	private bool rightTurnToggle;
	private bool leftTurnToggle;

	void Start ()
	{
		blinker = 0;
		headlightToggle = false;
		rightTurnToggle = false;
		leftTurnToggle = false;
		foreach (Light light in HeadlightsLight)
		{
			light.intensity = HeadlightIntensity;
			light.range = HeadlightRange;
		}
	}

	void Update ()
	{
		// Controls turn signal blinking
		blinker = Mathf.PingPong(Time.time * 2f, 1);

		// If braking
		if (Input.GetAxis(BrakelightInput) > 0)
		{
			foreach (GameObject light in BrakeLights)
				light.GetComponent<Renderer>().material = BrakeOn;
			foreach (Light light in BrakelightsLight)
				light.gameObject.SetActive(true);
		}

		// If not braking
		else
		{
			foreach (GameObject light in BrakeLights)
				light.GetComponent<Renderer>().material = BrakeOff;
			foreach (Light light in BrakelightsLight)
				light.gameObject.SetActive(false);
		}

		// Right turn signal
		if (Input.GetButtonUp(RightTurnSignalInput) && !leftTurnToggle) 
			rightTurnToggle = !rightTurnToggle;
		// Left turn signal
		else if (Input.GetButtonUp(LeftTurnSignalInput) && !rightTurnToggle)
			leftTurnToggle = !leftTurnToggle;

		if(rightTurnToggle && !leftTurnToggle)
		{
			foreach (GameObject signal in LeftTurnSignals)
				signal.GetComponent<Renderer>().material = TurnSignalOff;

			// Blink on
			if (blinker > 0 && blinker < .5f)
			{
				foreach (GameObject signal in RightTurnSignals)
					signal.GetComponent<Renderer>().material = TurnSignalOn;
				foreach (Light light in RightTurnSignalLight)
					light.gameObject.SetActive(true);
			}
		
			// Blink off
			else
			{
				foreach (GameObject signal in RightTurnSignals)
					signal.GetComponent<Renderer>().material = TurnSignalOff;
				foreach (Light light in RightTurnSignalLight)
					light.gameObject.SetActive(false);
			}

			rightTurn = true;
			leftTurn = false;
		}
		
		else if (leftTurnToggle && !rightTurnToggle)
		{
			foreach (GameObject signal in RightTurnSignals)
				signal.GetComponent<Renderer> ().material = TurnSignalOff;

			// Blink on
			if (blinker > 0 && blinker < .5f) {
				foreach (GameObject signal in LeftTurnSignals)
					signal.GetComponent<Renderer> ().material = TurnSignalOn;
				foreach (Light light in LeftTurnSignalLight)
					light.gameObject.SetActive (true);
			}
			// Blink off
			else
			{
				foreach (GameObject signal in LeftTurnSignals)
					signal.GetComponent<Renderer> ().material = TurnSignalOff;
				foreach (Light light in LeftTurnSignalLight)
					light.gameObject.SetActive (false);
			}

			leftTurn = true;
			rightTurn = false;
		}

		// No turn signal
		else
		{
			foreach (GameObject signal in RightTurnSignals)
				signal.GetComponent<Renderer>().material = TurnSignalOff;
			foreach (GameObject signal in LeftTurnSignals)
				signal.GetComponent<Renderer>().material = TurnSignalOff;
			foreach (Light light in RightTurnSignalLight)
				light.gameObject.SetActive(false);
			foreach (Light light in LeftTurnSignalLight)
				light.gameObject.SetActive(false);

			rightTurn = false;
			leftTurn = false;
		}

		if (Input.GetButtonUp(HeadlightInput))
			headlightToggle = !headlightToggle;

		if(headlightToggle)
		{
			foreach (GameObject headlight in HeadLights)
				headlight.GetComponent<Renderer>().material = HeadlightOn;
			foreach (Light light in HeadlightsLight)
				light.gameObject.SetActive(true);
		}
		else
		{
			foreach (GameObject headlight in HeadLights)
				headlight.GetComponent<Renderer>().material = HeadlightOff;
			foreach (Light light in HeadlightsLight)
				light.gameObject.SetActive(false);
		}

	}
}