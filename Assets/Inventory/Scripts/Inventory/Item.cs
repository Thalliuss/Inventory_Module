using DataManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
	[SerializeField] private string _id = "none";

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

	public void Setup(string p_id)
	{
		if (p_id == "none") return;

		_id = p_id;

		_dataReferences = SceneManager.Instance.DataReferences;

		_data = _dataReferences.FindElement<ItemData>(p_id);
		if (_data == null) {
			_data = _dataReferences.AddElement<ItemData>(p_id);
			return;
		}

		LoadAlife();
	}

	public void SaveAlife()
	{
		_data.Alife = alife;
		_data.Save();
	}
	public void LoadAlife()
	{
		alife = _data.Alife;
	}

	#endregion

	[SerializeField] public ItemProperties itemData;

	public bool munuallyPlaced = false;
	public bool alife = true;

	public SpriteRenderer sprite;
	public Text text;

	void Start()
	{
		Setup(_id);
		SetVisuals();
	}

	public void SetVisuals() 
	{
		text.text = itemData.name.Replace("(Clone)", "");
		sprite.sprite = itemData.sprite;
	}

	private void Update()
	{
		if (!alife)
		{
			Destroy(gameObject);

			if (!munuallyPlaced) 
			{
				_dataReferences.RemoveElement<ItemData>(_id);
				DropManager.Instance.RemoveDrop(_id);
			}
		}

		transform.LookAt(FindObjectOfType<Player>().transform);
	}

	public void Kill() 
	{
		alife = false;
		SaveAlife();
	}

	public string GetID() { return _id; }
	public void SetID(string p_input) { _id = p_input; }
}