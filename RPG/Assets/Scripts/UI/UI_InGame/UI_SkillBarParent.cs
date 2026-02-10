using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillBarParent : MonoBehaviour
{
    public void RecoverySkillSlot()
    {
        UI_SkillSlot []skillSlot = GetComponentsInChildren<UI_SkillSlot>();
        foreach (var slot in skillSlot)
        {
            slot.RecoverySkillSlot();
        }

    }
}
