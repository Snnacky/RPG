using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction;
    [Range(100f, 350f)] public float length;
    [Range(-50f, 50f)] public float rotation;
}
public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();//整个技能图本体的transfrom,包括连接线
    [SerializeField] private UI_TreeConnectDetails[] connectionDetails;//连接细节
    [SerializeField] private UI_TreeConnection[] connections;//多少个连接处//与details一对一

    private Image connectionImage;
    private Color originalColor;

    private void Awake()
    {
        if (connectionImage != null)
            originalColor = connectionImage.color;
    }

    private void OnValidate()
    {
        if (connectionDetails.Length != connections.Length)
        {
            Debug.Log("Amount of details should be same as amount of connections." + gameObject.name);
            return;
        }
        UpdateConnection();
    }

    //获取子技能的UI_TreeNode组件
    public UI_TreeNode[] GetChildNodes()
    {
        List<UI_TreeNode> childrenToReturn = new List<UI_TreeNode>();
        foreach (var node in connectionDetails)
        {
            if (node.childNode != null)
                childrenToReturn.Add(node.childNode.GetComponent<UI_TreeNode>());
        }
        return childrenToReturn.ToArray();
    }

    private void UpdateConnection()
    {
        for (int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];
            Vector2 targetPosition = connection.GetConnectionPoint(rect);//获取连接点的位置
            Image connectionImage = connection.GetConnectionImage();//获取连接线的图片

            connection.DirectConnection(detail.direction, detail.length, detail.rotation);//修改连接线的长度和方向

            if (detail.childNode == null)
                continue;

            detail.childNode.SetPosition(targetPosition);//更换位置
            detail.childNode.SetConnectionImage(connectionImage);//设置连接线
            detail.childNode.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnection()
    {
        UpdateConnection();

        foreach (var node in connectionDetails)
        {
            if (node.childNode == null) continue;
            node.childNode.UpdateConnection();
        }
    }

    //改变连接线颜色
    public void UnlockConnectionImage(bool unlocked)
    {
        if (connectionImage == null)
            return;
        connectionImage.color = unlocked ? Color.white : originalColor;
    }
    public void SetConnectionImage(Image image) => connectionImage = image;
    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
