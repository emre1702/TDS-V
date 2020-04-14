export interface ConstantsData {

    /** TDSId */
    [0]: number;

    /** RemoteId */
    [1]: number;

    /** UsernameChangeCost */
    [2]: number;

    /** UsernameChangeCooldownDays */
    [3]: number;

    /** MapBuyBasePrice */
    [4]: number;

    /** MapBuyCounterMultiplicator */
    [5]: number;

    /** Announcements */
    [6]: [
        /** DaysAgo */
        number,
        /** Text */
        string
    ][];

    /** Username */
    [7]: string;

    /** SCName */
    [8]: string;
}