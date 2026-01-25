using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRangerController : MonoBehaviour
{
    private AudioSource source;
    private Transform player;

    [SerializeField] private float minDistanceToHearSound = 12;
    [SerializeField] private bool showGizmos = false;
    private float maxVolume;

    private void Start()
    {
        player=Player.instance.transform;
        source = GetComponent<AudioSource>();

        maxVolume=source.volume;
    }

    private void Update()
    {
        if (player == null)
            return;

        float distacne = Vector2.Distance(player.position, transform.position);
        float t = Mathf.Clamp01(1 - (distacne / minDistanceToHearSound));
        float targetVolume = Mathf.Lerp(0, maxVolume, t * t);
        source.volume=Mathf.Lerp(source.volume, targetVolume, Time.deltaTime * 3);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos == false)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistanceToHearSound);
    }
}
