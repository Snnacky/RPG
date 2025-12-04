using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    private SkillObject_Sword currentSword;
    private float currentThrowPower;

    [Header("Regular Sword Upgrade")]
    [SerializeField] private GameObject swordPrefab;
    [Range(0,10)]
    [SerializeField] private float regularThrowPower = 5;

    [Header("Pierce Sword Upgrade")]
    [SerializeField] private GameObject pierceSwordPrefab;
    public int amountToPierce = 2;
    [Range(0, 10)]
    [SerializeField] private float pierceThrowPower = 5;

    [Header("Spin Sword Upgrade")]
    [SerializeField] private GameObject spinSwordPrefab;
    public int maxDistance = 20;
    public float attacksPerSecond = 6;
    public float maxSpinDuration = 5;
    [Range(0, 10)]
    [SerializeField] private float spinThrowPower = 5;

    [Header("Bounce Sword Upgrade")]
    [SerializeField] private GameObject bounceSwordPrefab;
    public int bounceCount = 5;
    public float bounceSpeed = 12;
    [Range(0, 10)]
    [SerializeField] private float bounceThrowPower = 5;

    [Header("Trajectory prediction")]
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweenDots = .05f;//每个点的时间间隔
    private float swordGravity;
    private Transform[] dots;
    private Vector2 confirmedDirection;//扔出剑的方向

    protected override void Awake()
    {
        base.Awake();
        dots = GenerateDots();
        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;//重力
    }

    public override bool CanUseSkill()
    {
        UpdateThrowPower();

        //当前已经有剑在外面,再次使用技能归还剑
        if (currentSword != null)
        {
            currentSword.GetSwordBackToPlayer();
            return false;
        }
        return base.CanUseSkill();
    }

    //攻击
    public void ThrowSword()
    {
        GameObject swordPrefab=GetSwordPrefab();
        GameObject newsword = Instantiate(swordPrefab, dots[1].position,Quaternion.identity);
        currentSword = newsword.GetComponent<SkillObject_Sword>();
        currentSword.SetupSword(this, GetThrowPower());
        SetSkillOnCoolDown();
    }

    private GameObject GetSwordPrefab()
    {
        if (Unlocked(SkillUpgradeType.SwordThrow))
            return swordPrefab;
        if (Unlocked(SkillUpgradeType.SwordThrow_Pierce))
            return pierceSwordPrefab;
        if (Unlocked(SkillUpgradeType.SwordThrow_Spin))
            return spinSwordPrefab;
        if(Unlocked(SkillUpgradeType.SwordThrow_Bounce))
            return bounceSwordPrefab;
        Debug.Log("no sword upgrade");
        return null;
    }

    private void UpdateThrowPower()
    {
        switch (upgradeType)
        {
            case SkillUpgradeType.SwordThrow:
                currentThrowPower = regularThrowPower;
                break;
            case SkillUpgradeType.SwordThrow_Pierce:
                currentThrowPower=pierceThrowPower;
                break;
            case SkillUpgradeType.SwordThrow_Spin:
                currentThrowPower=spinThrowPower;
                break;
            case SkillUpgradeType.SwordThrow_Bounce:
                currentThrowPower=bounceThrowPower;
                break;
        }

    }


    //扔出的力量 方向
    private Vector2 GetThrowPower() => confirmedDirection * (currentThrowPower * 10);

    //预测所有辅助点的位置
    public void PredictTrajectory(Vector2 direction)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i * spaceBetweenDots);
        }
    }

    //计算每个点的位置
    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = currentThrowPower * 10;//扔的力量大小

        Vector2 initialVelocity = direction * scaledThrowPower;//初始速度

        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t);//重力影响h=1/2gt^2    physical2D.gravity是负的

        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;//预测的位置 x=vt y=vt+1/2gt^2

        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
    }

    public void ConfirmTrajectory(Vector2 direction) => confirmedDirection = direction;

    public void EnableDots(bool enable)
    {
        foreach (var t in dots)
        {
            t.gameObject.SetActive(enable);
        }
    }

    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            newDots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }
        return newDots;
    }
}
