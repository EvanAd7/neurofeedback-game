using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerCarController : MonoBehaviour
{
    public float speed = 5;
    public int sign = 1;
    public float moveInterval = 5;
    public float timer = 5;

    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(Random.Range(0f, 4f), 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
        //randomly moves cars back and forth
        if (timer < -moveInterval)
        {
            timer = moveInterval;
            sign *= -1;
        }
        else if (timer < 0)
        {
            transform.position += new Vector3(speed * sign, 0, 0) * Time.deltaTime;
        }

        //makes sure cars do not go off screen
        if (transform.position.x - 1.5 < -9 | transform.position.x + 1.5 > 9)
        {
            sign *= -1;
        }
    }
}
