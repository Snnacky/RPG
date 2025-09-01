using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    [SerializeField] private bool randomOffest = true;
    [SerializeField] private bool randomRotation = true;

    [Header("Random Position")]
    [SerializeField] private float xMinOffest = -.3f;
    [SerializeField] private float xMaxOffest = .3f;
    [Space]
    [SerializeField] private float yMinOffest = -.3f;
    [SerializeField] private float yMaxOffest = -.3f;

    private void Start()
    {
        ApplyRandomOffest();
        ApplyRandomRotation();
        if (autoDestroy)
            Destroy(gameObject,destroyDelay);
    }

    private void ApplyRandomOffest()
    {
        if (randomOffest)
            return;
        float xOffest=Random.Range(xMinOffest, xMaxOffest);
        float yOffest=Random.Range(yMinOffest, yMaxOffest);
        transform.position=transform.position+ new Vector3(xOffest,yOffest);
    }
    private void ApplyRandomRotation()
    {
        if (randomRotation) return;

        float zRotation = Random.Range(0, 360);
        transform.Rotate(0,0,zRotation);
    }
}
