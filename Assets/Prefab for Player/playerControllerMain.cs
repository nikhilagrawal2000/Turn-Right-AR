using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerMain : MonoBehaviour
{
    [SerializeField]
    private float Drag;

    [SerializeField]
    private int Mass;

    [SerializeField]
    private float RotateSpeed = 2f;

    [SerializeField]
    private float initialRadius;

    private float radius;

    [SerializeField]
    private float radiusDecreaseRate;

    [SerializeField]
    private float freeVelocity;

    private Vector3 _centre;

    private Vector3 centerToCar;
    //private Vector3 offset;
    private float _angle;

    [SerializeField]
    private float velocity;
    private int state;

    public GameObject player;
    private Rigidbody playerRB;

    public AudioClip CarRunnning;
    private AudioSource audi;
    // Use this for initialization
    void Start()
    {
        audi = GetComponent<AudioSource>();
        state = 0;
        radius = initialRadius;
        playerRB = GetComponent<Rigidbody>();
        audi.clip = CarRunnning;
        audi.Play();
    }

    // Update is called once per frame	
    void Update()
    {
        switch (state)
        {
            case 0:
                {
                    transform.Translate(Vector3.forward * velocity * Time.deltaTime);
                    if (Input.GetMouseButtonDown(0))
                    {
                        state = 1;
                        radius = initialRadius;
                        /*
                            to set the coordinates of centre such that center is at a distance = radius from car and lies on the line
                            joining car and transform.right
                         */
                        _centre = transform.position + ((transform.right) * radius);

                        /* vector joining center and car */
                        centerToCar = -_centre + transform.position;

                        /* angle between carToCenter and -ve z axis */
                        if (centerToCar.z >= 0)
                        {
                            _angle = Mathf.Acos((centerToCar.x) / radius);
                        }
                        else
                        {
                            _angle = 2 * Mathf.PI - Mathf.Acos((centerToCar.x) / radius);
                        }

                    }
                    if (transform.GetChild(0).position.x < -100)
                    {
                        state = 2;
                    }
                    break;
                }
            case 1:
                {
                    radius -= Time.deltaTime * radiusDecreaseRate;
                    Vector3 vec = _centre - transform.position;
                    transform.right = vec / radius;

                    _angle = _angle - RotateSpeed * Time.deltaTime;
                    Vector3 offset = new Vector3(Mathf.Cos(_angle), 0, Mathf.Sin(_angle)) * radius;
                    transform.position = _centre + offset;
                    if (Input.GetMouseButtonUp(0))
                    {
                        state = 0;
                    }
                    if (transform.GetChild(0).position.x < -100)
                    {
                        state = 2;
                    }
                    break;
                }
            case 2:
                //transform.Translate(Vector3.forward * freeVelocity * Time.deltaTime);
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InnerBoundary")
        {
            Debug.Log("Inner boundary");
            state = 2;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "OuterBoundary")
        {
            state = 2;
            playerRB.velocity = transform.forward * freeVelocity;
            playerRB.mass = Mass;
            playerRB.drag = Drag;
            Debug.Log("Outside");
            audi.Stop();
        
        }
    }
}