using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemProperties : ScriptableObject
{
    public int UIIndex;

    public Sprite Sprite;
    public int Amount;
    public int MaxAmount;

    public void Reset() { Amount = 0; }
}