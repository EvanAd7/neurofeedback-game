using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    private GameObject background;
    public float spawnRate;
    private float timer = 0;

    [SerializeField]
    private IntSO theme;

    public GameObject background0;
    public GameObject background1;

    // Start is called before the first frame update
    void Start()
    {
        if (theme.Value == 0)
        {
            background = background0;
        }
        else if (theme.Value == 1)
        {
            background = background1;
        }

        spawnTrack();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawnTrack();
            timer = 0;
        }
    }

    void spawnTrack()
    {
        Instantiate(background, transform.position + new Vector3(18,0,0), transform.rotation);
    }
}
