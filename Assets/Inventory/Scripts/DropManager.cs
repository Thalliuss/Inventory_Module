using DataManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DropManager : MonoBehaviour
{

	#region Save implementation
	// A reference too all currently saved data.
	private DataReferences _dataReferences = null;

	// A reference too the data being saved in this class.
	private DropManagerData _data = null;

	// The ID under wich this data will be saved.
	[SerializeField] private string _id = "DropManager";

	[ContextMenu("Generate ID")]
	public void GenerateID()
	{
		#if UNITY_EDITOR
		if (!Application.isPlaying)
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

		_data = _dataReferences.FindElement<DropManagerData>(_id);
		if (_data == null)
		{
			_data = _dataReferences.AddElement<DropManagerData>(_id);
			return;
		}

		LoadDrops();
	}

	public void SaveDrops(GameObject p_input)
	{
		ItemProperties t_properties = p_input.GetComponent<Item>().itemData;

		_data.Count++;
		_data.Position.Add(p_input.transform.position);
		_data.IDs.Add(p_input.GetComponent<Item>().GetID());
		_data.ItemProperties.Add(null);
		_data.ItemReferences.Add(new ItemProperties.References());

		for (int i = 0; i < _data.Count; i++) 
		{
			for (int a = 0; a < _database.Length; a++)
			{
				if (p_input.name.Replace("(Clone)", "") == _database[a].name && _data.ItemProperties[i] == null)
				{
					_data.ItemProperties[i] = _database[a];

					_data.ItemReferences[i].amount = t_properties.amount;
					_data.ItemReferences[i].maxAmount = t_properties.maxAmount;
					_data.ItemReferences[i].sprite = t_properties.sprite;
					_data.ItemReferences[i].itemType = t_properties.itemType;
					_data.ItemReferences[i].damage = t_properties.damage;
					_data.ItemReferences[i].durability = t_properties.durability;
				}
			}
		}

		_data.Save();
	}

	public void LoadDrops()
	{
		for (int i = 0; i < _data.Count; i++)
		{

			GameObject t_object = Instantiate(_prefab);
			t_object.transform.position = _data.Position[i];

			Item t_item = t_object.GetComponent<Item>();

			print(_data.IDs[i] + " " + i);

			t_item.SetID(_data.IDs[i]);

			t_item.itemData = _data.ItemProperties[i];

			t_item.itemData.amount = _data.ItemReferences[i].amount;
			t_item.itemData.maxAmount = _data.ItemReferences[i].maxAmount;
			t_item.itemData.sprite = _data.ItemReferences[i].sprite;
			t_item.itemData.itemType = _data.ItemReferences[i].itemType;
			t_item.itemData.damage = _data.ItemReferences[i].damage;
			t_item.itemData.durability = _data.ItemReferences[i].durability;
		}
	}

	public void RemoveDrop(string p_id)
	{
		for (int i = 0; i < _data.Count; i++) 
		{
			if (_data.IDs[i] == p_id) 
			{
				_data.Count--;
				_data.IDs.RemoveAt(i);
				_data.Position.RemoveAt(i);
				_data.ItemReferences.RemoveAt(i);
				_data.ItemProperties.RemoveAt(i);
			}
		}
	}

	#endregion

	public static DropManager Instance { get; set; }

	[SerializeField] private GameObject _prefab = null;
	[SerializeField] private ItemProperties[] _database = null;

	private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

	private void Start()
	{
		Setup();
	}
}
