using CustomPlayerEffects;
using GameCore;
using HarmonyLib;
using MEC;
using PlayerRoles.FirstPersonControl;
using PlayerRoles;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using Log = PluginAPI.Core.Log;
using SinkholesTweaks.Patches;
using UnityEngine;
using System.Diagnostics;

namespace SinkholesTweaks
{
    public class SinkholesTweaks
    {
        public static SinkholesTweaks Instance;

        public static Harmony HarmonyInstance;

        public const string Version = "1.0.1";

        [PluginConfig]
        public Config Config;

        [PluginEntryPoint("SinkholesTweaks", Version, "NWAPI plugin that allows you to slightly change how SinkHoles work in SCPSL", "SrLicht")]
        public void OnLoad()
        {
            Instance = this;

            if (ConfigFile.ServerConfig.GetFloat("sinkhole_spawn_chance", 0f) == 0)
            {
                Log.Warning("sinkholes are disabled because in &3config_gameplay.txt&r &4sinkhole_spawn_chance&r is equal to &10&r.");
                return;
            }

            HarmonyInstance = new($"SinkholesTweaks.{Version}.{DateTime.Now.Ticks}");
            HarmonyInstance.PatchAll();

        }

        [PluginUnload]
        public void OnUnload()
        {
            HarmonyInstance.UnpatchAll();
            HarmonyInstance = null;
            Instance = null;
        }

        public void SuckPlayer(ReferenceHub hub)
        {
            Timing.RunCoroutine(SUCK(hub));
        }

        private IEnumerator<float> SUCK(ReferenceHub hub)
        {
            var playerRole = hub.GetRoleId();

            if (hub.roleManager.CurrentRole is IFpcRole fpc)
            {
                var stopwatch = new Stopwatch();
                hub.playerStats.GetModule<AdminFlagsStat>().SetFlag(AdminFlags.Noclip, true);
                float currentDepth = fpc.FpcModule.Position.y;
                float targetDepth = fpc.FpcModule.Position.y - 1.2f;
                LateDisableNoClip(hub);
                stopwatch.Start();

                while (currentDepth > targetDepth)
                {
                    if (hub.GetRoleId() != playerRole)
                        yield break;

                    if (stopwatch.Elapsed.TotalSeconds > 2)
                        break;

                    var newposition = Vector3.MoveTowards(fpc.FpcModule.Position, new Vector3(fpc.FpcModule.Position.x, targetDepth, fpc.FpcModule.Position.z), 0.05f);

                    hub.TryOverridePosition(newposition, Vector3.zero);

                    currentDepth = fpc.FpcModule.Position.y;
                    yield return Timing.WaitForSeconds(0.0025f);
                }

                stopwatch.Stop();

                yield return Timing.WaitForSeconds(0.3f);
                hub.playerEffectsController.EnableEffect<Corroding>(20f).AttackerHub = ReferenceHub._hostHub;
                hub.playerEffectsController.EnableEffect<PocketCorroding>();

                if (SinkholesTweaks.Instance.Config.BroadcastOnFall && !string.IsNullOrEmpty(SinkholesTweaks.Instance.Config.BroadcastText))
                    Server.Broadcast.TargetAddElement(hub.connectionToClient, SinkholesTweaks.Instance.Config.BroadcastText, SinkholesTweaks.Instance.Config.BroadcastDuration, Broadcast.BroadcastFlags.Normal);

                yield return Timing.WaitForSeconds(1);
                SinkholesOnStayPatch.AffectedHubs.Remove(hub);
            }
        }

        private void LateDisableNoClip(ReferenceHub hub)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                hub.playerStats.GetModule<AdminFlagsStat>().SetFlag(AdminFlags.Noclip, false);
            });
        }
    }
}
