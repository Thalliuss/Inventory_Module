using DragAndDrop;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : Draggable, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;

    public override void UpdateObject()
    {
        ItemProperties item = obj as ItemProperties;

        // set the visible data
        if (item)
        {
            image.sprite = item.Sprite;

            if (slot != null)
                InventoryUI.Instance.ReArrange(obj.name, slot.index, item.UIIndex);
        }

        // turn off if it was null
        gameObject.SetActive(item != null);
    }
}
