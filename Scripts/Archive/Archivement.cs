using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AchieveType
{
    Hunt,Floor,Item,Key,Feature,CheckPoint
}

[System.Serializable]
public class Achievement
{
    public int ID;            
    public string title;
    public int monId;
    public string type;
    public string description; 
    public bool isUnlocked;      
    public int currentCount;   
    public int goalCount;    
}