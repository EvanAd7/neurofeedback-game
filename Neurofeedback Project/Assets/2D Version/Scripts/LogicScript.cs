using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public GameObject player;

    //coin management vars
    [SerializeField]
    private IntSO coins;
    public Text coinsText;
    private float coinTimer;

    //countdown timer management vars
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        timerIsRunning = true;
        coinsText.text = coins.Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //add coins to the player when in first place
        if (player.transform.position.x > 2.5)
        {
            coinTimer += Time.deltaTime;
            if (coinTimer > 0.5)
            {
                coins.Value += 100;
                coinsText.text = coins.Value.ToString();
                coinTimer = 0;
            }
        }

        //countdown timer
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                SceneManager.LoadScene(0);
            }
        }

        void DisplayTime(float timeToDisplay)
        {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
    }
}