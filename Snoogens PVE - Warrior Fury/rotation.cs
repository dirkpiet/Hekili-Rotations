using System.Linq;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Drawing;
using AimsharpWow.API;

namespace AimsharpWow.Modules
{
    public class SnoogensPVEWarriorFury : Rotation
    {
        #region Variables
        string FiveLetters;
        #endregion

        #region Lists
        //Lists
        private List<string> m_IngameCommandsList = new List<string> { "StormBolt", "IntimidatingShout", "SpearofBastion", "DoorofShadows", "NoInterrupts" };
        private List<string> m_DebuffsList = new List<string> { };
        private List<string> m_BuffsList = new List<string> { "Defensive Stance", "Battle Shout", "Bladestorm", "Voracious Culling Blade", };
        private List<string> m_BloodlustBuffsList = new List<string> { "Bloodlust", "Heroism", "Time Warp", "Primal Rage", "Drums of Rage" };
        private List<string> m_ItemsList = new List<string> { "Phial of Serenity", "Healthstone" };

        private List<string> m_SpellBook_General = new List<string> {
            //Covenants
            "Condemn",
            "Ancient Aftershock",
            "Spear of Bastion",
            "Conqueror's Banner",
            "Door of Shadows",
            "Fleshcraft",
            "Summon Steward",

            //Interrupt
            "Pummel",

            //General
            "Ignore Pain",
            "Slam",
            "Intervene",
            "Rallying Cry",
            "Battle Shout",
            "Execute",
            "Victory Rush",
            "Intimidating Shout",
            "Charge",
            "Whirlwind",
            "Berserker Rage",
            "Hamstring",
            "Heroic Throw",
            "Bladestorm",
            "Defensive Stance",
            "Impending Victory",
            "Storm Bolt",
            "Spell Reflection",
        };

        private List<string> m_SpellBook_Fury = new List<string>
        {
            //Fury Warrior
            "Dragon Roar",
            "Onslaught",
            "Siegebreaker",
            "Bloodthirst",
            "Rampage",
            "Recklessness",
            "Enraged Regeneration",
            "Raging Blow",
        };

        private List<string> m_RaceList = new List<string> { "human", "dwarf", "nightelf", "gnome", "draenei", "pandaren", "orc", "scourge", "tauren", "troll", "bloodelf", "goblin", "worgen", "voidelf", "lightforgeddraenei", "highmountaintauren", "nightborne", "zandalaritroll", "magharorc", "kultiran", "darkirondwarf", "vulpera", "mechagnome" };
        private List<string> m_CastingList = new List<string> { "Manual", "Player" };

        private List<int> Torghast_InnerFlame = new List<int> { 258935, 258938, 329422, 329423, };

        List<int> InstanceIDList = new List<int>
        {
            2291,
            2287,
            2290,
            2289,
            2284,
            2285,
            2286,
            2293,
            1663,
            1664,
            1665,
            1666,
            1667,
            1668,
            1669,
            1674,
            1675,
            1676,
            1677,
            1678,
            1679,
            1680,
            1683,
            1684,
            1685,
            1686,
            1687,
            1692,
            1693,
            1694,
            1695,
            1697,
            1989,
            1990,
            1991,
            1992,
            1993,
            1994,
            1995,
            1996,
            1997,
            1998,
            1999,
            2000,
            2001,
            2002,
            2003,
            2004,
            2441,
            2450
        };

        List<int> TorghastList = new List<int> { 1618 - 1641, 1645, 1705, 1712, 1716, 1720, 1721, 1736, 1749, 1751 - 1754, 1756 - 1812, 1833 - 1911, 1913, 1914, 1920, 1921, 1962 - 1969, 1974 - 1988, 2010 - 2012, 2019 };

        List<int> SpecialUnitList = new List<int> { 176581, 176920, 178008, 168326, 168969, };
#endregion

        #region Misc Checks
        private bool TargetAlive()
        {
            if (Aimsharp.CustomFunction("UnitIsDead") == 2)
                return true;

            return false;
        }
        #endregion

        #region CanCasts

        #endregion

        #region Debuffs

        #endregion

        #region Buffs

        #endregion

        #region Initializations
        private void InitializeSettings()
        {
            FiveLetters = GetString("First 5 Letters of the Addon:");
        }

        private void InitializeMacros()
        {
            //Auto Target
            Macros.Add("TargetEnemy", "/targetenemy");

            //Trinket
            Macros.Add("TopTrinket", "/use 13");
            Macros.Add("BotTrinket", "/use 14");

            //Healthstone
            Macros.Add("Healthstone", "/use Healthstone");

            //Phial
            Macros.Add("PhialofSerenity", "/use Phial of Serenity");

            //SpellQueueWindow
            Macros.Add("SetSpellQueueCvar", "/console SpellQueueWindow " + (Aimsharp.Latency + 100));

            //Spear
            Macros.Add("SpearofBastionP", "/cast [@player] Spear of Bastion");

            //Queues
            Macros.Add("StormBoltOff", "/" + FiveLetters + " StormBolt");
            Macros.Add("IntimidatingShoutOff", "/" + FiveLetters + " IntimidatingShout");
            Macros.Add("SpearofBastionOff", "/" + FiveLetters + " SpearofBastion");
            Macros.Add("DoorofShadowsOff", "/" + FiveLetters + " DoorofShadows");

            //CancelAura
            Macros.Add("CancelBladestorm", "/cancelaura Bladestorm");

        }

