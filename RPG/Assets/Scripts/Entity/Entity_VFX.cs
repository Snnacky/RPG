using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [Header("�Ӿ�Ч��")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .2f;//�Ӿ�����ʱ��
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;
    [Header("����Ч��")]
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

    //��������������Ӿ�Ч��
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
