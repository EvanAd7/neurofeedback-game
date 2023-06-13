using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public GameObject player;
    public int coins;
    public Text coinsText;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > 2.5)
        {
            timer += Time.deltaTime;
            if (timer > 0.5)
            {
                coins += 100;
                coinsText.text = coins.ToString();
                timer = 0;
            }
        }
    }
}
