using Exiled.Events.EventArgs.Server;
using System;
using MEC;
using Respawning;
using UnityEngine;
using Cassie = Exiled.API.Features.Cassie;
using Map = Exiled.API.Features.Map;
using Server = Exiled.API.Features.Server;

namespace AnomalyHub
{
    public class EventHandlers
    {
        string AutoFF1s = Plugin.Instance.Config.AutoFF;
        public void AutoFF(RoundEndedEventArgs ev)
        {
            try
            {
                if (AutoFF1s == "true")
                {
                    Map.Broadcast(5, "FriendlyFire Active");
                    Server.FriendlyFire = true;
                }
            }
            catch (Exception e)
            {
                Map.Broadcast(60, $"AnomalyHub plugin error: {e.Message.ToString().ToUpper()}");
            }
        }

        public void Spawn(RespawningTeamEventArgs ev)
        {
            try
            {
                string CIspawnMessage = Plugin.Instance.Config.CIspawnMessage.Replace("Chaos Insurgency HasEntered from Gate Alpha", string.Copy(Plugin.Instance.Config.CIspawnMessage));

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
                    Cassie.Message(CIspawnMessage, true, true, true);
                }
            }
            catch (Exception e)
            {
                Map.Broadcast(60, $"AnomalyHub plugin error: {e.Message.ToString().ToUpper()}");
            }
        }

        public void OnRoundStarted()
        {
            try
            {
                string RoundStartMessage = Plugin.Instance.Config.RoundStartMessage;
                Map.ChangeLightsColor(Color.red);
                Cassie.Message(RoundStartMessage, true, true, true);
                Timing.CallDelayed(30f, () =>
                {
                    Map.ChangeLightsColor(Color.clear); 
                });

                if (AutoFF1s == "true")
                {
                    Server.FriendlyFire = false;
                }
            }

            catch (Exception e)
            {
                Map.Broadcast(60,
                    $"AnomalyHub plugin error: {e.Message.ToString().ToUpper()}");
            }
        }
    }
}