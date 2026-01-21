using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : Entity_Health
{
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
            Die();
    }

    public override void Die()
    {
        base.Die();
        Player.instance.ui.OpenDeathScreenUI();
    }
}
