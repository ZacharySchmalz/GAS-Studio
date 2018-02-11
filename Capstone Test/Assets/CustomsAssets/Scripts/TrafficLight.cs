using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum Light { GREEN, YELLOW, RED}

    public GameObject parent;
    public GameObject Green;
    public GameObject Yellow;
    public GameObject Red;
    public Material GreenOn;
    public Material GreenOff;
    public Material YellowOn;
    public Material YellowOff;
    public Material RedOn;
    public Material RedOff;
    public Light InitialLight;

    private TrafficController controller;
    private Light currentLight;
    private float timer;

    // Set all lights off
	void Start ()
    {
        Green.GetComponent<Renderer>().material = GreenOff;
        Yellow.GetComponent<Renderer>().material = YellowOff;
        Red.GetComponent<Renderer>().material = RedOff;

        controller = parent.GetComponent<TrafficController>();
        currentLight = InitialLight;

        // Set current light data
        switch(currentLight)
        {
            case Light.GREEN :
                Green.GetComponent<Renderer>().material = GreenOn;
                timer = controller.GreenTimer;
                break;
            case Light.YELLOW :
                Yellow.GetComponent<Renderer>().material = YellowOn;
                timer = controller.YellowTimer;
                break;
            case Light.RED :
                Red.GetComponent<Renderer>().material = RedOn;
                timer = controller.RedTimer;
                break;
        }
	}

    // Updates the next light
    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            switch (currentLight)
            {
                case Light.GREEN:
                    Green.GetComponent<Renderer>().material = GreenOff;
                    Yellow.GetComponent<Renderer>().material = YellowOn;
                    currentLight = Light.YELLOW;
                    timer = controller.YellowTimer;
                    break;

                case Light.YELLOW:
                    Yellow.GetComponent<Renderer>().material = YellowOff;
                    Red.GetComponent<Renderer>().material = RedOn;
                    currentLight = Light.RED;
                    timer = controller.RedTimer + controller.ChangeDelay;
                    break;

                case Light.RED:
                    Red.GetComponent<Renderer>().material = RedOff;
                    Green.GetComponent<Renderer>().material = GreenOn;
                    currentLight = Light.GREEN;
                    timer = controller.GreenTimer;
                    break;
            }
        }
    }
}