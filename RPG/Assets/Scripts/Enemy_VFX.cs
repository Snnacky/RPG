using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [Header("����Ԥ��")]
    [SerializeField] private GameObject attackAlert;

    public void EnableAttackAlert(bool enable)=>attackAlert.SetActive(enable);
}
