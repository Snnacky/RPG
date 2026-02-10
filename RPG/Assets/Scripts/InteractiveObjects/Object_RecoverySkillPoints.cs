using UnityEngine;

public class Object_RecoverySkillPoints : Object_NPC, IInteractable
{

    void IInteractable.Interact()
    {
        UI ui = FindFirstObjectByType<UI>();
        ui.skillTreeUI.RefundAllSkills();
    }
}
