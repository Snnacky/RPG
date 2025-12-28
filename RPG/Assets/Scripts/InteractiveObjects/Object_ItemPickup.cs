using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
//添加在物品模型
public class Object_ItemPickup : MonoBehaviour
{
    [SerializeField] private Vector2 dropForce = new Vector2(3, 10);//丢出的力
    [SerializeField] private ItemDataSO itemData;

    [Space]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer sr;

    private bool mustStop = false;

    private float dropDuration = 0.5f;

    private void OnValidate()
    {
        if (itemData == null) return;

        sr=GetComponent<SpriteRenderer>();
        
    }

    private void Start()
    {
        // 启动一个计时器，在 dropDuration 秒后调用 StopMovement
        Invoke("StopMovement", dropDuration);
    }

    void StopMovement()
    {
        mustStop = true;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    //初始化
    public void SetupItem(ItemDataSO itemData)
    {
        this.itemData = itemData;
        SetupVisuals();
        float xDropForce = Random.Range(-dropForce.x, dropForce.x);
        rb.velocity = new Vector2(xDropForce, dropForce.y);

        col.isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(mustStop)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && col.isTrigger == false)
            {
                 col.isTrigger = true;
                 col.excludeLayers = 0;//不排除所有层
                 rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        
    }

    private void SetupVisuals()
    {
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + itemData.itemName;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory_Player inventory=collision.GetComponent<Inventory_Player>();
        if (inventory == null) return;

        Inventory_Item itemToAdd = new Inventory_Item(itemData);
        Inventory_Storage storage = inventory.storage;

        if(itemData.itemType==ItemType.Material)
        {
            storage.AddMaterialToStash(itemToAdd);
            Destroy(gameObject);
            return;
        }

        if(inventory.CanAddItem(itemToAdd))
        {
            inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}
