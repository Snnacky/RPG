using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;//����
    [SerializeField] private float parallaxMultiplier;//�����ƶ��ĳ���
    [SerializeField] private float imageWidthOffset = 10;//�൱����Сͼ��,������չ

    private float imageFullWidth;
    private float imageHalfWidth;

    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }
    public void Move(float distanceToMove)
    {
        background.position += Vector3.right * (distanceToMove * parallaxMultiplier);
    }
    
    public void LoopBackground(float cameraLeftEdge,float cameraRightEdge)//��������ұ�Ե
    {
        float imageLeftEdge = (background.position.x - imageHalfWidth)+ imageWidthOffset;//ͼ������Ե
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;//ͼ���ұ�Ե
        //���ͼ���ұ�ԵС��������Ե,ͼ��Ҫ������չ
        if (imageRightEdge < cameraLeftEdge)
            background.position += Vector3.right * imageFullWidth;
        //���ͼ�����Ե��������ұ�Ե,ͼ��Ҫ������չ
        else if (imageLeftEdge > cameraRightEdge)
            background.position += Vector3.right * -imageFullWidth;

    }
}
