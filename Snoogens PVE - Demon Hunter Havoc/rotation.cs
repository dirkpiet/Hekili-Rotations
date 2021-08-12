using System.Linq;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Drawing;
using AimsharpWow.API;

namespace AimsharpWow.Modules
{
    public class SnoogensPVEDemonHunterHavoc : Rotation
    {
        #region Variables
        string FiveLetters;
        #endregion

        #region Lists
        //Lists
        private List<string> m_IngameCommandsList = new List<string> { "DoorofShadows", "NoInterrupts", "NoCycle", "NoMovement", "ChaosNova", "Imprison", "Darkness", "FelEruption", "FelRush", "Felblade", };
        private List<string> m_DebuffsList = new List<string> { "Imprison", };
        private List<string> m_BuffsList = new List<string> { "Netherwalk", };
        private List<string> m_BloodlustBuffsList = new List<string> { "Bloodlust", "Heroism", "Time Warp", "Primal Rage", "Drums of Rage" };
        private List<string> m_ItemsList = new List<string> { "Phial of Serenity", "Healthstone" };

        private List<string> m_SpellBook_General = new List<string> {
            //Covenants
            "Elysian Decree", "Fodder to the Flame", "The Hunt", "Sinful Brand",

            "Door of Shadows",
            "Fleshcraft",
            "Summon Steward",

            //Interrupt
            "Disrupt",

            //General
            "Immolation Aura", "Fel Rush", "Blade Dance", "Metamorphosis",
            "Consume Magic", "Throw Glaive", "Torment", "Chaos Nova", "Vengeful Retreat",
            "Chaos Strike", "Darkness", "Demon's Bite", "Eye Beam", "Blur",
            "Death Sweep", "Annihilation", "Felblade", "Glaive Tempest",
            "Netherwalk", "Essence Break", "Fel Eruption", "Fel Barrage",
            "Imprison",

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

            Macros.Add("MetamorphosisP", "/cast [@player] Metamorphosis");

            Macros.Add("ChaosNovaOff", "/" + FiveLetters + " ChaosNova");
            Macros.Add("ImprisonOff", "/" + FiveLetters + " Imprison");
            Macros.Add("DarknessOff", "/" + FiveLetters + " Darkness");
            Macros.Add("FelEruptionOff", "/" + FiveLetters + " FelEruption");
            Macros.Add("FelRushOff", "/" + FiveLetters + " FelRush");
            Macros.Add("FelbladeOff", "/" + FiveLetters + " Felblade");

        }

