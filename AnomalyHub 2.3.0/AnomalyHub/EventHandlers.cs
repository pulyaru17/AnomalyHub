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
using System.Collections.Generic;

namespace AnomalyHub
{
    public class EventHandlers
    {
        bool AutoFF1s = Plugin.Instance.Config.AutoFF;
        bool CiAnno = Plugin.Instance.Config.CIspawnAnnounce;
        bool CiLight = Plugin.Instance.Config.CIspawnLight;
        bool NTFLight = Plugin.Instance.Config.NTFspawnLight;
        bool UIULight = Plugin.Instance.Config.UIUspawnLight;
        bool blue = false;
        int count = 0;
        bool gr = false;
        public void AutoFF(RoundEndedEventArgs ev)
        {
            try
            {
                if (AutoFF1s)
                {
                    Map.Broadcast(Plugin.Instance.Config.AutoFFbroadcastDur, Plugin.Instance.Config.AutoFFBroadcast);
                    Server.FriendlyFire = true;
                    Log.Info("AutoFF is now OPENED!!");
                }
            }
            catch (Exception e)
            {
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
                Log.Info("AutoFF is now CLOSED!!");
            }
        }


        public IEnumerator<float> light(bool ci)
        {
            while (true)
            {
                count++;

                if (count == 5)
                {
                    Exiled.API.Features.Map.ChangeLightsColor(Color.clear);
                    count = 0;
                    break;
                }
                if (Round.IsEnded)
                {
                    break;
                }
                else if (ci)
                {
                    if (gr)
                    {
                        Exiled.API.Features.Map.ChangeLightsColor(Color.clear);
                        gr = false;
                    }
                    else
                    {
                        Exiled.API.Features.Map.ChangeLightsColor(Color.green);
                        gr = true;
                    }
                }
                else if (blue)
                {
                    Exiled.API.Features.Map.ChangeLightsColor(Color.clear);
                    blue = false;
                }
                else
                {
                    Exiled.API.Features.Map.ChangeLightsColor(Color.blue);
                    blue = true;
                }
                yield return Timing.WaitForSeconds(1f);
            }
        }
        public void Spawn(RespawningTeamEventArgs ev)
        {
            try
            {
                if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency && ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                {
                    if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                    {
                        if (NTFLight)
                        {
                            blue = true;
                            Timing.RunCoroutine(light(false));
                        }
                    }
                    if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
                    {
                        if (CiLight)
                        {
                            blue = false;
                            Timing.RunCoroutine(light(true));
                        }
                        if (CiAnno)
                        {
                            Cassie.Message(Plugin.Instance.Config.CIspawnMessage, true, true, true);
                        }
                    }
                }
                else
                {
                    if (UIULight)
                    {
                        blue = true;
                        Timing.RunCoroutine(light(false));
                    }
                }
            }
            catch (Exception e)
            {
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
                Log.Error(e.Message.ToString());
            }
        }
    }
}