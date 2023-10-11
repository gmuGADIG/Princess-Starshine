
public abstract class Passive : Equipment
{
    public abstract string GetLevelUpDescription();
    public abstract void ApplyLevelUp();

    
    // Add empty, concrete implementations to functions which most passives have no need for
    public override void Update() { }
    public override void ProcessOther(Equipment other) { }
    public override void ProcessOtherRemoval(Equipment other) { }
}