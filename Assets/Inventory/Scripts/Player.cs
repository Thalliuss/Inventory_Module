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
            _data.ItemReferences.Add(new PlayerData.ItemReference());

            for (int a = 0; a < _database.Count; a++)
            {
                if (inventory[i] != null && inventory[i].name.Replace("(Clone)", "") == _database[a].name) 
                {
                    _data.Inventory[i] = _database[a];

                    _data.ItemReferences[i].UIIndex = inventory[i].UIIndex;
                    _data.ItemReferences[i].MaxAmount = inventory[i].MaxAmount;
                    _data.ItemReferences[i].Sprite = inventory[i].Sprite;
                    _data.ItemReferences[i].Amount = inventory[i].Amount;
                    _data.ItemReferences[i].MaxAmount = inventory[i].MaxAmount;
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

                inventory[i].UIIndex = _data.ItemReferences[i].UIIndex;
                inventory[i].Sprite = _data.ItemReferences[i].Sprite;
                inventory[i].Amount = _data.ItemReferences[i].Amount;
                inventory[i].MaxAmount = _data.ItemReferences[i].MaxAmount;
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
            if (inventory[i] != null && inventory[i].name == t_itemData.name && inventory[i].Amount < inventory[i].MaxAmount)
            {
                inventory[i].Amount++;

                t_inventoryHandler.UpdateInventory();
                SaveInventory();
                return true;
            }

            if (inventory[i] == null)
            {
                inventory[i] = t_itemData;
                inventory[i].Amount++;

                t_inventoryHandler.UpdateInventory();
                SaveInventory();
                return true;
            }
        }
        return false;
    }

    public void AddItem(GameObject t_item) 
    {
        InventoryUI t_inventoryHandler = InventoryUI.Instance;
        ItemProperties t_itemData = Instantiate(t_item.GetComponent<Item>().itemData) as ItemProperties;

        if (t_item == null) return;

        for (int i = 0; i < _inventorySize; i++)
        {
            if (inventory[i] != null && inventory[i].name == t_itemData.name && inventory[i].Amount < inventory[i].MaxAmount)
            {
                inventory[i].Amount++;

                t_inventoryHandler.UpdateInventory();
                SaveInventory();
                return;
            }
        }

        for (int i = 0; i < _inventorySize; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = t_itemData;
                inventory[i].Amount++;
                inventory[i].UIIndex = i;

                t_inventoryHandler.UpdateInventory();
                SaveInventory();
                return;
            }
        }
    }

    public ItemProperties[] GetInventory() { return inventory.ToArray(); }
}
