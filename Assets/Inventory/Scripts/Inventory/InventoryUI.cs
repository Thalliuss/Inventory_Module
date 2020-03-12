using DragAndDrop;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryUI : ObjectContainerArray
{
    public static InventoryUI Instance { get; set; }

    public Player player;

    public bool inventoryOpened = false;
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

                test[i].uiIndex = player.inventory[i].uiIndex;
                test[i].name = player.inventory[i].name;
                test[i].sprite = player.inventory[i].sprite;
                test[i].amount = player.inventory[i].amount;
                test[i].maxAmount = player.inventory[i].maxAmount;
                test[i].itemType = player.inventory[i].itemType;
                test[i].durability = player.inventory[i].durability;
                test[i].damage = player.inventory[i].damage;
            }
            else test.Add(null);
        }


        test[p_out] = ScriptableObject.CreateInstance<ItemProperties>();

        test[p_out].uiIndex = p_out;
        test[p_out].name = player.inventory[p_in].name;
        test[p_out].sprite = player.inventory[p_in].sprite;
        test[p_out].amount = player.inventory[p_in].amount;
        test[p_out].maxAmount = player.inventory[p_in].maxAmount;
        test[p_out].itemType = player.inventory[p_in].itemType;
        test[p_out].durability = player.inventory[p_in].durability;
        test[p_out].damage = player.inventory[p_in].damage;

        if (player.inventory[p_out] == null) {
            test.RemoveAt(p_in);
            test.Insert(p_in, null);
        } else {
            test[p_in] = player.inventory[p_out];
            test[p_in].uiIndex = p_in;

            test[p_out] = player.inventory[p_in];
            test[p_out].uiIndex = p_out;
        }

        player.inventory = test;
        player.SaveInventory();
    }

    public void UpdateInventory() 
    {
        CreateSlots(player.GetInventory());

        for (int i = 0; i < 2; i++) 
        {
            inventoryOpened = !inventoryOpened;

            foreach (Transform child in transform)
                child.gameObject.SetActive(inventoryOpened);

            GetComponent<Image>().enabled = inventoryOpened;


            /*Cursor.visible = inventoryOpened;
            if (inventoryOpened)
            {
                Cursor.lockState = CursorLockMode.None;
                FindObjectOfType<FirstPersonController>().mouseLook.lockCursor = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                FindObjectOfType<FirstPersonController>().mouseLook.lockCursor = true;
            }*/ 
        }
        player.SaveInventory();
    }

    private void UIHandler()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpened = !inventoryOpened;

            foreach (Transform child in transform)
                child.gameObject.SetActive(inventoryOpened);

            GetComponent<Image>().enabled = inventoryOpened;

            Cursor.visible = inventoryOpened;
            if (!inventoryOpened)
            {
                FindObjectOfType<FirstPersonController>().mouseLook.lockCursor = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                FindObjectOfType<FirstPersonController>().mouseLook.lockCursor = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void Update()
    {
        UIHandler();
    }
}

