using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="FactionData", menuName="Data/Faction", order=6)]
public class FactionScriptableObject : ScriptableObject {
    public List<FactionModel> dataList;
}