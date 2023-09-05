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

        public const string Version = "1.0.3.1";

        [PluginConfig]
        public Config Config;

        [PluginEntryPoint("SinkholesTweaks", Version, "NWAPI plugin that allows you to slightly change how SinkHoles work in SCPSL", "SrLicht")]
        public void OnLoad()
        {
            Instance = this;

            if (ConfigFile.ServerConfig.GetFloat("sinkhole_spawn_chance", 0f) == 0)
            {
                Log.Warning("SinkholesTweaks are disabled because in &3config_gameplay.txt&r &4sinkhole_spawn_chance&r is equal to &10&r.");
                return;
            }

            HarmonyInstance = new($"SinkholesTweaks.{Version}.{DateTime.Now.Ticks}");
            HarmonyInstance.PatchAll();

            Log.Info("SinkholesTweaks fully loaded");
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
                var capturePosition = fpc.FpcModule.Position;
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

                // Teleport to pocket dimension
                fpc.FpcModule.ServerOverridePosition(Vector3.up * -1998.5f, Vector3.zero);

                if (Instance.Config.BroadcastOnFall && !string.IsNullOrEmpty(Instance.Config.BroadcastText))
                    Server.Broadcast.TargetAddElement(hub.connectionToClient, Instance.Config.BroadcastText, Instance.Config.BroadcastDuration, Broadcast.BroadcastFlags.Normal);

                yield return Timing.WaitForSeconds(0.5f);

                // Because of how this works, I have to give them the effect so that when they leave they are teleported outside and don't stay forever in the pocket dimension.
                var effect = hub.playerEffectsController.EnableEffect<PocketCorroding>();
                effect.CapturePosition = new(capturePosition);
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
