using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;
    [Header("�Ӿ�Ч��")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVfxDuration = .2f;//�Ӿ�����ʱ��
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;
    private Color originalSrColor;
    [Header("����Ч��")]
    [SerializeField] private Color hitvfxColor = Color.red;//��������ɫ
    [SerializeField] private GameObject hitvfx;
    [SerializeField] private GameObject critHitvfx;

    [Header("Ԫ��Ч��")]
    [SerializeField] private Color chillvfx = Color.cyan;//������ɫ��ɫ
    [SerializeField] private Color burnvfx = Color.red;//������ɫ
    [SerializeField] private Color shockvfx = Color.yellow;//�����ɫ

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr=GetComponentInChildren<SpriteRenderer>();
        originalMaterial=sr.material;
        originalSrColor = sr.color;
    }

    public void PlayOnStatusVfx(float duration,ElementType elementType)
    {
        if(elementType==ElementType.Ice)
           StartCoroutine(PlayStatusVfxCo(duration, chillvfx));
        if(elementType==ElementType.Fire)
           StartCoroutine(PlayStatusVfxCo(duration, burnvfx));
        if (elementType == ElementType.Lightning)
           StartCoroutine(PlayStatusVfxCo(duration, shockvfx));

    }

    public void StopAllvfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    //���Ľ�ɫ���������������ɫ
    private IEnumerator PlayStatusVfxCo(float duration,Color effectColor)
    {
        float tickInterval = .25f;//���
        float timeHasPassed = 0;
        Color lightColor = effectColor * 1.2f;//��ɫ����
        Color darkColor = effectColor * .8f;//��ɫ����
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

    //����Ԥ�������ɫ
    public void CreatOnHitVfx(Transform target,bool isCrit,ElementType element)
    {
        GameObject prefab = isCrit ? critHitvfx : hitvfx;
        GameObject vfx = Instantiate(prefab, target.position, Quaternion.identity);//����Ԥ����
        //vfx.GetComponentInChildren<SpriteRenderer>().color = GetElementColor(element);
        
        if (entity.facingDir == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }

    public Color GetElementColor(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Ice:
                return chillvfx;
            case ElementType.Fire:
                return burnvfx;
            case ElementType.Lightning:
                return shockvfx;
            default:
                return hitvfxColor;
        }

    }

    //��������������Ӿ�Ч��
    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);
        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());

    }

    //�任��ɫ��ɫ
    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVfxDuration);
        sr.material = originalMaterial;
    }
}
