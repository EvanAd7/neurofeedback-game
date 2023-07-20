using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    [SerializeField] public Transform path;

    public float maxSteerAngle;
    public float maxMotorTorque;
    public float maxBrakeTorque;
    public float currentSpeed;
    public float maxSpeed;

    public Vector3 centerOfMass;
    Rigidbody carRigidbody;
    
    public MeshRenderer meshRenderer;
    public Material[] brakeLightsOff;
    public Material[] brakeLightsOn;

    public Text speedText;
    float updateCounter = 0;

    public bool useSounds = false;
    public AudioSource carEngineSound;
    float initialCarEngineSoundPitch;

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    public MatLabListener listener;

    private bool isDriving = false;

    private List<Transform> nodes;
    private int currentNode = 0;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        centerOfMass = carRigidbody.centerOfMass = centerOfMass;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }

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
        if (Input.GetKey(KeyCode.Space))
        //if (listener.getInput() == 1)
        {
            isDriving = true;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
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
        updateCounter += Time.deltaTime;

        CarSounds();

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
