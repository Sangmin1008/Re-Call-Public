using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public List<APPLE> list;
    // Start is called before the first frame update
    void Start()
    {
        list = new List<APPLE>();
        APPLE ap=new APPLE();
        ap.level = 0;
        ap.name = "사과";
        list.Add(ap);
        SaveLoadJSON.SaveJSON("APPLE", list);
        List<APPLE> load = SaveLoadJSON.LoadJSON<APPLE>("APPLE");
        Debug.Log(load+" "+load.Count);
        foreach(APPLE aPPLE in load)
        {
            Debug.Log(aPPLE.name+aPPLE.level);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
