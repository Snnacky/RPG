using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_CheckPoint : MonoBehaviour,ISaveable
{
    private Object_CheckPoint[] allCheckPoints;
    private Animator anim;

    private void Awake()
    {
        anim=GetComponentInChildren<Animator>();
        allCheckPoints = FindObjectsByType<Object_CheckPoint>(FindObjectsSortMode.None);
    }

    public void ActivataCheckPoint(bool activate)
    {
        anim.SetBool("isActive", activate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var point in allCheckPoints)
        {
            point.ActivataCheckPoint(false);
        }

        SaveManager.instance.GetGameData().savedCheckPoint = transform.position;
        ActivataCheckPoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.savedCheckPoint == transform.position;
        ActivataCheckPoint(active);
        if (active)
            Player.instance.TeleportPlayer(transform.position);
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
