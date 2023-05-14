namespace RogueLibsCore.Test
{
    public class sellsuc : CustomEffect
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomEffect<sellsuc>()
                 .WithName(new CustomNameInfo("已推销"))
                 .WithDescription(new CustomNameInfo("已经推销"));
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
