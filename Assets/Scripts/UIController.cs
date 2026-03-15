using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("Item UI")]
    [SerializeField] private GameObject _itemUI;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDesc;
    [SerializeField] private TMP_Text _itemTags;
    [SerializeField] private PlayerCam _playerCam;

    [Header("Inventory UI")]
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private UnityEngine.UI.Image _leftInventory;
    [SerializeField] private UnityEngine.UI.Image _rightInventory;

    [Header("Quest UI")]
    [SerializeField] private TMP_Text _questUI;

    // Start is called before the first frame update
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
        GameObject Player = GameObject.FindWithTag("Player");
        Player.GetComponent<Player>()._inventoryManager.SwitchActiveE += SwitchActiveUI;

        _itemUI.SetActive(false);

        QuestManager.Instance.newQuestReceivedE += SwitchQuestUI;
    }

    // Update is called once per frame
    void Update() {
        if(_playerCam._lookingAt.rigidbody) {
            LookingAtUI();
        } else {
            if(_itemUI.activeSelf) {
                _itemUI.SetActive(false);            
            }
        }
    }

    private void LookingAtUI() {
        RaycastHit hit = _playerCam._lookingAt;
        ItemData itemData = hit.rigidbody.gameObject.GetComponent<Item>().Data;
        if(_itemUI.activeSelf && _itemName.text == itemData.ItemName) { return; } // do nothing if item we're looking at hasn't changed

        _itemUI.SetActive(true);
        _itemName.text = itemData.ItemName;
        _itemDesc.text = itemData.Description;

        string tags = "";
        foreach(ItemTag tag in itemData.Tags) {
            tags += tag.ToString() + ", ";
        }
        if(tags != "") { tags = tags.Substring(0, tags.Length - 2); } // remove last comma

        _itemTags.text = tags;
    }

    private void SwitchActiveUI(int whichIsNowActive) {
        Debug.Log($"adjusting UI: {whichIsNowActive} is now active");
        RectTransform lTransform = _leftInventory.GetComponent<RectTransform>();
        RectTransform rTransform = _rightInventory.GetComponent<RectTransform>();

        switch(whichIsNowActive) {
            case 0: // left is now active
                lTransform.anchoredPosition = new Vector3(lTransform.anchoredPosition.x, lTransform.anchoredPosition.y + 35);
                rTransform.anchoredPosition = new Vector3(rTransform.anchoredPosition.x, rTransform.anchoredPosition.y - 35);
                break;
            case 1: // right is now active
                lTransform.anchoredPosition = new Vector3(lTransform.anchoredPosition.x, lTransform.anchoredPosition.y - 35);
                rTransform.anchoredPosition = new Vector3(rTransform.anchoredPosition.x, rTransform.anchoredPosition.y + 35);
                break;
        }
    }

    private void SwitchQuestUI()
    {
        QuestData questData = QuestManager.Instance._currentQuest;
        _questUI.text = "Current Quest: " + questData.QuestName + "\n" + questData.QuestDesc;
    }
}
