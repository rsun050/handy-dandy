using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [Header("Movement")]
    [SerializeField] private GameObject _itemUI;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDesc;
    [SerializeField] private TMP_Text _itemTags;


    [SerializeField] private PlayerCam _playerCam;

    private GameObject _lookingAtItem;

    private void Awake()
    {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SubscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        LookingAtUI();
    }

    private void SubscribeToEvents()
    {
        // _playerCam.SawE += LookingAtUI;
        // _playerCam.DidntSeeE += NotLookingAtUI;
    }

    private void LookingAtUI()
    {
        GameObject playerCamLooking = _playerCam._lookingAt.collider.gameObject;
        
        if(playerCamLooking == null)
        {
            NotLookingAtUI();
        }
        else if(playerCamLooking != _lookingAtItem)
        {
            LookingAtUI(_playerCam._lookingAt);
        }
    }

    private void NotLookingAtUI()
    {
        _itemUI.SetActive(false);
    }

    private void LookingAtUI(RaycastHit hit)
    {
        _itemUI.SetActive(true);
        ItemData itemData = hit.collider.gameObject.GetComponent<Item>().Data;

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
