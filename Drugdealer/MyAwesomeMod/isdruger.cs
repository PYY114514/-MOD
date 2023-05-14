using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLibsCore.Test
{
    public class isdruger : CustomTrait
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomTrait<isdruger>()
                .WithName(new CustomNameInfo("吸毒者"))
                .WithDescription(new CustomNameInfo("是吸毒者"))
                .WithUnlock(new TraitUnlock { CharacterCreationCost = 0, IsAvailable = false, IsAddedToCC = false, IsAvailableInCC = false }
                );

        }

        public override void OnAdded() { }
        public override void OnRemoved() { }

    }
}