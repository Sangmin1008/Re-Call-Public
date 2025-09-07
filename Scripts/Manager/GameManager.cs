using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{
    public bool isEnd = false;
    public enum GameState { Start, End };
    private int floorNum;
    public int questNum = 0;
    private int monsterNum = 0;
    private int playTime = 0;
    public int FloorNum { get { return floorNum; } }
    public static readonly float GameSpeedFloat = 24 / 10.0f;
    private Dictionary<string, int> eventFlags;
    // Start is called before the first frame update
    public Timer mainTimer;
    public int loopCount = 0;
    public Action<object> loopInfo;
    public Action<object> floorInfo;
    public Action<object> timerInfo;
    public Action fadeAction;
    public Action<object> OnGameEnd;

    
    public int LoopCount { get { return loopCount; } }

    Vector3 spawnPos;
    public float duration = 4f;

    protected override void Awake()
    {
        base.Awake();
        mainTimer = GetComponent<Timer>();
        if (mainTimer == null)
            mainTimer = gameObject.AddComponent<Timer>();


        mainTimer.OnTimerEnd += GameOver;

        spawnPos = Camera.main.transform.position;
        

    }
    public void HuntMonster()
    {
        monsterNum++;
    }
    public void Start()
    {
        GameStart();
    }
    private void GameStart()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        loopCount++;
        List<float> l = new List<float> { 18 * 60 + 30, 12 * 60, 5 * 60 + 15 };
        mainTimer.Init(24, l);
        floorNum = 0;
        eventFlags = new Dictionary<string, int>();
        loopInfo?.Invoke(loopCount);
        floorInfo?.Invoke(floorNum);
        mainTimer.StartTimer();

    }
    public void GameOver()
    {

        OnGameEnd?.Invoke(null);
        //fadeAction.Invoke();
        Debug.Log("사망");
        //여기에 메뉴를 넣는다거나...
        Loop();
        //StartCoroutine(WaitTime(4, Loop));
    }
    IEnumerator WaitTime(float x, System.Action callback)
    {
        yield return new WaitForSeconds(x);
        callback?.Invoke();
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }

    //여기서 이벤트 체크
    public bool checkEvent(string name, int flag)
    {
        if (eventFlags.ContainsKey(name))
        {
            if (eventFlags[name] >= flag)
                return true;
        }
        return false;
    }
    //여기서 이벤트 설정:만약 방문을 열었을 경우 이름을 보내  플래그를 1로 설정
    //1 이상이 필요한 이벤트인 경우 숫자를 더 올려서
    public void SetEvent(string name)
    {
        if (eventFlags.ContainsKey(name))
            eventFlags[name]++;
        else
            eventFlags.Add(name, 1);

    }
    public void ResetEvent(string name)
    {
        if (eventFlags.ContainsKey(name))
            eventFlags.Remove(name);
    }
    public void savePoint(int nowFloor)
    {
        floorNum = Mathf.Max(floorNum, nowFloor);
    }
    private void Loop()
    {
        //mainTimer.StopTimer();
        playTime += (int)mainTimer.CurrentTime;
        //땜빵
        mainTimer.ReturnTime();
        Invoke("StopControl", 1f);
        EventBus.Publish("FadeEvent", null);

        EventBus.Publish("VerticalWipeEvent", null);
      
        Camera.main.transform.DOMove(spawnPos, duration).SetEase(Ease.InOutQuad).OnComplete(() => { SceneManager.LoadScene(floorNum); });

        //SceneManager.LoadScene(floorNum);
    }

    public void StopControl()
    {
        EventBus.PublishVoid("DisablePlayerInput");
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //EventBus.Clear();
        DroppedItemManager.Instance.RespawnAllItems();
        if (loopCount >= 5)
        {
            //Time.timeScale = 0f;
            isEnd = true;

            EventBus.Publish<object>("GameOver", 0);
            return;
        }
        loopCount++;
           
        List<float> l = new List<float> { 18 * 60 + 30, 12 * 60, 5 * 60 };
        mainTimer.Init(24, l);
        mainTimer.StartTimer();
        loopInfo?.Invoke(loopCount);
        floorInfo?.Invoke(floorNum);
    }
    //이걸 이용하여 시간별 이벤트 제작 가능.
    public void SetTimer(Action<float> callback, float time)
    {
        mainTimer.OnTick += callback;
        mainTimer.AddTargetTime(time);

    }

    public void GameClear()
    {
        isEnd = true;
        mainTimer.StopTimer();
        playTime += (int)mainTimer.CurrentTime;
        string str = $"루프 횟수 : {loopCount}\n\n 루프 시간 : {playTime}\n\n 사냥한 몬스터 수 :{monsterNum}";
        EventBus.Publish<string>("Ending", str);
       // Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        ;        //Debug.Log(mainTimer.ShowTime());


    }

}
