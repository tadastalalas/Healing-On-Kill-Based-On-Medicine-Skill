using MCM.Abstractions.Base.Global;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace HealingOnKillBasedOnMedicineSkill
{
    public class HealOnKillBasedOnMedicineSkill : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        private bool isInitialized = false;
        private List<Agent> allHeroes = new List<Agent>();
        private readonly MCMSettings settings = AttributeGlobalSettings<MCMSettings>.Instance ?? new MCMSettings();
        private float checkInterval = 5.0f;
        private float elapsedTime = 0.0f;

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            InitializeOnce();
            elapsedTime += dt;
            if (elapsedTime >= checkInterval)
            {
                elapsedTime = 0.0f;
                CheckForReinforcements();
            }
        }

        private void InitializeOnce()
        {
            if (!this.isInitialized) {
                this.PopulateAllAgentsInMissionList();
                this.isInitialized = true;
            }
        }

        private void PopulateAllAgentsInMissionList()
        {
            this.allHeroes.Clear();
            foreach (Agent agent in base.Mission.AllAgents)
            {
                if (agent.IsHero && agent.State == AgentState.Active)
                    this.allHeroes.Add(agent);
            }
        }

        private void CheckForReinforcements()
        {
            bool reinforcementsAdded = false;
            foreach (Agent agent in base.Mission.AllAgents)
            {
                if (agent.IsHero && agent.State == AgentState.Active && !this.allHeroes.Contains(agent))
                {
                    reinforcementsAdded = true;
                    break;
                }
            }

            if (reinforcementsAdded)
                this.PopulateAllAgentsInMissionList();
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);

            if (!settings.EnableHealingOnEnemyKill || !affectorAgent.IsHero)
                return;

            if (affectedAgent == null || affectorAgent == null || affectedAgent == affectorAgent)
                return;

            foreach (Agent agent in this.allHeroes)
            {
                if (agent != null && agent == affectorAgent)
                {
                    int maxHPToHeal = settings.MaxHPToHeal;
                    int maxMedSkillThreshold = settings.MaxMedSkillThreshold;
                    int currentMedSkill = GetMedicineSkillLevel(agent);
                    float calculatedNumber = ((float)currentMedSkill / ((float)maxMedSkillThreshold / (float)maxHPToHeal));

                    if (calculatedNumber > settings.MaxHPToHeal)
                        calculatedNumber = maxHPToHeal;

                    int amountToHeal = TaleWorlds.Library.MathF.Round(calculatedNumber);

                    if (affectedAgent.IsHero)
                    {
                        this.PopulateAllAgentsInMissionList();
                        amountToHeal *= 2;
                    }

                    agent.Health += amountToHeal;
                    if (agent.Health > agent.HealthLimit)
                        agent.Health = agent.HealthLimit;

                    if (settings.ShowDetailedInformation)
                        DisplayDetailedInformationMessage(agent, amountToHeal, currentMedSkill, calculatedNumber, affectedAgent);

                    if (settings.ShowWhenHeroKillsAnotherHero && affectedAgent.IsHero)
                    {
                        var message = new TextObject("{=HOKBOMS_93u1X}{HERO} knocked down {AFFECTED_AGENT}. Recovered {HEAL_AMOUNT} HP.")
                            .SetTextVariable("HERO", agent.Name)
                            .SetTextVariable("AFFECTED_AGENT", affectedAgent.Name)
                            .SetTextVariable("HEAL_AMOUNT", amountToHeal);

                        MBInformationManager.AddQuickInformation(new TextObject(message.ToString()), 2000, null, "event:/ui/notification/levelup");
                    }
                }
            }
        }

        private void DisplayDetailedInformationMessage(Agent agent, int amountToHeal, float currentMedSkill, float calculatedNumber, Agent affectedAgent)
        {
            var message = new TextObject("{=HOKBOMS_1ccm4}{HERO} killed {AFFECTED_AGENT}. + {HEAL_AMOUNT} HP.\nMedicine skill: {MED_SKILL}. Float: {CALC_HEAL}")
                .SetTextVariable("HERO", agent.Name)
                .SetTextVariable("AFFECTED_AGENT", affectedAgent.Name)
                .SetTextVariable("HEAL_AMOUNT", amountToHeal)
                .SetTextVariable("MED_SKILL", currentMedSkill)
                .SetTextVariable("CALC_HEAL", calculatedNumber.ToString("F2"));
            InformationManager.DisplayMessage(new InformationMessage(message.ToString(), Colors.Red));
        }

        private static int GetMedicineSkillLevel(Agent agent)
        {
            if (agent != null && agent.Character != null && agent.Character.IsHero)
                return agent.Character.GetSkillValue(DefaultSkills.Medicine);
            return 0;
        }
    }
}