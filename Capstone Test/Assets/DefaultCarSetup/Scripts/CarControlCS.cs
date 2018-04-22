//2016 Spyblood Productions
//Use for non-commerical games only. do not sell comercially
//without permission first

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class WC
{
	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelRL;
	public WheelCollider wheelRR;
}
[System.Serializable]
public class WT
{
	public Transform wheelFL;
	public Transform wheelFR;
	public Transform wheelRL;
	public Transform wheelRR;
}
[RequireComponent(typeof(AudioSource))]//needed audiosource
[RequireComponent(typeof(Rigidbody))]//needed Rigid body
public class CarControlCS : MonoBehaviour {

	[Header("Object Declarations")]
	public WC wheels;
	public WT tires;
	public WheelCollider[] extraWheels;
	public Transform[] extraWheelObjects;

	[Header("Car Statistics")]
	public Vector3 centerOfGravity;//car's center of mass offset
	public float maxTorque = 1000f;//car's acceleration value
	public float maxReverseSpeed = 50f;//top speed for the reverse gear
	public float handBrakeTorque = 500f;//hand brake value
	public float maxSteer = 25f;//max steer angle
	public float[] GearRatio;//determines how many gears the car has, and at what speed the car shifts to the appropriate gear
	private int throttleInput;//read only
	private int steerInput;//read only
	private bool reversing;//read only
	private float currentSpeed;//read only
	[Tooltip("Max vehicle speed")]
	public float maxSpeed = 150f;//how fast the vehicle can go
	private int gear;//current gear
	Vector3 localCurrentSpeed;

	[Header("Control Settings")]
	[Tooltip("Rotation until max turn value. (1 = 900degrees, 2 = 450degrees, etc)")]
	public float WheelSensitivity = 1;

	[Header("Current Control Values")]
	public float Acceleration;
	public float Brake;
    public float Gear;
	public float AccelerationLog;
	public float BrakeLog;
	public float Wheel;
    public float CurrentSpeedLog;
    public bool isControlActive;

	[Header("GPS")]
	public GameObject Arrow;
	public Vector3 Destination;
	public TextMesh DistanceText;
	private float Distance;

	[Header("Control Method")]
	public bool isKeyboard;
    public bool isAI;

    [Header("Data Recording")]
    public GameObject dataManger;
    public bool isRecording = false;
    public Text recordingText;

    private float accelAxis;
    private float wheelAxis;

    private GameObject GameManager;

    public float CurrentSteerAngle
    {
        get { return Wheel; }
        set { Wheel = value; }
    }

    public float AccelInput
    {
        get { return Acceleration; }
        set { Acceleration = Mathf.Clamp(value, 0 ,1); }
    }

    public float CurrentSpeed
    {
        get { return currentSpeed; }
    }


	// Use this for initialization
	void Start () {

		//Alter the center of mass for stability on your car
		GetComponent<Rigidbody>().centerOfMass = centerOfGravity;
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        GameManager.SendMessage("UpdatePlayers");
        Gear = 1;
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (GetComponent<Rigidbody>().centerOfMass != centerOfGravity)
			GetComponent<Rigidbody>().centerOfMass = centerOfGravity;

		AllignWheels ();
		HandleInputs ();

		Drive ();
		EngineAudio ();

		currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 2.23693629f;//convert currentspeed into MPH
        CurrentSpeedLog = currentSpeed;
		localCurrentSpeed = transform.InverseTransformDirection (GetComponent<Rigidbody> ().velocity);
		//if (currentSpeed > maxSpeed || (localCurrentSpeed.z*2.23693629f) < -maxReverseSpeed){
		Vector3 lookDirection = Destination - transform.position;
		Distance = lookDirection.magnitude;
		DistanceText.text = (int)Distance + " ft";
		lookDirection = new Vector3 (lookDirection.x, 0, lookDirection.z);
		Arrow.transform.rotation = Quaternion.LookRotation(lookDirection);
	}

	void HandleInputs()
    {
        // Recording Input
        if (Input.GetKeyDown(KeyCode.Return))
        {
            dataManger.SetActive(isRecording = !isRecording);
            if (dataManger.activeSelf)
            {
                recordingText.text = "Recording";
                recordingText.color = Color.red;
            }
            else
            {
                recordingText.text = "Not Recording";
                recordingText.color = Color.black;
            }
        }

        if (!isAI && isControlActive)
        {
            //Update wheel angle
            Wheel = Input.GetAxis("Wheel") * WheelSensitivity;
            Wheel = Mathf.Clamp(Wheel, -1, 1);

            //Update Brake
            if (isKeyboard)
            {
                Brake = Input.GetAxis("Brake");
            }
            else
            {
                Brake = Input.GetAxis("Brake") * 0.5f + 0.5f;
            }

            //Update Acceleration
            if (isKeyboard)
            {
                if(localCurrentSpeed.z > -0.01f)
                {
                    Gear = 1;
                } else
                {
                    Gear = -1;
                }
                Acceleration = (Input.GetAxis("Accel"));
                //If reversing while moving forward activate brake instead
                if (Input.GetAxis("Accel") < 0 && localCurrentSpeed.z > 0.1f)
                {
                    Acceleration = 0;
                    Brake = 1;
                }
                //If forward while moving backwards activate brake instead
                if (Input.GetAxis("Accel") > 0 && localCurrentSpeed.z < -0.1f)
                {
                    Acceleration = 0;
                    Brake = 1;
                }
            }
            else
            {
                if(Input.GetAxis("GearSwitch") == 1) { Gear = 1; }
                if (Input.GetAxis("GearSwitch") == -1) { Gear = -1; }

                Acceleration = (Input.GetAxis("Accel") * 0.5f + 0.5f) * Input.GetAxis("GearSwitch");
            }

            Acceleration = Mathf.Clamp(Acceleration, -1, 1);
            Brake = Mathf.Clamp(Brake, 0, 1);
            AccelerationLog = Acceleration;
            BrakeLog = Brake;
        }
    }

