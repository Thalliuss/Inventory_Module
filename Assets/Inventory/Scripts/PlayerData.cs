using UnityEngine;
using DataManagement;
using System.Collections.Generic;
using System;

public class PlayerData : DataElement {

	public List<ItemProperties> Inventory { get => _inventory; set => _inventory = value; }
	[SerializeField] private List<ItemProperties> _inventory;

	[Serializable]
	public class ItemReference 
	{
		public string Name;
		public Sprite Sprite;
		public int Amount;
		public int MaxAmount;
	}
	public List<ItemReference> ItemReferences { get => _itemReferences; set => _itemReferences = value; }
	[SerializeField] private List<ItemReference> _itemReferences;

	public PlayerData(string p_id) : base(p_id)
	{
		ID = p_id;
	}
}