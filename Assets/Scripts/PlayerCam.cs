using System;
using UnityEngine;
// https://www.youtube.com/watch?v=f473C43s8nE
public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

	[field: SerializeField] public Transform orientation { get; private set; }
	private float xRotation, yRotation;

    public RaycastHit _lookingAt;
    private const int itemLayer = 7;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false; 

        xRotation = 0.0f;
        yRotation = -360.0f;     
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // "where are we looking" ray
        Gizmos.DrawRay(transform.position, transform.forward * 10f);
    }

    // Update is called once per frame
    void Update() {

    }

    void LateUpdate()
    {
        MoveCam();
        Look();
    }

    private void MoveCam() {
		float mouseX = Input.GetAxisRaw("Mouse X") * _rotateSpeed;// * Time.deltaTime;
		float mouseY = Input.GetAxisRaw("Mouse Y") * _rotateSpeed;// * Time.deltaTime;

		yRotation += mouseX;
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -80f, 80f); // prevent player from looking directly up/down (neckbreaking behaviour)

		transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
		orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Look()
    {
        Physics.Raycast(transform.position, transform.forward, out _lookingAt, 10f, 1 << itemLayer);
    }
}
