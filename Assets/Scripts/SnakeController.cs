using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Unity.VisualScripting;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;

public class SnakeController : MonoBehaviour
{
    public static float BPM = 160;
    public float gameTimer = 120; // (seconds)
    public AudioClip click;
    public AudioClip clickOnBeat;
    public AudioSource audio;
    public AudioClip hitSFX;
    public AudioClip missSFX;
    public bool onBeat = false;
    public GameObject snakeBodyPrefab;
    public Material[] materialsArray;

    private static int subdivision = 8;
    private int beatCount = 0;
    private float beatInterval;
    private float timer;

    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI missedText;
    private int score;
    static int comboScore;

    public List<GameObject> inventory = new List<GameObject>();
    private List<Vector3> obstacles = new List<Vector3>();
    public GameObject tree;
    public List<GameObject> foodObjects = new List<GameObject>();
    string[] foods = { "banana", "banana", "banana", "banana" };

    public bool midiOnBeat = false;
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public List<double> timeStamps = new List<double>();
    public int spawnIndex = 0;
    public int inputIndex = 0;

    public static SnakeController Instance;
    public AudioSource audioSource;
    public float songDelayInSeconds;
    public int inputDelayInMilliseconds;
    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;
    public static double marginOfError=0;
    public static MidiFile midiFile;

    void Start()
    {
        Instance = this;
        string[] foods = { };
        CreateMap();


        // Load the MIDI file
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }

        // Get the notes from the MIDI file
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);
        SetTimeStamps(array);


        //Set Game Speed
        beatInterval = (120) / (BPM*subdivision);
        timer = beatInterval;

        audio = GetComponent<AudioSource>();

        score = 0;
        scoreText.text = "Score: " + score;
        missedText.text = " ";

        GameObject[] foodlights = GameObject.FindGameObjectsWithTag("FoodLight");
        foreach (var light in foodlights)
        {
            light.GetComponent<Light_Fader>().offBeat();
        }
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

   private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + fileLocation);
    }

    // Read MIDI Note Timestamps
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60 + metricTimeSpan.Seconds + metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

 

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }


    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    void CreateMap()
    {
        int i = 0;
        foreach (string food in foods)
        {
            int treeX = (int)(UnityEngine.Random.Range(10, 15) * System.Math.Pow(-1, i + 1));
            int treeY = (int)(UnityEngine.Random.Range(10, 15) * System.Math.Pow(-1, (int)i / 2));
            Vector3 treeVec = new Vector3(treeX, 1, treeY);

            GameObject foodObj = foodObjects[0];
            SpawnTree(treeVec, foodObj);
            i++;
        }
    }

    void SpawnTree(Vector3 pos, GameObject food)
    {
        obstacles.Add(pos);

        Instantiate(tree, pos, tree.transform.rotation);
        Instantiate(food, new Vector3(pos.x + 1, 0.5f, pos.z), food.transform.rotation);
        Instantiate(food, new Vector3(pos.x - 1, 0.5f, pos.z), food.transform.rotation);
        Instantiate(food, new Vector3(pos.x, 0.5f, pos.z + 1), food.transform.rotation);
        Instantiate(food, new Vector3(pos.x, 0.5f, pos.z - 1), food.transform.rotation);
    }

    public void StartSong()
    {
        audioSource.Play();
    }
    void Update()
    {
        GameObject[] foodlights = GameObject.FindGameObjectsWithTag("FoodLight");

        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0 || inputIndex >= timeStamps.Count)
        {
            Debug.Log("Game Over");
        }

    
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            onBeat = true;
            timer = beatInterval;
            beatCount++;
     

            moveSnake();
            
        
if (inputIndex < timeStamps.Count)
            {
                double timeStamp = timeStamps[inputIndex];
                double audioTime = GetAudioSourceTime() - inputDelayInMilliseconds / 1000f;

                if (audioTime > timeStamp){
                    inputIndex++;
                    Debug.Log("beat");
                    MidiBeatOn(foodlights);
                    
                }
                 else{
                
                MidiBeatOff(foodlights);
            }
            }
            else {
                MidiBeatOff(foodlights);
            }

        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveSnake();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -90, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Rotate(0, 90, 0);
        }
    }

    void moveSnake()
    {
        Vector3 prev = transform.position;
        transform.position += transform.forward;

        //stop from leaving map
        if (inMap(transform.position) && noObstacles(transform.position)) 
        {
            Vector3 temp;
            foreach (GameObject item in inventory)
            {
                temp = item.transform.position;
                item.transform.position = prev;
                prev = temp;
            }
        }
        else
        {
            transform.position = prev;
        }
    }

    void MidiBeatOn(GameObject[] foodlights)
    {
        midiOnBeat = true;
        //GetComponent<AudioSource>().PlayOneShot(clickOnBeat, 1);
        foreach (var light in foodlights)
        {
            light.GetComponent<Light_Fader>().Beat();
        }
    }

    void MidiBeatOff(GameObject[] foodlights)
    {
        midiOnBeat = false;
        //GetComponent<AudioSource>().PlayOneShot(click, 1);
        foreach (var light in foodlights)
        {
            light.GetComponent<Light_Fader>().offBeat();
        }
    }

    bool inMap(Vector3 pos)
    {
        return pos.x < 25 && pos.x > -25 && pos.z < 25 && pos.z > -25;
    }

    bool noObstacles(Vector3 pos)
    {
        foreach (Vector3 obstacle in obstacles)
        {
            Debug.Log(obstacle);
            if (Mathf.Abs(pos.x - obstacle.x) < 0.5f &&
                Mathf.Abs(pos.z - obstacle.z) < 0.5f)
            {
                return false;
            }
        }
        return true;
    }

    public void PickUpFood(GameObject food)
    {
        inventory.Add(food);
    }

    public void OnTriggerEnter(Collider other)
    {
   
        
        if (midiOnBeat || (beatCount % subdivision == 0))
        {
            comboScore++;
            GetComponent<AudioSource>().PlayOneShot(hitSFX, 1);
            Debug.Log("Nailed it!");
            missedText.text = "Nailed It!";

            if (!inventory.Contains(other.gameObject) && other.tag == "Food")
            {
                PickUpFood(other.gameObject);
                score++;
                scoreText.text = "Score: " + score;
            }
        }
        else
        {
            comboScore = 0;
            GetComponent<AudioSource>().PlayOneShot(missSFX, 1);

            if ((beatCount % subdivision == 1))
            {
                missedText.text = "Missed by " + 1 + " beat";
            }
            else 
            {
                missedText.text = "Missed by " + (beatCount % subdivision) + " beats";
            }
    
        // double timeStamp = timeStamps[inputIndex];

        Debug.Log("Missed by " + (beatCount % subdivision) + " beats");
        }
    }

    public Material RandomMaterial(Material[] _array_)
    {
        return _array_[UnityEngine.Random.Range(0, _array_.Length)];
    }
}
