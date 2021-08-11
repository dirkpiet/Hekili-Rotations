using System.Linq;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Drawing;
using AimsharpWow.API;

namespace AimsharpWow.Modules
{
    public class SnoogensPVEDeathKnightFrost : Rotation
    {
        #region Variables
        string FiveLetters;
        #endregion

        #region Lists
        //Lists
        private List<string> m_IngameCommandsList = new List<string> { "DoorofShadows", "NoInterrupts", "NoCycle", "Asphyxiate", "RaiseAlly", "DeathandDecay", "DeathsDue", "AntiMagicZone", "BlindingSleet", };
        private List<string> m_DebuffsList = new List<string> { };
        private List<string> m_BuffsList = new List<string> {  };
        private List<string> m_BloodlustBuffsList = new List<string> { "Bloodlust", "Heroism", "Time Warp", "Primal Rage", "Drums of Rage" };
        private List<string> m_ItemsList = new List<string> { "Phial of Serenity", "Healthstone" };

        private List<string> m_SpellBook_General = new List<string> {
            //Covenants
            "Shackle the Unworthy", "Swarming Mist", "Death's Due", "Abomination Limb",

            //Interrupt
            "Mind Freeze",

            //General
            "Anti-Magic Shell", "Death Coil", "Anti-Magic Zone", "Chains of Ice",
            "Death Grip", "Death Strike", "Dark Command", "Death's Advance",
            "Death and Decay", "Icebound Fortitude", "Lichborne", "Raise Ally",
            "Raise Dead",

            "Empower Rune Weapon", "Remorseless Winter", "Frost Strike", "Frostwyrm's Fury",
            "Howling Blast", "Obliterate", "Pillar of Frost", "Remorseless Winter",
            "Horn of Winter", "Asphyxiate", "Blinding Sleet", "Frostscythe",
            "Hypothermic Presence", "Glacial Advance", "Breath of Sindragosa",
        };

        private List<string> m_RaceList = new List<string> { "human", "dwarf", "nightelf", "gnome", "draenei", "pandaren", "orc", "scourge", "tauren", "troll", "bloodelf", "goblin", "worgen", "voidelf", "lightforgeddraenei", "highmountaintauren", "nightborne", "zandalaritroll", "magharorc", "kultiran", "darkirondwarf", "vulpera", "mechagnome" };
        private List<string> m_CastingList = new List<string> { "Manual", "Cursor", "Player" };

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

            //Queues
            Macros.Add("DeathandDecayOff", "/" + FiveLetters + " DeathandDecay");
            Macros.Add("RaiseAllyOff", "/" + FiveLetters + " RaiseAlly");
            Macros.Add("AsphyxiateOff", "/" + FiveLetters + " Asphyxiate");
            Macros.Add("DeathsDueOff", "/" + FiveLetters + " DeathsDue");
            Macros.Add("AntiMagicZoneOff", "/" + FiveLetters + " AntiMagicZone");
            Macros.Add("BlindingSleetOff", "/" + FiveLetters + " BlindingSleet");

            Macros.Add("RaiseAllyMO", "/cast [@mouseover] Raise Ally");
            Macros.Add("DeathandDecayP", "/cast [@player] Death and Decay");
            Macros.Add("DeathandDecayC", "/cast [@cursor] Death and Decay");
            Macros.Add("DeathsDueP", "/cast [@player] Death's Due");
            Macros.Add("DeathsDueC", "/cast [@cursor] Death's Due");
            Macros.Add("AntiMagicZoneP", "/cast [@player] Anti-Magic Zone");
            Macros.Add("AntiMagicZoneC", "/cast [@cursor] Anti-Magic Zone");

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

