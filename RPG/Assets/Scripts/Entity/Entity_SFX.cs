using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("SFX Names")]
    [SerializeField] private string attackHit;
    [SerializeField] private string attackMiss;
    [Space]
    [SerializeField] private float soundDistance = 15;
    [SerializeField] private bool showGizmos = false;

    private void Awake()
    {
        audioSource=GetComponentInChildren<AudioSource>();  
    }

    public void PlayAttackHit()
    {
        AudioManager.instance.PlaySFX(attackHit, audioSource, soundDistance);
    }

    public void PlayerAttackMiss()
    {
        AudioManager.instance.PlaySFX(attackMiss, audioSource, soundDistance);
    }

    private void OnDrawGizmos()
    {
        if(showGizmos == false)
            return; 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, soundDistance);
    }
}
