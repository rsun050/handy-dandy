using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