            CustomFunctions.Add("CRMouseover", "if UnitExists('mouseover') and UnitIsDead('mouseover') == true and UnitIsPlayer('mouseover') == true then return 1 end; return 0");

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
            Settings.Add(new Setting("Auto Death Pact @ HP%", 0, 100, 30));
            Settings.Add(new Setting("Auto Anti-Magic Shell @ HP%", 0, 100, 15));
            Settings.Add(new Setting("Auto Lichborne @ HP%", 0, 100, 40));
            Settings.Add(new Setting("Death and Decay Cast:", m_CastingList, "Player"));
            Settings.Add(new Setting("Anti-Magic Zone Cast:", m_CastingList, "Player"));
            Settings.Add(new Setting("Death's Due Cast:", m_CastingList, "Player"));
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

            Aimsharp.PrintMessage("Snoogens PVE - Death Knight Frost", Color.Yellow);
            Aimsharp.PrintMessage("This rotation requires the Hekili Addon", Color.Red);
            Aimsharp.PrintMessage("Hekili > Toggles > Unbind everything", Color.Brown);
            Aimsharp.PrintMessage("Hekili > Toggles > Bind \"Cooldowns\" & \"Display Mode\"", Color.Brown);
            Aimsharp.PrintMessage("-----", Color.Black);
            Aimsharp.PrintMessage("- General -", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx NoInterrupts - Disables Interrupts", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx NoCycle - Disables Target Cycle", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Asphyxiate - Casts Asphyxiate @ Target next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx BlindingSleet - Casts Blinding Sleet @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx RaiseAlly - Casts Raise Ally @ Mouseover next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx DeathandDecay - Casts Death and Decay @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx DeathsDue - Casts Death's Due @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx AntiMagicZone - Casts Anti-Magic Zone @ next GCD", Color.Yellow);
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

            if (Aimsharp.IsCustomCodeOn("DeathandDecay") && Aimsharp.SpellCooldown("Death and Decay") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("DeathandDecay") && Aimsharp.SpellCooldown("Defile") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("DeathsDue") && Aimsharp.SpellCooldown("Death's Due") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("AntiMagicZone") && Aimsharp.SpellCooldown("Anit-Magic Zone") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("DoorofShadows") && Aimsharp.SpellCooldown("Door of Shadows") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }
            #endregion

            #region Interrupts
            if (!NoInterrupts && (Aimsharp.UnitID("target") != 168105 || Torghast_InnerFlame.Contains(Aimsharp.CastingID("target"))) && (Aimsharp.UnitID("target") != 157571 || Torghast_InnerFlame.Contains(Aimsharp.CastingID("target"))))
            {
                if (Aimsharp.CanCast("Mind Freeze", "target", true, true))
                {
                    if (IsInterruptable && !IsChanneling && CastingRemaining < KickValue)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Target Casting ID: " + Aimsharp.CastingID("target") + ", Interrupting", Color.Purple);
                        }
                        Aimsharp.Cast("Mind Freeze", true);
                        return true;
                    }
                }

                if (Aimsharp.CanCast("Mind Freeze", "target", true, true))
                {
                    if (IsInterruptable && IsChanneling && CastingElapsed > KickChannelsAfter)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Target Channeling ID: " + Aimsharp.CastingID("target") + ", Interrupting", Color.Purple);
                        }
                        Aimsharp.Cast("Mind Freeze", true);
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

            //Auto Death Pact
            if (Aimsharp.CanCast("Death Pact", "player", false, true))
            {
                if (PlayerHP <= GetSlider("Auto Death Pact @ HP%"))
                {
                    Aimsharp.Cast("Death Pact");
                    return true;
                }
            }

            //Auto Anti-Magic Shell
            if (Aimsharp.CanCast("Anti-Magic Shell", "player", false, true))
            {
                if (PlayerHP <= GetSlider("Auto Anti-Magic Shell @ HP%"))
                {
                    Aimsharp.Cast("Anti-Magic Shell", true);
                    return true;
                }
            }

            //Auto Lichborne
            if (Aimsharp.CanCast("Lichborne", "player", false, true))
            {
                if (PlayerHP <= GetSlider("Auto Lichborne @ HP%"))
                {
                    Aimsharp.Cast("Lichborne", true);
                    return true;
                }
            }
            #endregion

