using CustomPlayerEffects;
using HarmonyLib;
using Hazards;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;

namespace SinkholesTweaks.Patches
{
    [HarmonyPatch(typeof(SinkholeEnvironmentalHazard), nameof(SinkholeEnvironmentalHazard.OnEnter))]
    public static class SinkholeOnEnterPatch
    {
        public static bool Prefix(SinkholeEnvironmentalHazard __instance, ReferenceHub player)
        {
            if (!__instance.IsActive)
                return true;


            if (player.IsSCP() && SinkholesTweaks.Instance.Config.SlowAffectScps && player.GetRoleId() != RoleTypeId.Scp106 && player.roleManager.CurrentRole is IFpcRole fpsRole)
            {
                player.playerEffectsController.EnableEffect<Sinkhole>(1);
                __instance.AffectedPlayers.Add(player);
                return false;
            }

            return true;
        }
    }
}

