using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ArchiveManager : MonoBehaviour
{
    public GameObject infopos;
    public AchievementInfo infoPre;
    private List<AchievementInfo> activeAchievements;
    List<Achievement> huntList;
    List<Achievement> floorList;
    List<Achievement> keyList;
    List<Achievement> FeatureList;
    List<Achievement> CheckPointList;
    List<Achievement> ItemList;
    //public int questNum = 0;
    private AchievementInfo nowQuest;
    Dictionary<int, Achievement> huntAchievements;
    Dictionary<int, Achievement> floorAchievements;
    Dictionary<int, Achievement> keyAchievements;
    Dictionary<int, Achievement> featureList;
    Dictionary <int, Achievement> checkPointList;
    Dictionary<int , Achievement> itemList;
    protected void Awake()
    {
        infopos = gameObject;
        huntList = new List<Achievement>();
        huntAchievements= new Dictionary<int, Achievement>();
        floorList = new List<Achievement>();
        floorAchievements = new Dictionary<int, Achievement>();
       
        keyList = new List<Achievement>();
        FeatureList = new List<Achievement>();
        CheckPointList = new List<Achievement>();
        ItemList = new List<Achievement>();
        getArchives();
        EventBus.Subscribe<object>("ItemAchieve", OnItemTriggered);
        EventBus.Subscribe<object>("FeatureAchieve", OnFeatureTriggered);
        EventBus.Subscribe<object>("CheckPointAchieve", OnCheckPointTriggered);
        EventBus.Subscribe<object>("FloorAchieve",OnFloorQuestTriggered);
        EventBus.Subscribe<int>("QuestClear",OnChangeQuest);
        
       
    }
    private void OnDestroy()
    {
        EventBus.Unsubscribe<object>("ItemAchieve", OnItemTriggered);
        EventBus.Unsubscribe<object>("FeatureAchieve", OnFeatureTriggered);
        EventBus.Unsubscribe<object>("CheckPointAchieve", OnCheckPointTriggered);
        EventBus.Unsubscribe<object>("FloorAchieve", OnFloorQuestTriggered);
        EventBus.Unsubscribe<int>("QuestClear", OnChangeQuest);
    }
    public string showArchive(Achievement a)
    {
        return a.title+"\n"+a.description;
    }
    private void Update()
    {
        
    }
    private void OnChangeQuest(int qNum)
    {
        if(qNum!=GameManager.Instance.questNum)
            { return; }
        if(nowQuest!=null)
            nowQuest.Clear();
        GameManager.Instance.questNum++;
        if (GameManager.Instance.questNum >= keyList.Count)
            return;
       
        Debug.Log(keyList[GameManager.Instance.questNum].title + " " + keyList[GameManager.Instance.questNum].ID);
        
        if (infopos == null)
        {
            infopos = gameObject;
            Debug.LogError("infopos is null");
        }
        AchievementInfo Info = Instantiate(infoPre, infopos.transform);
        Info.Init(keyList[GameManager.Instance.questNum]);
        nowQuest = Info;
       
    }
    private void getArchives()
    {
        List<Achievement> archives = SaveLoadJSON.LoadDataList<Achievement>();
        Debug.Log(archives.Count);
        activeAchievements = new List<AchievementInfo>();
        //for (int i = 0; i < 5; i++)
        //{
        //    Debug.Log(archives[i].title + " " + archives[i].ID);
        //    AchievementInfo Info = Instantiate(infoPre, infopos.transform);
        //    Info.Init(archives[i]);
        //    activeAchievements.Add(Info);
        //}
         Debug.Log(archives[GameManager.Instance.questNum].title + " " + archives[GameManager.Instance.questNum].ID);
        AchievementInfo Info = Instantiate(infoPre, infopos.transform);
         Info.Init(archives[GameManager.Instance.questNum]);

        nowQuest = Info;
       
        foreach (Achievement achievement in archives) {
            achievement.currentCount = 0;
            switch( (AchieveType)Enum.Parse(typeof(AchieveType),achievement.type))
            {
                case AchieveType.Key:
                    keyList.Add(achievement);
                    break;
                case AchieveType.Hunt:
                    huntList.Add(achievement);
                    huntAchievements[achievement.ID] = achievement;
                    break;
                case AchieveType.Floor:
                    floorList.Add(achievement);
                    floorAchievements[achievement.ID] = achievement;
                    break;
                case AchieveType.Feature:
                    FeatureList.Add(achievement);
                break;
                case AchieveType.CheckPoint:
                    CheckPointList.Add(achievement);
                    break;
                case AchieveType.Item:
                    ItemList.Add(achievement);
                    break;
            }
        }
    
    }
    private void OnCheckPointTriggered(object id)
    {
        var list = CheckPointList.Where(m => m.monId == (int)id && !m.isUnlocked).ToList();
        foreach (Achievement achievement in list)
        {
            AddAchievement(achievement);
        }
    }
    private void OnItemTriggered(object id)
    {
        var list = ItemList.Where(m => m.monId == (int)id && !m.isUnlocked).ToList();
        foreach (Achievement achievement in list)
        {
            AddAchievement(achievement);
        }
    }
    private void OnFeatureTriggered(object i)
    {
        var list = FeatureList.Where(m => m.monId == (int)i && !m.isUnlocked).ToList();
        foreach (Achievement achievement in list)
        {
            AddAchievement(achievement);
        }
    }
    private void OnHuntQuestTriggered(object i)
    {
        var list = huntList.Where(m => m.monId == (int)i &&!m.isUnlocked).ToList();
        foreach (Achievement achievement in list) {
                AddAchievement(achievement);
        }
    }
    private void OnFloorQuestTriggered(object i)
    {
        var list = floorList.Where(m => m.goalCount == (int)i && !m.isUnlocked).ToList();
        foreach (Achievement achievement in list)
        {
             OpenAchievement(achievement);
            
        }
    }
    private void OpenAchievement(Achievement achievement)
    {
        achievement.isUnlocked = true;
        //여기에 성공 시스템 추가
        List<AchievementInfo> a= activeAchievements.Where(i=>i.id==achievement.ID).ToList();
        foreach(AchievementInfo b in a)
        {
            Debug.Log(b.name+" "+b.id);
            b.Clear();
        }
        Debug.Log(achievement.title+"해금");
    }
    private void AddAchievement(Achievement achievement)
    {
        achievement.currentCount++;
        if (achievement.currentCount == achievement.goalCount)
        {
            OpenAchievement(achievement);
            //여기에 성공 시스템 추가
           
        }
    }

}

