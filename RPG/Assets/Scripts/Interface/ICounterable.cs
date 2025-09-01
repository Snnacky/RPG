using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICounterable 
{
    public bool CanBeCountered { get; }//可以被反击
    public void HandleCounter();//处理反击效果
}
