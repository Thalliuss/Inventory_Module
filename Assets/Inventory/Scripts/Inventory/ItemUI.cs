using DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : Draggable, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;
    [SerializeField] private GameObject _menu;

    private Button _equip;
    private Button _use;

    private bool _menuOpened = false;

    private Player _player;
    private InventoryUI _inventoryUI;
    private DropManager _dropManager;
    private ItemProperties _itemUI;


    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _inventoryUI = InventoryUI.Instance;
        _dropManager = DropManager.Instance;

        _equip = _menu.transform.GetChild(0).GetComponent<Button>();
        _use = _menu.transform.GetChild(1).GetComponent<Button>();
    }

    public override void UpdateObject()
    {
        _itemUI = obj as ItemProperties;

        // set the visible data
        if (_itemUI)
        {
            _image.sprite = _itemUI.sprite;
            _text.text = _itemUI.amount.ToString();

            if (slot != null)
                InventoryUI.Instance.ReArrange(obj.name, slot.index, _itemUI.uiIndex);
        }

        // turn off if it was null
        gameObject.SetActive(_itemUI != null);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKey(KeyCode.I))
        {
            _menuOpened = false;
            _menu.gameObject.SetActive(_menuOpened);
        }
    }

    public void MenuHandler() 
    {
        if (Input.GetKey(KeyCode.Mouse0)) return;

        _menuOpened = !_menuOpened;
        _menu.gameObject.SetActive(_menuOpened);

        if (_itemUI.itemType == ItemProperties.ItemType.Consumable)
            _use.interactable = true;

        if (_itemUI.itemType == ItemProperties.ItemType.Weapon)
            _equip.interactable = true;
    }

    public void Equip() 
    {
        
    }

    public void Use()
    {

    }

    public void Drop() 
    {
        if (_player.inventory[slot.index].amount > 1) 
        {
            _dropManager.GenerateItem(_itemUI);
            _player.inventory[slot.index].amount--;
            _inventoryUI.UpdateInventory();

            return;
        }

        _dropManager.GenerateItem(_itemUI);
        _player.inventory[slot.index] = null;
        _inventoryUI.UpdateInventory();
    }
}