        private void InitializeSpells()
        {
            foreach (string Spell in m_SpellBook_General)
                Spellbook.Add(Spell);

            foreach (string Spell in m_SpellBook_Fury)
                Spellbook.Add(Spell);

            foreach (string Buff in m_BuffsList)
                Buffs.Add(Buff);

            foreach (string Buff in m_BloodlustBuffsList)
                Buffs.Add(Buff);

            foreach (string Debuff in m_DebuffsList)
                Debuffs.Add(Debuff);

            foreach (string Item in m_ItemsList)
                Items.Add(Item);

            foreach (string MacroCommand in m_IngameCommandsList)
                CustomCommands.Add(MacroCommand);
        }

        private void InitializeCustomLUAFunctions()
        {
            CustomFunctions.Add("HekiliID1", "local loading, finished = IsAddOnLoaded(\"Hekili\") \r\nif loading == true and finished == true then \r\n    local id=Hekili_GetRecommendedAbility(\"Primary\",1)\r\n\tif id ~= nil then\r\n\t\r\n    if id<0 then \r\n\t if id == -9 then return 999999 end   local spell = Hekili.Class.abilities[id]\r\n\t    if spell ~= nil and spell.item ~= nil then \r\n\t    \tid=spell.item\r\n\t\t    local topTrinketLink = GetInventoryItemLink(\"player\",13)\r\n\t\t    local bottomTrinketLink = GetInventoryItemLink(\"player\",14)\r\n\t\t    if topTrinketLink  ~= nil then\r\n                local trinketid = GetItemInfoInstant(topTrinketLink)\r\n                if trinketid ~= nil then\r\n\t\t\t        if trinketid == id then\r\n\t\t\t\t        return 1\r\n                    end\r\n\t\t\t    end\r\n\t\t    end\r\n\t\t    if bottomTrinketLink ~= nil then\r\n                local trinketid = GetItemInfoInstant(bottomTrinketLink)\r\n                if trinketid ~= nil then\r\n    \t\t\t    if trinketid == id then\r\n\t    \t\t\t    return 2\r\n                    end\r\n\t\t\t    end\r\n\t\t    end\r\n\t  end \r\n    end\r\n    return id\r\nend\r\nend\r\nreturn 0");

            CustomFunctions.Add("PhialCount", "local count = GetItemCount(177278) if count ~= nil then return count end return 0");

            CustomFunctions.Add("GetSpellQueueWindow", "local sqw = GetCVar(\"SpellQueueWindow\"); if sqw ~= nil then return tonumber(sqw); end return 0");

            CustomFunctions.Add("CooldownsToggleCheck", "local loading, finished = IsAddOnLoaded(\"Hekili\") if loading == true and finished == true then local cooldowns = Hekili:GetToggleState(\"cooldowns\") if cooldowns == true then return 1 else if cooldowns == false then return 2 end end end return 0");

            CustomFunctions.Add("UnitIsDead", "if UnitIsDead(\"target\") ~= nil and UnitIsDead(\"target\") == true then return 1 end; if UnitIsDead(\"target\") ~= nil and UnitIsDead(\"target\") == false then return 2 end; return 0");

            CustomFunctions.Add("HekiliWait", "if HekiliDisplayPrimary.Recommendations[1].wait ~= nil and HekiliDisplayPrimary.Recommendations[1].wait * 1000 > 0 then return math.floor(HekiliDisplayPrimary.Recommendations[1].wait * 1000) end return 0");

            CustomFunctions.Add("TargetIsMouseover", "if UnitExists('mouseover') and UnitIsDead('mouseover') ~= true and UnitExists('target') and UnitIsDead('target') ~= true and UnitIsUnit('mouseover', 'target') then return 1 end; return 0");

            CustomFunctions.Add("IsTargeting", "if SpellIsTargeting()\r\n then return 1\r\n end\r\n return 0");

            CustomFunctions.Add("IsRMBDown", "local MBD = 0 local isDown = IsMouseButtonDown(\"RightButton\") if isDown == true then MBD = 1 end return MBD");
        }
        #endregion

