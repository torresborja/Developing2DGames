using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    // 1
    public GameObject slotPrefab;
    // 2
    public const int numSlots = 5;
    // 3
    Image[] itemImages = new Image[numSlots];

    // 4
    Item[] items = new Item[numSlots];
    // 5
    GameObject[] slots = new GameObject[numSlots];

    public void Start()
    {
        CreateSlots();
    }

    public void CreateSlots()
    {
        // 1
        if (slotPrefab != null)
        {
            // 2
            for (int i = 0; i < numSlots; i++)
            {
                // 3
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "ItemSlot_" + i;
                // 4
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                // 5
                slots[i] = newSlot;
                // 6
                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    // 1
    public bool AddItem(Item itemToAdd)
    {
        // 2
        for (int i = 0; i < items.Length; i++)
        {
            // 3
            if (items[i] != null && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable == true)
            {
                // Adding to existing slot
                // 4
                items[i].quantity = items[i].quantity + 1;
                // 5
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                // 6
                Text quantityText = slotScript.qtyText;
                // 7
                quantityText.enabled = true;
                // 8
                quantityText.text = items[i].quantity.ToString();
                // 9
                return true;
            }
            // 10
            if (items[i] == null)
            {
                // Adding to empty slot
                // Copy item & add to inventory. copying so we don’t change original Scriptable Object
                // 11
                items[i] = Instantiate(itemToAdd);
                // 12
                items[i].quantity = 1;
                // 13
                itemImages[i].sprite = itemToAdd.sprite;
                // 14
                itemImages[i].enabled = true;
                return true;
            }
        }
        // 15
        return false;
    }




}
