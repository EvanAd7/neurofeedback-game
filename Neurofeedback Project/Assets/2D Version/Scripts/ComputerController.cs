using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerController : MonoBehaviour
{
    public float speed;
    public int sign;
    public float moveInterval;
    public float timer;

    [SerializeField]
    private IntSO theme;

    public Sprite sprite0;
    public Sprite sprite1;

    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(Random.Range(0f, 4f), 0, 0);

        if (theme.Value == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite0;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            gameObject.transform.localScale = new Vector3(0.54f, 0.54f, 0);
        } 
        else if (theme.Value == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 270);
            gameObject.transform.localScale = new Vector3(0.51f, 0.57f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
        //randomly moves vehicles back and forth
        if (timer < -moveInterval)
        {
            timer = moveInterval;
            sign *= -1;
        }
        else if (timer < 0)
        {
            transform.position += new Vector3(speed * sign, 0, 0) * Time.deltaTime;
        }

        //makes sure vehicles do not go off screen
        if (transform.position.x - 1.5 < -9 | transform.position.x + 1.5 > 9)
        {
            sign *= -1;
        }
    }
}
