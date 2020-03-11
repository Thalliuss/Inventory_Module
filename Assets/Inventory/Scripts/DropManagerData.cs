using UnityEngine;
using DataManagement;
using System.Collections.Generic;

public class DropManagerData : DataElement {

	public int Count { get => _count; set => _count = value; }
	[SerializeField] private int _count;

	public List<Vector3> Position { get => _position; set => _position = value; }
	[SerializeField] private List<Vector3> _position;

	public List<ItemProperties> ItemProperties { get => _itemProperties; set => _itemProperties = value; }
	[SerializeField] private List<ItemProperties> _itemProperties;

	public List<ItemProperties.References> ItemReferences { get => _itemReferences; set => _itemReferences = value; }
	[SerializeField] private List<ItemProperties.References> _itemReferences;

	public List<string> IDs { get => _ids; set => _ids = value; }
	[SerializeField] private List<string> _ids;

	public DropManagerData(string p_id) : base(p_id)
	{
		ID = p_id;
	}
}