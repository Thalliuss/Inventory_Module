using DataManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif


public class Player : MonoBehaviour
{
    #region Save implementation
    // A reference too all currently saved data.
    private DataReferences _dataReferences = null;

    // A reference too the data being saved in this class.
    private PlayerData _data = null;

    // The ID under wich this data will be saved.
    [SerializeField] private string _id = "player";

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        #if UNITY_EDITOR
        EditorSceneManager.MarkSceneDirty(gameObject.scene);
        #endif

        _id = "";
        System.Random t_random = new System.Random();
        const string t_chars = "AaBbCcDdErFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        _id = new string(Enumerable.Repeat(t_chars, 8).Select(s => s[t_random.Next(s.Length)]).ToArray());
    }

    private void Setup()
    {
        _dataReferences = SceneManager.Instance.DataReferences;

        _data = _dataReferences.FindElement<PlayerData>(_id);
        if (_data == null) {
            _data = _dataReferences.AddElement<PlayerData>(_id);
            return;
        }

        if (_data.Inventory.Count != 0)
            LoadInventory();
    }

    public void SaveInventory()
    {
        _data.Inventory.Clear();
        _data.ItemReferences.Clear();

        for (int i = 0; i < inventory.Count; i++)
        {
            _data.Inventory.Add(null);
            _data.ItemReferences.Add(new ItemProperties.References());

            for (int a = 0; a < _database.Count; a++)
            {
                if (inventory[i] != null && inventory[i].name.Replace("(Clone)", "") == _database[a].name) 
                {
                    _data.Inventory[i] = _database[a];

                    _data.ItemReferences[i].uiIndex = inventory[i].uiIndex;
                    _data.ItemReferences[i].sprite = inventory[i].sprite;
                    _data.ItemReferences[i].amount = inventory[i].amount;
                    _data.ItemReferences[i].maxAmount = inventory[i].maxAmount;
                    _data.ItemReferences[i].itemType = inventory[i].itemType;
                    _data.ItemReferences[i].durability = inventory[i].durability;
                    _data.ItemReferences[i].damage = inventory[i].damage;
                }
            }
        }

        _data.Save();
    }
    public void LoadInventory()
    {
        for (int i = 0; i < _data.Inventory.Count; i++)
        {
            if (_data.Inventory[i] != null) 
            {
                inventory[i] = Instantiate(_data.Inventory[i] as ItemProperties);

                inventory[i].uiIndex = _data.ItemReferences[i].uiIndex;
                inventory[i].sprite = _data.ItemReferences[i].sprite;
                inventory[i].amount = _data.ItemReferences[i].amount;
                inventory[i].maxAmount = _data.ItemReferences[i].maxAmount;
                inventory[i].itemType = _data.ItemReferences[i].itemType;
                inventory[i].durability = _data.ItemReferences[i].durability;
                inventory[i].damage = _data.ItemReferences[i].damage;
            }
        }
    }


    #endregion

    [SerializeField] private int _inventorySize = 0;
    public List<ItemProperties> inventory = new List<ItemProperties>();
    [SerializeField] private List<ItemProperties> _database = new List<ItemProperties>();

    private void Start() 
    {
        for (int i = 0; i < _inventorySize; i++)
            inventory.Add(null);

        Setup();
        InventoryUI.Instance.UpdateInventory();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item>() != null)
        {
            Item t_item = other.GetComponent<Item>();

            if (AddItem(Instantiate(t_item.itemData) as ItemProperties)) 
                t_item.Kill();
        }
    }

    public bool AddItem(ItemProperties t_itemData)
    {
        InventoryUI t_inventoryHandler = InventoryUI.Instance;

        for (int i = 0; i < _inventorySize; i++)
        {
            if (inventory[i] != null && inventory[i].name == t_itemData.name && inventory[i].amount < inventory[i].maxAmount)
            {
                inventory[i].amount++;

                t_inventoryHandler.UpdateInventory();
                return true;
            }

            if (inventory[i] == null)
            {
                inventory[i] = t_itemData;
                inventory[i].amount++;
                inventory[i].uiIndex = i;

                t_inventoryHandler.UpdateInventory();
                return true;
            }
        }
        return false;
    }

    public ItemProperties[] GetInventory() { return inventory.ToArray(); }
}
