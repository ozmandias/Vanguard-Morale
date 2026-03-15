using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="WorldData", menuName="Data/World", order=2)] public class WorldScriptableObject : ScriptableObject {
    public List<KingdomModel> dataList;
}