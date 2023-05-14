namespace RogueLibsCore.Test
{
    public class sellfail : CustomEffect
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomEffect<sellfail>()
                 .WithName(new CustomNameInfo("推销失败"))
                 .WithDescription(new CustomNameInfo("已经失败"));
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
