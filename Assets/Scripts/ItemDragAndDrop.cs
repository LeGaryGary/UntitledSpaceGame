/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public ItemSlot CurrentSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        CurrentSlot = GetComponentInParent<ItemSlot>();
        CurrentSlot.CurrentItem = this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        var slots = FindObjectsOfType<ItemSlot>();
        var slot = slots[0];
        var closestDist = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        foreach (var slotTest in slots)
        {
            var dist = new Vector2(slotTest.transform.position.x, slotTest.transform.position.y) - eventData.position;
           if (dist.sqrMagnitude < closestDist.sqrMagnitude)
            {

                slot = slotTest;
                closestDist = dist;
            }
        }

        Debug.Log(slot.name);
        transform.position = slot.transform.position;

        CurrentSlot.CurrentItem = null;
        if (slot.CurrentItem != null)
        {
            CurrentSlot.CurrentItem = slot.CurrentItem; // Make target slot's item be in the slot we are coming from
            CurrentSlot.CurrentItem.CurrentSlot = CurrentSlot; // Make target slot's old item have the slot we are coming from set as its new current slot
            CurrentSlot.CurrentItem.transform.position = CurrentSlot.transform.position; // Make the slot we are coming from's new item's position be the slot we are coming from
            CurrentSlot.CurrentItem.rectTransform.SetParent(CurrentSlot.transform); // Make the item that was in the new slot have its parent set to the slot we came from
        }
        slot.CurrentItem = this;
        CurrentSlot = slot;
        rectTransform.SetParent(CurrentSlot.transform);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

}