            #region Queues
            //Queues
            //Queue Asphyxiate
            bool Asphyxiate = Aimsharp.IsCustomCodeOn("Asphyxiate");
            if (Aimsharp.SpellCooldown("Asphyxiate") - Aimsharp.GCD() > 2000 && Asphyxiate && Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                Aimsharp.Cast("AsphyxiateOff");
                return true;
            }

            if (Asphyxiate && Aimsharp.CanCast("Asphyxiate", "target", true, true))
            {
                Aimsharp.Cast("Asphyxiate");
                return true;
            }

            //Queue Blinding Sleet
            bool BlindingSleet = Aimsharp.IsCustomCodeOn("BlindingSleet");
            if (Aimsharp.SpellCooldown("Blinding Sleet") - Aimsharp.GCD() > 2000 && BlindingSleet)
            {
                Aimsharp.Cast("BlindingSleetOff");
                return true;
            }

            if (BlindingSleet && Aimsharp.CanCast("Blinding Sleet", "player", false, true))
            {
                Aimsharp.Cast("Blinding Sleet");
                return true;
            }

            //Queue Raise Ally
            bool RaiseAlly = Aimsharp.IsCustomCodeOn("RaiseAlly");
            if (Aimsharp.SpellCooldown("Raise Ally") - Aimsharp.GCD() > 2000 && RaiseAlly)
            {
                Aimsharp.Cast("RaiseAllyOff");
                return true;
            }

            if (RaiseAlly && Aimsharp.CanCast("Raise Ally", "mouseover", true, true))
            {
                Aimsharp.Cast("RaiseAllyMO");
                return true;
            }

