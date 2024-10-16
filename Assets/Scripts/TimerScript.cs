using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{

    private float timeLeft = 300f;
    public bool timerRunning = false;

    public TMPro.TextMeshProUGUI timerText;

private int score;

    // Start is called before the first frame update
    void Start()
    {
        timerRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerRunning){
            if(timeLeft > 0){
                timeLeft -= Time.deltaTime;
                
                timerText.text = "Time Left: " + timeLeft;
            } else {
                timerRunning = false;
                timerText.text = "Time's Up!";
                //Quit game
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
                
            }
        }
        
    }

    void updateTimer(float currentTime){
        currentTime+=1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
