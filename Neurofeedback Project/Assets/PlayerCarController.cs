using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) & transform.position.x <= 4)
        {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime; 
        } 
        else if (transform.position.x - 1.5 >= -9)
        {
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        }
    }
}