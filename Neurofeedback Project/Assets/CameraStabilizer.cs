using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStabilizer : MonoBehaviour
{
    public GameObject car;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(car.transform.eulerAngles.x - car.transform.eulerAngles.x, car.transform.eulerAngles.y, car.transform.eulerAngles.z - car.transform.eulerAngles.z);
    }
}
