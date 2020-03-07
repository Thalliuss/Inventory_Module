using DataManagement;
using DragAndDrop;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : ObjectContainerArray
{
    public static InventoryUI Instance { get; set; }

    public Player player;

    private bool _inventoryOpened;

    private void Start()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        CreateSlots(player.GetInventory());
    }

    public void ReArrange(string p_name, int p_index)
    {
        print(p_name + p_index);

        var test = new List<ItemProperties>();

        for (int i = 0; i < player.inventory.Count; i++) 
        {

            if (player.inventory[i] != null)
            {
                test.Add(ScriptableObject.CreateInstance<ItemProperties>());
                test[i].name = player.inventory[i].name;
                test[i].Sprite = player.inventory[i].Sprite;
                test[i].Amount = player.inventory[i].Amount;
                test[i].MaxAmount = player.inventory[i].MaxAmount;
            }
            else test.Add(null);

        }

        for (int i = 0; i < player.inventory.Count; i++) 
        {
            if (player.inventory[i] != null && player.inventory[i].name == p_name) 
            {
                test[p_index] = ScriptableObject.CreateInstance<ItemProperties>();
                test[p_index].name = p_name;
                test[p_index].Sprite = player.inventory[i].Sprite;
                test[p_index].Amount = player.inventory[i].Amount;
                test[p_index].MaxAmount = player.inventory[i].MaxAmount;

                test.RemoveAt(i);
                test.Insert(i, null);
            }
        }
        player.inventory = test;
        player.SaveInventory();
    }

    public void UpdateInventory() { CreateSlots(player.GetInventory()); }

    private void UIHandler()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _inventoryOpened = !_inventoryOpened;

            foreach (Transform child in transform)
                child.gameObject.SetActive(_inventoryOpened);

            GetComponent<Image>().enabled = _inventoryOpened;


            Cursor.visible = _inventoryOpened;
            if (_inventoryOpened)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
            }
        }
    }

    private void Update()
    {
        UIHandler();
    }
}

