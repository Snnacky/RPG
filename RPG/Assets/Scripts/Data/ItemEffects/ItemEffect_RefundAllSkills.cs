using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Refund all skills", fileName = "Item effect data - refund all skills ")]
public class ItemEffect_RefundAllSkills : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.RefundAllSkills();
    }
}
