using BepInEx;
using BepInEx.Logging;
using MyAwesomeMod;
using RogueLibsCore;
using System.Collections.Generic;
using UnityEngine;
namespace RogueLibsCore.Test
{
    public class drugmachine : CustomItem, IItemCombinable
    {
        [RLSetup]
        public static void Setup()
        {
            RogueLibs.CreateCustomItem<drugmachine>()
                .WithName(new CustomNameInfo("毒品制造机"))
                .WithDescription(new CustomNameInfo("用各种物品来制造你的毒品吧！"))
                .WithSprite(Drugdealer.Properties.Resources.drugmachine)
                .WithUnlock(new ItemUnlock
                {
                    UnlockCost = 0,
                    LoadoutCost = 0,
                    CharacterCreationCost = 10,
                    IsAvailableInCC = true,
                    IsAvailable = false,
                    IsAvailableInItemTeleporter = true,
                    
                });
            drugs = new List<string>();
            string[] d = new string[] { "Syringe", "ElectroPill",  
                "Steroids", "RagePoison","Giantizer","Shrinker","Cigarettes"
            ,"Cocaine","CritterUpper","CyanidePill","KillerThrower"};
            foreach (string i in d)
            {
                drugs.Add(i);
            }
        }
        static List<string> drugs;
        public override void SetupDetails()
        {
            Item.itemType = ItemTypes.Combine;
            Item.itemValue = -100;
            Item.initCount = 0;
            Item.rewardCount = 0;
            Item.stackable = true;
            Item.hasCharges = false;
            Item.goesInToolbar = false;
            Item.cantBeCloned = true;
            Item.cantStoreInATMMachine = true;
            Item.notInLoadoutMachine = true;
            Item.characterExclusive = true;
            Item.cantDrop = true;
        }
        public bool CombineFilter(InvItem other)
        {
            if (!Canmakedrug(other))
            {
                return false;
            }
            return true;
        }
        public bool CombineItems(InvItem other)
        {
            if(other.invItemName=="drugraw")
            {
                other.itemValue = 50;
            }
            Agent agent = GameController.gameController.playerAgent;
            if (!CombineFilter(other))
            {
                gc.audioHandler.Play(Owner, VanillaAudio.CantDo);
                return false;
            }
            {
                if (other.isArmor || other.isArmorHead || other.hasCharges || (other.isWeapon && other.itemType != "WeaponThrown")&&other.invItemName!="drugraw")
                {
                    agent.inventory.DestroyItem(other);
                }
                else
                {
                    agent.inventory.SubtractFromItemCount(other, 1);
                }
                gc.audioHandler.Play(Owner, VanillaAudio.BuyItem);
            }
            
            if (Count + other.itemValue*2 >= 100)
            {
                Count += other.itemValue*2;
                int num = Count / 100;
                InvItem invItem = new InvItem();
                invItem.invItemName = Randomdrug();
                invItem.ItemSetup(false);
                invItem.invItemCount = num;
                agent.inventory.AddItem(invItem);
                //agent.statusEffects.myStatusEffectDisplay.RefreshStatusEffectText();
                if (Count%100==0)
                {
                    Count = 1;
                }
                else
                {
                    Count %= 100;
                }
                Owner.Say("我的毒品完成了！哈哈哈！");
                if (other.invItemName == "drugraw")
                {
                    other.itemValue = -100;
                }
                return true;
            }
            else
            {
                Count += other.itemValue*2;
                if (other.invItemName == "drugraw")
                {
                    other.itemValue = -100;
                }
                return true;
            }

        }
        public CustomTooltip CombineCursorText(InvItem other) 
        {
            return new CustomTooltip(null,Color.magenta);
        }
        public CustomTooltip CombineTooltip(InvItem other)
        {
            if(!Canmakedrug(other))
            {
                return default;
            }
            if(other.invItemName=="drugraw")
            {
                return 100;
            }
            return other.itemValue;
        }
        public bool Canmakedrug(InvItem item)
        {
            if (drugs.Contains(item.invItemName) || item.isKey||item.questItem||item.invItemName== "drugmachine"||item.invItemName=="Money")
            {
                return false;
            }
            return true;
        }
        public string Randomdrug()
        {
            int n = Random.Range(0, drugs.Count);
            return drugs[n];
        }
    }
}