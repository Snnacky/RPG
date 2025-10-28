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
    private RectTransform rect => GetComponent<RectTransform>();//��������ͼ�����transfrom,����������
    [SerializeField] private UI_TreeConnectDetails[] connectionDetails;//����ϸ��
    [SerializeField] private UI_TreeConnection[] connections;//���ٸ����Ӵ�//��detailsһ��һ

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

    //��ȡ�Ӽ��ܵ�UI_TreeNode���
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
            Vector2 targetPosition = connection.GetConnectionPoint(rect);//��ȡ���ӵ��λ��
            Image connectionImage = connection.GetConnectionImage();//��ȡ�����ߵ�ͼƬ

            connection.DirectConnection(detail.direction, detail.length, detail.rotation);//�޸������ߵĳ��Ⱥͷ���

            if (detail.childNode == null)
                continue;

            detail.childNode.SetPosition(targetPosition);//����λ��
            detail.childNode.SetConnectionImage(connectionImage);//����������
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

    //�ı���������ɫ
    public void UnlockConnectionImage(bool unlocked)
    {
        if (connectionImage == null)
            return;
        connectionImage.color = unlocked ? Color.white : originalColor;
    }
    public void SetConnectionImage(Image image) => connectionImage = image;
    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
