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
        { get { return new TextObject("{=HOKBOMS_1ccm4}Healing On Kill Based On Medicine Skill").ToString(); } }

        public override string FolderName
        { get { return "HealingOnKillBasedOnMedicineSkill"; } }

        public override string FormatType
        { get { return "json2"; } }

        [SettingPropertyBool("{=HOKBOMS_WxWoy}Healing on enemy killed", Order = 0, RequireRestart = false, HintText = "{=HOKBOMS_mfxHR}Enable/Disable this modification. [Default: Enabled]")]
        [SettingPropertyGroup("{=HOKBOMS_Sa1Xq}Adjust settings for personal flavor", GroupOrder = 0)]
        public bool EnableHealingOnEnemyKill { get; set; } = true;

        [SettingPropertyInteger("{=HOKBOMS_qwsYu}Medicine skill threshold", 30, 600, "0", Order = 1, RequireRestart = false, HintText = "{=HOKBOMS_YikUB}Maximum medicine skill when healing stops increasing.\nAt this skill level hero will be healed by 'Maximum healing amount' setting below. [Default: 300]")]
        [SettingPropertyGroup("{=HOKBOMS_Sa1Xq}Adjust settings for personal flavor", GroupOrder = 0)]
        public int MaxMedSkillThreshold { get; set; } = 300;

        [SettingPropertyInteger("{=HOKBOMS_UkXap}Maximum healing amount", 1, 20, "0", Order = 2, RequireRestart = false, HintText = "{=HOKBOMS_d0Tuf}Maximum health amount that a hero can be healed on enemy kill when his medicine skill reaches 'Medicine Skill Threshold'. [Default: 8]")]
        [SettingPropertyGroup("{=HOKBOMS_Sa1Xq}Adjust settings for personal flavor", GroupOrder = 0)]
        public int MaxHPToHeal { get; set; } = 8;

        [SettingPropertyBool("{=HOKBOMS_AYLgG}Show detailed information", Order = 0, RequireRestart = false, HintText = "{=HOKBOMS_2fbFj}Shows which specific hero kills an enemy and how much health he heals after kill.\nAlso shows calculated floating number which is rounded to determine actual healing amount in integer number.\nFormula: currentMedSkill / (maxMedSkillThreshold / maxHPToHeal). [Default: Disabled]")]
        [SettingPropertyGroup("{=HOKBOMS_1aRZ3}Moderator settings", GroupOrder = 1)]
        public bool ShowDetailedInformation { get; set; } = false;
    }
}