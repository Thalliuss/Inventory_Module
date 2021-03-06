using UnityEngine;
using DataManagement;
using System.Collections.Generic;
using System;

public class PlayerData : DataElement {

	public Vector3 Position { get => _position; set => _position = value; }
	[SerializeField] private Vector3 _position;

	public Vector3 Rotation { get => _rotation; set => _rotation = value; }
	[SerializeField] private Vector3 _rotation;

	public List<ItemProperties> Inventory { get => _inventory; set => _inventory = value; }
	[SerializeField] private List<ItemProperties> _inventory;

	public List<ItemProperties.References> ItemReferences { get => _itemReferences; set => _itemReferences = value; }
	[SerializeField] private List<ItemProperties.References> _itemReferences;

	public PlayerData(string p_id) : base(p_id)
	{
		ID = p_id;
	}
}