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
using PlayerRoles;

namespace AnomalyHub
{
    public class EventHandlers
    {
        bool AutoFF1s = Plugin.Instance.Config.AutoFF;
        bool CiAnno = Plugin.Instance.Config.CIspawnAnnounce;
        bool CiLight = Plugin.Instance.Config.CIspawnLight;
        bool NTFLight = Plugin.Instance.Config.NTFspawnLight;
        bool UIULight = Plugin.Instance.Config.UIUspawnLight;
        bool AntiBmb = Plugin.Instance.Config.AntiBomb;
        bool AntiMic = Plugin.Instance.Config.AntiMicro;
        bool insta49 =  Plugin.Instance.Config.SCP049insta;
        bool insta106 = Plugin.Instance.Config.SCP106insta;
        bool AutoBroad = Plugin.Instance.Config.AutoBroadcast;
        bool WelcomeBroad = Plugin.Instance.Config.EnterBroadcastShow;
        bool blue = false;
        bool gr = false;
        int count = 0;
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
                Timing.KillCoroutines(Timing.RunCoroutine(AutoBro()));
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
            if (WelcomeBroad)
            {
                Map.Broadcast(Plugin.Instance.Config.EnterBroadcastDur, Plugin.Instance.Config.EnterBroadcast.Replace("%player%", ev.Player.DisplayNickname));
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

        public void AntiItems(ChangingItemEventArgs ev)
        {
            if (AntiBmb && !(ev.Item == null))
            {
                if (ev.Item.Type == ItemType.GrenadeHE)
                {
                    ev.Item.Destroy();
                }
            }
            if (AntiMic && !(ev.Item == null))
            {
                if (ev.Item.Type == ItemType.MicroHID)
                {
                    ev.Item.Destroy();
                }
            }
        }

        public void InstaKill(HurtingEventArgs ev)
        {
            if (insta49 && ev.Attacker.Role.Type == RoleTypeId.Scp049)
            {
                ev.Player.Kill(Exiled.API.Enums.DamageType.Scp049);
            }
            if (insta106 && ev.Attacker.Role.Type == RoleTypeId.Scp106)
            {
                ev.Player.Kill(Exiled.API.Enums.DamageType.Scp106);
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
                        Timing.CallDelayed(Plugin.Instance.Config.CIannoDelay, () =>
                        {
                            Cassie.Message(Plugin.Instance.Config.CIspawnMessage, true, true, true);
                        });
                    }
                }
                if (!(ev.NextKnownTeam == SpawnableTeamType.NineTailedFox || ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency))
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

        public void NewHealths(SpawningEventArgs ev)
        {
            Dictionary<RoleTypeId, float> RoleHealth = new Dictionary<RoleTypeId, float>()
            {
                { RoleTypeId.Scp096, Plugin.Instance.Config.SCP096newHP },
                { RoleTypeId.Scp106, Plugin.Instance.Config.SCP106newHP },
                { RoleTypeId.Scp173, Plugin.Instance.Config.SCP173newHP },
                { RoleTypeId.Scp939, Plugin.Instance.Config.SCP939newHP },
                { RoleTypeId.Scp049, Plugin.Instance.Config.SCP049newHP }
            };

            if (RoleHealth.ContainsKey(ev.Player.Role))
            {
                ev.Player.MaxHealth = RoleHealth[ev.Player.Role];
            }
        }

        public IEnumerator<float> AutoBro()
        {
            Map.Broadcast(5, Plugin.Instance.Config.AutoBroadcastME);

            yield return Timing.WaitForSeconds(Plugin.Instance.Config.AutoBroadcastDur);
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
                if (AutoBroad)
                {
                    Timing.RunCoroutine(AutoBro());
                }
            }

            catch (Exception e)
            {
                Log.Error(e.Message.ToString());
            }
        }
    }
}