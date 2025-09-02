using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MinHealthBar : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }
    private void OnEnable()
    {
        entity.OnFlipped += HandleFlip;//调用onflipped的时候也会调用handleFlip
    }

    private void OnDisable()
    {
        entity.OnFlipped -= HandleFlip;
    }

    private void HandleFlip()=>transform.rotation= Quaternion.identity;
}
