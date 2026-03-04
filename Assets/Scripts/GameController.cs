using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public static GameObject Player { get; private set; }

    [Header("Item UI")]
    [SerializeField] private GameObject _itemUI;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDesc;
    [SerializeField] private TMP_Text _itemTags;
    [SerializeField] private PlayerCam _playerCam;

    private void Awake()
    {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;

        Player = GameObject.FindWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        _itemUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerCam._lookingAt.rigidbody)
        {
            LookingAtUI();
        } else
        {
            if(_itemUI.activeSelf)
            {
                _itemUI.SetActive(false);            
            }
        }
    }

    private void LookingAtUI()
    {
        RaycastHit hit = _playerCam._lookingAt;
        ItemData itemData = hit.rigidbody.gameObject.GetComponent<Item>().Data;
        if(_itemUI.activeSelf && _itemName.text == itemData.ItemName) { return; } // do nothing if item we're looking at hasn't changed

        _itemUI.SetActive(true);
        _itemName.text = itemData.ItemName;
        _itemDesc.text = itemData.Description;

        string tags = "";
        foreach(ItemTag tag in itemData.Tags)
        {
            tags += tag.ToString() + ", ";
        }
        if(tags != "") { tags = tags.Substring(0, tags.Length - 2); } // remove last comma

        _itemTags.text = tags;
    }
}
