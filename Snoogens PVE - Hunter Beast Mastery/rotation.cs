using System.Linq;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Drawing;
using AimsharpWow.API;

namespace AimsharpWow.Modules
{
    public class SnoogensPVEHunterBeastMastery : Rotation
    {
        #region Variables
        string FiveLetters;
        #endregion

        #region Lists
        //Lists
        private List<string> m_IngameCommandsList = new List<string> { "FreezingTrap", "TarTrap", "Turtle", "Intimidation", "NoInterrupts" };
        private List<string> m_DebuffsList = new List<string> {  };
        private List<string> m_BuffsList = new List<string> { "Mend Pet", "Flayer's Mark", };
        private List<string> m_BloodlustBuffsList = new List<string> { "Bloodlust", "Heroism", "Time Warp", "Primal Rage", "Drums of Rage" };
        private List<string> m_ItemsList = new List<string> { "Healthstone" };

        private List<string> PVECC = new List<string>
        {
        "Crushing Strike",
        "Fearsome Howl",
        "River's Grasp",
        "River's Grasp",
        "Crushing Slam",
        "Torture",
        "Gauntlet Smash",
        "Bloodcurdling Howl",
        "Spite",
        "Chain Burst",
        "Shattered Destiny",
        "Stonecrash",
        "Sundering Smash",
        "Earthen Grasp",
        "Terrifying Shriek",
        "Ten of Towers",
        "Ten of Towers",
        "Chains of Eternity",
        "The Jailer's Gaze",
        "Overpower",
        "Unshakeable Dread",
        "Annihilating Smash",
        "Pulled Down",
        "Wings of Rage",
        "Wings of Rage",
        "Reverberating Refrain",
        "Reverberating Refrain",
        "Spite",
        "Spite",
        "Spite",
        "Hellscream",
        "Spiked Ball",
        "Spiked",
        "Spiked",
        "Suppression Field",
        "Disintegration",
        "Oblivion's Echo",
        "Oblivion's Echo",
        "Return of the Damned",
        "Blasphemy",
        "Merciless",
        "Crippling Defeat",
        "Terror Orb",
        "Arcane Stasiswave",
        "Subdue",
        "Doubt",
        "Crushing Doubt",
        "Insidious Anxieties",
        "Headbutt",
        "Horrified",
        "Return to Stone",
        "Stasis Trap",
        "Possession",
        "Warped Cognition",
        "Waltz of Blood",
        "Tactical Advance",
        "Scarlet Letter",
        "Destructive Impact",
        "Shattering Chain",
        "Falling Rubble",
        "Heedless Charge",
        "Ravenous Feast",
        "Ravenous Feast",
        "Crystalline Burst",
        "Petrified",
        "Blood Price",
        "March of the Penitent",
        "March of the Penitent",
        "Debilitating Injury",
        "Recharge Anima",
        "Belligerent Boast",
        "Gavel of Judgement",
        "Containment Cell",
        "Impound Contraband",
        "Titanic Crash",
        "Shock Mines",
        "Runic Feedback",
        "Infinite Breath",
        "Deadly Seas",
        "Stasis Beam",
        "Ground Stomp",
        "Shocklight Barrier",
        "Flock!",
        "Containment Cell",
        "Shocked",
        "Dancing",
        "Frenzied Charge",
        "Lightshard Retreat",
        "Boulder Throw",
        "Hyperlight Containment Cell",
        "Hyper Zap-o-matic Ultimate Mark III",
        "Haunted Urn",
        "W-00F",
        "Pacifying Mists",
        "Eruption",
        "Slipped",
        "Slipped",
        "Shimmerdust Sleep",
        "Absorbing Haze",
        "Wailing Grief",
        "Shadowfury",
        "Hex",
        "Sinlight Visions",
        "Sinlight Visions",
        "Turned to Stone",
        "Shredded Ankles",
        "Turn to Stone",
        "Soul Shackle",
        "Soul Shackle",
        "Droman's Wrath",
        "Bewildering Pollen",
        "Bewildering Pollen",
        "Repulsive Visage",
        "Freezing Burst",
        "Patty Cake",
        "Parasitic Pacification",
        "Parasitic Incapacitation",
        "Parasitic Domination",
        "Overgrowth",
        "Radiant Breath",
        "Envelopment of Mist",
        "Fetid Gas",
        "Dark Grasp",
        "Dark Grasp",
        "Forgotten Forgehammer",
        "Meat Hook",
        "Meat Hook",
        "Drain Fluids",
        "Rasping Scream",
        "Shadow Ambush",
        "Web Wrap",
        "Web Wrap",
        "Volatile Substance",
        "Rend Souls",
        "Blinding Flash",
        "Drained",
        "Pounce",
        "Terrifying Screech",
        "Spear of Destiny",
        "Opportunity Strikes",
        "Blood and Glory",
        "Blood and Glory",
        "Soulless",
        "Possession",
        "Curse of Desolation",
        "Vile Gas",
        "Vile Eruption",
        "Vile Eruption",
        "Death Grasp",
        "On the Hook",
        "Draw Soul",
        "Ground Smash",
        "Demoralizing Shout",
        "Soul Emanation",
        "Twisted Hellchoker",
        "Darkening Canopy",
        "Ogundimu's Fist",
        "Darkhelm of Nuren",
        "Pridebreaker's Anvil",
        "Volatile Augury",
        "Force Pull",
        "Ancient Drake Breath",
        "Imprison",
        "Big Clapper",
        "The Hunt",
        "Creeping Freeze",
        "Polymorph: Mawrat",
        "Polymorph",
        "Polymorph",
        "Polymorph",
        "Pandemonium",
        "Incomprehensible Glory",
        "Mad Wizard's Confusion",
        "Distracting Charges",
        "Briefcase Bash",
        "Fearsome Shriek",
        "Terrifying Screech",
        "Earthen Crush",
        "Suppress",
        "Ground Crush",
        "Shockwave",
        "Meat Hook",
        "Hulking Charge",
        "Fearsome Howl",
        "Fearsome Howl",
        "Fearsome Howl",
        "Fearsome Howl",
        "Visage of Lethality",
        "Writhing Shadow-Tendrils",
        "Frigid Wildseed",
        "Distracting Charges",
        "Crushing Shadows",
        "Terrifying Roar",
        "Terror",
        "Shadowed Iris",
        "Call of Thunder",
        "Coalesce Anima",
        "Coalesce Anima",
        "Leaping Maul",
        "Hematoma",
        "Soulburst Charm",
        "Slumberweb",
        "Thanatophobia",
        "Refractive Burst",
        "Sweet Dreams",
        };

