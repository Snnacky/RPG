using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Vector3 lastDeathPosition;

    private void Awake()
    {
        if(Instance!=null && Instance!=this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SetLastDeathPosition(Vector3 position) => lastDeathPosition = position;

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();

        string sceneName=SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.NoneSpecific);
    }

    public void ChangeScene(string sceneName,RespawnType respawnType)
    {
        SaveManager.instance.SaveGame();
        StartCoroutine(ChangeSceneCo(sceneName,respawnType));
    }


    private IEnumerator ChangeSceneCo(string sceneName,RespawnType respawnType)
    {
        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(.5f);

        Vector3 position = GetNewPlayerPosition(respawnType);
        yield return new WaitForSeconds(.2f);
        if(position!=Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }

    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if(type==RespawnType.Portal)
        {
            Object_Portal portal = Object_Portal.instance;
            Vector3 position = portal.GetPosition();

            portal.SetTrigger(false);
            portal.DisableIfNeeded();

            return position;
        }

        if (type == RespawnType.NoneSpecific)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_CheckPoint>(FindObjectsSortMode.None);
            var unlockedCheckpoints = checkpoints
                .Where(cp => data.unlockedCheckpoints.TryGetValue(cp.GetCheckpointId(), out bool unlocked) && unlocked)
                .Select(cp => cp.GetPosition())
                .ToList();

            var enterWaypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
                .Where(wp => wp.GetWaypointType() == RespawnType.Enter)
                .Select(wp => wp.GetPositionAndSetTtrigerFalse())
                .ToList();

            var selectedPositions = unlockedCheckpoints.Concat(enterWaypoints).ToList();

            if (selectedPositions.Count == 0)
                return Vector3.zero;

            return selectedPositions
                .OrderBy(position => Vector3.Distance(position, lastDeathPosition))
                .First();
        }
        return GetWaypointPosition(type);
    }

    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach(var waypoint in waypoints)
        {
            if(waypoint.GetWaypointType()==type)
            {
                return waypoint.GetPositionAndSetTtrigerFalse();
            }
        }

        return Vector3.zero;
    }

}
