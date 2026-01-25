using UnityEngine;

public class Object_CheckPoint : MonoBehaviour, ISaveable
{
    [SerializeField] private string checkpointId;
    [SerializeField] private Transform respawnPoint;
    public bool isActive { get; private set; }
    private Animator anim;

    private AudioSource audioSource;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public string GetCheckpointId() => checkpointId;

    public Vector3 GetPosition() => respawnPoint == null ? transform.position : respawnPoint.position;

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(checkpointId))
            checkpointId = System.Guid.NewGuid().ToString();
#endif
    }

    public void ActivataCheckPoint(bool activate)
    {
        isActive = activate;
        anim.SetBool("isActive", activate);
        if (isActive && audioSource.isPlaying == false)
            audioSource.Play();
        if (isActive == false)
            audioSource.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        ActivataCheckPoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.unlockedCheckpoints.TryGetValue(checkpointId, out active);
        ActivataCheckPoint(active);
    }

    public void SaveData(ref GameData data)
    {
        if (isActive == false) return;

        if (data.unlockedCheckpoints.ContainsKey(checkpointId) == false)
            data.unlockedCheckpoints.Add(checkpointId, true);
    }
}
