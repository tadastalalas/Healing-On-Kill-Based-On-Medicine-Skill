using System.Reflection;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace HealingOnKillBasedOnMedicineSkill
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);
            mission.AddMissionBehavior(new HealOnKillBasedOnMedicineSkill());
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            if (Campaign.Current is Campaign campaign && campaign.GameMode == CampaignGameMode.Campaign)
            {
                CampaignGameStarter campaignGameStarter = (CampaignGameStarter)gameStarterObject;
                campaignGameStarter.AddBehavior(new GiveMedicineExpForWoundedHeroes());
            }
        }

        public override void OnGameEnd(Game game)
        {
            var eventField = typeof(CampaignEvents).GetField("HourlyTickEvent", BindingFlags.Static | BindingFlags.NonPublic);
            var eventDelegate = (MulticastDelegate)eventField?.GetValue(null);
            if (eventDelegate != null && eventDelegate.GetInvocationList().Length > 0)
            {
                CampaignEvents.HourlyTickEvent.ClearListeners(this);
            }
            base.OnGameEnd(game);
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
        }
    }
}