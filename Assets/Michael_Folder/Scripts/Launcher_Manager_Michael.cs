using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher_Manager_Michael : MonoBehaviour
{
    public static Launcher_Manager_Michael Instance { get; private set; }

    //public Rigidbody rb;
    //public float angleInDegrees = 45f;
    //public float horizontalInDegrees = 45f;
    //public float launchForce = 10f;
    //private Vector3 startingPosition;
    //private float directionMultiplierY = 1f;
    //private float directionMultiplierZ = -1f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //startingPosition = transform.position;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    LaunchCube();
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    ResetCube();
        //}
    }

    //void LaunchCube()
    //{
    //    //rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

    //    int randomAngle = Random.Range(50, 80);
    //    directionMultiplierY *= -1;
    //    directionMultiplierZ *= -1;

    //    // Convert the angle to Radians for math functions
    //    float angleInRadians = randomAngle * Mathf.Deg2Rad;

    //    // Calculate direction
    //    float y = Mathf.Sin(angleInRadians) * directionMultiplierY;
    //    float z = Mathf.Cos(angleInRadians) * directionMultiplierZ;

    //    Vector3 launchDirection = new Vector3(0, y, z);

    //    // Apply the force!
    //    rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
    //}

    public void LaunchCube(int angle, Rigidbody rigidBody, int launchForce)
    {
        float verticalRad = angle * Mathf.Deg2Rad;

        float horizontalAngle = Random.Range(0f, 360f);
        float horizontalRad = horizontalAngle * Mathf.Deg2Rad;

        float y = Mathf.Sin(verticalRad);

        float horizontalTotal = Mathf.Cos(verticalRad);
        float x = Mathf.Cos(horizontalRad) * horizontalTotal;
        float z = Mathf.Sin(horizontalRad) * horizontalTotal;

        Vector3 launchDirection = new Vector3(x, y, z).normalized;

        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(launchDirection * launchForce, ForceMode.Impulse);
    }

    private void ResetCube()
    {
        //transform.position = startingPosition;
    }
}