        public override void LoadSettings()
        {
            Settings.Add(new Setting("First 5 Letters of the Addon:", "xxxxx"));
            Settings.Add(new Setting("Race:", m_RaceList, "orc"));
            Settings.Add(new Setting("Ingame World Latency:", 1, 200, 50));
            Settings.Add(new Setting(" "));
            Settings.Add(new Setting("Use Trinkets on CD, dont wait for Hekili:", false));
            Settings.Add(new Setting("Auto Healthstone @ HP%", 0, 100, 25));
            Settings.Add(new Setting("Auto Phial of Serenity @ HP%", 0, 100, 35));
            Settings.Add(new Setting("Kicks/Interrupts"));
            Settings.Add(new Setting("Kick at milliseconds remaining", 50, 1500, 500));
            Settings.Add(new Setting("Kick channels after milliseconds", 50, 1500, 500));
            Settings.Add(new Setting("General"));
            Settings.Add(new Setting("Auto Start Combat:", true));
            Settings.Add(new Setting("Auto Victory Rush @ HP%", 0, 100, 90));
            Settings.Add(new Setting("Auto Ignore Pain @ HP%", 0, 100, 35));
            Settings.Add(new Setting("Auto Rallying Cry @ HP%", 0, 100, 25));
            Settings.Add(new Setting("Auto Enraged Regeneration @ HP%", 0, 100, 10));
            Settings.Add(new Setting("Spear of Bastion Cast:", m_CastingList, "Player"));
            Settings.Add(new Setting("Door of Shadows Cast:", m_CastingList, "Manual"));
            Settings.Add(new Setting(" "));
            Settings.Add(new Setting("Debug:", false));

        }

        public override void Initialize()
        {

            if (GetCheckBox("Debug:") == true)
            {
                Aimsharp.DebugMode();
            }

            Aimsharp.Latency = GetSlider("Ingame World Latency:");
            Aimsharp.QuickDelay = 150;
            Aimsharp.SlowDelay = 350;

            Aimsharp.PrintMessage("Snoogens PVE - Warrior Fury", Color.Yellow);
            Aimsharp.PrintMessage("This rotation requires the Hekili Addon", Color.Red);
            Aimsharp.PrintMessage("Hekili > Toggles > Unbind everything", Color.Brown);
            Aimsharp.PrintMessage("Hekili > Toggles > Bind \"Cooldowns\" & \"Display Mode\"", Color.Brown);
            Aimsharp.PrintMessage("-----", Color.Black);
            Aimsharp.PrintMessage("- General -", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx NoInterrupts - Disables Interrupts", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx StormBolt - Casts Storm Bolt @ Target on the next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx IntimidatingShout - Casts Intimidating Shout @ Target on the next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx SpearofBastion - Casts Spear of Bastion @ Manual/Player on the next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx DoorofShadows - Casts Spear of Bastion @ Manual on the next GCD", Color.Yellow);
            Aimsharp.PrintMessage("-----", Color.Black);

            #region Racial Spells
            if (GetDropDown("Race:") == "draenei")
            {
                Spellbook.Add("Gift of the Naaru"); //28880
            }

            if (GetDropDown("Race:") == "dwarf")
            {
                Spellbook.Add("Stoneform"); //20594
            }

            if (GetDropDown("Race:") == "gnome")
            {
                Spellbook.Add("Escape Artist"); //20589
            }

            if (GetDropDown("Race:") == "human")
            {
                Spellbook.Add("Will to Survive"); //59752
            }

            if (GetDropDown("Race:") == "lightforgeddraenei")
            {
                Spellbook.Add("Light's Judgment"); //255647
            }

            if (GetDropDown("Race:") == "darkirondwarf")
            {
                Spellbook.Add("Fireblood"); //265221
            }

            if (GetDropDown("Race:") == "goblin")
            {
                Spellbook.Add("Rocket Barrage"); //69041
            }

            if (GetDropDown("Race:") == "tauren")
            {
                Spellbook.Add("War Stomp"); //20549
            }

            if (GetDropDown("Race:") == "troll")
            {
                Spellbook.Add("Berserking"); //26297
            }

            if (GetDropDown("Race:") == "scourge")
            {
                Spellbook.Add("Will of the Forsaken"); //7744
            }

            if (GetDropDown("Race:") == "nightborne")
            {
                Spellbook.Add("Arcane Pulse"); //260364
            }

            if (GetDropDown("Race:") == "highmountaintauren")
            {
                Spellbook.Add("Bull Rush"); //255654
            }

            if (GetDropDown("Race:") == "magharorc")
            {
                Spellbook.Add("Ancestral Call"); //274738
            }

            if (GetDropDown("Race:") == "vulpera")
            {
                Spellbook.Add("Bag of Tricks"); //312411
            }

            if (GetDropDown("Race:") == "orc")
            {
                Spellbook.Add("Blood Fury"); //20572, 33702, 33697
            }

            if (GetDropDown("Race:") == "bloodelf")
            {
                Spellbook.Add("Arcane Torrent"); //28730, 25046, 50613, 69179, 80483, 129597
            }

            if (GetDropDown("Race:") == "nightelf")
            {
                Spellbook.Add("Shadowmeld"); //58984
            }
            #endregion

            InitializeSettings();

            InitializeMacros();

            InitializeSpells();

            InitializeCustomLUAFunctions();
        }

