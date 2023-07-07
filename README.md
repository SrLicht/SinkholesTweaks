# SinkholesTweaks
 A NWAPI plugin that allows you to slightly change how SinkHoles work in SCP:SL

 # Installation
 You can install this using the command ``p install SrLicht/SinkholesTweaks`` from the console or by downloading the ``SinkholesTweaks.dll`` and put it in the plugins folder, also make sure you have ``0Harmony.dll`` in the dependencies folder.

 # Sinking players
 This plugin has a function that makes the players that are sunk by a Sinkhole to be seen in this way, it is disabled by default because to make a player go through the ground I have to give him NoClip and I only give it for 0.5 seconds but it is still disabled by default.

If ``backrooms_entrance`` is false the player will simply be teleported to the pocket dimension when touching the center.
 

**Demonstration Video:**



https://github.com/SrLicht/SinkholesTweaks/assets/36207738/532d99e9-9157-4a21-bb9f-75cd447a943a



# Default config

```yml
# Determines whether SCPs standing on a sinkhole are affected by the slow
slow_affect_scps: true
# If a player from these teams stands in the suction radius of the sinkhole, he will be moved to the pocket dimension.
teams_affected:
- FoundationForces
- Scientists
- ClassD
- ChaosInsurgency
# If players are within this distance from the center of the Sinkhole they will be sucked into the pocket dimension.
suction_radius: 1.4
# If this is not empty, this text will be sent as a broadcast to the player when he is taken to the pocket dimension.
broadcast_text: ''
# For how many seconds will the broadcast be shown to the player
broadcast_duration: 10
# If this option is activated, players will sink into the sinkhole and then be taken to the pocket dimension. WARNING to make this possible the player is given noclip for 0.5 seconds, that's why it is disabled by default.
backrooms_entrance: false

```
