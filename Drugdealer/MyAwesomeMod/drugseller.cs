using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLibsCore.Test
{
    public class drugseller : CustomTrait
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomTrait<drugseller>()
                .WithName(new CustomNameInfo("贩毒者"))
                .WithDescription(new CustomNameInfo("尝试向街区里的人推销你的毒品！如果对方是瘾君子你们将达成交易！贩毒时小心警察和朝阳群众！"))
                .WithUnlock(new TraitUnlock { CharacterCreationCost = 5, IsAvailable = false }
                );

        }

        public override void OnAdded() { }
        public override void OnRemoved() { }

    }
}