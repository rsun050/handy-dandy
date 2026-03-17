using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster_Scene_Game_Manager : MonoBehaviour
{
    public static Monster_Scene_Game_Manager Instance { get; private set; }

    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        CharacterController _controller = player.GetComponent<CharacterController>();

        // 2. Disable Controller (Crucial for CharacterController!)
        _controller.enabled = false;

        // 3. Move the player
        player.transform.position = spawnPoint.position;
        player.transform.rotation = spawnPoint.rotation;

        // 4. Re-enable Controller
        _controller.enabled = true;
    }
}
