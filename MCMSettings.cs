using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Localization;

namespace HealingOnKillBasedOnMedicineSkill
{
    internal class MCMSettings : AttributeGlobalSettings<MCMSettings>
    {
        public override string Id
        { get { return "HealingOnKillBasedOnMedicineSkillSettings"; } }

        public override string DisplayName
        { get { return new TextObject("{=HOKBOMS_WxWoy}Healing On Kill Based On Medicine Skill").ToString(); } }

        public override string FolderName
        { get { return "HealingOnKillBasedOnMedicineSkill"; } }

        public override string FormatType
        { get { return "json2"; } }

        [SettingPropertyBool("{=HOKBOMS_qwsYu}Healing on enemy killed", Order = 0, RequireRestart = false, HintText = "{=HOKBOMS_YikUB}Enable/Disable this modification. [Default: Enabled]")]
        [SettingPropertyGroup("{=HOKBOMS_mfxHR}Main modification settings", GroupOrder = 0)]
        public bool EnableHealingOnEnemyKill { get; set; } = true;

        [SettingPropertyBool("{=HOKBOMS_AYLgG}Minimum healing of 1 health", Order = 1, RequireRestart = false, HintText = "{=HOKBOMS_2fbFj}Minimum healing of 1 health even if hero has too low medicine skill. [Default: Enabled]")]
        [SettingPropertyGroup("{=HOKBOMS_mfxHR}Main modification settings", GroupOrder = 0)]
        public bool EnableMinimumHealingOfOneHealth { get; set; } = true;

        [SettingPropertyInteger("{=HOKBOMS_UkXap}Medicine skill threshold", 50, 600, "0", Order = 2, RequireRestart = false, HintText = "{=HOKBOMS_d0Tuf}Maximum medicine skill when healing stops increasing.\nAt this skill level hero will be healed by 'Maximum healing amount' setting below. [Default: 300]")]
        [SettingPropertyGroup("{=HOKBOMS_mfxHR}Main modification settings", GroupOrder = 0)]
        public int MaxMedSkillThreshold { get; set; } = 300;

        [SettingPropertyInteger("{=HOKBOMS_ccEcT}Maximum healing amount", 1, 20, "0", Order = 3, RequireRestart = false, HintText = "{=HOKBOMS_WvB9h}Maximum health amount that a hero can be healed on enemy kill when his medicine skill reaches 'Medicine skill threshold'. [Default: 10]")]
        [SettingPropertyGroup("{=HOKBOMS_mfxHR}Main modification settings", GroupOrder = 0)]
        public int MaxHPToHeal { get; set; } = 10;

        [SettingPropertyBool("{=HOKBOMS_9d2gl}Show hero vs hero on-screen notification", Order = 4, RequireRestart = false, HintText = "{=HOKBOMS_DnQYP}Shows on-screen notification when any hero koncks down another hero. [Default: Enabled]")]
        [SettingPropertyGroup("{=HOKBOMS_mfxHR}Main modification settings", GroupOrder = 0)]
        public bool ShowWhenHeroKillsAnotherHero { get; set; } = true;

        [SettingPropertyInteger("{=HOKBOMS_5MPXR}Main heroe's healing multiplier", 1, 20, "0", Order = 5, RequireRestart = false, HintText = "{=HOKBOMS_q46Oj}Main heroe's healing will be multiplied by this number. It multiplies calculated floating number while it is not rounded yet so when medicine skill is low the increase in healing will be not substantial. Overrides [Maximum healing amount] slider above. [Default: 1]")]
        [SettingPropertyGroup("{=HOKBOMS_mfxHR}Main modification settings", GroupOrder = 0)]
        public int MainHeroeHealingMultiplier { get; set; } = 1;

        [SettingPropertyInteger("{=HOKBOMS_2DtDS}NPC heroes's healing multiplier", 1, 20, "0", Order = 6, RequireRestart = false, HintText = "{=HOKBOMS_hoh62}NPC heroes's healing will be multiplied by this number. It multiplies calculated floating number while it is not rounded yet so when medicine skill is low the increase in healing will be not substantial. Overrides [Maximum healing amount] slider above. [Default: 2]")]
        [SettingPropertyGroup("{=HOKBOMS_mfxHR}Main modification settings", GroupOrder = 0)]
        public int NPCHeroesHealingMultiplier { get; set; } = 2;

        [SettingPropertyBool("{=HOKBOMS_Lc0vb}Earn medicine experience when healing", Order = 0, RequireRestart = false, HintText = "{=HOKBOMS_3gKf6}Every alive hero in the game gets medicine skill experience when hero is healing from wounds after battle. [Default: Enabled]")]
        [SettingPropertyGroup("{=HOKBOMS_RTLLF}Medicine skill progression adjustments", GroupOrder = 1)]
        public bool AllHeroesRecieveMedicineExpWhenWounded { get; set; } = true;

        [SettingPropertyBool("{=HOKBOMS_jqX8e}Use learning rate modifier", Order = 1, RequireRestart = false, HintText = "{=HOKBOMS_bZ2sg}Calculated experience will go through learning rate modifier as intended by the game. If disabled, heroes will get raw calculated experience as it is (as if learning rate is x1). [Default: Disabled]")]
        [SettingPropertyGroup("{=HOKBOMS_RTLLF}Medicine skill progression adjustments", GroupOrder = 1)]
        public bool UseLearningRateModifier { get; set; } = false;

        [SettingPropertyInteger("{=HOKBOMS_73qfZ}Experience percentage divisor", 20, 400, "0", Order = 2, RequireRestart = false, HintText = "{=HOKBOMS_QJk86}(Medicine skill threshold / Current hero's medicine skill) / Experience percentage divisor = Percentage to earn experience for hero's medicine skill. [Default: 200]")]
        [SettingPropertyGroup("{=HOKBOMS_RTLLF}Medicine skill progression adjustments", GroupOrder = 1)]
        public int ExpPercentageDivisor { get; set; } = 200;

        [SettingPropertyBool("{=HOKBOMS_rZdVS}Show healing information", Order = 0, RequireRestart = false, HintText = "{=HOKBOMS_O9bNV}Shows which specific hero kills an enemy and how much health he heals after kill.\nAlso shows calculated floating number which is rounded to determine actual healing amount in integer number.\nFormula: currentMedSkill / (maxMedSkillThreshold / maxHPToHeal). [Default: Disabled]")]
        [SettingPropertyGroup("{=HOKBOMS_q6hG1}Debug settings", GroupOrder = 2)]
        public bool ShowDetailedInformation { get; set; } = false;

        [SettingPropertyBool("{=HOKBOMS_h6J6P}Show medicine experience information", Order = 1, RequireRestart = false, HintText = "{=HOKBOMS_1aRZ3}Shows detailed information of all heroes in the log, who gets medicine skill experience every hour. [Default: Disabled]")]
        [SettingPropertyGroup("{=HOKBOMS_q6hG1}Debug settings", GroupOrder = 2)]
        public bool ShowMedicineExperienceInformation { get; set; } = false;
    }
}