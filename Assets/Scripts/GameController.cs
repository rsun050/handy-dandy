using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public static Player Player { get; private set; }
    public static GameObject PlayerObj { get; private set; }
    public static GameObject PlayerCam { get; private set; }

    private void Awake()
    {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;

        PlayerObj = GameObject.FindWithTag("Player");
        Player = PlayerObj.GetComponent<Player>();
        PlayerCam = GameObject.FindWithTag("Camera");
    }
}
