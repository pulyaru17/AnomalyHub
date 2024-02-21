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
        public void AutoFF(RoundEndedEventArgs ev)
        {
            try
            {
                if (AutoFF1s == true)
                {
                    Map.Broadcast(5, Plugin.Instance.Config.AutoFFBroadcast);
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
            ev.Player.Broadcast(Plugin.Instance.Config.BroadcastDuration, Plugin.Instance.Config.EnterBroadcast.Replace("%player%", ev.Player.DisplayNickname));
        }

        public void Spawn(RespawningTeamEventArgs ev)
        {
            try
            {

                if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                {
                    Timing.CallDelayed(1f, () => { Map.ChangeLightsColor(Color.clear); });
                    Timing.CallDelayed(2f, () => { Map.ChangeLightsColor(Color.blue); });
                    Timing.CallDelayed(3f, () => { Map.ChangeLightsColor(Color.clear); });
                    Timing.CallDelayed(4f, () => { Map.ChangeLightsColor(Color.blue); });
                    Timing.CallDelayed(5f, () => { Map.ChangeLightsColor(Color.clear); });
                }
                
                else
                {
                    Timing.CallDelayed(1f, () => { Map.ChangeLightsColor(Color.clear); });
                    Timing.CallDelayed(2f, () => { Map.ChangeLightsColor(Color.green); });
                    Timing.CallDelayed(3f, () => { Map.ChangeLightsColor(Color.clear); });
                    Timing.CallDelayed(4f, () => { Map.ChangeLightsColor(Color.green); });
                    Timing.CallDelayed(5f, () => { Map.ChangeLightsColor(Color.clear); });
                    Timing.CallDelayed(6f, () =>
                    {
                        Cassie.Message(Plugin.Instance.Config.CIspawnMessage, true, true, true);
                    });
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
                if (Exiled.API.Features.Round.IsLobby)
                {
                    if (AutoFF1s == true)
                    {
                        Server.FriendlyFire = false;
                        Log.Info("FF is now closed");
                    }
                }
                Map.ChangeLightsColor(Color.red);
                Cassie.Message(Plugin.Instance.Config.RoundStartMessage, true, true, true);
                Timing.CallDelayed(30f, () =>
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