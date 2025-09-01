using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamgable //½Ó¿Ú
{
    public void TakeDamage(float damage, Transform damageDealer);
}
