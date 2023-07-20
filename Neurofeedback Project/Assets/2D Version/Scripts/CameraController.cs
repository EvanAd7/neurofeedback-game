using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private IntSO theme;

    // Start is called before the first frame update
    void Start()
    {
        if (theme.Value == 0)
        {
            Camera.main.backgroundColor = new Color(0.2775f, 0.2959f, 0.3113f);
        } 
        else if (theme.Value == 1)
        {
            Camera.main.backgroundColor = new Color(0, 0, 0);
        }
    }
}
