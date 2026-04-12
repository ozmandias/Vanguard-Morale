// can rename ...Type to ...Set
public enum Morality {
    Good = 60,
    Neutral = 50,
    Evil = 40
}

public enum PlayerType {
    Single,
    Group
}

public enum PersonType {
    Friend,
    Normal,
    Enemy,
    Companion,
    Boss
}

public enum StateMachine {
    Init,
    Idle,
    Move,
    Attack,
    Combat,
    Work,
    Follow,
    Hurt,
    Dead
}

public enum PlayerCharacter {
    Vanguard,
    Player
}

public enum PersonCharacter {
    Citizen,
    Soldier,
    Guard,
    Leader,
    Vendor
}

public enum Gender {
    Male,
    Female
}

public enum Faction {
    Fortis,
    Desertus,
    Unmortus,
    Ghostian,
    Damnum,
    Ignis,
    Scientia,
    Karus,
    CentralForest,
    Storia
}

public enum Reputation {
    Friendly,
    Neutral,
    Hostile
}

public enum CircleType {
    Semicircle,
    FullCircle
}

public enum UIType {
    Intro,
    MainMenu,
    Selection,
    Game,
    Speak,
    Shop,
    Skill,
    Effect
}

public enum CombatType {
    Melee,
    Range
}

public enum AIType {
    StateMachine,
    CombatAI,
    BossAI,
    QuestAI
}

public enum EffectType {
    Play,
    Spawn
}

public enum QuestType {
    Kill,
    Collect,
    Talk,
    Travel,
    Protect,
    Destroy
}

public enum WorkType {
    Farming,
    ShopManaging,
    Lumbering,
    GoldMining,
    BuildingDestroying
}

public enum RewardType {
    Level,
    Health,
    Damage,
    Strength,
    Agility,
    Intelligence,
    Magic,
    Item,
    Weapon
}

public enum GameMode {
    Story,
    Arena
}