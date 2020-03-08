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

    public void ReArrange(string p_name, int p_out, int p_in)
    {
        var test = new List<ItemProperties>();

        for (int i = 0; i < player.inventory.Count; i++) 
        {
            if (player.inventory[i] != null)
            {
                test.Add(ScriptableObject.CreateInstance<ItemProperties>());

                test[i].UIIndex = player.inventory[i].UIIndex;
                test[i].name = player.inventory[i].name;
                test[i].Sprite = player.inventory[i].Sprite;
                test[i].Amount = player.inventory[i].Amount;
                test[i].MaxAmount = player.inventory[i].MaxAmount;
            }
            else test.Add(null);
        }

        test[p_out] = ScriptableObject.CreateInstance<ItemProperties>();

        test[p_out].UIIndex = p_out;
        test[p_out].name = player.inventory[p_in].name;
        test[p_out].Sprite = player.inventory[p_in].Sprite;
        test[p_out].Amount = player.inventory[p_in].Amount;
        test[p_out].MaxAmount = player.inventory[p_in].MaxAmount;

        if (player.inventory[p_out] == null) {
            test.RemoveAt(p_in);
            test.Insert(p_in, null);
        } else {
            //TODO FIX SWAP PLACES
            test[p_out] = player.inventory[p_out];
        }

        player.inventory = test;
        player.SaveInventory();
    }

    public void UpdateInventory() 
    {
        CreateSlots(player.GetInventory());

        for (int i = 0; i < 2; i++) 
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

