using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamgable //�ӿ�
{
    //д��Entity_Health
    public bool TakeDamage(float damage, float elementalDamage,ElementType elementType, Transform damageDealer);
}
