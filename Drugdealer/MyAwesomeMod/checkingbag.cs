using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLibsCore.Test
{
    public class checkingbag : CustomTrait
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomTrait<checkingbag>()
                .WithName(new CustomNameInfo("正在看包"))
                .WithDescription(new CustomNameInfo("正在看"))
                .WithUnlock(new TraitUnlock { CharacterCreationCost = 0, IsAvailable = false, IsAddedToCC = false, IsAvailableInCC = false }
                );

        }

        public override void OnAdded() { }
        public override void OnRemoved() { }

    }
}