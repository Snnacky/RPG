using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("播放器")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioDatabaseSO audioDatabase;

    private AudioClip lastMusicPlayed;
    private Transform player;
    private string currentBgmGroupName;
    private Coroutine currentBgmCo;
    [SerializeField] private bool bgmShouldPlay;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(bgmSource.isPlaying==false && bgmShouldPlay)
        {
            if (string.IsNullOrEmpty(currentBgmGroupName)==false)
                NextBGM(currentBgmGroupName);
        }

        if (bgmSource.isPlaying && bgmShouldPlay == false)
            StopBGM();
    }

    public void StartBGM(string musicGroup)
    {
        bgmShouldPlay = true;

        if (musicGroup == currentBgmGroupName) return;

        NextBGM(musicGroup);
    }


    public void NextBGM(string musicGroup)
    {
        bgmShouldPlay = true;
        currentBgmGroupName = musicGroup;

        if(currentBgmCo != null)
            StopCoroutine(currentBgmCo);
        currentBgmCo = StartCoroutine(SwitchMusicCo(musicGroup));
    }

    public void StopBGM()
    {
        bgmShouldPlay = false;

        StartCoroutine(FadeVolumeCo(bgmSource, 0f, 1f));

        if (currentBgmCo != null)
            StopCoroutine(currentBgmCo);
       
    }

    private IEnumerator SwitchMusicCo(string musicGrouop)
    {
        AudioClipData data = audioDatabase.Get(musicGrouop);
        AudioClip nextMusic = data.GetRandomClip();

        if (data == null || nextMusic == null)
            yield break;    

        if(data.clips.Count>1)
            while(nextMusic == lastMusicPlayed)
                nextMusic = data.GetRandomClip();
        if(bgmSource.isPlaying)
            yield return FadeVolumeCo(bgmSource, 0f, 1f);

        lastMusicPlayed = nextMusic;
        bgmSource.clip = nextMusic;
        bgmSource.volume = 0f;
        bgmSource.Play();

        StartCoroutine(FadeVolumeCo(bgmSource, data.maxVolume, 1f));
    }


    private IEnumerator FadeVolumeCo(AudioSource source,float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            yield return null;
        }
        source.volume = targetVolume;
    }

    public void PlaySFX(string soundName,AudioSource sfxSource,float minDistanceToHearSound=5)
    {
        if (player == null)
            player = Player.instance.transform;
        var data=audioDatabase.Get(soundName);
        if(data == null)
            return;

        var clip = data.GetRandomClip();//获取随机音效
        if (clip == null) return;

        //根据距离调整音量
        float maxVolume = data.maxVolume;
        float distacne=Vector2.Distance(player.position, sfxSource.transform.position);
        float t = Mathf.Clamp01(1 - (distacne / minDistanceToHearSound));

        sfxSource.clip = clip;
        sfxSource.volume = Mathf.Lerp(0, maxVolume, t* t);
        sfxSource.PlayOneShot(clip);
    }

    public void PlayGlobalSFX(string soundName)
    {
        var data=audioDatabase.Get(soundName);
        if (data == null) return;
        
        var clip=data.GetRandomClip();//获取随机音效
        if(clip==null)return;

        sfxSource.clip = clip;
        sfxSource.volume = data.maxVolume;
        sfxSource.PlayOneShot(clip);
    }
}
