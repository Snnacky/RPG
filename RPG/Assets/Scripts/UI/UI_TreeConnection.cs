using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{

    [SerializeField] private RectTransform rotationPoint;//���ӵĵ�
    [SerializeField] private RectTransform connectionLength;//������
    [SerializeField] private RectTransform childNodeConnectionPoint;//�����ӵĵ�

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
        //����������ת���ɱ��ؾֲ�����
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rect.parent as RectTransform,//����
            childNodeConnectionPoint.position,//�������ӵĵ��λ��ת�����ڻ����ϵı�������
            null,
            out var localPosition//��������
        );
        return localPosition;

    }
    //��ȡ������
    public Image GetConnectionImage()=>connectionLength.GetComponent<Image>();

    //ȷ������
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
