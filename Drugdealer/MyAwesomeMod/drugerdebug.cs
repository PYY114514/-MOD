namespace RogueLibsCore.Test
{
    public class drugerdebug : CustomEffect
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomEffect<drugerdebug>()
                 .WithName(new CustomNameInfo("吸毒者"))
                 .WithDescription(new CustomNameInfo("吸毒者"));
        }

        public override int GetEffectTime() => 0;
        public override int GetEffectHate() => 0;
        public override void OnAdded()
        {
        }
        public override void OnRemoved()
        {

        }
        public override void OnUpdated(EffectUpdatedArgs e)
        {
        }
    }
}
