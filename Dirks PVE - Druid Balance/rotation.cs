using System.Linq;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Drawing;
using AimsharpWow.API;

namespace AimsharpWow.Modules
{
    public class DirksPVEDruidBalance : Rotation
    {
        #region Variables
        string FiveLetters;
        #endregion

        #region Lists
        //Lists
        private List<string> m_IngameCommandsList = new List<string> { "NoDecurse", "NoCycle", "DoorofShadows", "SolarBeam", "MightyBash", "MassEntanglement", "ForceofNature", "UrsolsVortex", "Typhoon", "Rebirth", "Innervate", "Hibernate", "Cyclone", "EntanglingRoots", "Regrowth",};
        private List<string> m_DebuffsList = new List<string> { };
        private List<string> m_BuffsList = new List<string> { "Bear Form", "Cat Form", "Mount Form", "Travel Form", "Treant Form",};
        private List<string> m_BloodlustBuffsList = new List<string> { "Bloodlust", "Heroism", "Time Warp", "Primal Rage", "Drums of Rage" };
        private List<string> m_ItemsList = new List<string> { "Phial of Serenity", "Healthstone", };

        private List<string> m_SpellBook = new List<string> {
            //Covenants
            "Convoke the Spirits", 
            "Adaptive Swarm", 
            "Empower Bond", 
            "Ravenous Frenzy",

            //Interrupt
            "Solar Beam", //78675

            //General
            "Entangling Roots", //339
            "Bear Form", //5487
            "Cat Form", //768
            "Treant Form", //114282
            "Ferocious Bite", //22568
            "Hibernate", //2637
            "Moonfire", //8921
            "Shred", //5521
            "Soothe", //2908
            "Stampeding Roar", //106898
            "Rebirth", //20484
            "Regrowth", //8936
            "Wrath", //190984
            "Cyclone", //33786
            "Barkskin", //22812

            //Balance
            "Celestial Alignment", //194223
            "Starfire", //194153
            "Innervate", //29166
            "Starsurge", //78674
            "Moonkin Form", //24858
            "Sunfire", //93402
            "Remove Corruption", //2782
            "Typhoon", //132469            
            "Starfall", //191034
            "Warrior of Elune", //202425
            "Force of Nature", //205636
            "Tiger Dash", //252216
            "Renewal", //108238
            "Wild Charge", //102401
            "Mighty Bash", //5211
            "Mass Entanglement", //102359
            "Heart of the Wild", //319454
            "Incarnation: Chosen of Elune", //102560
            "Stellar Flare", //202347
            "Fury of Elune", //202770
            "Wild Mushroom", //88747
            "New Moon", //274281
            "Half Moon", //202768, 274282
            "Full Moon", //274283
            "Ursol's Vortex",

            "Summon Steward", "Fleshcraft", "Door of Shadows",

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

        List<int> SpecialUnitList = new List<int> { 176581, 176920, 178008, 168326, 168969, 175861, };

        public Dictionary<string, int> PartyDict = new Dictionary<string, int>() { };
        #endregion

        #region Misc Checks
        private bool TargetAlive()
        {
            if (Aimsharp.CustomFunction("UnitIsDead") == 2)
                return true;

            return false;
        }

        public enum CleansePlayers
        {
            player = 1,
            party1 = 2,
            party2 = 4,
            party3 = 8,
            party4 = 16,
        }

        private bool isUnitCleansable(CleansePlayers unit, int states)
        {
            if ((states & (int)unit) == (int)unit)
            {
                return true;
            }
            return false;
        }

        public bool UnitFocus(string unit)
        {
            if (Aimsharp.CustomFunction("UnitIsFocus") == 1 && unit == "party1" || Aimsharp.CustomFunction("UnitIsFocus") == 2 && unit == "party2" || Aimsharp.CustomFunction("UnitIsFocus") == 3 && unit == "party3" || Aimsharp.CustomFunction("UnitIsFocus") == 4 && unit == "party4" || Aimsharp.CustomFunction("UnitIsFocus") == 5 && unit == "player")
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

            Macros.Add("FOC_party1", "/focus party1");
            Macros.Add("FOC_party2", "/focus party2");
            Macros.Add("FOC_party3", "/focus party3");
            Macros.Add("FOC_party4", "/focus party4");
            Macros.Add("FOC_player", "/focus player");
            Macros.Add("RC_FOC", "/cast [@focus] Remove Corruption");

            //Mouseover Macros
            Macros.Add("SootheMO", "/cast [@mouseover] Soothe");
            Macros.Add("RebirthMO", "/cast [@mouseover] Rebirth");
            Macros.Add("InnervateMO", "/cast [@mouseover] Innervate");
            Macros.Add("HibernateMO", "/cast [@mouseover] Hibernate");
            Macros.Add("CycloneMO", "/cast [@mouseover] Cyclone");
            Macros.Add("EntanglingRootsMO", "/cast [@mouseover] Entangling Roots");

            Macros.Add("ForceofNatureP", "/cast [@player] Force of Nature");
            Macros.Add("ForceofNatureC", "/cast [@cursor] Force of Nature");
            Macros.Add("CelestialAlignmentC", "/cast [@cursor] Celestial Alignment");
            Macros.Add("UrsolsVortexP", "/cast [@player] Ursol's Vortex");
            Macros.Add("UrsolsVortexC", "/cast [@cursor] Ursol's Vortex");

            //Queues
            Macros.Add("MightyBashOff", "/" + FiveLetters + " MightyBash");
            Macros.Add("MassEntanglementOff", "/" + FiveLetters + " MassEntanglement");
            Macros.Add("SolarBeamOff", "/" + FiveLetters + " SolarBeam");
            Macros.Add("RebirthOff", "/" + FiveLetters + " Rebirth");
            Macros.Add("InnervateOff", "/" + FiveLetters + " Innervate");
            Macros.Add("HibernateOff", "/" + FiveLetters + " Hibernate");
            Macros.Add("CycloneOff", "/" + FiveLetters + " Cyclone");
            Macros.Add("TyphoonOff", "/" + FiveLetters + " Typhoon");
            Macros.Add("ForceofNatureOff", "/" + FiveLetters + " ForceofNature");
            Macros.Add("UrsolsVortexOff", "/" + FiveLetters + " UrsolsVortex");
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

            foreach (string Item in m_ItemsList)
                Items.Add(Item);

            foreach (string MacroCommand in m_IngameCommandsList)
                CustomCommands.Add(MacroCommand);
        }

        private void InitializeCustomLUAFunctions()
        {
            CustomFunctions.Add("HekiliID1", "local loading, finished = IsAddOnLoaded(\"Hekili\") \r\nif loading == true and finished == true then \r\n    local id=Hekili_GetRecommendedAbility(\"Primary\",1)\r\n\tif id ~= nil then\r\n\t\r\n    if id<0 then \r\n\t if id == -101 then return 999999 end local spell = Hekili.Class.abilities[id]\r\n\t    if spell ~= nil and spell.item ~= nil then \r\n\t    \tid=spell.item\r\n\t\t    local topTrinketLink = GetInventoryItemLink(\"player\",13)\r\n\t\t    local bottomTrinketLink = GetInventoryItemLink(\"player\",14)\r\n\t\t    if topTrinketLink  ~= nil then\r\n                local trinketid = GetItemInfoInstant(topTrinketLink)\r\n                if trinketid ~= nil then\r\n\t\t\t        if trinketid == id then\r\n\t\t\t\t        return 1\r\n                    end\r\n\t\t\t    end\r\n\t\t    end\r\n\t\t    if bottomTrinketLink ~= nil then\r\n                local trinketid = GetItemInfoInstant(bottomTrinketLink)\r\n                if trinketid ~= nil then\r\n    \t\t\t    if trinketid == id then\r\n\t    \t\t\t    return 2\r\n                    end\r\n\t\t\t    end\r\n\t\t    end\r\n\t    end \r\n    end\r\n    return id\r\nend\r\nend\r\nreturn 0");

            CustomFunctions.Add("GetSpellQueueWindow", "local sqw = GetCVar(\"SpellQueueWindow\"); if sqw ~= nil then return tonumber(sqw); end return 0");

            CustomFunctions.Add("CooldownsToggleCheck", "local loading, finished = IsAddOnLoaded(\"Hekili\") if loading == true and finished == true then local cooldowns = Hekili:GetToggleState(\"cooldowns\") if cooldowns == true then return 1 else if cooldowns == false then return 2 end end end return 0");
            
            CustomFunctions.Add("UnitIsDead", "if UnitIsDead(\"target\") ~= nil and UnitIsDead(\"target\") == true then return 1 end; if UnitIsDead(\"target\") ~= nil and UnitIsDead(\"target\") == false then return 2 end; return 0");

            CustomFunctions.Add("IsTargeting", "if SpellIsTargeting()\r\n then return 1\r\n end\r\n return 0");

            CustomFunctions.Add("IsRMBDown", "local MBD = 0 local isDown = IsMouseButtonDown(\"RightButton\") if isDown == true then MBD = 1 end return MBD");

            CustomFunctions.Add("HekiliWait", "if HekiliDisplayPrimary.Recommendations[1].wait ~= nil and HekiliDisplayPrimary.Recommendations[1].wait * 1000 > 0 then return math.floor(HekiliDisplayPrimary.Recommendations[1].wait * 1000) end return 0");

            CustomFunctions.Add("HekiliCycle", "if HekiliDisplayPrimary.Recommendations[1].indicator ~= nil and HekiliDisplayPrimary.Recommendations[1].indicator == 'cycle' then return 1 end return 0");

            CustomFunctions.Add("HekiliEnemies", "if Hekili.State.active_enemies ~= nil and Hekili.State.active_enemies > 0 then return Hekili.State.active_enemies end return 0");

            CustomFunctions.Add("PhialCount", "local count = GetItemCount(177278) if count ~= nil then return count end return 0");

            CustomFunctions.Add("EnrageBuffCheckMouseover", "local markcheck = 0; if UnitExists('mouseover') and UnitIsDead('mouseover') ~= true and UnitAffectingCombat('mouseover') and IsSpellInRange('Soothe','mouseover') == 1 then markcheck = markcheck +1  for y = 1, 40 do local name,_,_,debuffType  = UnitBuff('mouseover', y) if debuffType == '' then markcheck = markcheck + 2 end end return markcheck end return 0");

            CustomFunctions.Add("EnrageBuffCheckTarget", "local markcheck = 0; if UnitExists('target') and UnitIsDead('target') ~= true and UnitAffectingCombat('target') and IsSpellInRange('Soothe','target') == 1 then markcheck = markcheck +1  for y = 1, 40 do local name,_,_,debuffType  = UnitBuff('target', y) if debuffType == '' then markcheck = markcheck + 2 end end return markcheck end return 0");

            CustomFunctions.Add("UnitIsFocus", "local foc=0; " +
            "\nif UnitExists('focus') and UnitIsUnit('party1','focus') then foc = 1; end" +
            "\nif UnitExists('focus') and UnitIsUnit('party2','focus') then foc = 2; end" +
            "\nif UnitExists('focus') and UnitIsUnit('party3','focus') then foc = 3; end" +
            "\nif UnitExists('focus') and UnitIsUnit('party4','focus') then foc = 4; end" +
            "\nif UnitExists('focus') and UnitIsUnit('player','focus') then foc = 5; end" +
            "\nreturn foc");

            CustomFunctions.Add("CursePoisonCheck", "local y=0; " +
                "for i=1,25 do local name,_,_,type=UnitDebuff(\"player\",i,\"RAID\"); " +
                "if type ~= nil and type == \"Curse\" or type == \"Poison\" then y = y +1; end end " +
                "for i=1,25 do local name,_,_,type=UnitDebuff(\"party1\",i,\"RAID\"); " +
                "if type ~= nil and type == \"Curse\" or type == \"Poison\" then y = y +2; end end " +
                "for i=1,25 do local name,_,_,type=UnitDebuff(\"party2\",i,\"RAID\"); " +
                "if type ~= nil and type == \"Curse\" or type == \"Poison\" then y = y +4; end end " +
                "for i=1,25 do local name,_,_,type=UnitDebuff(\"party3\",i,\"RAID\"); " +
                "if type ~= nil and type == \"Curse\" or type == \"Poison\" then y = y +8; end end " +
                "for i=1,25 do local name,_,_,type=UnitDebuff(\"party4\",i,\"RAID\"); " +
                "if type ~= nil and type == \"Curse\" or type == \"Poison\" then y = y +16; end end " +
                "return y");

            CustomFunctions.Add("GroupTargets",
                    "local UnitTargeted = 0 " +
                    "for i = 1, 20 do local unit = 'nameplate'..i " +
                        "if UnitExists(unit) then " +
                            "if UnitCanAttack('player', unit) then " +
                                "if GetNumGroupMembers() < 6 then " +
                                    "for p = 1, 4 do local partymember = 'party'..p " +
                                        "if UnitIsUnit(unit..'target', partymember) then UnitTargeted = p end " +
                                    "end " +
                                "end " +
                                "if GetNumGroupMembers() > 5 then " +
                                    "for r = 1, 40 do local raidmember = 'raid'..r " +
                                        "if UnitIsUnit(unit..'target', raidmember) then UnitTargeted = r end " +
                                    "end " +
                                "end " +
                                "if UnitIsUnit(unit..'target', 'player') then UnitTargeted = 5 end " +
                            "else UnitTargeted = 0 " +
                            "end " +
                        "end " +
                    "end " +
                    "return UnitTargeted");

        }
        #endregion

        public override void LoadSettings()
        {
            Settings.Add(new Setting("First 5 Letters of the Addon:", "xxxxx"));
            Settings.Add(new Setting("Race:", m_RaceList, "nightelf"));
            Settings.Add(new Setting("Ingame World Latency:", 1, 200, 50));
            Settings.Add(new Setting(" "));
            Settings.Add(new Setting("Use Trinkets on CD, dont wait for Hekili:", false));
            Settings.Add(new Setting("Auto Healthstone @ HP%", 0, 100, 25));
            Settings.Add(new Setting("Auto Phial of Serenity @ HP%", 0, 100, 35));
            Settings.Add(new Setting("General"));
            Settings.Add(new Setting("Auto Start Combat:", true));
            Settings.Add(new Setting("Soothe Mouseover:", true));
            Settings.Add(new Setting("Soothe Target:", true));
            Settings.Add(new Setting("Auto Renewal @ HP%", 0, 100, 20));
            Settings.Add(new Setting("Auto Barkskin @ HP%", 0, 100, 40));
            Settings.Add(new Setting("Ursol's Vortex Cast:", m_CastingList, "Manual"));
            Settings.Add(new Setting("Force of Nature Cast:", m_CastingList, "Manual"));
            Settings.Add(new Setting("Celestial Alignment Cast:", m_CastingList, "Manual"));
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

            Aimsharp.PrintMessage("Dirks PVE - Druid Balance", Color.Yellow);
            Aimsharp.PrintMessage("Version: 0.1", Color.Black);
            Aimsharp.PrintMessage("This rotation requires the Hekili Addon", Color.Red);
            Aimsharp.PrintMessage("Hekili > Toggles > Unbind everything", Color.Brown);
            Aimsharp.PrintMessage("Hekili > Toggles > Bind \"Cooldowns\" & \"Display Mode\"", Color.Brown);
            Aimsharp.PrintMessage("-----", Color.Black);
            Aimsharp.PrintMessage("- General -", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx NoCycle - Disables Target Cycle", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx NoDecurse - Disables Decurse", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx MightyBash - Casts Mighty Bash @ Target next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx MassEntanglement - Casts Mass Entanglement @ Target next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx SolarBeam - Casts Solar Beam @ Target next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx ForceofNature - Casts Force of Nature @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx UrsolsVortex - Casts Ursol's Vortex @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Typhoon - Casts Typhoon @ next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Rebirth - Casts Rebirth @ Mouseover next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Innervate - Casts Innervate @ Mouseover next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Hibernate - Casts Hibernate @ Mouseover next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Cyclone - Casts Cyclone @ Mouseover next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx EntanglingRoots - Casts Entangling Roots @ Mouseover next GCD", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx Regrowth - Casts Regrowth until turned Off", Color.Yellow);
            Aimsharp.PrintMessage("/xxxxx DoorofShadows - Casts Door of Shadows @ next GCD", Color.Yellow);
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
            int CooldownsToggle = Aimsharp.CustomFunction("CooldownsToggleCheck");
            int Wait = Aimsharp.CustomFunction("HekiliWait");
            int Enemies = Aimsharp.CustomFunction("HekiliEnemies");
            int TargetingGroup = Aimsharp.CustomFunction("GroupTargets");

            int CursePoisonCheck = Aimsharp.CustomFunction("CursePoisonCheck");
            int EnrageBuffMO = Aimsharp.CustomFunction("EnrageBuffCheckMouseover");
            int EnrageBuffTarget = Aimsharp.CustomFunction("EnrageBuffCheckTarget");

            bool NoDecurse = Aimsharp.IsCustomCodeOn("NoDecurse");
            bool NoCycle = Aimsharp.IsCustomCodeOn("NoCycle");

            bool MOSoothe = GetCheckBox("Soothe Mouseover:") == true;
            bool TargetSoothe = GetCheckBox("Soothe Target:") == true;

            bool Debug = GetCheckBox("Debug:") == true;
            bool UseTrinketsCD = GetCheckBox("Use Trinkets on CD, dont wait for Hekili:") == true;

            bool Enemy = Aimsharp.TargetIsEnemy();
            int EnemiesInMelee = Aimsharp.EnemiesInMelee();
            bool Moving = Aimsharp.PlayerIsMoving();
            int PlayerHP = Aimsharp.Health("player");

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

            #region Above Pause Logic
            if (Aimsharp.CastingID("player") == 2637 && Aimsharp.CastingRemaining("player") > 0 && Aimsharp.CastingRemaining("player") <= 400 && Aimsharp.IsCustomCodeOn("Hibernate"))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Hibernate Queue", Color.Purple);
                }
                Aimsharp.Cast("HibernateOff");
                return true;
            }

            if (Aimsharp.CastingID("player") == 33786 && Aimsharp.CastingRemaining("player") > 0 && Aimsharp.CastingRemaining("player") <= 400 && Aimsharp.IsCustomCodeOn("Cyclone"))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Cyclone Queue", Color.Purple);
                }
                Aimsharp.Cast("CycloneOff");
                return true;
            }