        private List<string> m_SpellBook = new List<string> {
            //Covenants
            "Flayed Shot",

            //Interrupt
            "Counter Shot",

            //General
            "Barbed Shot", "Aspect of the Wild", "Kill Command", "A Murder of Crows", "Dire Beast", "Multi-Shot",
            "Kill Shot", "Cobra Shot", "Bite", "Cobra Spit", "Arcane Shot", "Auto Shot", "Bestial Wrath",
            "Hunter's Mark", "Tranquilizing Shot", "Exhilaration", "Spirit Pulse", "Spirit Mend", "Mend Pet",

            "Freezing Trap", "Tar Trap", "Aspect of the Turtle", "Intimidation", "Bloodshed",


            "Summon Steward",

        };

        private List<string> m_RaceList = new List<string> { "dwarf" };

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
        private bool CanCastKillShot(string unit)
        {
            if (Aimsharp.CanCast("Kill Shot", "target", true, true) || (Aimsharp.SpellCooldown("Kill Shot") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Range(unit) <= 40 && (Aimsharp.Health(unit) < 20 || Aimsharp.HasBuff("Flayer's Mark", "player", true)) && (Aimsharp.Power("player") >= 10 || Aimsharp.HasBuff("Flayer's Mark", "player", true)) && TargetAlive() && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastFlayedShot(string unit)
        {
            if (Aimsharp.CanCast("Flayed Shot", unit, true, true) || (Aimsharp.SpellCooldown("Flayed Shot") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Range(unit) <= 40 && Aimsharp.CovenantID() == 2 && TargetAlive() && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastMultiShot(string unit)
        {
            if (Aimsharp.CanCast("Multi-Shot", unit, true, true) || (Aimsharp.SpellCooldown("Multi-Shot") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Range(unit) <= 40 && Aimsharp.Power("player") >= 40 && TargetAlive() && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastFreezingTrap(string unit)
        {
            if (Aimsharp.CanCast("Freezing Trap", unit, false, true) || (Aimsharp.SpellCooldown("Freezing Trap") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastTarTrap(string unit)
        {
            if (Aimsharp.CanCast("Tar Trap", unit, false, true) || (Aimsharp.SpellCooldown("Tar Trap") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastMendPet(string unit)
        {
            if (Aimsharp.CanCast("Mend Pet", unit, true, true) || (Aimsharp.SpellCooldown("Mend Pet") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Health("pet") > 1 && Aimsharp.Range("pet") <= 45 && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastAspectoftheTurtle(string unit)
        {
            if (Aimsharp.CanCast("Aspect of the Turtle", unit, false, true) || (Aimsharp.SpellCooldown("Aspect of the Turtle") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastBindingShot(string unit)
        {
            if (Aimsharp.CanCast("Binding Shot", unit, false, true) || (Aimsharp.SpellCooldown("Binding Shot") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Talent(5, 3) && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastCounterShot(string unit)
        {
            if (Aimsharp.CanCast("Counter Shot", unit, true, true) || (Aimsharp.SpellCooldown("Counter Shot") <= 0 && Aimsharp.Range(unit) <= 40 && TargetAlive()))
                return true;

            return false;
        }

        private bool CanCastExhilaration(string unit)
        {
            if (Aimsharp.CanCast("Exhilaration", unit, false, true) || (Aimsharp.SpellCooldown("Exhilaration") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastTranquilizingShot(string unit)
        {
            if (Aimsharp.CanCast("Tranquilizing Shot", unit, true, true) || (Aimsharp.SpellCooldown("Tranquilizing Shot") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Range(unit) <= 40 && TargetAlive() && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastAspectoftheWild(string unit)
        {
            if (Aimsharp.CanCast("Aspect of the Wild", unit, false, true) || (Aimsharp.SpellCooldown("Aspect of the Wild") <= 0 && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastBarbedShot(string unit)
        {
            if (Aimsharp.CanCast("Barbed Shot", "target", true, true) || ((Aimsharp.SpellCooldown("Barbed Shot") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) || Aimsharp.SpellCharges("Barbed Shot") >= 1 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0)) && Aimsharp.Range(unit) <= 40 && TargetAlive() && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastBestialWrath(string unit)
        {
            if (Aimsharp.CanCast("Bestial Wrath", unit, false, true) || (Aimsharp.SpellCooldown("Bestial Wrath") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Health("pet") > 1 && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastBloodshed(string unit)
        {
            if (Aimsharp.CanCast("Bloodshed", unit, true, true) || (Aimsharp.SpellCooldown("Bloodshed") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Health("pet") > 1 && Aimsharp.Range(unit) <= 50 && Aimsharp.Talent(7, 3) && TargetAlive() && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastCobraShot(string unit)
        {
            if (Aimsharp.CanCast("Cobra Shot", unit, true, true) || (Aimsharp.SpellCooldown("Cobra Shot") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Range(unit) <= 40 && Aimsharp.Power("player") >= 35 && TargetAlive() && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastIntimidation(string unit)
        {
            if (Aimsharp.CanCast("Intimidation", unit, true, true) || (Aimsharp.SpellCooldown("Intimidation") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Health("pet") > 1 && TargetAlive() && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }

        private bool CanCastKillCommand(string unit)
        {
            if (Aimsharp.CanCast("Kill Command", unit, true, true) || (Aimsharp.SpellCooldown("Kill Command") - Aimsharp.GCD() <= 0 && (Aimsharp.GCD() > 0 && Aimsharp.GCD() < Aimsharp.CustomFunction("GetSpellQueueWindow") || Aimsharp.GCD() == 0) && Aimsharp.Range(unit) <= 50 && Aimsharp.Power("player") >= 30 && Aimsharp.Health("pet") > 1 && TargetAlive() && Aimsharp.GetPlayerLevel() >= 60 && !TorghastList.Contains(Aimsharp.GetMapID())))
                return true;

            return false;
        }
        #endregion

        #region Debuffs
        private int UnitCC(string unit)
        {
            foreach (string Debuff in PVECC)
            {
                if (Aimsharp.HasDebuff(Debuff, unit, false))
                    return Aimsharp.DebuffRemaining(Debuff, unit, false);
            }

            return 0;
        }
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

            //SpellQueueWindow
            Macros.Add("SetSpellQueueCvar", "/console SpellQueueWindow " + (Aimsharp.Latency + 100));

            Macros.Add("FreezingTrapOff", "/" + FiveLetters + " FreezingTrap");
            Macros.Add("TarTrapOff", "/" + FiveLetters + " TarTrap");
            Macros.Add("IntimidationOff", "/" + FiveLetters + " Intimidation");

            Macros.Add("SpiritMendPlayer", "/cast [@player] Spirit Mend");
            Macros.Add("SpiritMendPet", "/cast [@pet] Spirit Mend");
            Macros.Add("KillShotSQW", "/cqs\\n/cast Kill Shot");
            Macros.Add("BarbedShotSQW", "/cqs\\n/cast Barbed Shot");

        }

        private void InitializeSpells()
        {
            foreach (string Spell in m_SpellBook)
                Spellbook.Add(Spell);

            foreach (string Buff in m_BuffsList)
                Buffs.Add(Buff);

            foreach (string Buff in m_BloodlustBuffsList)
                Buffs.Add(Buff);

            foreach (string Debuff in m_DebuffsList)
                Debuffs.Add(Debuff);

            foreach (string Debuff in PVECC)
                Debuffs.Add(Debuff);

            foreach (string Item in m_ItemsList)
                Items.Add(Item);

            foreach (string MacroCommand in m_IngameCommandsList)
                CustomCommands.Add(MacroCommand);
        }

        private void InitializeCustomLUAFunctions()
        {
            CustomFunctions.Add("HekiliID1", "local loading, finished = IsAddOnLoaded(\"Hekili\") \r\nif loading == true and finished == true then \r\n    local id=Hekili_GetRecommendedAbility(\"Primary\",1)\r\n\tif id ~= nil then\r\n\t\r\n    if id<0 then \r\n\t    local spell = Hekili.Class.abilities[id]\r\n\t    if spell ~= nil and spell.item ~= nil then \r\n\t    \tid=spell.item\r\n\t\t    local topTrinketLink = GetInventoryItemLink(\"player\",13)\r\n\t\t    local bottomTrinketLink = GetInventoryItemLink(\"player\",14)\r\n\t\t    if topTrinketLink  ~= nil then\r\n                local trinketid = GetItemInfoInstant(topTrinketLink)\r\n                if trinketid ~= nil then\r\n\t\t\t        if trinketid == id then\r\n\t\t\t\t        return 1\r\n                    end\r\n\t\t\t    end\r\n\t\t    end\r\n\t\t    if bottomTrinketLink ~= nil then\r\n                local trinketid = GetItemInfoInstant(bottomTrinketLink)\r\n                if trinketid ~= nil then\r\n    \t\t\t    if trinketid == id then\r\n\t    \t\t\t    return 2\r\n                    end\r\n\t\t\t    end\r\n\t\t    end\r\n\t    end \r\n    end\r\n    return id\r\nend\r\nend\r\nreturn 0");

            CustomFunctions.Add("GetSpellQueueWindow", "local sqw = GetCVar(\"SpellQueueWindow\"); if sqw ~= nil then return tonumber(sqw); end return 0");

            CustomFunctions.Add("CooldownsToggleCheck", "local loading, finished = IsAddOnLoaded(\"Hekili\") if loading == true and finished == true then local cooldowns = Hekili:GetToggleState(\"cooldowns\") if cooldowns == true then return 1 else if cooldowns == false then return 2 end end end return 0");
            
            CustomFunctions.Add("UnitIsDead", "if UnitIsDead(\"target\") ~= nil and UnitIsDead(\"target\") == true then return 1 end; if UnitIsDead(\"target\") ~= nil and UnitIsDead(\"target\") == false then return 2 end; return 0");

            CustomFunctions.Add("IsTargeting", "if SpellIsTargeting()\r\n then return 1\r\n end\r\n return 0");

            CustomFunctions.Add("IsRMBDown", "local MBD = 0 local isDown = IsMouseButtonDown(\"RightButton\") if isDown == true then MBD = 1 end return MBD");

            CustomFunctions.Add("SpiritBeastDispel", "local y=0; " +
                "for i=1,25 do local name,_,_,type=UnitDebuff(\"pet\",i); " +
                    "if type ~= nil and (type == \"Disease\" or type == \"Poison\" or type == \"Magic\") then y = 1; end end " +
                "return y");

            CustomFunctions.Add("HekiliWait", "if HekiliDisplayPrimary.Recommendations[1].wait ~= nil and HekiliDisplayPrimary.Recommendations[1].wait * 1000 > 0 then return math.floor(HekiliDisplayPrimary.Recommendations[1].wait * 1000) end return 0");

        }
        #endregion

        public override void LoadSettings()
        {
            Settings.Add(new Setting("First 5 Letters of the Addon:", "xxxxx"));
            Settings.Add(new Setting("Race:", m_RaceList, "dwarf"));
            Settings.Add(new Setting("Ingame World Latency:", 1, 200, 50));
            Settings.Add(new Setting(" "));
            Settings.Add(new Setting("Use Trinkets on CD, dont wait for Hekili:", false));
            Settings.Add(new Setting("Auto Healthstone @ HP%", 0, 100, 25));
            Settings.Add(new Setting("Kicks/Interrupts"));
            Settings.Add(new Setting("Kick at milliseconds remaining", 50, 1500, 500));
            Settings.Add(new Setting("Kick channels after milliseconds", 50, 1500, 500));
            Settings.Add(new Setting("General"));
            Settings.Add(new Setting("Auto Start Combat:", true));
            Settings.Add(new Setting("Auto Aspect of the Turtle @ HP%", 0, 100, 20));
            Settings.Add(new Setting("Auto Exhilaration @ HP%", 0, 100, 40));
            Settings.Add(new Setting("Auto Spirit Heal Player @ HP%", 0, 100, 60));
            Settings.Add(new Setting("Auto Spirit Heal Pet @ HP%", 0, 100, 30));
            Settings.Add(new Setting("Auto Mend Pet @ HP%", 0, 100, 60));

            Settings.Add(new Setting(" "));
            Settings.Add(new Setting("Debug:", false));

        }

        public override void Initialize()
        {

                Aimsharp.DebugMode();


            Aimsharp.Latency = GetSlider("Ingame World Latency:");
            Aimsharp.QuickDelay = 150;
            Aimsharp.SlowDelay = 350;

            Aimsharp.PrintMessage("Snoogens PVE - Hunter Beast Mastery", Color.Yellow);
            Aimsharp.PrintMessage("This rotation requires the Hekili Addon", Color.Red);
            Aimsharp.PrintMessage("Hekili > Toggles > Unbind everything", Color.Brown);
            Aimsharp.PrintMessage("Hekili > Toggles > Bind \"Cooldowns\" & \"Display Mode\"", Color.Brown);
            Aimsharp.PrintMessage("-----", Color.Black);
            Aimsharp.PrintMessage("- General -", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx NoInterrupts - Disables Interrupts", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx FreezingTrap - Casts Freezing Trap @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx TarTrap - Casts Tar Trap @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Intimidation - Casts Intimidation @ Target next GCD", Color.Yellow);
            Aimsharp.PrintMessage("-----", Color.Black);

            if (GetDropDown("Race:") == "dwarf")
            {
                Spellbook.Add("Stoneform"); //20594
            }

            InitializeSettings();

            InitializeMacros();

            InitializeSpells();

            InitializeCustomLUAFunctions();
        }

        public override bool CombatTick()
        {
            #region Declarations
            int SpellID1 = Aimsharp.CustomFunction("HekiliID1");
            int CooldownsToggle = Aimsharp.CustomFunction("CooldownsToggleCheck");
            int Wait = Aimsharp.CustomFunction("HekiliWait");

            bool NoInterrupts= Aimsharp.IsCustomCodeOn("NoInterrupts");

            bool Debug = GetCheckBox("Debug:") == true;
            bool UseTrinketsCD = GetCheckBox("Use Trinkets on CD, dont wait for Hekili:") == true;

            bool IsInterruptable = Aimsharp.IsInterruptable("target");
            int CastingRemaining = Aimsharp.CastingRemaining("target");
            int CastingElapsed = Aimsharp.CastingElapsed("target");
            bool IsChanneling = Aimsharp.IsChanneling("target");
            int KickValue = GetSlider("Kick at milliseconds remaining");
            int KickChannelsAfter = GetSlider("Kick channels after milliseconds");

            bool Enemy = Aimsharp.TargetIsEnemy();
            int EnemiesInMelee = Aimsharp.EnemiesInMelee();

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

            if (Aimsharp.IsCustomCodeOn("FreezingTrap") && Aimsharp.SpellCooldown("Freezing Trap") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("TarTrap") && Aimsharp.SpellCooldown("Tar Trap") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }
            #endregion

            #region Interrupts
            if (!NoInterrupts && (Aimsharp.UnitID("target") != 168105 || Torghast_InnerFlame.Contains(Aimsharp.CastingID("target"))) && (Aimsharp.UnitID("target") != 157571 || Torghast_InnerFlame.Contains(Aimsharp.CastingID("target"))))
            {
                if (CanCastCounterShot("target"))
                {
                    if (IsInterruptable && !IsChanneling && CastingRemaining < KickValue)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Target Casting ID: " + Aimsharp.CastingID("target") + ", Interrupting", Color.Purple);
                        }
                        Aimsharp.Cast("Counter Shot", true);
                        return true;
                    }
                }

                if (CanCastCounterShot("target"))
                {
                    if (IsInterruptable && IsChanneling && CastingElapsed > KickChannelsAfter)
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Target Channeling ID: " + Aimsharp.CastingID("target") + ", Interrupting", Color.Purple);
                        }
                        Aimsharp.Cast("Counter Shot", true);
                        return true;
                    }
                }
            }
            #endregion

            #region Auto Spells and Items
            //Auto Turtle
            if (CanCastAspectoftheTurtle("player"))
            {
                if (Aimsharp.Health("player") <= GetSlider("Auto Aspect of the Turtle @ HP%"))
                {
                    if (Debug)
                    {
                        Aimsharp.PrintMessage("Using Aspect of the Turtle- Player HP% " + Aimsharp.Health("player") + " due to setting being on HP% " + GetSlider("Auto Aspect of the Turtle @ HP%"), Color.Purple);
                    }
                    Aimsharp.Cast("Aspect of the Turtle");
                    return true;
                }
            }

            //Auto Exhilaration
            if (CanCastExhilaration("player"))
            {
                if (Aimsharp.Health("player") <= GetSlider("Auto Exhilaration @ HP%"))
                {
                    if (Debug)
                    {
                        Aimsharp.PrintMessage("Using Exhilaration - Player HP% " + Aimsharp.Health("player") + " due to setting being on HP% " + GetSlider("Auto Exhilaration @ HP%"), Color.Purple);
                    }
                    Aimsharp.Cast("Exhilaration");
                    return true;
                }
            }

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

            //Auto Mend Pet
            if (Aimsharp.PlayerHasPet() && !Aimsharp.LineOfSighted() && Aimsharp.Health("pet") > 1 && Aimsharp.Health("pet") <= GetSlider("Auto Mend Pet @ HP%") && CanCastMendPet("pet") && !Aimsharp.HasBuff("Mend Pet", "pet", true) && Aimsharp.LastCast() != "Mend Pet")
            {
                Aimsharp.Cast("Mend Pet");
                return true;
            }

            //Auto Spirit Mend - Player
            if (Aimsharp.Health("player") <= GetSlider("Auto Spirit Heal Player @ HP%") && Aimsharp.CanCast("Spirit Mend", "player", true, false) && UnitCC("pet") == 0)
            {
                Aimsharp.Cast("SpiritMendPlayer");
                return true;
            }

            //Auto Spirit Mend - Pet
            if (Aimsharp.Health("pet") <= GetSlider("Auto Spirit Heal Pet @ HP%") && Aimsharp.CanCast("Spirit Mend", "pet", true, false) && UnitCC("pet") == 0)
            {
                Aimsharp.Cast("SpiritMendPet");
                return true;
            }

            //Auto Spirit Pulse
            if (Aimsharp.CustomFunction("SpiritBeastDispel") == 1 && Aimsharp.CanCast("Spirit Pulse", "pet", false, false) && UnitCC("pet") == 0)
            {
                Aimsharp.Cast("Spirit Pulse");
                return true;
            }
            #endregion

            #region Queues
            //Queue Freezing Trap
            if (Aimsharp.IsCustomCodeOn("FreezingTrap") && Aimsharp.SpellCooldown("Freezing Trap") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Freezing Trap Queue", Color.Purple);
                }
                Aimsharp.Cast("FreezingTrapOff");
                return true;
            }

            if (Aimsharp.IsCustomCodeOn("FreezingTrap") && CanCastFreezingTrap("player"))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Freezing Trap through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Freezing Trap");
                return true;
            }

            //Queue Tar Trap
            if (Aimsharp.IsCustomCodeOn("TarTrap") && Aimsharp.SpellCooldown("Tar Trap") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Tar Trap Queue", Color.Purple);
                }
                Aimsharp.Cast("TarTrapOff");
                return true;
            }

            if (Aimsharp.IsCustomCodeOn("TarTrap") && CanCastTarTrap("player"))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Tar Trap through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Tar Trap");
                return true;
            }

            //Queue Intimidation
            if (Aimsharp.IsCustomCodeOn("Intimidation") && Aimsharp.SpellCooldown("Intimidation") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Intimidation Queue", Color.Purple);
                }
                Aimsharp.Cast("IntimidationOff");
                return true;
            }

            if (Aimsharp.IsCustomCodeOn("Intimidation") && CanCastIntimidation("target") && Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Intimidation through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Intimidation");
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

            if (Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat && Wait <= 200)
            {
                if (Aimsharp.Range("target") <= 42)
                {
                    #region Trinkets
                    if (CooldownsToggle == 1 && UseTrinketsCD && Aimsharp.CanUseTrinket(0))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Using Top Trinket on Cooldown", Color.Purple);
                        }
                        Aimsharp.Cast("TopTrinket");
                        return true;
                    }

                    if (CooldownsToggle == 2 && UseTrinketsCD && Aimsharp.CanUseTrinket(1))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Using Bot Trinket on Cooldown", Color.Purple);
                        }
                        Aimsharp.Cast("BotTrinket");
                        return true;
                    }

                    if (SpellID1 == 1 && Aimsharp.CanUseTrinket(0))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Using Top Trinket", Color.Purple);
                        }
                        Aimsharp.Cast("TopTrinket");
                        return true;
                    }

                    if (SpellID1 == 2 && Aimsharp.CanUseTrinket(1))
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
                    if (SpellID1 == 20594 && Aimsharp.CanCast("Stoneform", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Stoneform - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Stoneform");
                        return true;
                    }
                    #endregion

                    #region Covenants
                    ///Covenants
                    if (SpellID1 == 324149 && CanCastFlayedShot("target"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Flayed Shot - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Flayed Shot");
                        return true;
                    }
                    #endregion

                    #region General Spells - No GCD
                    ///Class Spells
                    //Target - No GCD
                    if (SpellID1 == 106839 && CanCastCounterShot("target"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Counter Shot - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Counter Shot", true);
                        return true;
                    }
                    #endregion

                    #region General Spells - Target GCD
                    //Target - GCD
                    if (SpellID1 == 19801 && CanCastTranquilizingShot("target"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Tranquilizing Shot - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Tranquilizing Shot");
                        return true;
                    }

                    if (SpellID1 == 53351 && CanCastKillShot("target"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Kill Shot w/ SQW Cancel - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("KillShotSQW");
                        return true;
                    }

                    if (SpellID1 == 2643 && CanCastMultiShot("target"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Multi-Shot - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Multi-Shot");
                        return true;
                    }
                    #endregion

                    #region General Spells - Player GCD

                    #endregion

                    #region Beast Mastery Spells - Target GCD
                    if (SpellID1 == 321530 && CanCastBloodshed("target"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Bloodshed - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Bloodshed");
                        return true;
                    }

                    if (SpellID1 == 193455 && CanCastCobraShot("target"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Cobra Shot - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Cobra Shot");
                        return true;
                    }

                    if (SpellID1 == 217200 && CanCastBarbedShot("target"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Barbed Shot - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("BarbedShotSQW");
                        return true;
                    }

                    if (SpellID1 == 34026 && CanCastKillCommand("target"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Kill Command - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Kill Command");
                        return true;
                    }
                    #endregion

                    #region Beast Mastery Spells - Player GCD
                    if (SpellID1 == 19574 && CanCastBestialWrath("player"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Bestial Wrath- " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Bestial Wrath");
                        return true;
                    }

                    if (SpellID1 == 193530 && CanCastAspectoftheWild("player"))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Aspect of the Wild - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Aspect of the Wild");
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

            bool Debug = GetCheckBox("Debug:") == true;

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

            if (Aimsharp.IsCustomCodeOn("FreezingTrap") && Aimsharp.SpellCooldown("Freezing Trap") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("TarTrap") && Aimsharp.SpellCooldown("Tar Trap") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }
            #endregion

            #region Queues
            //Queue Freezing Trap
            if (Aimsharp.IsCustomCodeOn("FreezingTrap") && Aimsharp.SpellCooldown("Freezing Trap") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Freezing Trap Queue", Color.Purple);
                }
                Aimsharp.Cast("FreezingTrapOff");
                return true;
            }

            if (Aimsharp.IsCustomCodeOn("FreezingTrap") && Aimsharp.CanCast("Freezing Trap", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Freezing Trap through queue toggle", Color.Purple);
                }
                Aimsharp.PrintMessage("Queued Freezing Trap");
                Aimsharp.Cast("Freezing Trap");
                return true;
            }

            //Queue Tar Trap
            if (Aimsharp.IsCustomCodeOn("TarTrap") && Aimsharp.SpellCooldown("Tar Trap") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Tar Trap Queue", Color.Purple);
                }
                Aimsharp.Cast("TarTrapOff");
                return true;
            }

            if (Aimsharp.IsCustomCodeOn("TarTrap") && Aimsharp.CanCast("Tar Trap", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Tar Trap through queue toggle", Color.Purple);
                }
                Aimsharp.PrintMessage("Queued Tar Trap");
                Aimsharp.Cast("Tar Trap");
                return true;
            }

            //Queue Intimidation
            if (Aimsharp.IsCustomCodeOn("Intimidation") && Aimsharp.SpellCooldown("Intimidation") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Intimidation Queue", Color.Purple);
                }
                Aimsharp.Cast("IntimidationOff");
                return true;
            }

            if (Aimsharp.IsCustomCodeOn("Intimidation") && Aimsharp.CanCast("Intimidation", "target", true, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Intimidation through queue toggle", Color.Purple);
                }
                Aimsharp.PrintMessage("Queued Intimidation");
                Aimsharp.Cast("Intimidation");
                return true;
            }
            #endregion

            #region Auto Combat
            //Auto Combat
            if (GetCheckBox("Auto Start Combat:") == true && Aimsharp.TargetIsEnemy() && TargetAlive() && Aimsharp.Range("target") <= 42 && TargetInCombat)
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