using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;
    [Header("视觉效果")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .2f;//视觉持续时间
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;
    private Color originalSrColor;
    [Header("攻击效果")]
    [SerializeField] private Color hitvfxColor = Color.red;
    [SerializeField] private GameObject hitvfx;
    [SerializeField] private GameObject critHitvfx;

    [Header("元素效果")]
    [SerializeField] private Color chillvfx = Color.cyan;//冰冻颜色青色
    [SerializeField] private Color burnvfx = Color.red;//灼烧颜色
    [SerializeField] private Color electrifyvfx = Color.yellow;//电击颜色
    private Color originalHitVfxColor;//初始攻击颜色

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr=GetComponentInChildren<SpriteRenderer>();
        originalMaterial=sr.material;
        originalHitVfxColor=hitvfxColor;
        originalSrColor = sr.color;
    }

    public void PlayOnStatusVfx(float duration,ElementType elementType)
    {
        if(elementType==ElementType.Ice)
           StartCoroutine(PlayStatusVfxCo(duration, chillvfx));
        if(elementType==ElementType.Fire)
           StartCoroutine(PlayStatusVfxCo(duration, burnvfx));
        if (elementType == ElementType.Lightning)
           StartCoroutine(PlayStatusVfxCo(duration, electrifyvfx));

    }

    public void StopAllvfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    private IEnumerator PlayStatusVfxCo(float duration,Color effectColor)
    {
        float tickInterval = .25f;//间隔
        float timeHasPassed = 0;
        Color lightColor = effectColor * 1.2f;//颜色更亮
        Color darkColor = effectColor * .8f;//颜色更暗
        bool toggle = false;
        while (timeHasPassed<duration)
        {
            sr.color = toggle? lightColor : darkColor;
            toggle = !toggle;
            yield return new WaitForSeconds(tickInterval);
            timeHasPassed += tickInterval;
        }
        sr.color = originalSrColor;
    }

    public void CreatOnHitVfx(Transform target,bool isCrit)
    {
        GameObject prefab = isCrit ? critHitvfx : hitvfx;
        GameObject vfx = Instantiate(prefab, target.position, Quaternion.identity);//生成预制体
        
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitvfxColor;
        if (entity.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }

    public void UpdateOnHitColor(ElementType elementType)
    {
        if (elementType == ElementType.Ice)
            hitvfxColor = chillvfx;
        if (elementType == ElementType.None)
            hitvfxColor = originalHitVfxColor;
    }

    //更换被攻击后的视觉效果
    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);
        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());

    }

    //变换角色颜色
    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
}
