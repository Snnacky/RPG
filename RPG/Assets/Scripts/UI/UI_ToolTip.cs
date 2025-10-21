using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    private RectTransform rect;//ע��ͼ
    [SerializeField] private Vector2 offest = new Vector2(300, 20);
    protected virtual void Awake()
    {
        rect= GetComponent<RectTransform>();
    }

    public virtual void ShowToolTip(bool show,RectTransform targetRect)//targetRect�Ǽ���ͼƬ
    {
        if(show==false)
        {
            rect.position = new Vector2(9999, 9999);
            return;
        }
        UpdatePosition(targetRect);
    }    

    private void UpdatePosition(RectTransform targetRect)
    {
        float screenCenterX = Screen.width / 2f;//��Ļ����x
        float screenTop = Screen.height;//��Ļ���
        float screenBottom = 0;//��Ļ���

        Vector2 targetPosition = targetRect.position;//Ŀ��λ��
        targetPosition.x = targetPosition.x > screenCenterX ? targetPosition.x - offest.x : targetPosition.x + offest.x;//�޸�xλ��

        float veritcalHalf = rect.sizeDelta.y / 2;//ע��ͼ�߶ȵ�һ��
        float topY = targetPosition.y + veritcalHalf;//ע��ͼ����ߵ�
        float bottonY = targetPosition.y - veritcalHalf;//ע��ͼ����͵�

        if(topY>screenTop)//ע��ͼ������Ļ����
            targetPosition.y=screenTop-veritcalHalf-offest.y;
        else if(bottonY<screenBottom)//����
            targetPosition.y=screenBottom+veritcalHalf+offest.y;

            rect.position = targetPosition;//�޸�ע��ͼλ��
    }
    protected string GetColoredText(string color, string text)
    {
        return $"<color={color}>{text}</color>";
    }

}
