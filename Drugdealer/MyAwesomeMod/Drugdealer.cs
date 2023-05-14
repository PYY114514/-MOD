using BepInEx;
using BepInEx.Logging;
using System.Collections;
using RogueLibsCore;
using UnityEngine;
using System.Collections.Generic;

namespace MyAwesomeMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(RogueLibs.GUID, RogueLibs.CompiledVersion)]
    public class Drugdealer : BaseUnityPlugin
    {
        public const string PluginGuid = "pyy.streetsofrogue.drugdealer";
        public const string PluginName = "Drugdealer";
        public const string PluginVersion = "0.1.0";

        public new static ManualLogSource Logger = null!;
        public void Update()
        {
           if(Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log(GameController.gameController.playerAgent.curOwnerTile);
            }
        }
        static Dictionary<string, string> transeff = new Dictionary<string, string>();
        public void Awake()
        {
            {
                Logger = base.Logger;
                RogueLibs.LoadFromAssembly();
                RoguePatcher patcher = new RoguePatcher(this);
            }
            /* My Awesome Code */
            RoguePatcher roguePatcher = new RoguePatcher(this);
            roguePatcher.Postfix(typeof(LoadLevel), "SetupRels", null);
            roguePatcher.Postfix(typeof(AgentInteractions), "DetermineButtons", null);
            roguePatcher.Postfix(typeof(AgentInteractions), "PressedButton", null);
            //roguePatcher.Postfix(typeof(ATMMachine), "PressedButton", null);
            //roguePatcher.Postfix(typeof(ATMMachine), "DetermineButtons", null);
           // roguePatcher.Postfix(typeof(Agent), "AgentUpdate", null);
            //初始化dic
            {
                transeff.Add("SyringeSlow", "减速");
                transeff.Add("SyringeFast", "疾走");
                transeff.Add("SyringeStrength", "力量");
                transeff.Add("SyringeWeak", "虚弱");
                transeff.Add("SyringeAcid", "硫酸");
                transeff.Add("SyringeInvincible", "无敌");
                transeff.Add("SyringeInvisible", "隐身");
                transeff.Add("SyringeConfused", "困惑");

            }
            drugs = new List<string>(new string[] { "SyringeSlow","SyringeFast","SyringeStrength","SyringeWeak","SyringeAcid",
"SyringeInvincible","SyringeInvisible","SyringeConfused", "ElectroPill",
                "Steroids", "RagePoison","Giantizer","Shrinker","Cigarettes"
            ,"Cocaine","CritterUpper","KillerThrower"});

            RogueLibs.CreateCustomName("tuixiao", "Interface", new CustomNameInfo
            {
                Chinese = "推销毒品"
            });
            RogueLibs.CreateCustomName("makeads", "Interface", new CustomNameInfo
            {
                Chinese = "打广告"
            });
            {

                RogueLibs.CreateCustomName("售出SyringeSlow", "Interface", new CustomNameInfo
                {
                    Chinese = "售出减速注射器"
                });

                RogueLibs.CreateCustomName("售出SyringeFast", "Interface", new CustomNameInfo
                {
                    Chinese = "售出疾走注射器"
                });

                RogueLibs.CreateCustomName("售出SyringeStrength", "Interface", new CustomNameInfo
                {
                    Chinese = "售出力量注射器"
                });

                RogueLibs.CreateCustomName("售出SyringeWeak", "Interface", new CustomNameInfo
                {
                    Chinese = "售出虚弱注射器"
                });

                RogueLibs.CreateCustomName("售出SyringeAcid", "Interface", new CustomNameInfo
                {
                    Chinese = "售出硫酸注射器"
                });

                RogueLibs.CreateCustomName("售出SyringeInvincible", "Interface", new CustomNameInfo
                {
                    Chinese = "售出无敌注射器"
                });

                RogueLibs.CreateCustomName("售出SyringeInvisible", "Interface", new CustomNameInfo
                {
                    Chinese = "售出隐身注射器"
                });

                RogueLibs.CreateCustomName("售出SyringeConfused", "Interface", new CustomNameInfo
                {
                    Chinese = "售出困惑注射器"
                });
                RogueLibs.CreateCustomName("售出ElectroPill", "Interface", new CustomNameInfo
                {
                    Chinese = "售出电击药丸"
                });

                RogueLibs.CreateCustomName("售出Steroids", "Interface", new CustomNameInfo
                {
                    Chinese = "售出大力丸"
                });

                RogueLibs.CreateCustomName("售出RagePoison", "Interface", new CustomNameInfo
                {
                    Chinese = "售出狂怒毒药"
                });

                RogueLibs.CreateCustomName("售出Giantizer", "Interface", new CustomNameInfo
                {
                    Chinese = "售出巨人饮料"
                });

                RogueLibs.CreateCustomName("售出Shrinker", "Interface", new CustomNameInfo
                {
                    Chinese = "售出缩小仪"
                });

                RogueLibs.CreateCustomName("售出Cigarettes", "Interface", new CustomNameInfo
                {
                    Chinese = "售出香烟"
                });

                RogueLibs.CreateCustomName("售出Cocaine", "Interface", new CustomNameInfo
                {
                    Chinese = "售出白粉"
                });

                RogueLibs.CreateCustomName("售出CritterUpper", "Interface", new CustomNameInfo
                {
                    Chinese = "售出暴击增强器"
                });

                RogueLibs.CreateCustomName("售出KillerThrower", "Interface", new CustomNameInfo
                {
                    Chinese = "售出杀手飞镖"
                });
            }
        }
        public static void Agent_AgentUpdate(Agent __instance)
        {
            if(__instance.movement.HasLOSAgent(GameController.gameController.playerAgent) && Vector2.Distance(__instance.tr.position, GameController.gameController.playerAgent.tr.position) < __instance.LOSRange / __instance.hardToSeeFromDistance)
            {
                if (__instance.HasTrait("seeads") && !__instance.HasTrait("followdealer") && !__instance.HasTrait("sellfail") && !__instance.HasTrait("sellsuc") && !__instance.HasTrait("wantbuy"))
                {
                    __instance.statusEffects.RemoveTrait("seeads");
                    if (__instance.HasTrait("isdruger"))
                    {
                        __instance.StartCoroutine(Selectdrug(__instance, GameController.gameController.playerAgent.inventory, false));
                    }
                    else if (__instance.HasTrait("isrealdruger"))
                    {
                        __instance.StartCoroutine(Drugerfollow(__instance, GameController.gameController.playerAgent.inventory, false));
                    }
                }
            }

        }
        public static bool ads=false;
        public static void AgentInteractions_DetermineButtons(AgentInteractions __instance, Agent agent, Agent interactingAgent)
        {
            if(interactingAgent.HasTrait("drugseller"))
            {
                if (!agent.HasTrait("sellfail") && !agent.HasTrait("sellsuc") && !agent.HasTrait("wantbuy") && !agent.HasTrait("checkingbag"))
                {
                    __instance.AddButton("tuixiao");
                }
                if(agent.HasTrait("wantbuy"))
                {
                    foreach (Npcsdrug i in npcswant)
                    {
                        if (i.agent == agent)
                        {
                            InvItem invItem = new InvItem();
                            invItem.invItemName = i.drug;
                            invItem.ItemSetup(false);
                            Debug.Log(i.drug);
                            if (i.drug.Contains("Syringe"))
                            {
                                Debug.Log(i.drug);
                                if (GameController.gameController.sessionDataBig.curLevel > 12)
                                {
                                    __instance.AddButton("售出" + i.drug , invItem.itemValue * 5);
                                }
                                else
                                {
                                    __instance.AddButton("售出" + i.drug, invItem.itemValue * 2);
                                }
                            }
                            else
                            {
                                if (GameController.gameController.sessionDataBig.curLevel > 12)
                                {
                                    __instance.AddButton("售出" + i.drug, invItem.itemValue * 5);
                                }
                                else
                                {
                                    __instance.AddButton("售出" + i.drug, invItem.itemValue * 2);
                                }
                                    
                            }                            
                            break;
                        }
                    }
                }
            }
        }
        public class Npcsdrug
        {
            public Npcsdrug(Agent a,string d)
            {
                agent = a;
                drug = d;
            }
            public Agent agent;
            public string drug;
        }
        public static List<string> drugs;
        public static List<Npcsdrug> npcswant=new List<Npcsdrug>();
        public static void AgentInteractions_PressedButton(AgentInteractions __instance, Agent agent, Agent interactingAgent, string buttonText, int buttonPrice)
        {
            GameController gc = GameController.gameController;
            if(buttonText=="tuixiao")
            {
                if(agent.HasTrait("isdruger"))
                {
                    agent.StartCoroutine(Selectdrug(agent, interactingAgent.inventory,false));
                }
                else if(agent.HasTrait("isrealdruger"))
                {
                    agent.StartCoroutine(Drugerfollow(agent, interactingAgent.inventory,false));
                }
                else
                {
                    if(agent.HasTrait("TheLaw"))
                    {
                        agent.relationships.SetRel(interactingAgent, "Hateful");
                        agent.relationships.SetRelHate(interactingAgent, 5);
                        gc.audioHandler.Play(agent, "AgentAnnoyed");
                        agent.Say("没把我的警徽当回事是吧！！");
                    }
                    else if(Random.Range(0,2)==0)
                    {
                        //agent.relationships.SetRelHate(interactingAgent, 2);
                        agent.relationships.SetRel(interactingAgent, "Annoyed");
                        agent.relationships.SetStrikes(interactingAgent, 2);
                        Debug.Log(agent.relationships.GetRelCode(interactingAgent));
                        gc.audioHandler.Play(agent, "AgentAnnoyed");

                        agent.Say("我才不要，滚！");
                    }
                    else
                    {
                        agent.Say("不要！");
                    }
                    agent.AddTrait("sellfail");
                }
            }
            if(buttonText.Contains("售出"))
            {
                string syringef="";
                InvItem thedrug = new InvItem();
                foreach (Npcsdrug i in npcswant)
                {
                    if (i.agent == agent)
                    {
                        if(i.drug.Contains("Syringe"))
                        {
                            thedrug.invItemName = "Syringe";
                            syringef = i.drug.Replace("Syringe", "");
                        }
                        else
                        {
                            thedrug.invItemName = i.drug;
                        }
                        
                        thedrug.SetupDetails(false);
                        break;
                    }
                }
                List<string> a = new List<string>();
                a.Add(syringef);
                if(interactingAgent.inventory.HasItem(thedrug.invItemName)&&(thedrug.invItemName != "Syringe" || interactingAgent.inventory.FindItem("Syringe", a) != null))
                {
                    if(thedrug.invItemName!="Syringe")
                    {
                        interactingAgent.inventory.SubtractFromItemCount(interactingAgent.inventory.FindItem(thedrug.invItemName), 1); 
                    }
                    else
                    {

                        interactingAgent.inventory.SubtractFromItemCount(interactingAgent.inventory.FindItem(thedrug.invItemName,a),1);

                    }
                    if(gc.sessionDataBig.curLevel>12)
                    {
                        interactingAgent.inventory.AddItem("Money", thedrug.itemValue * 5);
                    }
                    else
                    {
interactingAgent.inventory.AddItem("Money", thedrug.itemValue*2);
                    }
                    
                    gc.audioHandler.Play(agent, VanillaAudio.BuyItem);
                    if(thedrug.invItemName!="Syringe")
                    {
                        agent.AddEffect(thedrug.statusEffect);
                    }
                    else
                    {
                        agent.AddEffect(syringef);
                    }
                    agent.statusEffects.RemoveTrait("wantbuy");
                    agent.AddTrait("sellsuc");
                    agent.Say("成交！");
                    bool zhaoyang = false;
                    //其他人反应
                    foreach(Agent i in gc.agentList)
                    {
                        
                        if(i==agent||i==interactingAgent)
                        {
                            continue;
                        }
                        if(i.movement.HasLOSAgent(interactingAgent)&&Vector2.Distance(i.tr.position,interactingAgent.tr.position)<i.LOSRange/i.hardToSeeFromDistance)
                        {

                            if (i.HasTrait("isdruger")&&i.relationships.GetRel(interactingAgent)!="Hateful"&& i.relationships.GetRel(interactingAgent) != "Annoyed")
                            {
                                
                                if (!i.HasTrait("sellfail") && !i.HasTrait("sellsuc") && !i.HasTrait("wantbuy") && !i.HasTrait("checkingbag"))
                                {
                                    i.StartCoroutine(Selectdrug(i, interactingAgent.inventory,true));
                                }                                  
                            }
                            else if (i.HasTrait("isrealdruger") && i.relationships.GetRel(interactingAgent) != "Hateful" && i.relationships.GetRel(interactingAgent) != "Annoyed")
                            {
                                
                                if (!i.HasTrait("sellfail") && !i.HasTrait("sellsuc") && !i.HasTrait("wantbuy") && !i.HasTrait("checkingbag"))
                                {
                                    i.StartCoroutine(Drugerfollow(i, interactingAgent.inventory,true));
                                }                                   
                            }
                            else
                            {
                                
                                if (i.HasTrait("TheLaw"))
                                {
                                    i.relationships.SetRelHate(interactingAgent, 5);
                                    i.StartCoroutine(copsay(i));
                                }
                                else if (i.ownerID == interactingAgent.curOwnerTile&&i.ownerID!=0&&i.agentName!="Guard"&&i.agentName!="Guard2")
                                {
                                    i.Say("别在我这贩毒，滚！！！");
                                    gc.audioHandler.Play(i, VanillaAudio.AgentAnnoyed);
                                    i.relationships.SetStrikes(interactingAgent, 2);
                                    i.relationships.SetRel(agent, "Annoyed");
                                }
                                else if(i.agentName== "UpperCruster"&& zhaoyang==false)
                                {
                                    i.StartCoroutine(guizusay(i));
                                    i.relationships.SetRelHate(interactingAgent, 5);
                                    i.agentInteractions.UseWalkieTalkie(interactingAgent, i);
                                }
                                else
                                {                                   
                                    gc.audioHandler.Play(i, VanillaAudio.AgentAnnoyed);
                                    i.relationships.SetStrikes(interactingAgent, 2);
                                    i.relationships.SetRel(agent, "Annoyed");
                                    i.Say("滚开！");
                                }
                            }
                        }
                    }
                }
                else
                {
                   
                     ele: interactingAgent.Say("我没有" + thedrug.invItemRealName + "！");
                    gc.audioHandler.Play(interactingAgent, VanillaAudio.CantDo);
                }
            }
         /*   if (buttonText == "YouCanGo")
            {
                if(agent.HasTrait("followdealer"))
                {
                    agent.Say("不卖的话你推销个啥啊？？？");
                    agent.statusEffects.RemoveTrait("followdealer");
                }
                if(agent.HasTrait("realfollowdealer"))
                {
                    agent.Say("不要这样……");
                    gc.audioHandler.Play(agent, VanillaAudio.CantDo);
                    agent.StartCoroutine(agent.relationships.joinPartyDelay(interactingAgent, ""));
                }
            }*/
        }
        public static IEnumerator Selectdrug(Agent buyer,InvDatabase bag,bool zhudong)
        {
            buyer.AddTrait("checkingbag");
            buyer.SetFollowing(bag.agent);
            if (zhudong)
            {
                buyer.Say("你卖的玩意好像不错，能不能给我一支？");
            }
            else
            {
                buyer.Say("好！让我康康你卖什么……");   
            }
                
            yield return new WaitForSeconds(2);
            buyer.statusEffects.RemoveTrait("checkingbag");
            if (!Hasdrug(bag))
            {
                buyer.Say("屁都没有还贩什么毒啊！");
                GameController.gameController.audioHandler.Play(buyer, "AgentAnnoyed");
                buyer.agentInteractions.LetGo(buyer, bag.agent);
            }
            else
            {
                buyer.AddTrait("wantbuy");
                string drug;
                if (Random.Range(0, 2) == 0)
                {
                    drug = Npcselectdrug();
                }
                else
                {
                    drug = Npcrealselectdrug();
                }
                InvItem item = new InvItem();
                string syringef = "";
                List<string> a = new List<string>();
                if (drug.Contains("Syringe"))
                {
                    item.invItemName = "Syringe";
                    syringef = drug.Replace("Syringe", "");
                    a.Add(syringef);
                }
                else
                {
                    item.invItemName = drug;
                }                
                item.ItemSetup(false);
                npcswant.Add(new Npcsdrug(buyer, drug));
                if (bag.HasItem(item.invItemName) && (!drug.Contains ("Syringe") || bag.FindItem("Syringe", a) != null))
                {
                    if (item.invItemName != "Syringe")
                    {
                        buyer.Say("给我来个" + item.invItemRealName + "!");
                    }
                    else
                    {
                        buyer.Say("给我来个" +transeff[drug]+ item.invItemRealName + "!");
                    }
                    for(float i=0;i<20;i+=Time.fixedDeltaTime)
                    {
                        if(i>10&&i<11)
                        {
                            buyer.Say("你卖不卖啊，不卖我走了");
                        }
                        if(buyer.HasTrait("sellsuc")||buyer.dead||buyer.relationships.GetRel(bag.agent)=="Hateful")
                        {
                            buyer.agentInteractions.LetGo(buyer, bag.agent);
                            goto end;
                        }
                        yield return new WaitForEndOfFrame();
                    }
                    buyer.Say("不卖算了……");
                }
                else
                {
                    if(item.invItemName=="Syringe")
                    {
                        buyer.Say("我想要" + transeff[drug]+item.invItemRealName + "，可是你没有！");
                    }
                    else
                    {
                        buyer.Say("我想要" + item.invItemRealName + "，可是你没有！");
                    }
                    
                    
                }
            end:;
                buyer.agentInteractions.LetGo(buyer, bag.agent);
            }            
        }
        public static IEnumerator Drugerfollow(Agent buyer,InvDatabase bag,bool zhudong)
        {
            
            buyer.AddTrait("checkingbag");
            buyer.SetFollowing(bag.agent);
            if (zhudong)
            {
                buyer.Say("嘿！我也要来一支！");
            }
            else
            {
                buyer.Say("快，快让我看看你有没有我要的……");
            }
            
            yield return new WaitForSeconds(2);
            buyer.statusEffects.RemoveTrait("checkingbag");
            if (!Hasdrug(bag))
            {
                buyer.Say("屁都没有还贩什么毒啊！");
                GameController.gameController.audioHandler.Play(buyer, "AgentAnnoyed");
                buyer.agentInteractions.LetGo(buyer, bag.agent);
            }
            else
            {
                buyer.AddTrait("wantbuy");
                string drug;
                if (Random.Range(0, 2) == 0)
                {
                    drug = Npcselectdrug();
                }
                else
                {
                    drug = Npcrealselectdrug();
                }
                InvItem item = new InvItem();
                string syringef = "";
                List<string> a = new List<string>();
                if (drug.Contains("Syringe"))
                {
                    item.invItemName = "Syringe";
                    syringef = drug.Replace("Syringe", "");
                    a.Add(syringef);
                }
                else
                {
                    item.invItemName = drug;
                }
                item.ItemSetup(false);
                npcswant.Add(new Npcsdrug(buyer, drug));
                if (bag.HasItem(item.invItemName) && (!drug.Contains("Syringe") || bag.FindItem("Syringe", a) != null))
                {
                    if(item.invItemName!="Syringe")
                    {
                        buyer.Say("快！我要" + item.invItemRealName + "!");
                    }
                    else
                    {
                        buyer.Say("快！我要" + transeff[drug]+item.invItemRealName + "!");
                    }
                    for (float i = 0;; i += Time.fixedDeltaTime)
                    {
                        if (i > 10 && i < 11)
                        {
                            buyer.Say("求你了，给我一支吧……");
                        }
                        if(i>20&&i<21)
                        {
                            buyer.Say("真的求求你了，我真的活不下去了");
                            buyer.relationships.SetRel(buyer, "Submissive");
                            if(!buyer.HasEffect(VanillaEffects.Withdrawal))
                            {
                                buyer.AddEffect(VanillaEffects.Withdrawal.ToString());
                            }
                            
                        }
                        if (buyer.HasTrait("sellsuc")||buyer.dead  || buyer.relationships.GetRel(bag.agent) == "Hateful")
                        {
                            buyer.agentInteractions.LetGo(buyer, bag.agent);
                            buyer.statusEffects.RemoveStatusEffect(VanillaEffects.Withdrawal);
                            break;
                        }
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    if (item.invItemName == "Syringe")
                    {
                        buyer.Say("你居然没有" + transeff[drug]+item.invItemRealName + "！啊啊啊……");
                    }
                    else
                    {
                        buyer.Say("你居然没有" + item.invItemRealName + "！啊啊啊……");
                    }
                       
                    buyer.agentInteractions.LetGo(buyer, bag.agent);
                }

            }


        }
        public static string Npcselectdrug()
        {
            List<string> playerdrugs=new List<string>();
            foreach(InvItem i in GameController.gameController.playerAgent.inventory.InvItemList)
            {
                foreach (string j in drugs)
                {
                    if(i.invItemName== "Syringe")
                    {
                        if (i.invItemName+i.contents[0] == j && !playerdrugs.Contains(j))
                        {
                            playerdrugs.Add(j);
                        }
                    }
                    else
                    {
                        if (i.invItemName == j && !playerdrugs.Contains(i.invItemName))
                        {
                            playerdrugs.Add(i.invItemName);
                        }
                    }
                }
            }
            return playerdrugs[Random.Range(0, playerdrugs.Count)];
        }
        public static string Npcrealselectdrug()
        {
            return drugs[Random.Range(0, drugs.Count)];
        }
        const int minodds=20, maxodds=40;
        public static void LoadLevel_SetupRels()
        {
            ads = false;
            npcswant.Clear();
            GameController gc = GameController.gameController;
            if(gc.sessionDataBig.curLevel==0)
            {
                return;
            }
            int num = gc.agentCount;
            int r;
            if(gc.sessionDataBig.curLevel<=9&&gc.sessionDataBig.curLevel>=7)
            {
                r = Random.Range(minodds+10, maxodds+10);
            }
            else if(gc.sessionDataBig.curLevel >12)
            {
                r = Random.Range(minodds- 10, maxodds - 10);
            }
            else
            {
                r = Random.Range(minodds - 10, maxodds - 10);
            }
            int drugercount = num * r / 100;
            List<int> druger = Randomdruger(drugercount);
            foreach(int i in druger)
            {
                if(gc.agentList[i].isPlayer>0)
                {
                    continue;
                }
                if(Random.Range(0,2)==0)
                {
                    gc.agentList[i].AddTrait("isrealdruger");
                    if(gc.playerAgent.HasTrait("easymod"))
                    {
                        gc.agentList[i].statusEffects.AddStatusEffect("realdrugerdebug");
                    }                    
                }
                else
                {
                    gc.agentList[i].AddTrait("isdruger");
                    if (gc.playerAgent.HasTrait("easymod"))
                    {
                        gc.agentList[i].statusEffects.AddStatusEffect("drugerdebug");
                    }
                        
                }

            }
        }
        public static List<int> Randomdruger(int count)
        {
            List<int> all = new List<int>();
            List<int> res = new List<int>();
            int r;
            for(int i=0; i<GameController.gameController.agentList.Count;i++)
            {
                
                all.Add(i);
            }
            for(int i=0;i<count;i++)
            {
                r = Random.Range(0, all.Count);
                res.Add(all[r]);
                all.RemoveAt(r);
            }
            return res;
        }
        public static bool Hasdrug(InvDatabase bag)
        {
            foreach(string i in drugs)
            {
                if(i.Contains("Syringe")&& bag.HasItem("Syringe"))
                {
                    return true;
                }
                if (bag.HasItem(i))
                {
                        return true;
                }
                
            }
            return false;
        }
        public static List<Agent> whichcopsaw()
        {
            List<Agent> cops=new List<Agent>();
            foreach (Agent i in GameController.gameController.agentList)
            {
                if (i.HasTrait("TheLaw") && i.movement.HasLOSAgent360(GameController.gameController.playerAgent)&&!(i.HasTrait("isdruger")||i.HasTrait("isrealdruger"))&& Vector2.Distance(i.tr.position, GameController.gameController.playerAgent.tr.position) < i.LOSRange / i.hardToSeeFromDistance)
                {
                    cops.Add(i);
                }
            }
            return cops;
        }
        static IEnumerator copsay(Agent agent)
        {
            for(float i=0;i<2;i+=Time.fixedDeltaTime)
            {
                agent.Say("傻逼毒贩，逮到你了！");
                yield return new WaitForEndOfFrame();
            }
        }
        static IEnumerator guizusay(Agent agent)
        {
            for (float i = 0; i < 2; i += Time.fixedDeltaTime)
            {
                agent.Say("警察！这里有人贩毒！");
                yield return new WaitForEndOfFrame();
            }
        }
        static bool Cancallcop(Agent user)
        {
            GameController gc = GameController.gameController;
            List<Agent> list = new List<Agent>();
            for (int i = 0; i < gc.agentList.Count; i++)
            {
                Agent agent7 = gc.agentList[i];
                if (agent7.enforcer && agent7.isPlayer == 0 && Vector2.Distance(agent7.tr.position, user.tr.position) < 17f)
                {
                    list.Add(agent7);
                }
            }
            if (list.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
