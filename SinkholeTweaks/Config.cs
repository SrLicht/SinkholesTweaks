using PlayerRoles;
using System.ComponentModel;

namespace SinkholesTweaks
{
    public class Config
    {
        [Description("Determines whether SCPs standing on a sinkhole are affected by the slow")]
        public bool SlowAffectScps { get; set; } = true;

        [Description("If a player from these teams stands in the suction radius of the sinkhole, he will be moved to the pocket dimension.")]
        public Team[] TeamsAffected { get; set; } = new Team[]
        {
            Team.FoundationForces,
            Team.Scientists,
            Team.ClassD,
            Team.ChaosInsurgency
        };

        [Description("If players are within this distance from the center of the Sinkhole they will be sucked into the pocket dimension.")]
        public float SuctionRadius { get; set; } = 1.4f;

        [Description("When a player falls into the pocket dimension a broadcast will be sent to the player if this is true.")]
        public bool BroadcastOnFall { get; set; } = false;

        [Description("if broadcast_on_fall is true this will be the broadcast sent to the player")]
        public string BroadcastText { get; set; } = "You fell to the pocket dimension";

        [Description("For how many seconds will the broadcast be shown to the player")]
        public ushort BroadcastDuration { get; set; } = 10;

        [Description("If this option is activated, players will sink into the sinkhole and then be taken to the pocket dimension. WARNING to make this possible the player is given noclip for 0.5 seconds, that's why it is disabled by default.")]
        public bool BackroomsEntrance { get; set; } = false;
    }
}
