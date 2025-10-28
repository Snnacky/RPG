using System.Collections;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    [SerializeField] private bool randomOffest = true;
    [SerializeField] private bool randomRotation = true;

    [Header("Fade Effect")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1;

    [Header("Random rotation")]
    [SerializeField] private float minRotation = 0;
    [SerializeField] private float maxRotation = 360;

    [Header("Random Position")]
    [SerializeField] private float xMinOffest = -.3f;
    [SerializeField] private float xMaxOffest = .3f;
    [Space]
    [SerializeField] private float yMinOffest = -.3f;
    [SerializeField] private float yMaxOffest = -.3f;

   
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        if (canFade)
            StartCoroutine(FadeCo());
        ApplyRandomOffest();
        ApplyRandomRotation();
        if (autoDestroy)
            Destroy(gameObject, destroyDelay);//延迟摧毁
    }
    //逐渐消失效果
    private IEnumerator FadeCo()
    {
        Color targetColor = Color.white;
        while (targetColor.a > 0)
        {
            targetColor.a -= fadeSpeed * Time.deltaTime;
            sr.color = targetColor;
            yield return null;
        }
        sr.color = targetColor;
    }
    //随机位置
    private void ApplyRandomOffest()
    {
        if (randomOffest == false)
            return;
        float xOffest = Random.Range(xMinOffest, xMaxOffest);
        float yOffest = Random.Range(yMinOffest, yMaxOffest);
        transform.position = transform.position + new Vector3(xOffest, yOffest);
    }
    //随机方向
    private void ApplyRandomRotation()
    {
        if (randomRotation == false) return;

        float zRotation = Random.Range(minRotation, maxRotation);
        transform.Rotate(0, 0, zRotation);
    }
}
