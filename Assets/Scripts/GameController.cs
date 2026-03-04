using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public static GameObject Player { get; private set; }

    private void Awake()
    {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;

        Player = GameObject.FindWithTag("Player");
    }
}
