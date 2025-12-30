using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;//注解图
    [SerializeField] private Vector2 offest = new Vector2(300, 20);
    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show, RectTransform targetRect)//targetRect是技能图片
    {
        if (show == false)
        {
            if (rect != null)
                rect.position = new Vector2(9999, 9999);
            return;
        }
        UpdatePosition(targetRect);
    }

    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2f;//屏幕中心x
        float screenTop = Screen.height;//屏幕最高
        float screenBottom = 0;//屏幕最低

        Vector2 targetPosition = targetRect.position;//目标位置
        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offest.x : targetPosition.x + offest.x;//修改x位置

        float veritcalHalf = rect.sizeDelta.y / 2;//注解图高度的一半
        float topY = targetPosition.y + veritcalHalf;//注解图的最高点
        float bottonY = targetPosition.y - veritcalHalf;//注解图的最低点

        if (topY > screenTop)//注解图超出屏幕以上
            targetPosition.y = screenTop - veritcalHalf - offest.y;
        else if (bottonY < screenBottom)//以下
            targetPosition.y = screenBottom + veritcalHalf + offest.y;

        rect.position = targetPosition;//修改注解图位置
    }
    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }

}
