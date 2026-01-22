using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();    
    }

    public void PlayBTN()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance 为 null！请确保 GameManager 已经初始化。");
            return;
        }
        GameManager.Instance.ContinuePlay();
    }

    public void QuitGameBTN()
    {
        Application.Quit();
    }

}
