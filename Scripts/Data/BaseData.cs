using System.Collections.Generic;
using UnityEngine;

public class BaseData<T> where T : IDataAble
{
    List<T> data;
    string name;
    public BaseData(){
        data = new List<T>();
        //data = SaveLoadJSON.LoadData<T>(typeof(T).Name);
        name = typeof(T).Name+"DATA";
    }
   
}
