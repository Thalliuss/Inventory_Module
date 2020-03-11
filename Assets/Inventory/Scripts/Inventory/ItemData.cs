using UnityEngine;
using DataManagement;
using System.Collections.Generic;

public class ItemData : DataElement {

	public bool Alife { get => _alife; set => _alife = value; }
	[SerializeField] private bool _alife = true;

	public ItemData(string p_id) : base(p_id)
	{
		ID = p_id;
	}
}