            if (Aimsharp.CastingID("player") == 339 && Aimsharp.CastingRemaining("player") > 0 && Aimsharp.CastingRemaining("player") <= 400 && Aimsharp.IsCustomCodeOn("EntanglingRoots"))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Entangling Roots Queue", Color.Purple);
                }
                Aimsharp.Cast("EntanglingRootsOff");
                return true;
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

            if (Aimsharp.HasBuff("Bear Form", "player", true) || Aimsharp.HasBuff("Cat Form", "player", true) || Aimsharp.HasBuff("Mount Form", "player", true) || Aimsharp.HasBuff("Travel Form", "player", true) || Aimsharp.HasBuff("Treant Form", "player", true))
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("UrsolsVortex") && Aimsharp.SpellCooldown("Ursol's Vortex") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("ForceofNature") && Aimsharp.SpellCooldown("Force of Nature") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("DoorofShadows") && Aimsharp.SpellCooldown("Door of Shadows") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }
            #endregion

            #region Interrupts
            bool SolarBeam = Aimsharp.IsCustomCodeOn("SolarBeam");
            //Queue Solar Beam
            if (SolarBeam && Aimsharp.SpellCooldown("Solar Beam") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Solar Beam queue toggle", Color.Purple);
                }
                Aimsharp.Cast("SolarBeamOff");
                return true;
            }

            if (SolarBeam && Aimsharp.CanCast("Solar Beam", "target", true, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Solar Beam through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Solar Beam");
                return true;
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

            //Phial of Serenity
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

            //Auto Renewal
            if (Aimsharp.CanCast("Renewal", "player", false, true))
            {
                if (Aimsharp.Health("player") <= GetSlider("Auto Renewal @ HP%"))
                {
                    if (Debug)
                    {
                        Aimsharp.PrintMessage("Using Renewal - Player HP% " + Aimsharp.Health("player") + " due to setting being on HP% " + GetSlider("Auto Renewal @ HP%"), Color.Purple);
                    }
                    Aimsharp.Cast("Renewal");
                    return true;
                }
            }

            //Auto Barkskin
            if (Aimsharp.CanCast("Barkskin", "player", false, true))
            {
                if (Aimsharp.Health("player") <= GetSlider("Auto Barkskin @ HP%"))
                {
                    if (Debug)
                    {
                        Aimsharp.PrintMessage("Using Barkskin - Player HP% " + Aimsharp.Health("player") + " due to setting being on HP% " + GetSlider("Auto Barkskin @ HP%"), Color.Purple);
                    }
                    Aimsharp.Cast("Barkskin");
                    return true;
                }
            }
            #endregion

            #region Queues
            //Queue Rebirth
            bool Rebirth = Aimsharp.IsCustomCodeOn("Rebirth");
            if (Aimsharp.SpellCooldown("Rebirth") - Aimsharp.GCD() > 2000 && Rebirth)
            {
                Aimsharp.Cast("RebirthOff");
                return true;
            }

            if (Rebirth && Aimsharp.CanCast("Rebirth", "mouseover", true, true))
            {
                Aimsharp.Cast("RebirthMO");
                return true;
            }

            //Queue Hibernate
            bool Hibernate = Aimsharp.IsCustomCodeOn("Hibernate");
            if (Aimsharp.SpellCooldown("Hibernate") - Aimsharp.GCD() > 2000 && Hibernate)
            {
                Aimsharp.Cast("HibernateOff");
                return true;
            }

            if (Hibernate && Aimsharp.CanCast("Hibernate", "mouseover", true, true))
            {
                Aimsharp.Cast("HibernateMO");
                return true;
            }

            //Queue Cyclone
            bool Cyclone = Aimsharp.IsCustomCodeOn("Cyclone");
            if (Aimsharp.SpellCooldown("Cyclone") - Aimsharp.GCD() > 2000 && Cyclone)
            {
                Aimsharp.Cast("CycloneOff");
                return true;
            }

            if (Cyclone && Aimsharp.CanCast("Cyclone", "mouseover", true, true))
            {
                Aimsharp.Cast("CycloneMO");
                return true;
            }

            //Queue Entangling Roots
            bool EntanglingRoots = Aimsharp.IsCustomCodeOn("EntanglingRoots");
            if (Aimsharp.SpellCooldown("Entangling Roots") - Aimsharp.GCD() > 2000 && EntanglingRoots)
            {
                Aimsharp.Cast("EntanglingRootsOff");
                return true;
            }

            if (EntanglingRoots && Aimsharp.CanCast("Entangling Roots", "mouseover", true, true))
            {
                Aimsharp.Cast("EntanglingRootsMO");
                return true;
            }

            //Queue Innervate
            bool Innervate = Aimsharp.IsCustomCodeOn("Innervate");
            if (Aimsharp.SpellCooldown("Innervate") - Aimsharp.GCD() > 2000 && Innervate)
            {
                Aimsharp.Cast("InnervateOff");
                return true;
            }

            if (Innervate && Aimsharp.CanCast("Innervate", "mouseover", true, true))
            {
                Aimsharp.Cast("InnervateMO");
                return true;
            }

            bool Typhoon = Aimsharp.IsCustomCodeOn("Typhoon");
            //Queue Typhoon
            if (Typhoon && Aimsharp.SpellCooldown("Typhoon") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Typhoon queue toggle", Color.Purple);
                }
                Aimsharp.Cast("TyphoonOff");
                return true;
            }

            if (Typhoon && Aimsharp.CanCast("Typhoon", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Typhoon through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Typhoon");
                return true;
            }

            bool MightyBash = Aimsharp.IsCustomCodeOn("MightyBash");
            //Queue Mighty Bash
            if (MightyBash && Aimsharp.SpellCooldown("Mighty Bash") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Mighty Bash queue toggle", Color.Purple);
                }
                Aimsharp.Cast("MightyBashOff");
                return true;
            }

            if (MightyBash && Aimsharp.CanCast("Mighty Bash", "target", true, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Mighty Bash through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Mighty Bash");
                return true;
            }

            bool MassEntanglement = Aimsharp.IsCustomCodeOn("MassEntanglement");
            //Queue Mass Entanglement
            if (MassEntanglement && Aimsharp.SpellCooldown("Mass Entanglement") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Mass Entanglement queue toggle", Color.Purple);
                }
                Aimsharp.Cast("MassEntanglementOff");
                return true;
            }

            if (MassEntanglement && Aimsharp.CanCast("Mass Entanglement", "target", true, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Mass Entanglement through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Mass Entanglement");
                return true;
            }

            //Queue Force of Nature
            string ForceofNatureCast = GetDropDown("Force of Nature Cast:");
            bool ForceofNature = Aimsharp.IsCustomCodeOn("ForceofNature");
            if (Aimsharp.SpellCooldown("Force of Nature") - Aimsharp.GCD() > 2000 && ForceofNature)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Force of Nature Queue", Color.Purple);
                }
                Aimsharp.Cast("ForceofNatureOff");
                return true;
            }

            if (ForceofNature && Aimsharp.CanCast("Force of Nature", "player", false, true))
            {
                switch (ForceofNatureCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Force of Nature - " + ForceofNatureCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Force of Nature");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Force of Nature - " + ForceofNatureCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("ForceofNatureP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Force of Nature - " + ForceofNatureCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("ForceofNatureC");
                        return true;
                }
            }

            //Queue Ursol's Vortex
            string UrsolsVortexCast = GetDropDown("Ursol's Vortex Cast:");
            bool UrsolsVortex = Aimsharp.IsCustomCodeOn("UrsolsVortex");
            if (Aimsharp.SpellCooldown("Ursol's Vortex") - Aimsharp.GCD() > 2000 && UrsolsVortex)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Ursol's Vortex Queue", Color.Purple);
                }
                Aimsharp.Cast("UrsolsVortexOff");
                return true;
            }

            if (UrsolsVortex && Aimsharp.CanCast("Ursol's Vortex", "player", false, true))
            {
                switch (UrsolsVortexCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Ursol's Vortex - " + UrsolsVortexCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Ursol's Vortex");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Ursol's Vortex - " + UrsolsVortexCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("UrsolsVortexP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Ursol's Vortex - " + UrsolsVortexCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("UrsolsVortexC");
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

            bool Regrowth = Aimsharp.IsCustomCodeOn("Regrowth");
            if (Regrowth && Aimsharp.CanCast("Regrowth", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Spamming Regrowth through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Regrowth");
                return true;
            }
            #endregion

            #region Remove Corruption
            if (!NoDecurse && CursePoisonCheck > 0 && Aimsharp.GroupSize() <= 5 && Aimsharp.LastCast() != "Remove Corruption")
            {

                Random rdCurse = new Random();
                int DecurseValue = rdCurse.Next(200,300);                
                
                PartyDict.Clear();
                PartyDict.Add("player", Aimsharp.Health("player"));

                var partysize = Aimsharp.GroupSize();
                for (int i = 1; i < partysize; i++)
                {
                    var partyunit = ("party" + i);
                    if (Aimsharp.Health(partyunit) > 0 && Aimsharp.Range(partyunit) <= 40)
                    {
                        PartyDict.Add(partyunit, Aimsharp.Health(partyunit));
                    }
                }

                int states = Aimsharp.CustomFunction("CursePoisonCheck");
                CleansePlayers target;

                foreach (var unit in PartyDict.OrderBy(unit => unit.Value))
                {
                    Enum.TryParse(unit.Key, out target);
                    if (Aimsharp.CanCast("Remove Corruption", unit.Key, false, true) && (unit.Key == "player" || Aimsharp.Range(unit.Key) <= 40) && isUnitCleansable(target, states))
                    {
                        if (!UnitFocus(unit.Key))
                        {
                            System.Threading.Thread.Sleep(DecurseValue);
                            Aimsharp.Cast("FOC_" + unit.Key, true);
                            return true;
                        }
                        else
                        {
                            if (UnitFocus(unit.Key))
                            {
                                System.Threading.Thread.Sleep(DecurseValue);
                                Aimsharp.Cast("RC_FOC");
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Remove Corruption @ " + unit.Key + " - " + unit.Value, Color.Purple);
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            #endregion

            #region Auto Target
            //Hekili Cycle
            if (!NoCycle && Aimsharp.CustomFunction("HekiliCycle") == 1 && Enemies > 1)
            {
                System.Threading.Thread.Sleep(50);
                Aimsharp.Cast("TargetEnemy");
                System.Threading.Thread.Sleep(50);
                return true;
            }

            //Auto Target
            if (!NoCycle && (!Enemy || Enemy && !TargetAlive() || Enemy && !TargetInCombat) && (EnemiesInMelee > 0 || TargetingGroup > 0))
            {
                System.Threading.Thread.Sleep(50);
                Aimsharp.Cast("TargetEnemy");
                System.Threading.Thread.Sleep(50);
                return true;
            }
            #endregion

            if (Aimsharp.TargetIsEnemy() && TargetAlive() && TargetInCombat && !Regrowth)
            {
                if (Wait <= 200)
                {
                    #region Mouseover Spells
                    //Soothe Mouseover
                    if (Aimsharp.CanCast("Soothe", "mouseover", true, true))
                    {
                        if (MOSoothe && EnrageBuffMO == 3)
                        {
                            Aimsharp.Cast("SootheMO");
                            if (Debug)
                            {
                                Aimsharp.PrintMessage("Casting Soothe (Mouseover)", Color.Purple);
                            }
                            return true;
                        }
                    }

                    //Soothe Target
                    if (Aimsharp.CanCast("Soothe", "target", true, true))
                    {
                        if (TargetSoothe && EnrageBuffTarget == 3)
                        {
                            Aimsharp.Cast("Soothe");
                            if (Debug)
                            {
                                Aimsharp.PrintMessage("Casting Soothe (Target)", Color.Purple);
                            }
                            return true;
                        }
                    }
                    #endregion

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
                    //Racials
                    if (SpellID1 == 28880 && Aimsharp.CanCast("Gift of the Naaru", "player", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Gift of the Naaru - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Gift of the Naaru");
                        return true;
                    }

                    if (SpellID1 == 20594 && Aimsharp.CanCast("Stoneform", "player", true, true))
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

                    if (SpellID1 == 255647 && Aimsharp.CanCast("Light's Judgment", "player", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Light's Judgment - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Light's Judgment");
                        return true;
                    }

                    if (SpellID1 == 265221 && Aimsharp.CanCast("Fireblood", "player", true, true))
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

                    if (SpellID1 == 20549 && Aimsharp.CanCast("War Stomp", "player", true, true))
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

                    if (SpellID1 == 260364 && Aimsharp.CanCast("Arcane Pulse", "player", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Arcane Pulse - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Arcane Pulse");
                        return true;
                    }

                    if (SpellID1 == 255654 && Aimsharp.CanCast("Bull Rush", "player", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Bull Rush - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Bull Rush");
                        return true;
                    }

                    if (SpellID1 == 312411 && Aimsharp.CanCast("Bag of Tricks", "player", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Bag of Tricks - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Bag of Tricks");
                        return true;
                    }

                    if ((SpellID1 == 20572 || SpellID1 == 33702 || SpellID1 == 33697) && Aimsharp.CanCast("Blood Fury", "player", true, true))
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

                    if ((SpellID1 == 28730 || SpellID1 == 25046 || SpellID1 == 50613 || SpellID1 == 69179 || SpellID1 == 80483 || SpellID1 == 129597) && Aimsharp.CanCast("Arcane Torrent", "player", true, false))
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
                    ///Covenants 323764
                    if (SpellID1 == 391528 && Aimsharp.CanCast("Convoke the Spirits", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Convoke the Spirits - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Convoke the Spirits");
                        return true;
                    }

                    if (SpellID1 == 325727 && Aimsharp.CanCast("Adaptive Swarm", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Adaptive Swarm - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Adaptive Swarm");
                        return true;
                    }

                    if (SpellID1 == 323546 && Aimsharp.CanCast("Ravenous Frenzy", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Ravenous Frenzy - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Ravenous Frenzy");
                        return true;
                    }

                    if ((SpellID1 == 327139 || SpellID1 == 327022 || SpellID1 == 327148 || SpellID1 == 327071 || SpellID1 == 326647) && Aimsharp.CanCast("Empower Bond", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Empower Bond - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Empower Bond");
                        return true;
                    }

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

                    #region General Spells - No GCD
                    ///Class Spells
                    //Target - No GCD
                    if (SpellID1 == 78675 && Aimsharp.CanCast("Solar Beam", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Solar Beam - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Solar Beam", true);
                        return true;
                    }
                    
                    

                    if (SpellID1 == 102560 && Aimsharp.CanCast("Incarnation: Chosen of Elune", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Incarnation: Chosen of Elune - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Incarnation: Chosen of Elune", true);
                        return true;
                    }
                    #endregion

                    #region General Spells - Target GCD
                    //Target - GCD
                    if (SpellID1 == 8921 && Aimsharp.CanCast("Moonfire", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Moonfire - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Moonfire");
                        return true;
                    }

                    if (SpellID1 == 383410 && Aimsharp.CanCast("Celestial Alignment", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Celestial Alignment - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("CelestialAlignmentC");
                        return true;
                    }

                    if (SpellID1 == 190984 && Aimsharp.CanCast("Wrath", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Wrath - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Wrath");
                        return true;
                    }

                    if (SpellID1 == 2908 && Aimsharp.CanCast("Soothe", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Soothe - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Soothe");
                        return true;
                    }
                    #endregion

                    #region General Spells - Player GCD
                    if (SpellID1 == 5487 && Aimsharp.CanCast("Bear Form", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Bear Form - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Bear Form");
                        return true;
                    }

                    if (SpellID1 == 768 && Aimsharp.CanCast("Cat Form", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Cat Form - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Cat Form");
                        return true;
                    }

                    if (SpellID1 == 114282 && Aimsharp.CanCast("Treant Form", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Treant Form - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Treant Form");
                        return true;
                    }

                    if (SpellID1 == 22812 && Aimsharp.CanCast("Barkskin", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Barkskin - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Barkskin");
                        return true;
                    }
                    #endregion

                    #region Balance - Target GCD
                    if ((SpellID1 == 274282 || SpellID1 == 274282) && Aimsharp.CanCast("Half Moon", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Half Moon - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Half Moon");
                        return true;
                    }

                    if (SpellID1 == 274283 && Aimsharp.CanCast("Full Moon", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Full Moon - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Full Moon");
                        return true;
                    }

                    if (SpellID1 == 194153 && Aimsharp.CanCast("Starfire", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Starfire - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Starfire");
                        return true;
                    }

                    if (SpellID1 == 78674 && Aimsharp.CanCast("Starsurge", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Starsurge - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Starsurge");
                        return true;
                    }

                    if (SpellID1 == 93402 && Aimsharp.CanCast("Sunfire", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Sunfire - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Sunfire");
                        return true;
                    }

                    if (SpellID1 == 202347 && Aimsharp.CanCast("Stellar Flare", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Stellar Flare - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Stellar Flare");
                        return true;
                    }

                    if (SpellID1 == 202770 && Aimsharp.CanCast("Fury of Elune", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Fury of Elune - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Fury of Elune");
                        return true;
                    }

                    if (SpellID1 == 88747 && Aimsharp.CanCast("Wild Mushroom", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Wild Mushroom - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Wild Mushroom");
                        return true;
                    }

                    if (SpellID1 == 274281 && Aimsharp.CanCast("New Moon", "target", true, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting New Moon - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("New Moon");
                        return true;
                    }
                    #endregion

                    #region Balance - Player GCD
                    if (SpellID1 == 205636 && Aimsharp.CanCast("Force of Nature", "player", false, true))
                    {
                        switch (ForceofNatureCast)
                        {
                            case "Manual":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Force of Nature - " + ForceofNatureCast, Color.Purple);
                                }
                                Aimsharp.Cast("Force of Nature");
                                return true;
                            case "Player":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Force of Nature - " + ForceofNatureCast, Color.Purple);
                                }
                                Aimsharp.Cast("ForceofNatureP");
                                return true;
                            case "Cursor":
                                if (Debug)
                                {
                                    Aimsharp.PrintMessage("Casting Force of Nature - " + ForceofNatureCast, Color.Purple);
                                }
                                Aimsharp.Cast("ForceofNatureC");
                                return true;
                        }
                    }

                    if (SpellID1 == 24858 && Aimsharp.CanCast("Moonkin Form", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Moonkin Form - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Moonkin Form");
                        return true;
                    }

                    if (SpellID1 == 191034 && Aimsharp.CanCast("Starfall", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Starfall - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Starfall");
                        return true;
                    }

                    if (SpellID1 == 202425 && Aimsharp.CanCast("Warrior of Elune", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Warrior of Elune - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Warrior of Elune");
                        return true;
                    }

                    if (SpellID1 == 319454 && Aimsharp.CanCast("Heart of the Wild", "player", false, true))
                    {
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Heart of the Wild - " + SpellID1, Color.Purple);
                        }
                        Aimsharp.Cast("Heart of the Wild");
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
            int PhialCount = Aimsharp.CustomFunction("PhialCount");
            bool TargetInCombat = Aimsharp.InCombat("target") || SpecialUnitList.Contains(Aimsharp.UnitID("target")) || !InstanceIDList.Contains(Aimsharp.GetMapID());
            bool Moving = Aimsharp.PlayerIsMoving();
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

            #region Above Pause Logic
            if (Aimsharp.CastingID("player") == 2637 && Aimsharp.CastingRemaining("player") > 0 && Aimsharp.CastingRemaining("player") <= 400 && Aimsharp.IsCustomCodeOn("Hibernate"))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Hibernate Queue", Color.Purple);
                }
                Aimsharp.Cast("HibernateOff");
                return true;
            }

            if (Aimsharp.CastingID("player") == 33786 && Aimsharp.CastingRemaining("player") > 0 && Aimsharp.CastingRemaining("player") <= 400 && Aimsharp.IsCustomCodeOn("Cyclone"))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Cyclone Queue", Color.Purple);
                }
                Aimsharp.Cast("CycloneOff");
                return true;
            }

            if (Aimsharp.CastingID("player") == 339 && Aimsharp.CastingRemaining("player") > 0 && Aimsharp.CastingRemaining("player") <= 400 && Aimsharp.IsCustomCodeOn("EntanglingRoots"))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Entangling Roots Queue", Color.Purple);
                }
                Aimsharp.Cast("EntanglingRootsOff");
                return true;
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

            if (Aimsharp.IsCustomCodeOn("UrsolsVortex") && Aimsharp.SpellCooldown("Ursol's Vortex") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("ForceofNature") && Aimsharp.SpellCooldown("Force of Nature") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }

            if (Aimsharp.IsCustomCodeOn("DoorofShadows") && Aimsharp.SpellCooldown("Door of Shadows") - Aimsharp.GCD() <= 0 && Aimsharp.CustomFunction("IsRMBDown") == 1)
            {
                return false;
            }
            #endregion

            #region Queues
            //Queue Rebirth
            bool Rebirth = Aimsharp.IsCustomCodeOn("Rebirth");
            if (Aimsharp.SpellCooldown("Rebirth") - Aimsharp.GCD() > 2000 && Rebirth)
            {
                Aimsharp.Cast("RebirthOff");
                return true;
            }

            if (Rebirth && Aimsharp.CanCast("Rebirth", "mouseover", true, true))
            {
                Aimsharp.Cast("RebirthMO");
                return true;
            }

            //Queue Hibernate
            bool Hibernate = Aimsharp.IsCustomCodeOn("Hibernate");
            if (Aimsharp.SpellCooldown("Hibernate") - Aimsharp.GCD() > 2000 && Hibernate)
            {
                Aimsharp.Cast("HibernateOff");
                return true;
            }

            if (Hibernate && Aimsharp.CanCast("Hibernate", "mouseover", true, true))
            {
                Aimsharp.Cast("HibernateMO");
                return true;
            }

            //Queue Cyclone
            bool Cyclone = Aimsharp.IsCustomCodeOn("Cyclone");
            if (Aimsharp.SpellCooldown("Cyclone") - Aimsharp.GCD() > 2000 && Cyclone)
            {
                Aimsharp.Cast("CycloneOff");
                return true;
            }

            if (Cyclone && Aimsharp.CanCast("Cyclone", "mouseover", true, true))
            {
                Aimsharp.Cast("CycloneMO");
                return true;
            }

            //Queue Entangling Roots
            bool EntanglingRoots = Aimsharp.IsCustomCodeOn("EntanglingRoots");
            if (Aimsharp.SpellCooldown("Entangling Roots") - Aimsharp.GCD() > 2000 && EntanglingRoots)
            {
                Aimsharp.Cast("EntanglingRootsOff");
                return true;
            }

            if (EntanglingRoots && Aimsharp.CanCast("Entangling Roots", "mouseover", true, true))
            {
                Aimsharp.Cast("EntanglingRootsMO");
                return true;
            }

            //Queue Innervate
            bool Innervate = Aimsharp.IsCustomCodeOn("Innervate");
            if (Aimsharp.SpellCooldown("Innervate") - Aimsharp.GCD() > 2000 && Innervate)
            {
                Aimsharp.Cast("InnervateOff");
                return true;
            }

            if (Innervate && Aimsharp.CanCast("Innervate", "mouseover", true, true))
            {
                Aimsharp.Cast("InnervateMO");
                return true;
            }

            bool Typhoon = Aimsharp.IsCustomCodeOn("Typhoon");
            //Queue Typhoon
            if (Typhoon && Aimsharp.SpellCooldown("Typhoon") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Typhoon queue toggle", Color.Purple);
                }
                Aimsharp.Cast("TyphoonOff");
                return true;
            }

            if (Typhoon && Aimsharp.CanCast("Typhoon", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Typhoon through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Typhoon");
                return true;
            }

            bool MightyBash = Aimsharp.IsCustomCodeOn("MightyBash");
            //Queue Mighty Bash
            if (MightyBash && Aimsharp.SpellCooldown("Mighty Bash") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Mighty Bash queue toggle", Color.Purple);
                }
                Aimsharp.Cast("MightyBashOff");
                return true;
            }

            if (MightyBash && Aimsharp.CanCast("Mighty Bash", "target", true, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Mighty Bash through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Mighty Bash");
                return true;
            }

            bool MassEntanglement = Aimsharp.IsCustomCodeOn("MassEntanglement");
            //Queue Mass Entanglement
            if (MassEntanglement && Aimsharp.SpellCooldown("Mass Entanglement") - Aimsharp.GCD() > 2000)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Mass Entanglement queue toggle", Color.Purple);
                }
                Aimsharp.Cast("MassEntanglementOff");
                return true;
            }

            if (MassEntanglement && Aimsharp.CanCast("Mass Entanglement", "target", true, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Casting Mass Entanglement through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Mass Entanglement");
                return true;
            }

            //Queue Force of Nature
            string ForceofNatureCast = GetDropDown("Force of Nature Cast:");
            bool ForceofNature = Aimsharp.IsCustomCodeOn("ForceofNature");
            if (Aimsharp.SpellCooldown("Force of Nature") - Aimsharp.GCD() > 2000 && ForceofNature)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Force of Nature Queue", Color.Purple);
                }
                Aimsharp.Cast("ForceofNatureOff");
                return true;
            }

            if (ForceofNature && Aimsharp.CanCast("Force of Nature", "player", false, true))
            {
                switch (ForceofNatureCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Force of Nature - " + ForceofNatureCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Force of Nature");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Force of Nature - " + ForceofNatureCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("ForceofNatureP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Force of Nature - " + ForceofNatureCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("ForceofNatureC");
                        return true;
                }
            }

            //Queue Ursol's Vortex
            string UrsolsVortexCast = GetDropDown("Ursol's Vortex Cast:");
            bool UrsolsVortex = Aimsharp.IsCustomCodeOn("UrsolsVortex");
            if (Aimsharp.SpellCooldown("Ursol's Vortex") - Aimsharp.GCD() > 2000 && UrsolsVortex)
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Turning Off Ursol's Vortex Queue", Color.Purple);
                }
                Aimsharp.Cast("UrsolsVortexOff");
                return true;
            }

            if (UrsolsVortex && Aimsharp.CanCast("Ursol's Vortex", "player", false, true))
            {
                switch (UrsolsVortexCast)
                {
                    case "Manual":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Ursol's Vortex - " + UrsolsVortexCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("Ursol's Vortex");
                        return true;
                    case "Player":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Ursol's Vortex - " + UrsolsVortexCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("UrsolsVortexP");
                        return true;
                    case "Cursor":
                        if (Debug)
                        {
                            Aimsharp.PrintMessage("Casting Ursol's Vortex - " + UrsolsVortexCast + " - Queue", Color.Purple);
                        }
                        Aimsharp.Cast("UrsolsVortexC");
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

            bool Regrowth = Aimsharp.IsCustomCodeOn("Regrowth");
            if (Regrowth && Aimsharp.CanCast("Regrowth", "player", false, true))
            {
                if (Debug)
                {
                    Aimsharp.PrintMessage("Spamming Regrowth through queue toggle", Color.Purple);
                }
                Aimsharp.Cast("Regrowth");
                return true;
            }
            #endregion

            #region Out of Combat Spells
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
            if (PhialCount <= 0 && Aimsharp.CanCast("Summon Steward", "player") && Aimsharp.GetMapID() != 2286 && Aimsharp.GetMapID() != 1666 && Aimsharp.GetMapID() != 1667 && Aimsharp.GetMapID() != 1668 && Aimsharp.CastingID("player") == 0)
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
            if (GetCheckBox("Auto Start Combat:") == true && Aimsharp.TargetIsEnemy() && TargetAlive() && Aimsharp.Range("target") <= 45 && TargetInCombat)
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