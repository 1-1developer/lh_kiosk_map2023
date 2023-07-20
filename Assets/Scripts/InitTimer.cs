using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitTimer : MonoBehaviour
{
    public float timer = 0;
    float MaxTime = 5f;

    public bool isStart = false;

    [SerializeField]
    UImanager uImanager;

    [SerializeField]
    MainScreenNav nav;

    [SerializeField]
    HomeScreen home;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.touchCount > 0)
        {
            timer = 0;
        }
        if(isStart && timer > MaxTime)
        {
            uImanager.ShowHomeScreen();
            nav.initNav();
            home.initHome();
            AudioManager.StopSound();
            isStart = false;
            timer = 0;
        }
    }
}
