
public abstract class Passive : Equipment
{
    // Add empty, concrete implementations to functions which most passives have no need for
    public override void Update() { }
    public override void ProcessOther(Equipment other) { }
    public override void ProcessOtherRemoval(Equipment other) { }
}