        private void InitializeSpells()
        {
            foreach (string Spell in m_SpellBook_General)
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

            CustomFunctions.Add("HekiliCycle", "if HekiliDisplayPrimary.Recommendations[1].indicator ~= nil and HekiliDisplayPrimary.Recommendations[1].indicator == 'cycle' then return 1 end return 0");

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
            Settings.Add(new Setting("Suggest but don't cast Movement Based Abilities:", false));
            Settings.Add(new Setting("Auto Darkness @ HP%", 0, 100, 40));
            Settings.Add(new Setting("Auto Blur @ HP%", 0, 100, 15));
            Settings.Add(new Setting("Auto Netherwalk @ HP%", 0, 100, 5));
            Settings.Add(new Setting("Misc"));
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

            Aimsharp.PrintMessage("Snoogens PVE - Demon Hunter Havoc", Color.Yellow);
            Aimsharp.PrintMessage("This rotation requires the Hekili Addon", Color.Red);
            Aimsharp.PrintMessage("Hekili > Toggles > Unbind everything", Color.Brown);
            Aimsharp.PrintMessage("Hekili > Toggles > Bind \"Cooldowns\" & \"Display Mode\"", Color.Brown);
            Aimsharp.PrintMessage("-----", Color.Black);
            Aimsharp.PrintMessage("- General -", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx NoInterrupts - Disables Interrupts", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx NoCycle - Disables Target Cycle", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx ChaosNova - Casts Chaos Nova @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx FelEruption - Casts Fel Eruption @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Darkness - Casts Darkness @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Imprison - Casts Imprison @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx FelRush - Casts Fel Rush @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Felblade - Casts Felblade @ next GCD", Color.Yellow);
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
            bool NoCycle = Aimsharp.IsCustomCodeOn("NoCycle");

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
            #endregion

            #region Interrupts
            if (!NoInterrupts && (Aimsharp.UnitID("target") != 168105 || Torghast_InnerFlame.Contains(Aimsharp.CastingID("target"))) && (Aimsharp.UnitID("target") != 157571 || Torghast_InnerFlame.Contains(Aimsharp.CastingID("target"))))
            {
                if (Aimsharp.CanCast("Disrupt", "target", true, true))
                {
                    if (IsInterruptable && !IsChanneling && CastingRemaining < KickValue)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Target Casting ID: " + Aimsharp.CastingID("target") + ", Interrupting", Color.Purple);
                        }
                        Aimsharp.Cast("Disrupt", true);
                        return true;
                    }
                }

                if (Aimsharp.CanCast("Disrupt", "target", true, true))
                {
                    if (IsInterruptable && IsChanneling && CastingElapsed > KickChannelsAfter)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Target Channeling ID: " + Aimsharp.CastingID("target") + ", Interrupting", Color.Purple);
                        }
                        Aimsharp.Cast("Disrupt", true);
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

            //Auto Darkness
            if (Aimsharp.CanCast("Darkness", "player", false, true) && !Aimsharp.HasBuff("Netherwalk", "player", true))
            {
                if (PlayerHP <= GetSlider("Auto Darkness @ HP%"))
                {
                    Aimsharp.Cast("Darkness");
                    return true;
                }
            }

            //Auto Blur
            if (Aimsharp.CanCast("Blur", "player", false, true) && !Aimsharp.HasBuff("Netherwalk", "player", true))
            {
                if (PlayerHP <= GetSlider("Auto Blur @ HP%"))
                {
                    Aimsharp.Cast("Blur");
                    return true;
                }
            }

            //Auto Netherwalk
            if (Aimsharp.CanCast("Netherwalk", "player", false, true))
            {
                if (PlayerHP <= GetSlider("Auto Netherwalk @ HP%"))
                {
                    Aimsharp.Cast("Netherwalk");
                    return true;
                }
            }
            #endregion

            #region Queues
            //Queues
            bool FelRush = Aimsharp.IsCustomCodeOn("FelRush");
            //Queue Fel Rush
            if (FelRush && (Aimsharp.SpellCooldown("Fel Rush") - Aimsharp.GCD() > 1000 && Aimsharp.SpellCharges("Fel Rush") == 0 || Aimsharp.RechargeTime("Fel Rush") > 9500 && Aimsharp.SpellCharges("Fel Rush") == 1))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Fel Rush queue toggle", Color.Purple);
                }
                Aimsharp.Cast("FelRushOff");
                return true;
            }

            if (FelRush && Aimsharp.CanCast("Fel Rush", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Fel Rush through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Fel Rush");
                return true;
            }

            bool Felblade = Aimsharp.IsCustomCodeOn("Felblade");
            //Queue Felblade
            if (Felblade && Aimsharp.SpellCooldown("Felblade") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Felblade queue toggle", Color.Purple);
                }
                Aimsharp.Cast("FelbladeOff");
                return true;
            }

            if (Felblade && Aimsharp.CanCast("Felblade", "target", true, true) && Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Felblade through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Felblade");
                return true;
            }

