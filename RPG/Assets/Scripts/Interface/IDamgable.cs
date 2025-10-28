using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamgable //½Ó¿Ú
{
    //Ð´ÔÚEntity_Health
    public bool TakeDamage(float damage, float elementalDamage,ElementType elementType, Transform damageDealer);
}
