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
        public float SuctionRadius { get; set; } = 1.3f;

        [Description("If this is not empty, this text will be sent as a broadcast to the player when he is taken to the pocket dimension.")]
        public string BroadcastText { get; set; } = "";

        [Description("For how many seconds will the broadcast be shown to the player")]
        public ushort BroadcastDuration { get; set; } = 10;

        [Description("If this option is activated, players will sink into the sinkhole and then be taken to the pocket dimension. WARNING to make this possible the player is given noclip for 0.5 seconds, that's why it is disabled by default.")]
        public bool BackroomsEntrance { get; set; } = false;
    }
}
