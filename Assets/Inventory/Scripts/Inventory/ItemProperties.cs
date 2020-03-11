using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemProperties : ScriptableObject
{
	[HideInInspector] public int uiIndex;
	 public int amount;

	public enum ItemType { Item, Weapon, Consumable }
	public ItemType itemType;

	public Sprite sprite;
	public int maxAmount;
	public int damage;
	public int durability;

	[Serializable]
	public class References
	{
		[HideInInspector] public int uiIndex;
		[HideInInspector] public int amount;

		public ItemType itemType;

		public Sprite sprite;
		public int maxAmount;
		public int damage;
		public int durability;
	}
}