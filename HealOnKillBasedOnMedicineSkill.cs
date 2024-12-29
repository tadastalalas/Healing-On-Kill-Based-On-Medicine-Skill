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

            if (!settings.EnableHealingOnEnemyKill)
                return;

            if (affectedAgent == null || affectorAgent == null || affectedAgent == affectorAgent)
                return;

            foreach (Agent agent in this.allHeroes)
            {
                int amountToHeal = CalculateHowMuchToHeal(agent);

                if (affectedAgent.IsHero)
                    amountToHeal *= 2;

                if (agent != null && agent == affectorAgent)
                {
                    agent.Health += amountToHeal;
                    if (agent.Health > agent.HealthLimit)
                        agent.Health = agent.HealthLimit;
                }
            }
        }

        private int CalculateHowMuchToHeal(Agent agent)
        {
            int maxHPToHeal = settings.MaxHPToHeal;
            int maxMedSkillThreshold = settings.MaxMedSkillThreshold;
            int currentMedSkill = GetMedicineSkillLevel(agent);
            float calculatedNumber = ((float)currentMedSkill / ((float)maxMedSkillThreshold / (float)maxHPToHeal));
            int amountToHeal = TaleWorlds.Library.MathF.Round(calculatedNumber);
            if (settings.ShowDetailedInformation)
            {
                var message = new TextObject("{=HOKBOMS_93u1X}{HERO} healed {HEAL_AMOUNT} HP. Med skill: {MED_SKILL}.\nCalculated (float): {CALC_HEAL}")
                    .SetTextVariable("HERO", agent.Name)
                    .SetTextVariable("HEAL_AMOUNT", amountToHeal)
                    .SetTextVariable("MED_SKILL", currentMedSkill)
                    .SetTextVariable("CALC_HEAL", calculatedNumber);
                InformationManager.DisplayMessage(new InformationMessage(message.ToString(), Colors.Red));
            }
            if (calculatedNumber > maxHPToHeal)
            calculatedNumber = maxHPToHeal;
            return amountToHeal;
        }

        private static int GetMedicineSkillLevel(Agent agent)
        {
            if (agent != null && agent.Character != null && agent.Character.IsHero)
                return agent.Character.GetSkillValue(DefaultSkills.Medicine);
            return 0;
        }
    }
}