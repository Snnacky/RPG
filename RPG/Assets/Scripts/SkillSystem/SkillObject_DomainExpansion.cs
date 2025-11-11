using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion domainManager;

    private float expandSpeed;
    private float slowDownPercent;
    private float duration;
    private Vector3 targetScale;
    private bool isShrinking;//缩小
    private string slowSourceName = "domainExpansion";

    //初始化
    public void SetupDomain(Skill_DomainExpansion domainManager)
    {
        this.domainManager = domainManager;
        originalPlayer = domainManager.player.transform;

        float maxSize = domainManager.maxDomainSize;
        duration=domainManager.GetDomainDuration();

        slowDownPercent=domainManager.GetSlowPercentage();
        expandSpeed=domainManager.expandSpeed;

        targetScale = Vector3.one * maxSize;

        //持续时间到了缩小
        Invoke(nameof(ShrinkDomain), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    private void HandleScaling()
    {
        float sizeDifference = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDifference > .1f;
        //处理变大或变小
        if (shouldChangeScale)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);

        if (isShrinking && sizeDifference <= .1f)
        {
            domainManager.ClearTarget();
            Destroy(gameObject);
        }

    }

    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinking = true;//缩小
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy=collision.GetComponent<Enemy>();
        if (enemy == null)
            return;
        domainManager.AddTarget(enemy);
        enemy.SlowDownEntity(duration,slowDownPercent,slowSourceName,true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null)
            return;
        enemy.RemoveSlow(slowSourceName);//删去该来源的所有减速效果
    }
}
