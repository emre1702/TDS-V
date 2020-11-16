namespace TDS.Server.Database.Entity.LobbyEntities
{
    public class LobbyFightSettings
    {
        #region Public Properties

        public short AmountLifes { get; set; }
        public virtual Lobbies Lobby { get; set; }
        public int LobbyId { get; set; }
        public int SpawnAgainAfterDeathMs { get; set; }
        public short StartArmor { get; set; }
        public short StartHealth { get; set; }

        #endregion Public Properties

        //Todo: Add SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER 
        //Goes from 1 to 1,49 

        //Todo: Add SetMoveRateOverride
        //Goes from 0 to 10 

        //Todo: Add SET_SWIM_MULTIPLIER_FOR_PLAYER
        //Goes from 1 to 1,49 

        //Todo: Add SET_AIR_DRAG_MULTIPLIER_FOR_PLAYERS_VEHICLE
        //Goes from 1 to 14,9
        //Makes the car not shift(?), driving slow.

        //Todo: Add SET_PED_ACCURACY
        //Goes from 0 to 100
        //This is for npcs only, doesn't affect the player

        //Todo: Add SET_PED_MIN_GROUND_TIME_FOR_STUNGUN

        //Todo: Add SET_PED_SHOOT_RATE
        //Goes from 0 to 1000
        //This is for npcs only, doesn't affect the player

        //Todo: Add SET_PED_GRAVITY
        //Didn't affect me

        //Todo: Add SET_PED_MAX_HEALTH
        //Works, dunno what the maximum is

        //Todo: Add SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE

        //Todo: Add SET_GRAVITY_LEVEL
        //This isn't good, should ignore it

        //Todo: Add SET_EXPLOSIVE_AMMO_THIS_FRAME
        //Todo: Add SET_FIRE_AMMO_THIS_FRAME
        //Todo: Add SET_EXPLOSIVE_MELEE_THIS_FRAME
        //Todo: Add SET_SUPER_JUMP_THIS_FRAME
        //Todo: Add SET_PED_INFINITE_AMMO
        //Todo: Add SET_PED_INFINITE_AMMO_CLIP
    }
}
