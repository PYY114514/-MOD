using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLibsCore.Test
{
    public class wantbuy : CustomTrait
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomTrait<wantbuy>()
                .WithName(new CustomNameInfo("想要买毒"))
                .WithDescription(new CustomNameInfo("想买毒"))
                .WithUnlock(new TraitUnlock { CharacterCreationCost = 0, IsAvailable = false, IsAddedToCC = false, IsAvailableInCC = false }
                );

        }

        public override void OnAdded() { }
        public override void OnRemoved() { }

    }
}