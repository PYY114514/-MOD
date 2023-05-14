namespace RogueLibsCore.Test
{
    public class realdrugerdebug : CustomEffect
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomEffect<realdrugerdebug>()
                 .WithName(new CustomNameInfo("毒瘾者"))
                 .WithDescription(new CustomNameInfo("毒瘾者"));
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
