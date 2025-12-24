using UnityEngine;

public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;

    [SerializeField] private Transform npc;
    public GameObject interactToolTip;//提示


    [Header("上下移动效果")]
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    private bool isPlayerInRange = false;
    private bool facingRight = true;

    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPosition=interactToolTip.transform.position;
    }

    protected virtual void Start()
    {
       
    }

    protected virtual void Update()
    {
        
            HandleNpcFlip();
       
            HandleToolTipFloat();
    }

    //提示框
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
        if(isPlayerInRange)
        {
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
        }else
        {
            if (facingRight == false)
            {
                facingRight = true;
                npc.transform.Rotate(0, 180, 0);
            }
        }
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.transform;
        interactToolTip.SetActive(true);
        isPlayerInRange = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerInRange = false;
        interactToolTip.SetActive(false);
    }
}
