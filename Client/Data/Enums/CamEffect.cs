namespace TDS_Client.Data.Enums
{
    /**
     * <summary>
     * if p0 is 0, effect is cancelled
     * if p0 is 1, effect zooms in, gradually tilts
     * cam clockwise apx 30 degrees, wobbles slowly. Motion blur is active until cancelled.
     * if p0 is 2, effect immediately tilts cam clockwise apx 30 degrees, begins to
     * wobble slowly, then gradually tilts cam back to normal. The wobbling will continue
     * until the effect is cancelled.
     * </summary>
    */

    public enum CamEffect
    {
        /**
         * <summary>Effect is canceled</summary>
         */
        Cancel = 0,

        /**
         *  <summary>
         *  Effect zooms in, gradually tilts cam clockwise apx 30 degrees,
         *  wobbles slowly. Motion blur is active until cancelled.
         *  </summary>
         */
        ZoomIn_Tilt30Deg_WobbleSlowly = 1,

        /**
         * <summary>
         * Effect immediately tilts cam clockwise apx 30 degrees, begins to
         * wobble slowly, then gradually tilts cam back to normal. The wobbling will continue
         * until the effect is cancelled.
         * </summary>
         */
        Tilt30Deg_WobbleSlowly_TiltBack = 2
    }
}
