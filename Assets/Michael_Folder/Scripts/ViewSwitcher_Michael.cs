using UnityEngine;

public class ViewSwitcher_Michael : MonoBehaviour
{
    public Transform mainCamera;     
    public Transform fpAnchor;      
    public Transform tpAnchor;

    [SerializeField] private GameObject playerMesh;

    private bool isFirstPerson = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isFirstPerson = !isFirstPerson;

            if (isFirstPerson)
            {
                mainCamera.position = fpAnchor.position;
                playerMesh.SetActive(false);
            }
            else
            {
                mainCamera.position = tpAnchor.position;
                playerMesh.SetActive(true);
            }
        }
    }
}
