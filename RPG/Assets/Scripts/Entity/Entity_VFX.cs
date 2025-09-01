using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [Header("视觉效果")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .2f;//视觉持续时间
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;
    [Header("攻击效果")]
    [SerializeField] private Color hitvfxColor = Color.red;
    [SerializeField] private GameObject hitvfx;

    private void Awake()
    {
        sr=GetComponentInChildren<SpriteRenderer>();
        originalMaterial=sr.material;
    }

    public void CreatOnHitVfx(Transform target)
    {
        GameObject vfx = Instantiate(hitvfx, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitvfxColor;
    }

    //更换被攻击后的视觉效果
    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);
        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());

    }

    
    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
}
