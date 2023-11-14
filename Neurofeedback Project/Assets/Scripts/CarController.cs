using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    //path finding variables
    [SerializeField] public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;

    //driving variables
    public float maxSteerAngle;
    public float maxMotorTorque;
    public float maxBrakeTorque;
    public float currentSpeed;
    public float maxSpeed;
    
    //renderer variables
    public MeshRenderer meshRenderer;
    public Material[] brakeLightsOff;
    public Material[] brakeLightsOn;

    //UI variables
    public Text speedText;
    public Text currentLapText;
    public Text bestLapText;
    float updateCounter = 0;

    //sound variables
    public bool useSounds = false;
    public AudioSource carEngineSound;
    float initialCarEngineSoundPitch;

    //wheel colliders
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    //lap time tracker vars
    public Transform startLine;
    public Transform lapTracker;
    private bool didLap = true;
    private float lapTimer = 0;
    private int lapsDone = -1;
    private float bestLapTime = 10000;

    //misc bools
    private bool isDriving = false;
    private bool turning = false;
    public bool turnBraking = true;

    //misc vars
    public MatLabListener listener;
    public Vector3 centerOfMass;
    Rigidbody carRigidbody;

    void Start()
    {
        //lower car's center of mass
        carRigidbody = GetComponent<Rigidbody>();
        centerOfMass = carRigidbody.centerOfMass = centerOfMass;

        //create path
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

        //initiate sound loop
        if (carEngineSound != null)
        {
            initialCarEngineSoundPitch = carEngineSound.pitch;
        }

        if (useSounds)
        {
            InvokeRepeating("CarSounds", 0f, 0.1f);
        }
        else if (!useSounds)
        {
            if (carEngineSound != null)
            {
                carEngineSound.Stop();
            }
        }
    }

    void FixedUpdate()
    {   
        ApplySteer();
        
        //figure out if the car is turning
        if (Math.Abs(wheelFL.steerAngle) > 5f && turnBraking)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        //drive upon input
        //if (Input.GetKey(KeyCode.Space))
        if (listener.getInput() == 1)
        {
            isDriving = true;
            if (turning && currentSpeed > 40)
            {
                wheelRL.brakeTorque = 500;
                wheelRR.brakeTorque = 500;
                print("turning");
            } 
            else
            {
                wheelRL.brakeTorque = 0;
                wheelRR.brakeTorque = 0;
            }
            meshRenderer.materials = brakeLightsOff;
        } 
        else
        {
            isDriving = false;
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;
            meshRenderer.materials = brakeLightsOn;
        }
        Drive();
        CheckNodeDistance();
        CarSounds();
        currentLapText.text = "Current Lap: " + string.Format("{0:0}:{1:00}", Mathf.FloorToInt(lapTimer / 60), Mathf.FloorToInt(lapTimer % 60));

        //restart lap timer
        if (Vector3.Distance(transform.position, startLine.position) < 5f && didLap)
        {
            lapsDone++;

            if (lapTimer < bestLapTime && lapsDone > 0)
            {
                bestLapTime = lapTimer;
                bestLapText.text = "Best Lap: " + string.Format("{0:0}:{1:00}", Mathf.FloorToInt(bestLapTime / 60), Mathf.FloorToInt(bestLapTime % 60));
            }

            lapTimer = 0;
            didLap = false;
        }

        //make sure the player completes a lap
        if (Vector3.Distance(transform.position, lapTracker.position) < 5f)
        {
            didLap = true;
        }

        if (lapsDone >= 0)
        {
            lapTimer += Time.deltaTime;
        }
        updateCounter += Time.deltaTime;

        //update the speedometer
        if (updateCounter > 1)
        {
            speedText.text = Mathf.FloorToInt(currentSpeed).ToString() + " mph";
            updateCounter = 0;
        }
    }

    void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteerAngle = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;

        wheelFL.steerAngle = newSteerAngle;
        wheelFR.steerAngle = newSteerAngle;
    }

    void Drive()
    {
        currentSpeed = (2 * Mathf.PI * wheelFL.radius * wheelFL.rpm * 60 / 1000) + 0.1f;

        if (currentSpeed < maxSpeed && isDriving)
        {
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
        } 
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }
    }

    void CheckNodeDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 5f)
        {
            if (currentNode == nodes.Count - 1)
            {
                currentNode = 0;
            } 
            else
            {
                currentNode++;
            }
        }
    }

    void CarSounds()
    {
        if (useSounds)
        {
            try
            {
                if (carEngineSound != null)
                {
                    float engineSoundPitch = initialCarEngineSoundPitch + (Mathf.Abs(carRigidbody.velocity.magnitude) / 15f);
                    carEngineSound.pitch = engineSoundPitch;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
        }
        else if (!useSounds)
        {
            if (carEngineSound != null && carEngineSound.isPlaying)
            {
                carEngineSound.Stop();
            }
        }
    }
}
