using DataManagement;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class Item : MonoBehaviour
{
	#region Save implementation
	// A reference too all currently saved data.
	private DataReferences _dataReferences = null;

	// A reference too the data being saved in this class.
	private ItemData _data = null;

	// The ID under wich this data will be saved.
	[SerializeField] private string _id = "item";

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

		_data = _dataReferences.FindElement<ItemData>(_id);
		if (_data == null) {
			_data = _dataReferences.AddElement<ItemData>(_id);
			return;
		}

		LoadAlife();
	}

	public void SaveAlife()
	{
		_data.Alife = _alife;
		_data.Save();
	}
	public void LoadAlife()
	{
		_alife = _data.Alife;
	}

#endregion

	[SerializeField] public ItemProperties itemData;

    private bool _alife = true;

	void Start()
	{
		Setup();
		SaveAlife();
		Dispose();
	}

	private void Dispose() { if (!_alife) Destroy(gameObject); }

	public void Kill() 
	{ 
		_alife = false;
		Dispose();
		SaveAlife();
	}
}