    void AllignWheels()
	{
		//allign the wheel objs to their colliders

		Quaternion quat;
		Vector3 pos;
		wheels.wheelFL.GetWorldPose(out pos,out quat);
		tires.wheelFL.position = pos;
		tires.wheelFL.rotation = quat;

		wheels.wheelFR.GetWorldPose(out pos,out quat);
		tires.wheelFR.position = pos;
		tires.wheelFR.rotation = quat;

		wheels.wheelRL.GetWorldPose(out pos,out quat);
		tires.wheelRL.position = pos;
		tires.wheelRL.rotation = quat;

		wheels.wheelRR.GetWorldPose(out pos,out quat);
		tires.wheelRR.position = pos;
		tires.wheelRR.rotation = quat;

		for (int i = 0; i < extraWheels.Length; i++)
		{

			for (int k = 0; k < extraWheelObjects.Length; k++) {

				Quaternion quater;
				Vector3 vec3;

				extraWheels [i].GetWorldPose (out vec3, out quater);
				extraWheelObjects [k].position = vec3;
				extraWheelObjects [k].rotation = quater;

			}

		}
	}



	void Drive()
	{

		//dont call this function if mobile input is checked in the editor
		float gasMultiplier = 0f;

		if (!reversing) {
			if (currentSpeed < maxSpeed)
				gasMultiplier = 1f;
			else
				gasMultiplier = 0f;

		} else {
			if (currentSpeed < maxReverseSpeed)
				gasMultiplier = 1f;
			else
				gasMultiplier = 0f;
		}

		wheels.wheelFL.motorTorque = maxTorque * Acceleration * gasMultiplier;
		wheels.wheelFR.motorTorque = maxTorque * Acceleration * gasMultiplier;
		wheels.wheelRL.motorTorque = maxTorque * Acceleration * gasMultiplier;
		wheels.wheelRR.motorTorque = maxTorque * Acceleration * gasMultiplier;

		if (localCurrentSpeed.z < -0.1f && wheels.wheelRL.rpm < 10) {//in local space, if the car is travelling in the direction of the -z axis, (or in reverse), reversing will be true
			reversing = true;
		} else {
			reversing = false;
		}

        //if (isAI)
       // {
            wheels.wheelFL.steerAngle = maxSteer * Wheel;
            wheels.wheelFR.steerAngle = maxSteer * Wheel;
       // }

		if (Brake > 0)//pressing space triggers the car's handbrake
		{
			wheels.wheelFL.brakeTorque = handBrakeTorque * Brake;
			wheels.wheelFR.brakeTorque = handBrakeTorque * Brake;
			wheels.wheelRL.brakeTorque = handBrakeTorque * Brake;
			wheels.wheelRR.brakeTorque = handBrakeTorque * Brake;
		}
		else//letting go of space disables the handbrake
		{
			wheels.wheelFL.brakeTorque = 0f;
			wheels.wheelFR.brakeTorque = 0f;
			wheels.wheelRL.brakeTorque = 0f;
			wheels.wheelRR.brakeTorque = 0f;
		}
	}

	void EngineAudio()
	{
		//the function called to give the car basic audio, as well as some gear shifting effects
		//it's prefered you use the engine sound included, but you can use your own if you have one.
		//~~~~~~~~~~~~[IMPORTANT]~~~~~~~~~~~~~~~~
		//make sure your last gear value is higher than the max speed variable or else you will
		//get unwanted errors!!

		//anyway, let's get started

		for (int i = 0; i < GearRatio.Length; i++) {
			if (GearRatio [i] > currentSpeed) {
				//break this value
				break;
			}

			float minGearValue = 0f;
			float maxGearValue = 0f;
			if (i == 0) {
				minGearValue = 0f;
			} else {
				minGearValue = GearRatio [i];
			}
			maxGearValue = GearRatio [i+1];

			float pitch = ((currentSpeed - minGearValue) / (maxGearValue - minGearValue)+0.3f * (gear+1));
			GetComponent<AudioSource> ().pitch = pitch;

			gear = i;
		}
	}

	void OnGUI()
	{
		//show the GUI for the speed and gear we are on.
		//GUI.Box(new Rect(10,10,70,30),"MPH: " + Mathf.Round(GetComponent<Rigidbody>().velocity.magnitude * 2.23693629f));
		/*if (!reversing)
			GUI.Box(new Rect(10,70,70,30),"Gear: " + (gear+1));
		if (reversing)//if the car is going backwards display the gear as R
			GUI.Box(new Rect(10,70,70,30),"Gear: R");*/
	}

    public void SetInputMethod(int method)
    {
        switch (method) //0 = Wheel, 1 = Keyboard, 2 = AI
        {
            case 0: //Wheel
                isKeyboard = false;
                isAI = false;
                break;
            case 1: //Keyboard
                isKeyboard = true;
                isAI = false;
                break;
            case 2://AI
                isKeyboard = false;
                isAI = true;
                break;
            default:
                break;
        }
        GameManager.SendMessage("InputSelected");
    }

    public void StartRace()
    {
        GameManager.SendMessage("StartRace");
    }

    public void Resume()
    {
        GameManager.SendMessage("ResumeGame");
    }

}
