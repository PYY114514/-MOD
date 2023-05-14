using BepInEx;
using BepInEx.Logging;
using RogueLibsCore;
using UnityEngine;
namespace RogueLibsCore.Test
{
    [ItemCategories(RogueCategories.Usable, RogueCategories.Social)]
    public class drugraw : CustomItem
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomItem<drugraw>()
                .WithName(new CustomNameInfo("毒品原料"))
                .WithDescription(new CustomNameInfo("非常适合用来制作毒品！"))
                .WithSprite(Drugdealer.Properties.Resources.raw)
                .WithUnlock(new ItemUnlock
                {
                    UnlockCost = 0,
                    LoadoutCost = 0,
                    CharacterCreationCost = 5,
                    IsAvailableInCC = true,
                    IsAvailable = false,
                    IsAvailableInItemTeleporter = true,


                });
        }

        public override void SetupDetails()
        {
            Item.itemType = ItemTypes.Consumable;
            Item.itemValue = -100;
            Item.initCount = 10;
            Item.rewardCount = 10;
            Item.stackable = true;
            Item.hasCharges = false;
            Item.goesInToolbar = true;
            Item.cantBeCloned = true;
            Item.cantStoreInATMMachine = true;
            Item.notInLoadoutMachine = true;
        }
    }
}