namespace TDS_Server.Database.Entity.LobbyEntities
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
        //Todo: Add SET_SWIM_MULTIPLIER_FOR_PLAYER
        //Todo: Add SET_AIR_DRAG_MULTIPLIER_FOR_PLAYERS_VEHICLE
        //Todo: Add SET_PED_ACCURACY
        //Todo: Add SET_PED_MIN_GROUND_TIME_FOR_STUNGUN
        //Todo: Add SET_PED_SHOOT_RATE
        //Todo: Add SET_PED_GRAVITY
        //Todo: Add SET_PED_MAX_HEALTH
        //Todo: Add SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE
        //Todo: Add SET_PED_CAN_RAGDOLL
        //Todo: Add SET_GRAVITY_LEVEL
        //Todo: Add SET_EXPLOSIVE_AMMO_THIS_FRAME
        //Todo: Add SET_FIRE_AMMO_THIS_FRAME
        //Todo: Add SET_EXPLOSIVE_MELEE_THIS_FRAME
        //Todo: Add SET_SUPER_JUMP_THIS_FRAME
        //Todo: Add SET_PED_INFINITE_AMMO
        //Todo: Add SET_PED_INFINITE_AMMO_CLIP
    }
}
