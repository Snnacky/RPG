using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour,ISaveable
{
    public static GameManager Instance;
    private Vector3 lastPlayerPosition;
    private string lastScenePlayed;
    private bool dataLoaded = false;

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
    public void SetLastPlayerPosition(Vector3 position) => lastPlayerPosition = position;

    public void ContinuePlay()
    {
        Debug.LogWarning(lastScenePlayed);
        ChangeScene(lastScenePlayed, RespawnType.NoneSpecific);
    }
    public void RestartScene()
    {
        SaveManager.instance.SaveGame();

        string sceneName=SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.NoneSpecific);
    }

    public void ChangeScene(string sceneName,RespawnType respawnType)
    {
        SaveManager.instance.SaveGame();
        Time.timeScale = 1;
        StartCoroutine(ChangeSceneCo(sceneName,respawnType));
    }


    private IEnumerator ChangeSceneCo(string sceneName,RespawnType respawnType)
    {
        UI_FadeScreen fadeScreen = FindFadeScreenUI();

        fadeScreen.DoFadeOut();
        yield return fadeScreen.fadeEffectCo;

        SceneManager.LoadScene(sceneName);
        dataLoaded = false;//data loaded becomes true when you load game from save manager
        yield return null;


        while (dataLoaded == false)
            yield return null;

        fadeScreen=FindFadeScreenUI();
        fadeScreen.DoFadeIn();

        Player player = Player.instance;
        if (player == null) yield break;

        Vector3 position = GetNewPlayerPosition(respawnType);
        //yield return new WaitForSeconds(.2f);
        if(position!=Vector3.zero)
            player.TeleportPlayer(position);
    }

    private UI_FadeScreen FindFadeScreenUI()
    {
        if(UI.instance!=null)
            return UI.instance.fadeScreenUI;
        else
            return FindFirstObjectByType<UI_FadeScreen>();
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
                .OrderBy(position => Vector3.Distance(position, lastPlayerPosition))
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

    public void LoadData(GameData data)
    {
        lastScenePlayed=data.lastScenePlayed;
        lastPlayerPosition = data.lastPlayerPosition;

        if(string.IsNullOrEmpty(lastScenePlayed))
        {
            lastScenePlayed = "Level_0";
        }
        dataLoaded = true;
    }

    public void SaveData(ref GameData data)
    {
        string currentScene=SceneManager.GetActiveScene().name;

        if (currentScene == "MainMenu") return;
        data.lastScenePlayed = currentScene;
        data.lastPlayerPosition = Player.instance.transform.position;
        dataLoaded = false;
    }
}
