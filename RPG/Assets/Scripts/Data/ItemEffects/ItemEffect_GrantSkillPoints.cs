using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/grant skill point", fileName = "Item effect data - grant skill point ")]
public class ItemEffect_GrantSkillPoints : ItemEffectDataSO
{
    [SerializeField] private int PointsToAdd;
    public override void ExecuteEffect()
    {
        UI ui=FindFirstObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(PointsToAdd);
    }
}
