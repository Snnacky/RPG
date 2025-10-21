using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{

    [SerializeField] private RectTransform rotationPoint;//链接的点
    [SerializeField] private RectTransform connectionLength;//连接线
    [SerializeField] private RectTransform childNodeConnectionPoint;//子链接的点

    public void DirectConnection(NodeDirectionType direction, float length, float offest)
    {
        bool shouldBeActive = direction != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0, 0, angle + offest);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        //将世界坐标转换成本地局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rect.parent as RectTransform,//画布
            childNodeConnectionPoint.position,//将子链接的点的位置转换成在画布上的本地坐标
            null,
            out var localPosition//本地坐标
        );
        return localPosition;

    }
    //获取连接线
    public Image GetConnectionImage()=>connectionLength.GetComponent<Image>();

    //确定方向
    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch (type)
        {
            case NodeDirectionType.UpLeft: return 135f;
            case NodeDirectionType.Up: return 90f;
            case NodeDirectionType.UpRight: return 45f;
            case NodeDirectionType.Left: return 180f;
            case NodeDirectionType.Right: return 0f;
            case NodeDirectionType.DownLeft: return -135f;
            case NodeDirectionType.Down: return -90f;
            case NodeDirectionType.DownRight: return -45f;
            default: return 0f;
        }
    }
}

public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Left,
    Right,
    DownLeft,
    Down,
    DownRight
}
