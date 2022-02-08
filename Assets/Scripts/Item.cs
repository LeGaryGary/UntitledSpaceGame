using UnityEngine;

public class Item : MonoBehaviour
{
    public string Id { get; set; }
    public int Quantity { get; set; }
    public GameObject InventoryIconPrefab;
    public GameObject BlockPrefab;
}
