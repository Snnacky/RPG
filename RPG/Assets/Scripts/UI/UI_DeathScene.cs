using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DeathScene : MonoBehaviour
{
    public void GoToCampBTN()
    {
        GameManager.Instance.ChangeScene("Level_0", RespawnType.NoneSpecific);
    }

    public void GoToCheckpoint()
    {
        GameManager.Instance.RestartScene();
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);
    }
}