        public override bool CombatTick()
        {
            #region Declarations
            int SpellID1 = Aimsharp.CustomFunction("HekiliID1");
            int Wait = Aimsharp.CustomFunction("HekiliWait");

            bool NoInterrupts = Aimsharp.IsCustomCodeOn("NoInterrupts");

            bool Debug = GetCheckBox("Debug:") == true;
            bool UseTrinketsCD = GetCheckBox("Use Trinkets on CD, dont wait for Hekili:") == true;
            int CooldownsToggle = Aimsharp.CustomFunction("CooldownsToggleCheck");

            bool IsInterruptable = Aimsharp.IsInterruptable("target");
            int CastingRemaining = Aimsharp.CastingRemaining("target");
            int CastingElapsed = Aimsharp.CastingElapsed("target");
            bool IsChanneling = Aimsharp.IsChanneling("target");
            int KickValue = GetSlider("Kick at milliseconds remaining");
            int KickChannelsAfter = GetSlider("Kick channels after milliseconds");

            bool Enemy = Aimsharp.TargetIsEnemy();
            int EnemiesInMelee = Aimsharp.EnemiesInMelee();
            bool Moving = Aimsharp.PlayerIsMoving();
            int PlayerHP = Aimsharp.Health("player");
            bool MeleeRange = Aimsharp.Range("target") <= 6;
            bool TargetInCombat = Aimsharp.InCombat("target") || SpecialUnitList.Contains(Aimsharp.UnitID("target")) || !InstanceIDList.Contains(Aimsharp.GetMapID());
            #endregion

            #region SpellQueueWindow
            if (Aimsharp.CustomFunction("GetSpellQueueWindow") != (Aimsharp.Latency + 100))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Setting SQW to: " + (Aimsharp.Latency + 100), Color.Purple);
                }
                Aimsharp.Cast("SetSpellQueueCvar");
            }
            #endregion

            #region Pause Checks
            if (Aimsharp.CastingID("player") > 0 || Aimsharp.IsChanneling("player"))
            {
                return false;
            }

            if (Aimsharp.CustomFunction("IsTargeting") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("SpearofBastion") && Aimsharp.SpellCooldown("Spear of Bastion") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("DoorofShadows") && Aimsharp.SpellCooldown("Door of Shadows") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }
            #endregion

            #region CancelLogic
            if (SpellID1 == 999999 && Aimsharp.HasBuff("Bladestorm", "player", true))
            {
                Aimsharp.Cast("CancelBladestorm");
                return true;
            }
            #endregion

