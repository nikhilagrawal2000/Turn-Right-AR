using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    private int LapCount;
    private AudioSource audi;
    private int carState;
    public float increaseRate;
    private Vector3 ycarRotation;
    public float decreaseRate;
    public int mass;
    public float drag;
    public GameObject RestartMenu;
    public GameObject player;
    public AudioClip Slipsound;
    public AudioClip BackGroundsound;
    public bool isGameOver;
    public Text Score;
    public Text HighScore;
    private float StartTime;
    public GameObject StartMenu;
    public Text Lap;


    [SerializeField]
    private float freeVelocity;

    private enum State
    {
        STRAIGHT, TURN, FREE
    };

    State current;

    // Use this for initialization
    void Start()
    {
        current = State.STRAIGHT;
        ycarRotation = transform.localEulerAngles;
        audi = GetComponent<AudioSource>();

        audi.clip = BackGroundsound;
        audi.Play();
        RestartMenu.SetActive(false);
        isGameOver = false;
        StartTime = Time.time;
        HighScore.text = "HighScore : " + PlayerPrefs.GetFloat("HighScore").ToString("f0");
        Time.timeScale = 0.0f;
        LapCount = 0;
        Lap.text = "Laps : 0";
    }

    // Update is called once per frame
    void Update()
    {
        SetHighScore();
        switch (current)
        {

            case State.STRAIGHT:
                //Debug.Log("in straight");
                if (ycarRotation.y > 0)
                {
                    ycarRotation.y = ycarRotation.y - decreaseRate * Time.deltaTime;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    current = State.TURN;
                }
                transform.localRotation = Quaternion.Euler(ycarRotation);
                if (transform.position.x < -100)
                {
                    current = State.FREE;
                    transform.gameObject.AddComponent<Rigidbody>();
                    Rigidbody rb = GetComponent<Rigidbody>();
                    //transform.Translate(Vector3.forward*freeVelocity*Time.deltaTime);
                    rb.velocity = transform.forward * freeVelocity;
                    rb.mass = mass;
                    rb.drag = drag;
                }
                break;

            case State.TURN:
                // Debug.Log("in turn");

                if (Input.GetMouseButton(0))
                {
                    ycarRotation.y += increaseRate * Time.deltaTime;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    current = State.STRAIGHT;
                }
                transform.localRotation = Quaternion.Euler(ycarRotation);
                // Debug.Log(ycarRotation.y);
                break;

            case State.FREE:
                //Debug.Log("FREE");
                break;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InnerBoundary")
        {
            isGameOver = true;
            audi.clip = Slipsound;
            audi.Play();
            current = State.FREE;
            transform.gameObject.AddComponent<Rigidbody>();
            Rigidbody rb = GetComponent<Rigidbody>();
            //transform.Translate(Vector3.forward*freeVelocity*Time.deltaTime);
            rb.velocity = transform.forward * freeVelocity;
            rb.mass = mass;
            rb.useGravity = false;
            rb.drag = drag;
            Debug.Log("Inner boundary child");
            current = State.FREE;
            Invoke("GameOver", 2.0f);

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "OuterBoundary")
        {
            if (audi.clip != Slipsound)
            {
                audi.clip = Slipsound;
                audi.Play();
            }
            isGameOver = true;
            current = State.FREE;
            Debug.Log("Outside");
            Invoke("GameOver", 2.0f);

        }

        if(other.gameObject.tag == "LapBoundary")
        {
            LapCount++;
            Lap.text = "Laps : " + LapCount.ToString();
        }
    }

    void SetHighScore()
    {
        float t = Time.time - StartTime;
        float score = ((t % 60) * 100);
        string score_string = score.ToString("f0");

        if (!isGameOver)
        {
            Score.text = "Score : " + score_string;
            if (PlayerPrefs.GetFloat("HighScore") < score)
            {
                PlayerPrefs.SetFloat("HighScore", score);
                HighScore.text = "HighScore : " + score.ToString("f0");
            }

        }
        else
        {
            HighScore.text = "HighScore : " + PlayerPrefs.GetFloat("HighScore").ToString("f0");
        }
    }

    public void GameOver()
    {
        print("retarted");
        RestartMenu.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("LoadScene");
    }

    public void StartGame()
    {
        Time.timeScale = 1.0f;
        StartMenu.SetActive(false);

    }

}
