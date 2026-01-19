using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Portal", fileName = "Item effect data - PoralScroll ")]
public class ItemEffect_Portal : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        if(SceneManager.GetActiveScene().name == "Level_0")
        {
            Debug.Log("无法在town打开");
            return;
        }

        Player player = Player.instance;
        Vector3 portalPostion = player.transform.position + new Vector3(player.facingDir * 1.5f, 0);

        Object_Portal.instance.ActivatePortal(portalPostion,player.facingDir);
    }
}
