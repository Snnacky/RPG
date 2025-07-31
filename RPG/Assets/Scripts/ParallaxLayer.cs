using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;//背景
    [SerializeField] private float parallaxMultiplier;//背景移动的乘数
    [SerializeField] private float imageWidthOffset = 10;//相当于缩小图像,提早扩展

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
    
    public void LoopBackground(float cameraLeftEdge,float cameraRightEdge)//相机的左右边缘
    {
        float imageLeftEdge = (background.position.x - imageHalfWidth)+ imageWidthOffset;//图像的左边缘
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;//图像右边缘
        //如果图像右边缘小于相机左边缘,图像要向右扩展
        if (imageRightEdge < cameraLeftEdge)
            background.position += Vector3.right * imageFullWidth;
        //如果图像左边缘大于相机右边缘,图像要向左扩展
        else if (imageLeftEdge > cameraRightEdge)
            background.position += Vector3.right * -imageFullWidth;

    }
}
