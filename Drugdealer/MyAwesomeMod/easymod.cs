using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLibsCore.Test
{
    public class easymod : CustomTrait
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomTrait<easymod>()
                .WithName(new CustomNameInfo("贩毒的简单模式"))
                .WithDescription(new CustomNameInfo("你可以一眼看出哪些人是瘾君子"))
                .WithUnlock(new TraitUnlock { CharacterCreationCost = 1, IsAvailable = false }
                );

        }

        public override void OnAdded() { }
        public override void OnRemoved() { }

    }
}