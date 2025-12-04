using UnityEngine;

public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;

    [SerializeField] private Transform npc;
    public GameObject interactToolTip;//提示

    private bool facingRight = true;

    [Header("上下移动效果")]
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPosition=interactToolTip.transform.position;
    }

    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTipFloat();
    }

    private void HandleToolTipFloat()
    {
        if (interactToolTip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactToolTip.transform.position = startPosition + new Vector3(0, yOffset);
        }
    }

    private void HandleNpcFlip()
    {
        if (npc == null || player == null) return;

        if (npc.position.x > player.position.x && facingRight == true)
        {
            npc.transform.Rotate(0, 180, 0);
            facingRight = false;
        }
        else if (npc.position.x < player.position.x && facingRight == false)
        {
            npc.transform.Rotate(0, 180, 0);
            facingRight = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interactToolTip.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactToolTip.SetActive(false);
    }
}
