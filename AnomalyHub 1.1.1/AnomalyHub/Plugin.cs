using System;
using Exiled.API.Features;
using SRV = Exiled.Events.Handlers.Server;

namespace AnomalyHub
{

    public sealed class Plugin : Plugin<Config>
    {
        public override string Author => "Treaxy";

        public override string Name => "AnomalyHub";

        public override Version Version => new Version(1, 1, 1);

        public override string Prefix => Name;

        public static Plugin Instance;

        private EventHandlers _handlers;

        public override void OnEnabled()
        {
            Instance = this;

            RegisterEvents();

            base.OnEnabled();
            Log.Info("Successfully Active, By Treaxy");
        }

        public override void OnDisabled()
        {
            UnregisterEvents();

            Instance = null;

            base.OnDisabled();
            Log.Info("Successfully DeActive, By Treaxy");
        }

        private void RegisterEvents()
        {
            _handlers = new EventHandlers();
            SRV.RoundEnded += _handlers.AutoFF;
            SRV.RespawningTeam += _handlers.Spawn;
            SRV.RoundStarted += _handlers.OnRoundStarted;
            base.OnEnabled();
            Log.Info("REGISTERED");
        }

        private void UnregisterEvents()
        {
            SRV.RoundEnded -= _handlers.AutoFF;
            SRV.RespawningTeam -= _handlers.Spawn;
            SRV.RoundStarted -= _handlers.OnRoundStarted;
            _handlers = null;
            base.OnDisabled();
            Log.Info("UNREGISTERED");

        }
    }

}