using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitDataBase", menuName = "Scriptable Objects/UnitDataBase")]
public class UnitDataBase : ScriptableObject {

    public List<UnitData> unitDatas;
}

[Serializable]
public class UnitData {
        
    [field: SerializeField]
    public string name {get; private set; }

    [field: SerializeField]
    public int id {get; private set; }

    [field: SerializeField]
    public int cost {get; private set; }

    [field: SerializeField]
    public int time {get; private set; }
    
    [field: SerializeField]
    public Texture headshot {get; private set; }

    [field: SerializeField]
    public GameObject prefab {get; private set; }
}
