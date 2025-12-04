using System.Collections;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    private Player_Stats statsToModify;
    [Header("buff 细节")]
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private float buffDuration = 4;
    [SerializeField] private string buffName;
    [Header("上下移动效果")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = .1f;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float yoffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yoffset);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        statsToModify = collision.GetComponent<Player_Stats>();

        if (statsToModify.CanApplyBuffof(buffName))
        {
            statsToModify.ApplyBuff(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }
    }

}
