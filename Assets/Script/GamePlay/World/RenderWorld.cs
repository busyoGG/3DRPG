

public class RenderWorld : ECSWorld
{
    public override void SystemAdd()
    {
        Add(new SkillSystem());
        Add(new RenderSystem());
        Add(new HealthBarSystem());
    }
}
