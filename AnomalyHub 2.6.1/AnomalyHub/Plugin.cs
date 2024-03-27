using System;
using Exiled.API.Features;
using SRV = Exiled.Events.Handlers.Server;

namespace AnomalyHub
{

    public sealed class Plugin : Plugin<Config>
    {
        public override string Author => "Treaxy";

        public override string Name => "AnomalyHub";

        public override Version Version => new Version(2, 6, 1);

        public override string Prefix => Name;

        public static Plugin Instance;

        private EventHandlers _handlers;

        public override void OnEnabled()
        {
            Instance = this;

            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();

            Instance = null;

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _handlers = new EventHandlers();
            SRV.RoundEnded += _handlers.AutoFF;
            Exiled.Events.Handlers.Player.Verified += _handlers.PlayerEnter;
            Exiled.Events.Handlers.Player.Spawning += _handlers.NewHealths;
            Exiled.Events.Handlers.Player.ChangingItem += _handlers.AntiItems;
            Exiled.Events.Handlers.Player.Hurting += _handlers.InstaKill;
            SRV.RespawningTeam += _handlers.Spawn;
            SRV.WaitingForPlayers += _handlers.PlayerWait;
            SRV.RoundStarted += _handlers.OnRoundStarted;
            base.OnEnabled();
            Log.Info("REGISTERED");
        }

        private void UnregisterEvents()
        {
            SRV.RoundEnded -= _handlers.AutoFF;
            Exiled.Events.Handlers.Player.Verified -= _handlers.PlayerEnter;
            Exiled.Events.Handlers.Player.Spawning -= _handlers.NewHealths;
            Exiled.Events.Handlers.Player.ChangingItem -= _handlers.AntiItems;
            Exiled.Events.Handlers.Player.Hurting -= _handlers.InstaKill;
            SRV.RespawningTeam -= _handlers.Spawn;
            SRV.WaitingForPlayers -= _handlers.PlayerWait;
            SRV.RoundStarted -= _handlers.OnRoundStarted;
            _handlers = null;
            base.OnDisabled();
            Log.Info("UNREGISTERED");
        }
    }

}