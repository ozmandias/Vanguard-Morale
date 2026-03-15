using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="SceneData", menuName="Data/Scene", order=4)] public class SceneScriptableObject : ScriptableObject {
    public List<SceneModel> dataList;
}