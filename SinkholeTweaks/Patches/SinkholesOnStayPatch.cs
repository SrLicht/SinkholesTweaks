using CustomPlayerEffects;
using HarmonyLib;
using Hazards;
using MEC;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using PluginAPI.Core;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using YamlDotNet.Core.Tokens;
using Vector3 = UnityEngine.Vector3;

namespace SinkholesTweaks.Patches
{
    [HarmonyPatch(typeof(SinkholeEnvironmentalHazard), nameof(SinkholeEnvironmentalHazard.OnStay))]
    public static class SinkholesOnStayPatch
    {
        public static HashSet<ReferenceHub> AffectedHubs = new();

        public static bool Prefix(SinkholeEnvironmentalHazard __instance, ReferenceHub player)
        {
            if (SinkholesTweaks.Instance.Config.TeamsAffected.Contains(player.GetTeam()) && player.roleManager.CurrentRole is IFpcRole fpsRole)
            {
                var distance = (__instance.SourcePosition - fpsRole.FpcModule.Position).SqrMagnitudeIgnoreY();

                if (distance <= SinkholesTweaks.Instance.Config.SuctionRadius && !AffectedHubs.Contains(player))
                {
                    if (SinkholesTweaks.Instance.Config.BackroomsEntrance)
                    {
                        AffectedHubs.Add(player);
                        SinkholesTweaks.Instance.SuckPlayer(player);
                    }
                    else
                    {
                        player.playerEffectsController.EnableEffect<Corroding>(20f).AttackerHub = ReferenceHub._hostHub;
                        player.playerEffectsController.EnableEffect<PocketCorroding>();

                        if (SinkholesTweaks.Instance.Config.BroadcastOnFall && !string.IsNullOrEmpty(SinkholesTweaks.Instance.Config.BroadcastText))
                            Server.Broadcast.TargetAddElement(player.connectionToClient, SinkholesTweaks.Instance.Config.BroadcastText, SinkholesTweaks.Instance.Config.BroadcastDuration, Broadcast.BroadcastFlags.Normal);
                    }

                    // Cancels the execution of the original code
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return true;
        }
    }
}
