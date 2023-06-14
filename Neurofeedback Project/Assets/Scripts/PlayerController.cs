using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;

    [SerializeField]
    private IntSO theme;

    public Sprite sprite0;
    public Sprite sprite1;

    // Start is called before the first frame update
    void Start()
    {
        if (theme.Value == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite0;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            gameObject.transform.localScale = new Vector3(0.38f, 0.38f, 0);
        }
        else if (theme.Value == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 270);
            gameObject.transform.localScale = new Vector3(0.63f, 0.68f, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) & transform.position.x <= 3)
        {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime; 
        } 
        else if (transform.position.x - 1.5 >= -9)
        {
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        }
    }
}
