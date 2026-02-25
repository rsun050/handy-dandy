using System;
using UnityEngine;
// https://www.youtube.com/watch?v=f473C43s8nE
public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

	public Transform orientation;
	float xRotation, yRotation;

    private RaycastHit _lookingAt;
    private const int itemLayer = 7;

    public event Action DidntSeeE;
    public event Action<RaycastHit> SawE; // "saw" event

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // "where are we looking" ray
        Gizmos.DrawRay(transform.position, transform.forward * 10f);

    }

    // Update is called once per frame
    void Update()
    {
        MoveCam();
        Look();
    }

    private void MoveCam()
    {
		float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _rotateSpeed;
		float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _rotateSpeed;

		yRotation += mouseX;
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f); // prevent player from looking directly up/down (neckbreaking behaviour)

		transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
		orientation.rotation = Quaternion.Euler(0, yRotation, 0);        
    }

    private void Look()
    {
        if(Physics.Raycast(transform.position, transform.forward, out _lookingAt, 10f, 1 << itemLayer))
        {
            SawE?.Invoke(_lookingAt);
        } else
        {
            DidntSeeE?.Invoke();
        }
    }
}
