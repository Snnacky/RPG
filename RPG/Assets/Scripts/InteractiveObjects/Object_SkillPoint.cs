using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_SkillPoint : Object_NPC, IInteractable
{
    [SerializeField] private int PointsToAdd;
    void IInteractable.Interact()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.AddSkillPoints(PointsToAdd);
    }
}
