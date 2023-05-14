using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLibsCore.Test
{
    public class followdealer : CustomTrait
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomTrait<followdealer>()
                .WithName(new CustomNameInfo("正在跟着毒贩"))
                .WithDescription(new CustomNameInfo("正在跟着毒贩"))
                .WithUnlock(new TraitUnlock { CharacterCreationCost = 0, IsAvailable = false, IsAddedToCC = false, IsAvailableInCC = false }
                );

        }

        public override void OnAdded() { }
        public override void OnRemoved() { }

    }
}