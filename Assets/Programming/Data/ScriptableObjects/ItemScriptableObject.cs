using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="ItemData", menuName="Data/Item", order=3)] public class ItemScriptableObject : ScriptableObject {
    public List<ItemSerializable> dataList;
}