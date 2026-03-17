using TMPro;
using UnityEngine;

public enum Hand { Left, Right };
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
    [SerializeField] private GameObject _leftInventory;
    [SerializeField] private GameObject _rightInventory;
    [SerializeField] private UnityEngine.UI.Image _leftHeld;
    [SerializeField] private UnityEngine.UI.Image _rightHeld;

    [Header("Quest UI")]
    [SerializeField] private GameObject _questUI;
    [SerializeField] private TMP_Text _questText;

    // Start is called before the first frame update
    private void Awake()
    {
		if(Instance != null && Instance != this) {
			Destroy(this); return;
		}
		Instance = this;

        SwitchHeldItem(Hand.Left, null);
        SwitchHeldItem(Hand.Right, null);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject Player = GameObject.FindWithTag("Player");
        Player.GetComponent<Player>()._inventoryManager.SwitchActiveE += SwitchActiveUI;

        _itemUI.SetActive(false);

        QuestManager.Instance.newQuestReceivedE += SwitchQuestUI;

        SwitchQuestUI();
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
        if(hit.rigidbody.gameObject.CompareTag("Item")) {
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
        } else if(hit.rigidbody.gameObject.CompareTag("NPC")) {
            NPCData npcData = hit.rigidbody.gameObject.GetComponent<NPC>().npcData;

            if(_itemUI.activeSelf && _itemName.text == npcData.NPCName) { return; }

            if(npcData == null) {
                _itemUI.SetActive(false);            
            } else {
                _itemUI.SetActive(true);
                _itemName.text = npcData.NPCName;
                _itemDesc.text = npcData.NPCDesc;
                _itemTags.text = "";                
            }
        } else if(hit.rigidbody.gameObject.CompareTag("Boss")) {
            NPCData monsterData = hit.rigidbody.gameObject.GetComponent<Monster>().data;

            if(_itemUI.activeSelf && _itemName.text == monsterData.NPCName) { return; }
            _itemUI.SetActive(true);
            _itemName.text = monsterData.NPCName;
            _itemDesc.text = monsterData.NPCDesc;   
            _itemTags.text = "";
        }

    }
    public void SwitchHeldItem(Hand hand, ItemData item) {
        // Debug.Log("switching held item image");
        UnityEngine.UI.Image handRenderer = null;
        switch(hand) {
            case Hand.Left:
                handRenderer = _leftHeld;
                break;
            case Hand.Right:
                handRenderer = _rightHeld;
                break;
        }

        if(item == null) {
            handRenderer.gameObject.SetActive(false);
        } else {
            handRenderer.gameObject.SetActive(true);
            handRenderer.sprite = item.heldSprite;
            handRenderer.SetNativeSize();
        }
    }

    private void SwitchActiveUI(int whichIsNowActive) {
        // Debug.Log($"adjusting UI: {whichIsNowActive} is now active");
        RectTransform lTransform = _leftInventory.GetComponent<RectTransform>();
        RectTransform rTransform = _rightInventory.GetComponent<RectTransform>();

        switch(whichIsNowActive) {
            case 0: // left is now active
                lTransform.anchoredPosition = new Vector3(lTransform.anchoredPosition.x, lTransform.anchoredPosition.y + 70);
                rTransform.anchoredPosition = new Vector3(rTransform.anchoredPosition.x, rTransform.anchoredPosition.y - 70);
                break;
            case 1: // right is now active
                lTransform.anchoredPosition = new Vector3(lTransform.anchoredPosition.x, lTransform.anchoredPosition.y - 70);
                rTransform.anchoredPosition = new Vector3(rTransform.anchoredPosition.x, rTransform.anchoredPosition.y + 70);
                break;
        }
    }

    private void SwitchQuestUI()
    {
        QuestData questData = QuestManager.Instance._currentQuest;
        if(questData != null) {
            _questUI.SetActive(true);
            _questText.text = "Current Quest: " + questData.QuestName + "\n" + questData.QuestDesc;
        } else {
            _questUI.SetActive(false);            
        }
    }
}