            //Queue Death and Decay
            string DeathandDecayCast = GetDropDown("Death and Decay Cast:");
            bool DeathandDecay = Aimsharp.IsCustomCodeOn("DeathandDecay");
            if (Aimsharp.SpellCooldown("Death and Decay") - Aimsharp.GCD() > 2000 && DeathandDecay)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Death and Decay Queue", Color.Purple);
                }
                Aimsharp.Cast("DeathandDecayOff");
                return true;
            }

            if (DeathandDecay && Aimsharp.CanCast("Death and Decay", "player", false, true))
            {
                switch (DeathandDecayCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death and Decay - " + DeathandDecayCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Death and Decay");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death and Decay - " + DeathandDecayCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("DeathandDecayP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death and Decay - " + DeathandDecayCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("DeathandDecayC");
                        return true;
                }
            }

            //Queue Death's Due
            string DeathsDueCast = GetDropDown("Death's Due Cast:");
            bool DeathsDue = Aimsharp.IsCustomCodeOn("DeathsDue");
            if (Aimsharp.SpellCooldown("Death's Due") - Aimsharp.GCD() > 2000 && DeathsDue)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Death's Due Queue", Color.Purple);
                }
                Aimsharp.Cast("DeathsDueOff");
                return true;
            }

            if (DeathsDue && Aimsharp.CanCast("Death's Due", "player", false, true))
            {
                switch (DeathsDueCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death's Due - " + DeathsDueCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Death's Due");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death's Due - " + DeathsDueCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("DeathsDueP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death's Due - " + DeathsDueCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("DeathsDueC");
                        return true;
                }
            }

            //Queue Anti-Magic Zone
            string AntiMagicZoneCast = GetDropDown("Anti-Magic Zone Cast:");
            bool AntiMagicZone = Aimsharp.IsCustomCodeOn("AntiMagicZone");
            if (Aimsharp.SpellCooldown("Anti-Magic Zone") - Aimsharp.GCD() > 2000 && AntiMagicZone)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Anti-Magic Zone Queue", Color.Purple);
                }
                Aimsharp.Cast("AntiMagicZoneOff");
                return true;
            }

            if (AntiMagicZone && Aimsharp.CanCast("Anti-Magic Zone", "player", false, true))
            {
                switch (AntiMagicZoneCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Anti-Magic Zone - " + AntiMagicZoneCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Anti-Magic Zone");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Anti-Magic Zone - " + AntiMagicZoneCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("AntiMagicZoneP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Anti-Magic Zone - " + AntiMagicZoneCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("AntiMagicZoneC");
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
            //Hekili Cycle
            if (Aimsharp.CustomFunction("HekiliCycle") == 1 && EnemiesInMelee > 1)
            {
                System.Threading.Thread.Sleep(50);
                Aimsharp.Cast("TargetEnemy");
                System.Threading.Thread.Sleep(50);
                return true;
            }

            //Auto Target
            if ((!Enemy || Enemy && !TargetAlive() || Enemy && !TargetInCombat) && EnemiesInMelee > 0)
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
                    if (SpellID1 == 312202 && Aimsharp.CanCast("Shackle of the Unworthy", "target", true, true))
                    {
                        Aimsharp.Cast("Shackle of the Unworthy");
                        return true;
                    }

                    if (SpellID1 == 311648 && Aimsharp.CanCast("Swarming Mist", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Swarming Mist");
                        return true;
                    }

                    if (SpellID1 == 324128 && Aimsharp.CanCast("Death's Due", "player", false, true))
                    {
                        switch (DeathsDueCast)
                        {
                            case "Manual":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Death's Due - " + DeathsDueCast + " - Queue", Color.Purple);
                                }
                                Aimsharp.Cast("Death's Duey");
                                return true;
                            case "Player":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Death's Due - " + DeathsDueCast + " - Queue", Color.Purple);
                                }
                                Aimsharp.Cast("DeathsDueP");
                                return true;
                            case "Cursor":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Death's Due - " + DeathsDueCast + " - Queue", Color.Purple);
                                }
                                Aimsharp.Cast("DeathsDueC");
                                return true;
                        }
                    }

                    if (SpellID1 == 315443 && Aimsharp.CanCast("Abomination Limb", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Abomination Limb");
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
                    if (SpellID1 == 47528 && Aimsharp.CanCast("Mind Freeze", "target", true, true))
                    {
                        Aimsharp.Cast("Mind Freeze", true);
                        return true;
                    }

                    if (SpellID1 == 48707 && Aimsharp.CanCast("Anti-Magic Shell", "player", false, true))
                    {
                        Aimsharp.Cast("Anti-Magic Shell", true);
                        return true;
                    }

                    if (SpellID1 == 48792 && Aimsharp.CanCast("Icebound Fortitude", "player", false, true))
                    {
                        Aimsharp.Cast("Icebound Fortitude", true);
                        return true;
                    }

                    if (SpellID1 == 49039 && Aimsharp.CanCast("Lichborne", "player", false, true))
                    {
                        Aimsharp.Cast("Lichborne", true);
                        return true;
                    }

                    if (SpellID1 == 56222 && Aimsharp.CanCast("Dark Command", "target", true, true))
                    {
                        Aimsharp.Cast("Dark Command", true);
                        return true;
                    }
                    #endregion

                    #region General Spells - Player GCD
                    //Instant [GCD]
                    ///Player
                    if ((SpellID1 == 152280 || SpellID1 == 43265) && Aimsharp.CanCast("Death and Decay", "player", false, true))
                    {
                        switch (DeathandDecayCast)
                        {
                            case "Manual":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Death and Decay - " + DeathandDecayCast + " - Queue", Color.Purple);
                                }
                                Aimsharp.Cast("Death and Decay");
                                return true;
                            case "Player":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Death and Decay - " + DeathandDecayCast + " - Queue", Color.Purple);
                                }
                                Aimsharp.Cast("DeathandDecayP");
                                return true;
                            case "Cursor":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Death and Decay - " + DeathandDecayCast + " - Queue", Color.Purple);
                                }
                                Aimsharp.Cast("DeathandDecayC");
                                return true;
                        }
                    }

                    if (SpellID1 == 46585 && Aimsharp.CanCast("Raise Dead", "player", false, true) && !Moving)
                    {
                        Aimsharp.Cast("Raise Dead");
                        return true;
                    }
                    #endregion

                    #region General Spells - Target GCD
                    ///Target
                    if (SpellID1 == 47541 && Aimsharp.CanCast("Death Coil", "target", true, true))
                    {
                        Aimsharp.Cast("Death Coil");
                        return true;
                    }

                    if (SpellID1 == 45524 && Aimsharp.CanCast("Chains of Ice", "target", true, true))
                    {
                        Aimsharp.Cast("Chains of Ice");
                        return true;
                    }

                    if (SpellID1 == 49998 && Aimsharp.CanCast("Death Strike", "target", true, true))
                    {
                        Aimsharp.Cast("Death Strike");
                        return true;
                    }
                    #endregion

                    #region Frost Spells - Player GCD
                    ////Player
                    if (SpellID1 == 47568 && Aimsharp.CanCast("Empower Rune Weapon", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Empower Rune Weapon");
                        return true;
                    }

                    if (SpellID1 == 196770 && Aimsharp.CanCast("Remorseless Winter", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Remorseless Winter");
                        return true;
                    }

                    if (SpellID1 == 279302 && Aimsharp.CanCast("Frostwyrm's Fury", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Frostwyrm's Fury");
                        return true;
                    }

                    if (SpellID1 == 51271 && Aimsharp.CanCast("Pillar of Frost", "player", true, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Pillar of Frost");
                        return true;
                    }

                    if (SpellID1 == 57330 && Aimsharp.CanCast("Horn of Winter", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Horn of Winter");
                        return true;
                    }

                    if (SpellID1 == 207167 && Aimsharp.CanCast("Blinding Sleet", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Blinding Sleet");
                        return true;
                    }

                    if (SpellID1 == 207230 && Aimsharp.CanCast("Frostscythe", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Frostscythe");
                        return true;
                    }

                    if (SpellID1 == 321995 && Aimsharp.CanCast("Hypothermic Presence", "player", false, true) && Aimsharp.Range("target") <= 5)
                    {
                        Aimsharp.Cast("Hypothermic Presence");
                        return true;
                    }

                    if (SpellID1 == 194913 && Aimsharp.CanCast("Glacial Advance", "player", false, true))
                    {
                        Aimsharp.Cast("Glacial Advance");
                        return true;
                    }

                    if (SpellID1 == 152279 && Aimsharp.CanCast("Breath of Sindragosa", "player", false, true))
                    {
                        Aimsharp.Cast("Breath of Sindragosa");
                        return true;
                    }

                    #endregion

                    #region Frost Spells - Target GCD
                    ////Target
                    if (SpellID1 == 49020 && Aimsharp.CanCast("Obliterate", "target", true, true))
                    {
                        Aimsharp.Cast("Obliterate");
                        return true;
                    }

                    if (SpellID1 == 49143 && Aimsharp.CanCast("Frost Strike", "target", true, true))
                    {
                        Aimsharp.Cast("Frost Strike");
                        return true;
                    }

                    if (SpellID1 == 49184 && Aimsharp.CanCast("Howling Blast", "target", true, true))
                    {
                        Aimsharp.Cast("Howling Blast");
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

            if (Aimsharp.IsCustomCodeOn("DeathandDecay") && Aimsharp.SpellCooldown("Death and Decay") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("DeathandDecay") && Aimsharp.SpellCooldown("Defile") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("DeathsDue") && Aimsharp.SpellCooldown("Death's Due") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("AntiMagicZone") && Aimsharp.SpellCooldown("Anit-Magic Zone") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
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
            //Queue Asphyxiate
            bool Asphyxiate = Aimsharp.IsCustomCodeOn("Asphyxiate");
            if (Aimsharp.SpellCooldown("Asphyxiate") - Aimsharp.GCD() > 2000 && Asphyxiate && Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                Aimsharp.Cast("AsphyxiateOff");
                return true;
            }

            if (Asphyxiate && Aimsharp.CanCast("Asphyxiate", "target", true, true))
            {
                Aimsharp.Cast("Asphyxiate");
                return true;
            }

            //Queue Blinding Sleet
            bool BlindingSleet = Aimsharp.IsCustomCodeOn("BlindingSleet");
            if (Aimsharp.SpellCooldown("Blinding Sleet") - Aimsharp.GCD() > 2000 && BlindingSleet)
            {
                Aimsharp.Cast("BlindingSleetOff");
                return true;
            }

            if (BlindingSleet && Aimsharp.CanCast("Blinding Sleet", "player", false, true))
            {
                Aimsharp.Cast("Blinding Sleet");
                return true;
            }

            //Queue Raise Ally
            bool RaiseAlly = Aimsharp.IsCustomCodeOn("RaiseAlly");
            if (Aimsharp.SpellCooldown("Raise Ally") - Aimsharp.GCD() > 2000 && RaiseAlly)
            {
                Aimsharp.Cast("RaiseAllyOff");
                return true;
            }

            if (RaiseAlly && Aimsharp.CanCast("Raise Ally", "mouseover", true, true))
            {
                Aimsharp.Cast("RaiseAllyMO");
                return true;
            }

            //Queue Death and Decay
            string DeathandDecayCast = GetDropDown("Death and Decay Cast:");
            bool DeathandDecay = Aimsharp.IsCustomCodeOn("DeathandDecay");
            if (Aimsharp.SpellCooldown("Death and Decay") - Aimsharp.GCD() > 2000 && DeathandDecay)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Death and Decay Queue", Color.Purple);
                }
                Aimsharp.Cast("DeathandDecayOff");
                return true;
            }

            if (DeathandDecay && Aimsharp.CanCast("Death and Decay", "player", false, true))
            {
                switch (DeathandDecayCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death and Decay - " + DeathandDecayCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Death and Decay");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death and Decay - " + DeathandDecayCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("DeathandDecayP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death and Decay - " + DeathandDecayCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("DeathandDecayC");
                        return true;
                }
            }

            //Queue Death's Due
            string DeathsDueCast = GetDropDown("Death's Due Cast:");
            bool DeathsDue = Aimsharp.IsCustomCodeOn("DeathsDue");
            if (Aimsharp.SpellCooldown("Death's Due") - Aimsharp.GCD() > 2000 && DeathsDue)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Death's Due Queue", Color.Purple);
                }
                Aimsharp.Cast("DeathsDueOff");
                return true;
            }

            if (DeathsDue && Aimsharp.CanCast("Death's Due", "player", false, true))
            {
                switch (DeathsDueCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death's Due - " + DeathsDueCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Death's Due");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death's Due - " + DeathsDueCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("DeathsDueP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Death's Due - " + DeathsDueCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("DeathsDueC");
                        return true;
                }
            }

            //Queue Anti-Magic Zone
            string AntiMagicZoneCast = GetDropDown("Anti-Magic Zone Cast:");
            bool AntiMagicZone = Aimsharp.IsCustomCodeOn("AntiMagicZone");
            if (Aimsharp.SpellCooldown("Anti-Magic Zone") - Aimsharp.GCD() > 2000 && AntiMagicZone)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Anti-Magic Zone Queue", Color.Purple);
                }
                Aimsharp.Cast("AntiMagicZoneOff");
                return true;
            }

            if (AntiMagicZone && Aimsharp.CanCast("Anti-Magic Zone", "player", false, true))
            {
                switch (AntiMagicZoneCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Anti-Magic Zone - " + AntiMagicZoneCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Anti-Magic Zone");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Anti-Magic Zone - " + AntiMagicZoneCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("AntiMagicZoneP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Anti-Magic Zone - " + AntiMagicZoneCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("AntiMagicZoneC");
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

            if (SpellID1 == 46585 && Aimsharp.CanCast("Raise Dead", "player", false, true) && !Moving)
            {
                Aimsharp.Cast("Raise Dead");
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