using UnityEngine;
using DataManagement;
using System.Collections.Generic;
using System;

public class PlayerData : DataElement {

	public List<ItemProperties> Inventory { get => _inventory; set => _inventory = value; }
	[SerializeField] private List<ItemProperties> _inventory;

	public List<ItemProperties.References> ItemReferences { get => _itemReferences; set => _itemReferences = value; }
	[SerializeField] private List<ItemProperties.References> _itemReferences;

	public Vector3 Position { get => _position; set => _position = value; }
	[SerializeField] private Vector3 _position;

	public Quaternion Rotation { get => _rotation; set => _rotation = value; }
	[SerializeField] private Quaternion _rotation;

	public PlayerData(string p_id) : base(p_id)
	{
		ID = p_id;
	}
}