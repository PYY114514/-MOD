using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLibsCore.Test
{
    public class isrealdruger : CustomTrait
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomTrait<isrealdruger>()
                .WithName(new CustomNameInfo("毒瘾者"))
                .WithDescription(new CustomNameInfo("是毒瘾者"))
                .WithUnlock(new TraitUnlock { CharacterCreationCost = 0, IsAvailable = false, IsAddedToCC = false, IsAvailableInCC = false }
                );

        }

        public override void OnAdded() { }
        public override void OnRemoved() { }

    }
}