            #region Interrupts
            if (!NoInterrupts && (Aimsharp.UnitID("target") != 168105 || Torghast_InnerFlame.Contains(Aimsharp.CastingID("target"))) && (Aimsharp.UnitID("target") != 157571 || Torghast_InnerFlame.Contains(Aimsharp.CastingID("target"))))
            {
                if (Aimsharp.CanCast("Pummel", "target", true, true))
                {
                    if (IsInterruptable && !IsChanneling && CastingRemaining < KickValue)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Target Casting ID: " + Aimsharp.CastingID("target") + ", Interrupting", Color.Purple);
                        }
                        Aimsharp.Cast("Pummel", true);
                        return true;
                    }
                }

                if (Aimsharp.CanCast("Pummel", "target", true, true))
                {
                    if (IsInterruptable && IsChanneling && CastingElapsed > KickChannelsAfter)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Target Channeling ID: " + Aimsharp.CastingID("target") + ", Interrupting", Color.Purple);
                        }
                        Aimsharp.Cast("Pummel", true);
                        return true;
                    }
                }
            }
            #endregion

            #region Auto Spells and Items
            //Auto Healthstone
            if (Aimsharp.CanUseItem("Healthstone", false) && Aimsharp.ItemCooldown("Healthstone") == 0)
            {
                if (Aimsharp.Health("player") <= GetSlider("Auto Healthstone @ HP%"))
                {
                    if (Debug)
                    {
                        Aimsharp.PrintMessage("Using Healthstone - Player HP% " + Aimsharp.Health("player") + " due to setting being on HP% " + GetSlider("Auto Healthstone @ HP%"), Color.Purple);
                    }
                    Aimsharp.Cast("Healthstone");
                    return true;
                }
            }

            //Auto Phial of Serenity
            if (Aimsharp.CanUseItem("Phial of Serenity", false) && Aimsharp.ItemCooldown("Phial of Serenity") == 0)
            {
                if (Aimsharp.Health("player") <= GetSlider("Auto Phial of Serenity @ HP%"))
                {
                    if (Debug)
                    {
                        Aimsharp.PrintMessage("Using Phial of Serenity - Player HP% " + Aimsharp.Health("player") + " due to setting being on HP% " + GetSlider("Auto Phial of Serenity @ HP%"), Color.Purple);
                    }
                    Aimsharp.Cast("PhialofSerenity");
                    return true;
                }
            }

            //Auto Rallying Cry
            if (Aimsharp.CanCast("Rallying Cry", "player", false, true))
            {
                if (PlayerHP <= GetSlider("Auto Rallying Cry @ HP%"))
                {
                    Aimsharp.Cast("Rallying Cry");
                    return true;
                }
            }

            //Auto Ignore Pain
            if (Aimsharp.CanCast("Ignore Pain", "player", false, true))
            {
                if (PlayerHP <= GetSlider("Auto Ignore Pain @ HP%"))
                {
                    Aimsharp.Cast("Ignore Pain", true);
                    return true;
                }
            }

            //Auto Enraged Regeneration       
            if (Aimsharp.CanCast("Enraged Regeneration", "player", false, true))
            {
                if (PlayerHP <= GetSlider("Auto Enraged Regeneration @ HP%"))
                {
                    Aimsharp.Cast("Enraged Regeneration");
                    return true;
                }
            }

            //Auto Victory Rush
            if (Aimsharp.CanCast("Victory Rush", "target", true, true))
            {
                if (PlayerHP <= GetSlider("Auto Victory Rush @ HP%"))
                {
                    Aimsharp.Cast("Victory Rush");
                    return true;
                }
            }
            #endregion

            #region Queues
            //Queues
            //Queue StormBolt
            bool StormBolt = Aimsharp.IsCustomCodeOn("StormBolt");
            if (Aimsharp.SpellCooldown("Storm Bolt") - Aimsharp.GCD() > 2000 && StormBolt && Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                Aimsharp.Cast("StormBoltOff");
                return true;
            }

            if (StormBolt && Aimsharp.CanCast("Storm Bolt", "target", true, true))
            {
                Aimsharp.PrintMessage("Queued Storm Bolt");
                Aimsharp.Cast("Storm Bolt");
                return true;
            }

            //Queue Intimidating Shout
            bool IntimidatingShout = Aimsharp.IsCustomCodeOn("IntimidatingShout");
            if (Aimsharp.SpellCooldown("Intimidating Shout") - Aimsharp.GCD() > 2000 && IntimidatingShout && Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                Aimsharp.Cast("IntimidatingShoutOff");
                return true;
            }

            if (IntimidatingShout && Aimsharp.CanCast("Intimidating Shout", "target", true, true))
            {
                Aimsharp.PrintMessage("Queued Intimidating Shout");
                Aimsharp.Cast("Intimidating Shout");
                return true;
            }

            string SpearofBastionCast = GetDropDown("Spear of Bastion Cast:");
            bool SpearofBastion = Aimsharp.IsCustomCodeOn("SpearofBastion");
            if (Aimsharp.SpellCooldown("Spear of Bastion") - Aimsharp.GCD() > 2000 && SpearofBastion)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Spear of Bastion Queue", Color.Purple);
                }
                Aimsharp.Cast("SpearofBastionOff");
                return true;
            }

            if (SpearofBastion && Aimsharp.CanCast("Spear of Bastion", "player", false, true))
            {
                switch (SpearofBastionCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Spear of Bastion - " + SpearofBastionCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Spear of Bastion");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Spear of Bastion - " + SpearofBastionCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("SpearofBastionP");
                        return true;
                }
            }

            bool DoorofShadows = Aimsharp.IsCustomCodeOn("DoorofShadows");
            if ((Aimsharp.SpellCooldown("Door of Shadows") - Aimsharp.GCD() > 2000 || Moving) && DoorofShadows)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Door of Shadows Queue", Color.Purple);
                }
                Aimsharp.Cast("DoorofShadowsOff");
                return true;
            }

            if (DoorofShadows && Aimsharp.CanCast("Door of Shadows", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Door of Shadows - Queue", Color.Purple);
                }
                Aimsharp.Cast("Door of Shadows");
                return true;
            }
            #endregion

            #region Auto Target
            //Auto Target
            if ((!Enemy || Enemy && !TargetAlive()) && EnemiesInMelee > 0)
            {
                System.Threading.Thread.Sleep(50);
                Aimsharp.Cast("TargetEnemy");
                System.Threading.Thread.Sleep(50);
                return true;
            }
            #endregion

            if (Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                if (Wait <= 200)
                {
                    #region Trinkets
                    //Trinkets
                    if (CooldownsToggle == 1 && UseTrinketsCD && Aimsharp.CanUseTrinket(0) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Using Top Trinket on Cooldown", Color.Purple);
                        }
                        Aimsharp.Cast("TopTrinket");
                        return true;
                    }

                    if (CooldownsToggle == 2 && UseTrinketsCD && Aimsharp.CanUseTrinket(1) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Using Bot Trinket on Cooldown", Color.Purple);
                        }
                        Aimsharp.Cast("BotTrinket");
                        return true;
                    }

                    if (SpellID1 == 1 && Aimsharp.CanUseTrinket(0) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Using Top Trinket", Color.Purple);
                        }
                        Aimsharp.Cast("TopTrinket");
                        return true;
                    }

                    if (SpellID1 == 2 && Aimsharp.CanUseTrinket(1) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Using Bot Trinket", Color.Purple);
                        }
                        Aimsharp.Cast("BotTrinket");
                        return true;
                    }
                    #endregion

                    #region Racials
                    //Racials
                    if (SpellID1 == 28880 && Aimsharp.CanCast("Gift of the Naaru", "player", true, true) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Gift of the Naaru - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Gift of the Naaru");
                        return true;
                    }

                    if (SpellID1 == 20594 && Aimsharp.CanCast("Stoneform", "player", true, true) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Stoneform - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Stoneform");
                        return true;
                    }

                    if (SpellID1 == 20589 && Aimsharp.CanCast("Escape Artist", "player", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Escape Artist - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Escape Artist");
                        return true;
                    }

                    if (SpellID1 == 59752 && Aimsharp.CanCast("Will to Survive", "player", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Will to Survive - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Will to Survive");
                        return true;
                    }

                    if (SpellID1 == 255647 && Aimsharp.CanCast("Light's Judgment", "player", true, true) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Light's Judgment - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Light's Judgment");
                        return true;
                    }

                    if (SpellID1 == 265221 && Aimsharp.CanCast("Fireblood", "player", true, true) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Fireblood - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Fireblood");
                        return true;
                    }

                    if (SpellID1 == 69041 && Aimsharp.CanCast("Rocket Barrage", "player", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Rocket Barrage - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Rocket Barrage");
                        return true;
                    }

                    if (SpellID1 == 20549 && Aimsharp.CanCast("War Stomp", "player", true, true) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting War Stomp - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("War Stomp");
                        return true;
                    }

                    if (SpellID1 == 7744 && Aimsharp.CanCast("Will of the Forsaken", "player", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Will of the Forsaken - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Will of the Forsaken");
                        return true;
                    }

                    if (SpellID1 == 260364 && Aimsharp.CanCast("Arcane Pulse", "player", true, true) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Arcane Pulse - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Arcane Pulse");
                        return true;
                    }

                    if (SpellID1 == 255654 && Aimsharp.CanCast("Bull Rush", "player", true, true) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Bull Rush - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Bull Rush");
                        return true;
                    }

                    if (SpellID1 == 312411 && Aimsharp.CanCast("Bag of Tricks", "player", true, true) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Bag of Tricks - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Bag of Tricks");
                        return true;
                    }

                    if ((SpellID1 == 20572 || SpellID1 == 33702 || SpellID1 == 33697) && Aimsharp.CanCast("Blood Fury", "player", true, true) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Blood Fury - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Blood Fury");
                        return true;
                    }

                    if (SpellID1 == 26297 && Aimsharp.CanCast("Berserking", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Berserking - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Berserking");
                        return true;
                    }

                    if (SpellID1 == 274738 && Aimsharp.CanCast("Ancestral Call", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Ancestral Call - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Ancestral Call");
                        return true;
                    }

                    if ((SpellID1 == 28730 || SpellID1 == 25046 || SpellID1 == 50613 || SpellID1 == 69179 || SpellID1 == 80483 || SpellID1 == 129597) && Aimsharp.CanCast("Arcane Torrent", "player", true, false) && MeleeRange)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Arcane Torrent - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Arcane Torrent");
                        return true;
                    }

                    if (SpellID1 == 58984 && Aimsharp.CanCast("Shadowmeld", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Shadowmeld - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Shadowmeld");
                        return true;
                    }
                    #endregion

                    #region Covenants
                    //Covenants
                    if ((SpellID1 == 317349 || SpellID1 == 317485 || SpellID1 == 330334 || SpellID1 == 330325 || SpellID1 == 317320 || Aimsharp.HasBuff("Voracious Culling Blade", "player", true)) && Aimsharp.CanCast("Condemn", "target", true, true))
                    {
                        Aimsharp.Cast("Condemn");
                        return true;
                    }
                    if (SpellID1 == 325886 && Aimsharp.CanCast("Ancient Aftershock", "player", false, true))
                    {
                        Aimsharp.Cast("Ancient Aftershock");
                        return true;
                    }
                    if (SpellID1 == 324143 && Aimsharp.CanCast("Conqueror's Banner", "player", false, true))
                    {
                        Aimsharp.Cast("Conqueror's Banner");
                        return true;
                    }

                    if (SpellID1 == 307865 && Aimsharp.CanCast("Spear of Bastion", "player", false, true))
                    {
                        switch (SpearofBastionCast)
                        {
                            case "Manual":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Spear of Bastion - " + SpearofBastionCast + " - Queue", Color.Purple);
                                }
                                Aimsharp.Cast("Spear of Bastion");
                                return true;
                            case "Player":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Spear of Bastion - " + SpearofBastionCast + " - Queue", Color.Purple);
                                }
                                Aimsharp.Cast("SpearofBastionP");
                                return true;
                        }
                    }

                    //Auto Fleshcraft
                    if (SpellID1 == 324631 && Aimsharp.CanCast("Fleshcraft", "player", false, true) && !Moving)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Fleshcraft - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Fleshcraft");
                        return true;
                    }
                    #endregion

                    #region General Spells - NoGCD
                    //Class Spells
                    //Instant [GCD FREE]
                    if (SpellID1 == 190456 && Aimsharp.CanCast("Ignore Pain", "player", false, true))
                    {
                        Aimsharp.Cast("Ignore Pain", true);
                        return true;
                    }
                    if (SpellID1 == 18499 && Aimsharp.CanCast("Berserker Rage", "player", false, true))
                    {
                        Aimsharp.Cast("Berserker Rage", true);
                        return true;
                    }
                    if (SpellID1 == 355 && Aimsharp.CanCast("Taunt", "target", true, true))
                    {
                        Aimsharp.Cast("Taunt", true);
                        return true;
                    }
                    if (SpellID1 == 6552 && Aimsharp.CanCast("Pummel", "target", true, true))
                    {
                        Aimsharp.Cast("Pummel", true);
                        return true;
                    }
                    #endregion

                    #region General Spells - Player GCD
                    //Instant [GCD]
                    ///Player
                    if (SpellID1 == 97462 && Aimsharp.CanCast("Rallying Cry", "player", false, true))
                    {
                        Aimsharp.Cast("Rallying Cry");
                        return true;
                    }

                    if (SpellID1 == 6673 && Aimsharp.CanCast("Battle Shout", "player", false, true))
                    {
                        Aimsharp.Cast("Battle Shout");
                        return true;
                    }

                    if ((SpellID1 == 1680 || SpellID1 == 190411) && Aimsharp.CanCast("Whirlwind", "player", false, true) && MeleeRange)
                    {
                        Aimsharp.Cast("Whirlwind");
                        return true;
                    }

                    if ((SpellID1 == 227847 || SpellID1 == 46924) && Aimsharp.CanCast("Bladestorm", "player", false, true) && MeleeRange)
                    {
                        Aimsharp.Cast("Bladestorm");
                        return true;
                    }

                    if (SpellID1 == 23920 && Aimsharp.CanCast("Spell Reflection", "player", false, true))
                    {
                        Aimsharp.Cast("Spell Reflection");
                        return true;
                    }
                    #endregion

                    #region General Spells - Target GCD
                    ///Target
                    if (SpellID1 == 1464 && Aimsharp.CanCast("Slam", "target", true, true))
                    {
                        Aimsharp.Cast("Slam");
                        return true;
                    }

                    if ((SpellID1 == 281000 || SpellID1 == 163201 || SpellID1 == 317349 || SpellID1 == 317485 || SpellID1 == 330334 || SpellID1 == 330325 || SpellID1 == 317320 || SpellID1 == 280735 || SpellID1 == 5308) && Aimsharp.CanCast("Execute", "target", true, true))
                    {
                        Aimsharp.Cast("Execute");
                        return true;
                    }
                    if (SpellID1 == 64382 && Aimsharp.CanCast("Shattering Throw", "target", true, true))
                    {
                        Aimsharp.Cast("Shattering Throw");
                        return true;
                    }
                    if (SpellID1 == 34428 && Aimsharp.CanCast("Victory Rush", "target", true, true))
                    {
                        Aimsharp.Cast("Victory Rush");
                        return true;
                    }

                    if (SpellID1 == 100 && Aimsharp.CanCast("Charge", "target", true, true))
                    {
                        Aimsharp.Cast("Charge");
                        return true;
                    }

                    if (SpellID1 == 107570 && Aimsharp.CanCast("Storm Bolt", "target", true, true))
                    {
                        Aimsharp.Cast("Storm Bolt");
                        return true;
                    }

                    if (SpellID1 == 202168 && Aimsharp.CanCast("Impending Victory", "target", true, true))
                    {
                        Aimsharp.Cast("Impending Victory");
                        return true;
                    }

                    if (SpellID1 == 1715 && Aimsharp.CanCast("Hamstring", "target", true, true))
                    {
                        Aimsharp.Cast("Hamstring");
                        return true;
                    }
                    if (SpellID1 == 57755 && Aimsharp.CanCast("Heroic Throw", "target", true, true))
                    {
                        Aimsharp.Cast("Heroic Throw");
                        return true;
                    }
                    #endregion

                    #region Fury Spells - Player GCD
                    ////Player
                    if (SpellID1 == 118000 && Aimsharp.CanCast("Dragon Roar", "player", false, true) && MeleeRange)
                    {
                        Aimsharp.Cast("Dragon Roar");
                        return true;
                    }

                    if (SpellID1 == 183364 && Aimsharp.CanCast("Enraged Regenerationr", "player", false, true))
                    {
                        Aimsharp.Cast("Enraged Regeneration");
                        return true;
                    }

                    if (SpellID1 == 1719 && Aimsharp.CanCast("Recklessness", "player", false, true) && MeleeRange)
                    {
                        Aimsharp.Cast("Recklessness", true);
                        return true;
                    }
                    #endregion

                    #region Fury Spells - Target GCD
                    ////Target

                    if (SpellID1 == 315720 && Aimsharp.CanCast("Onslaught", "target", true, true))
                    {
                        Aimsharp.Cast("Onslaught");
                        return true;
                    }

                    if (SpellID1 == 280772 && Aimsharp.CanCast("Siegebreaker", "target", true, true))
                    {
                        Aimsharp.Cast("Siegebreaker");
                        return true;
                    }

                    if ((SpellID1 == 23881 || SpellID1 == 335096) && Aimsharp.CanCast("Bloodthirst", "target", true, true))
                    {
                        Aimsharp.Cast("Bloodthirst");
                        return true;
                    }

                    if (SpellID1 == 184367 && Aimsharp.CanCast("Rampage", "target", true, true))
                    {
                        Aimsharp.Cast("Rampage");
                        return true;
                    }

                    if ((SpellID1 == 85288 || SpellID1 == 335097) && Aimsharp.CanCast("Raging Blow", "target", true, true))
                    {
                        Aimsharp.Cast("Raging Blow");
                        return true;
                    }
                    #endregion

                }

            }
            return false;
        }

        public override bool OutOfCombatTick()
        {
            #region Declarations
            int SpellID1 = Aimsharp.CustomFunction("HekiliID1");
            int PhialCount = Aimsharp.CustomFunction("PhialCount");
            bool Debug = GetCheckBox("Debug:") == true;
            bool Moving = Aimsharp.PlayerIsMoving();
            bool TargetInCombat = Aimsharp.InCombat("target") || SpecialUnitList.Contains(Aimsharp.UnitID("target")) || !InstanceIDList.Contains(Aimsharp.GetMapID());
            #endregion

            #region SpellQueueWindow
            if (Aimsharp.CustomFunction("GetSpellQueueWindow") != (Aimsharp.Latency + 100))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Setting SQW to: " + (Aimsharp.Latency + 100), Color.Purple);
                }
                Aimsharp.Cast("SetSpellQueueCvar");
            }
            #endregion

            #region Pause Checks
            if (Aimsharp.CastingID("player") > 0 || Aimsharp.IsChanneling("player"))
            {
                return false;
            }

            if (Aimsharp.CustomFunction("IsTargeting") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("SpearofBastion") && Aimsharp.SpellCooldown("Spear of Bastion") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("DoorofShadows") && Aimsharp.SpellCooldown("Door of Shadows") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }
            #endregion

            #region Queues
            //Queues
            //Queue StormBolt
            bool StormBolt = Aimsharp.IsCustomCodeOn("StormBolt");
            if (Aimsharp.SpellCooldown("Storm Bolt") - Aimsharp.GCD() > 2000 && StormBolt && Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                Aimsharp.Cast("StormBoltOff");
                return true;
            }

            if (StormBolt && Aimsharp.CanCast("Storm Bolt", "target", true, true))
            {
                Aimsharp.PrintMessage("Queued Storm Bolt");
                Aimsharp.Cast("Storm Bolt");
                return true;
            }

            //Queue Intimidating Shout
            bool IntimidatingShout = Aimsharp.IsCustomCodeOn("IntimidatingShout");
            if (Aimsharp.SpellCooldown("Intimidating Shout") - Aimsharp.GCD() > 2000 && IntimidatingShout && Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                Aimsharp.Cast("IntimidatingShoutOff");
                return true;
            }

            if (IntimidatingShout && Aimsharp.CanCast("Intimidating Shout", "target", true, true))
            {
                Aimsharp.PrintMessage("Queued Intimidating Shout");
                Aimsharp.Cast("Intimidating Shout");
                return true;
            }

            string SpearofBastionCast = GetDropDown("Spear of Bastion Cast:");
            bool SpearofBastion = Aimsharp.IsCustomCodeOn("SpearofBastion");
            if (Aimsharp.SpellCooldown("Spear of Bastion") - Aimsharp.GCD() > 2000 && SpearofBastion)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Spear of Bastion Queue", Color.Purple);
                }
                Aimsharp.Cast("SpearofBastionOff");
                return true;
            }

            if (SpearofBastion && Aimsharp.CanCast("Spear of Bastion", "player", false, true))
            {
                switch (SpearofBastionCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Spear of Bastion - " + SpearofBastionCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Spear of Bastion");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Spear of Bastion - " + SpearofBastionCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("SpearofBastionP");
                        return true;
                }
            }

            bool DoorofShadows = Aimsharp.IsCustomCodeOn("DoorofShadows");
            if ((Aimsharp.SpellCooldown("Door of Shadows") - Aimsharp.GCD() > 2000 || Moving) && DoorofShadows)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Door of Shadows Queue", Color.Purple);
                }
                Aimsharp.Cast("DoorofShadowsOff");
                return true;
            }

            if (DoorofShadows && Aimsharp.CanCast("Door of Shadows", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Door of Shadows - Queue", Color.Purple);
                }
                Aimsharp.Cast("Door of Shadows");
                return true;
            }
            #endregion

            #region Out of Combat Spells
            //Auto Fleshcraft
            if (SpellID1 == 324631 && Aimsharp.CanCast("Fleshcraft", "player", false, true) && !Moving)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Fleshcraft - " + SpellID1, Color.Purple);
                }
                Aimsharp.Cast("Fleshcraft");
                return true;
            }

            //Auto Battle Shout
            if (SpellID1 == 6673 && Aimsharp.CanCast("Battle Shout", "player", false, true))
            {
                Aimsharp.Cast("Battle Shout");
                return true;
            }

            //Auto Call Steward
            if (PhialCount <= 0 && Aimsharp.CanCast("Summon Steward", "player") && !Aimsharp.HasBuff("Stealth", "player", true) && Aimsharp.GetMapID() != 2286 && Aimsharp.GetMapID() != 1666 && Aimsharp.GetMapID() != 1667 && Aimsharp.GetMapID() != 1668 && Aimsharp.CastingID("player") == 0)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Summon Steward due to Phial Count being: " + PhialCount, Color.Purple);
                }
                Aimsharp.Cast("Summon Steward");
                return true;
            }
            #endregion

            #region Auto Combat
            //Auto Combat
            if (GetCheckBox("Auto Start Combat:") == true && Aimsharp.TargetIsEnemy() && TargetAlive() && Aimsharp.Range("target") <= 6 && TargetInCombat)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Starting Combat from Out of Combat", Color.Purple);
                }
                return CombatTick();
            }
            #endregion

            return false;
        }

    }
}