using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="WarData", menuName="Data/War", order=7)]
public class WarScriptableObject : ScriptableObject {
    public List<WarModel> dataList;
}