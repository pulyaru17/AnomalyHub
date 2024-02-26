using Exiled.Events.EventArgs.Server;
using System;
using MEC;
using Respawning;
using UnityEngine;
using Cassie = Exiled.API.Features.Cassie;
using Map = Exiled.API.Features.Map;
using Server = Exiled.API.Features.Server;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace AnomalyHub
{
    public class EventHandlers
    {
        bool AutoFF1s = Plugin.Instance.Config.AutoFF;
        bool CIspawn = Plugin.Instance.Config.CIspawnfeature;
        public void AutoFF(RoundEndedEventArgs ev)
        {
            try
            {
                if (AutoFF1s)
                {
                    Map.Broadcast(Plugin.Instance.Config.AutoFFbroadcastDur, Plugin.Instance.Config.AutoFFBroadcast);
                    Server.FriendlyFire = true;
                    Log.Info("FF is now opened");
                }
            }
            catch (Exception e)
            {
                Map.Broadcast(60, $"AnomalyHub plugin error: {e.Message.ToString().ToUpper()}");
                Log.Error(e.Message.ToString());
            }
        }

        public void PlayerEnter(VerifiedEventArgs ev)
        {
            if (ev.Player == null)
            {
                Log.Info("Null");
                return;
            }
            ev.Player.Broadcast(Plugin.Instance.Config.EnterBroadcastDur, Plugin.Instance.Config.EnterBroadcast.Replace("%player%", ev.Player.DisplayNickname));
        }
        
        public void PlayerWait()
        {
            if (AutoFF1s)
            {
                Server.FriendlyFire = false;
                Log.Info("FF is now closed");
            }
        }

        public void Spawn(RespawningTeamEventArgs ev)
        {
            try
            {

                if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                {
                    ChangeLightColorSequence(new[] { Color.clear, Color.blue, Color.clear, Color.blue, Color.clear }, 1f);
                }
                else if (CIspawn && ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
                {
                    ChangeLightColorSequence(new[] { Color.clear, Color.green, Color.clear, Color.green, Color.clear }, 1f);
                    Timing.CallDelayed(6f, () => { Cassie.Message(Plugin.Instance.Config.CIspawnMessage, true, true, true); });
                }

                void ChangeLightColorSequence(Color[] colors, float startDelay)
                {
                    float delay = startDelay;
                    foreach (var color in colors)
                    {
                        Timing.CallDelayed(delay, () => { Map.ChangeLightsColor(color); });
                        delay++;
                    }
                }
            }
            catch (Exception e)
            {
                Map.Broadcast(60, $"AnomalyHub plugin error: {e.Message.ToString().ToUpper()}");
                Log.Error(e.Message.ToString());
            }
        }

        public void OnRoundStarted()
        {
            try
            {
                Map.ChangeLightsColor(Color.red);
                Cassie.Message(Plugin.Instance.Config.RoundStartMessage, true, true, true);
                Timing.CallDelayed(Plugin.Instance.Config.RoundStartLight, () =>
                {
                    Map.ChangeLightsColor(Color.clear); 
                });
            }

            catch (Exception e)
            {
                Map.Broadcast(60,
                    $"AnomalyHub plugin error: {e.Message.ToString().ToUpper()}");
                Log.Error(e.Message.ToString());
            }
        }
    }
}