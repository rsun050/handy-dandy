using UnityEngine;
// https://www.youtube.com/watch?v=f473C43s8nE
public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;

	public Transform orientation;
	float xRotation, yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
		float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _rotateSpeed;
		float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _rotateSpeed;

		yRotation += mouseX;
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f); // prevent player from looking directly up/down (neckbreaking behaviour)

		transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
		orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        // Rotate();
    }

    // this sucks.
    // private void Rotate()
    // {
    //     transform.eulerAngles += _rotateSpeed * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
    // }
}
