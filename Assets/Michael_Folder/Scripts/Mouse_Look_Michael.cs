using UnityEngine;

public class Mouse_Look_Michael : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;
    private bool isScriptEnabled = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(isScriptEnabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -85f, 85f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void Set_Mouse_Sensitivity(float paramFloat)
    {
        mouseSensitivity = paramFloat;
    }

    public void Enable_Mouse_Look(bool paramBool)
    {
        isScriptEnabled = paramBool;
        Debug.Log("Mouse Look " +  paramBool);
    }
}
