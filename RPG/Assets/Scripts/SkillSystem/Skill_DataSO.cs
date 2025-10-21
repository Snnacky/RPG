using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data")]
public class Skill_DataSO : ScriptableObject
{
    public int cost;

    [Header("ººƒ‹ΩÈ…‹")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;

}