            bool ChaosNova = Aimsharp.IsCustomCodeOn("ChaosNova");
            //Queue Chaos Nova
            if (ChaosNova && Aimsharp.SpellCooldown("Chaos Nova") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Chaos Nova queue toggle", Color.Purple);
                }
                Aimsharp.Cast("ChaosNovaOff");
                return true;
            }

            if (ChaosNova && Aimsharp.CanCast("Chaos Nova", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Chaos Nova through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Chaos Nova");
                return true;
            }

            bool FelEruption = Aimsharp.IsCustomCodeOn("FelEruption");
            //Queue Fel Eruption
            if (FelEruption && Aimsharp.SpellCooldown("Fel Eruption") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Fel Eruption queue toggle", Color.Purple);
                }
                Aimsharp.Cast("FelEruptionOff");
                return true;
            }

            if (FelEruption && Aimsharp.CanCast("Fel Eruption", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Fel Eruption through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Fel Eruption");
                return true;
            }

            bool Darkness = Aimsharp.IsCustomCodeOn("Darkness");
            //Queue Darkness
            if (Darkness && Aimsharp.SpellCooldown("Darkness") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Darkness queue toggle", Color.Purple);
                }
                Aimsharp.Cast("DarknessOff");
                return true;
            }

            if (Darkness && Aimsharp.CanCast("Darkness", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Darkness through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Darkness");
                return true;
            }

            bool Imprison = Aimsharp.IsCustomCodeOn("Imprison");
            //Queue Imprison
            if (Imprison && Aimsharp.SpellCooldown("Imprison") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Imprison queue toggle", Color.Purple);
                }
                Aimsharp.Cast("ImprisonOff");
                return true;
            }

            if (Imprison && Aimsharp.CanCast("Imprison", "target", true, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Imprison through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Imprison");
                return true;
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
            //Hekili Cycle
            if (!NoCycle && Aimsharp.CustomFunction("HekiliCycle") == 1 && EnemiesInMelee > 1)
            {
                System.Threading.Thread.Sleep(50);
                Aimsharp.Cast("TargetEnemy");
                System.Threading.Thread.Sleep(50);
                return true;
            }

            //Auto Target
            if (!NoCycle && (!Enemy || Enemy && !TargetAlive() || Enemy && !TargetInCombat) && EnemiesInMelee > 0)
            {
                System.Threading.Thread.Sleep(50);
                Aimsharp.Cast("TargetEnemy");
                System.Threading.Thread.Sleep(50);
                return true;
            }
            #endregion

            if (Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                if (Wait <= 200 && !Aimsharp.HasDebuff("Imprison", "target", true) && !Imprison && !Felblade && !FelRush)
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
                    if (SpellID1 == 306830 && Aimsharp.CanCast("Elysian Decree", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Elysian Decree - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Elysian Decree");
                        return true;
                    }

                    if (SpellID1 == 329554 && Aimsharp.CanCast("Fodder to the Flame", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Fodder to the Flame - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Fodder to the Flame");
                        return true;
                    }

                    if (SpellID1 == 323639 && Aimsharp.CanCast("The Hunt", "target", true, true) && !Moving)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting The Hunt - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("The Hunt");
                        return true;
                    }

                    if (SpellID1 == 317009 && Aimsharp.CanCast("Sinful Brand", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Sinful Brand - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Sinful Brand");
                        return true;
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
                    if (SpellID1 == 6552 && Aimsharp.CanCast("Disrupt", "target", true, true))
                    {
                        Aimsharp.Cast("Disrupt", true);
                        return true;
                    }
                    #endregion

                    #region General Spells - Player GCD
                    //Instant [GCD]
                    ///Player
                    if (SpellID1 == 258920 && Aimsharp.CanCast("Immolation Aura", "player", false, true) && Aimsharp.Range("target") <= 8)
                    {
                        Aimsharp.Cast("Immolation Aura");
                        return true;
                    }

                    if (SpellID1 == 191427 && Aimsharp.CanCast("Metamorphosis", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("MetamorphosisP");
                        return true;
                    }

                    if (SpellID1 == 198793 && Aimsharp.CanCast("Vengeful Retreat", "player", false, true) && !Moving && !GetCheckBox("Suggest but don't cast Movement Based Abilities:"))
                    {
                        Aimsharp.Cast("Vengeful Retreat");
                        return true;
                    }

                    if (SpellID1 == 196718 && Aimsharp.CanCast("Darkness", "player", false, true))
                    {
                        Aimsharp.Cast("Darkness");
                        return true;
                    }

                    if (SpellID1 == 198589 && Aimsharp.CanCast("Blur", "player", false, true))
                    {
                        Aimsharp.Cast("Blur");
                        return true;
                    }

                    if (SpellID1 == 195072 && Aimsharp.CanCast("Fel Rush", "player", false, true) && !Moving && !GetCheckBox("Suggest but don't cast Movement Based Abilities:"))
                    {
                        Aimsharp.Cast("Fel Rush");
                        return true;
                    }

                    if (SpellID1 == 179057 && Aimsharp.CanCast("Chaos Nova", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Chaos Nova");
                        return true;
                    }

                    if (SpellID1 == 198013 && Aimsharp.CanCast("Eye Beam", "player", false, true) && Aimsharp.Range("target") <= 5 && !Moving)
                    {
                        Aimsharp.Cast("Eye Beam");
                        return true;
                    }

                    if (SpellID1 == 342817 && Aimsharp.CanCast("Glaive Tempest", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Glaive Tempest");
                        return true;
                    }

                    if (SpellID1 == 258860 && Aimsharp.CanCast("Essence Break", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Essence Break");
                        return true;
                    }

                    if (SpellID1 == 258925 && Aimsharp.CanCast("Fel Barrage", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Fel Barrage");
                        return true;
                    }
                    #endregion

                    #region General Spells - Target GCD
                    ///Target
                    if (SpellID1 == 278326 && Aimsharp.CanCast("Consume Magic", "target", true, true))
                    {
                        Aimsharp.Cast("Consume Magic");
                        return true;
                    }

                    if (SpellID1 == 210152 && Aimsharp.CanCast("Death Sweep", "target", true, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Death Sweep");
                        return true;
                    }

                    if (SpellID1 == 201427 && Aimsharp.CanCast("Annihilation", "target", true, true))
                    {
                        Aimsharp.Cast("Annihilation");
                        return true;
                    }

                    if (SpellID1 == 162243 && Aimsharp.CanCast("Demon's Bite", "target", true, true))
                    {
                        Aimsharp.Cast("Demon's Bite");
                        return true;
                    }

                    if (SpellID1 == 162794 && Aimsharp.CanCast("Chaos Strike", "target", true, true))
                    {
                        Aimsharp.Cast("Chaos Strike");
                        return true;
                    }

                    if (SpellID1 == 188499 && Aimsharp.CanCast("Blade Dance", "target", true, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Blade Dance");
                        return true;
                    }

                    if (SpellID1 == 232893 && Aimsharp.CanCast("Felblade", "target", true, true) && !GetCheckBox("Suggest but don't cast Movement Based Abilities:"))
                    {
                        Aimsharp.Cast("Felblade");
                        return true;
                    }

                    if (SpellID1 == 211881 && Aimsharp.CanCast("Fel Eruption", "target", true, true))
                    {
                        Aimsharp.Cast("Fel Eruption");
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
            #endregion

            #region Queues
            //Queues
            bool FelRush = Aimsharp.IsCustomCodeOn("FelRush");
            //Queue Fel Rush
            if (FelRush && (Aimsharp.SpellCooldown("Fel Rush") - Aimsharp.GCD() > 1000 && Aimsharp.SpellCharges("Fel Rush") == 0 || Aimsharp.RechargeTime("Fel Rush") > 9500 && Aimsharp.SpellCharges("Fel Rush") == 1))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Fel Rush queue toggle", Color.Purple);
                }
                Aimsharp.Cast("FelRushOff");
                return true;
            }

            if (FelRush && Aimsharp.CanCast("Fel Rush", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Fel Rush through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Fel Rush");
                return true;
            }

            bool Felblade = Aimsharp.IsCustomCodeOn("Felblade");
            //Queue Felblade
            if (Felblade && Aimsharp.SpellCooldown("Felblade") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Felblade queue toggle", Color.Purple);
                }
                Aimsharp.Cast("FelbladeOff");
                return true;
            }

            if (Felblade && Aimsharp.CanCast("Felblade", "target", true, true) && Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Felblade through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Felblade");
                return true;
            }

            bool ChaosNova = Aimsharp.IsCustomCodeOn("ChaosNova");
            //Queue Chaos Nova
            if (ChaosNova && Aimsharp.SpellCooldown("Chaos Nova") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Chaos Nova queue toggle", Color.Purple);
                }
                Aimsharp.Cast("ChaosNovaOff");
                return true;
            }

            if (ChaosNova && Aimsharp.CanCast("Chaos Nova", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Chaos Nova through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Chaos Nova");
                return true;
            }

            bool FelEruption = Aimsharp.IsCustomCodeOn("FelEruption");
            //Queue Fel Eruption
            if (FelEruption && Aimsharp.SpellCooldown("Fel Eruption") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Fel Eruption queue toggle", Color.Purple);
                }
                Aimsharp.Cast("FelEruptionOff");
                return true;
            }

            if (FelEruption && Aimsharp.CanCast("Fel Eruption", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Fel Eruption through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Fel Eruption");
                return true;
            }

            bool Darkness = Aimsharp.IsCustomCodeOn("Darkness");
            //Queue Darkness
            if (Darkness && Aimsharp.SpellCooldown("Darkness") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Darkness queue toggle", Color.Purple);
                }
                Aimsharp.Cast("DarknessOff");
                return true;
            }

            if (Darkness && Aimsharp.CanCast("Darkness", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Darkness through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Darkness");
                return true;
            }

            bool Imprison = Aimsharp.IsCustomCodeOn("Imprison");
            //Queue Imprison
            if (Imprison && Aimsharp.SpellCooldown("Imprison") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Imprison queue toggle", Color.Purple);
                }
                Aimsharp.Cast("ImprisonOff");
                return true;
            }

            if (Imprison && Aimsharp.CanCast("Imprison", "target", true, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Imprison through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Imprison");
                return true;
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
            if (GetCheckBox("Auto Start Combat:") == true && Aimsharp.TargetIsEnemy() && TargetAlive() && Aimsharp.Range("target") <= 6 && TargetInCombat && !Imprison && !Aimsharp.HasBuff("Imprison", "player", true))
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