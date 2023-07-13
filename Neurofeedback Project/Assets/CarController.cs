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

    public Text speedText;
    
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    private bool isDriving = false;

    private List<Transform> nodes;
    private int currentNode = 0;

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;

        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
            {
                nodes.Add(pathTransforms[i]);
            }
        }
    }

    void FixedUpdate()
    {
        ApplySteer();
        if (Input.GetKey(KeyCode.Space))
        {
            isDriving = true;
            wheelRL.brakeTorque = 0;
            wheelRR.brakeTorque = 0;
        } 
        else
        {
            isDriving = false;
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;
        }
        Drive();
        CheckNodeDistance();

        speedText.text = Mathf.FloorToInt(currentSpeed).ToString() + " km/h";
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
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 1f)
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
}
