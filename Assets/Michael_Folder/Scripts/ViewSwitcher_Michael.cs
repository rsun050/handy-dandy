using UnityEngine;

public class ViewSwitcher_Michael : MonoBehaviour
{
    public Transform mainCamera;     
    public Transform fpAnchor;      
    public Transform tpAnchor;       

    public MeshRenderer playerMesh;

    private bool isFirstPerson = true;

    private void Start()
    {
        playerMesh.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isFirstPerson = !isFirstPerson;

            if (isFirstPerson)
            {
                mainCamera.position = fpAnchor.position;
                playerMesh.enabled = false;
            }
            else
            {
                mainCamera.position = tpAnchor.position;
                playerMesh.enabled = true;
            }
        }
    }
}
