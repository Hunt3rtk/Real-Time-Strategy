using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectsDataBase", menuName = "Scriptable Objects/ObjectsDataBase")]
public class ObjectsDataBaseSO : ScriptableObject {

    public List<ObjectData> objectsData;
    
}

[Serializable]
public class ObjectData {
    [field: SerializeField]
    public string name {get; private set; }
    [field: SerializeField]
    public int id {get; private set; }
    [field: SerializeField]
    public int cost {get; private set; }
    [field: SerializeField]
    public int buildTime {get; private set; }
    [field: SerializeField]
    public Vector2Int size {get; private set; }
    [field: SerializeField]
    public GameObject prefab {get; private set; }

}
