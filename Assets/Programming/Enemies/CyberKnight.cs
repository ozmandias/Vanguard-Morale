using UnityEngine;

public class CyberKnight : Boss {
    public override void Start() {
        base.Start();
        
        personInfo.damage = 50;
    }

    public override void Update() {
        base.Update();
    }

    public override void SpecialAbility() {
        
    }
}