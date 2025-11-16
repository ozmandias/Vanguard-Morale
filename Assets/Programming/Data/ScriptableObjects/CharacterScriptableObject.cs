using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="CharacterData", menuName="Data/Character", order=1)] public class CharacterScriptableObject : ScriptableObject {
    public List<CharacterSerializable> dataList;
}