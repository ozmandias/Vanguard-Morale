using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="QuestData", menuName="Data/Quest", order=5)] public class QuestScriptableObject : ScriptableObject {
    public List<QuestModel> dataList;
}