using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICounterable 
{
    public bool CanBeCountered { get; }//���Ա�����
    public void HandleCounter();//������Ч��
}
