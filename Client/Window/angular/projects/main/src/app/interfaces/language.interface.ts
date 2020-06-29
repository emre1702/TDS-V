import { ExecSyncOptionsWithBufferEncoding } from 'child_process';

export interface Language {
    MapVoting: string;
    MapCreator: string;
    All: string;
    Normal: string;
    Bomb: string;
    Favourites: string;
    Voting: string;
    Vote: string;
    Creator: string;
    Name: string;
    German: string;
    English: string;
    MapSettings: string;
    Description: string;
    Type: string;
    TeamSpawns: string;
    MapLimit: string;
    MapCenter: string;
    Objects: string;
    BombPlaces: string;
    NumberShort: string;
    Remove: string;
    AddPos: string;
    Teleport: string;
    MapCenterInfo: string;
    MinPlayers: string;
    MaxPlayers: string;
    CouldNotDeserialize: string;
    NameAlreadyExists: string;
    Unknown: string;
    Cooldown: string;
    MapCreatedSuccessfully: string;
    SavedMapLoadSuccessful: string;
    Cancel: string;
    ChooseAMapFromList: string;
    Choose: string;
    Map: string;
    Chars: string;
    FreeroamMenu: string;
    Spawn: string;

    Car: string;
    Helicopter: string;
    Plane: string;
    Bike: string;
    Boat: string;

    Password: string;
    ShowRanking: string;
    StartHealth: string;
    StartArmor: string;
    AmountLifes: string;
    SpawnAgainAfterDeathMs: string;
    CreateLobby: string;
    Send: string;
    Test: string;
    New: string;
    Save: string;
    Load: string;
    PasswordIncorrect: string;
    JoinLobby: string;
    MixTeamsAfterRound: string;
    BombDetonateTimeMs: string;
    BombDefuseTimeMs: string;
    BombPlantTimeMs: string;
    RoundTime: string;
    CountdownTime: string;
    Default: string;
    Player: string;
    Teams: string;
    Times: string;
    KillAfterTime: string;
    TeleportBackAfterTime: string;
    Block: string;
    None: string;
    MapLimitType: string;
    MapLimitTime: string;
    AreYouSure: string;
    Yes: string;
    No: string;
    Confirmation: string;
    Saved: string;
    Created: string;
    Added: string;
    OthersSaved: string;
    OthersCreated: string;
    OthersAdded: string;
    Deactivated: string;
    ChangePos: string;
    Confirm: string;
    LeaveLobby: string;
    Spectate: string;
    RandomTeam: string;
    Place: string;
    Points: string;

    Arena: string;
    Gang: string;
    UserLobbies: string;
    Usage: string;
    Usages: string;
    General: string;
    Fight: string;
    Voice: string;
    Graphical: string;
    Revert: string;
    Standard: string;
    Language: string;
    AllowDataTransfer: string;
    ShowConfettiAtRanking: string;
    DiscordUserId: string;
    Bloodscreen: string;
    Hitsound: string;
    FloatingDamageInfo: string;
    Voice3D: string;
    VoiceAutoVolume: string;
    VoiceVolume: string;
    MapBorder: string;
    of: string;
    FirstPageLabel: string;
    ItemsPerPage: string;
    LastPageLabel: string;
    NextPageLabel: string;
    PreviousPageLabel: string;
    Hold: string;
    PresetLabels: string;
    PlayerSkin: string;
    SkinHash: string;
    BlipColor: string;
    Color: string;
    Colors: string;
    Transparent: string;
    HeightArrowDoesntBlink: string;
    AddTeam: string;
    RemoveTeam: string;
    Back: string;
    TimeZone: string;
    Message: string;
    Invite: string;
    Accept: string;
    Reject: string;
    CreateNewRequest: string;
    TextLengthBetween: string;
    Question: string;
    Help: string;
    Compliment: string;
    Complaint: string;
    Title: string;
    OfflineMessages: string;
    Open: string;
    Create: string;
    Answer: string;
    Answers: string;
    ToAnswer: string;
    Buy: string;
    SettingsSpecial: string;
    SettingsNormal: string;
    SettingsCommands: string;
    Username: string;
    UsernameChangeInfo: string;
    BuyUsername: string;
    EmailAddress: string;
    ConfirmPassword: string;
    UsernameSettingSaved: string;
    PasswordSettingSaved: string;
    EmailSettingSaved: string;
    XDaysAgo: string;
    DaysLeft: string;
    HoursLeft: string;
    NoTimeLimit: string;
    "Current:": string;
    AutoFiringMode: string;
    BurstFiringMode: string;
    SingleFiringMode: string;
    Target: string;
    TargetInfo: string;
    Vehicles: string;
    AvailableMaps: string;
    SelectedMaps: string;
    User: string;
    TDSTeam: string;
    VIP: string;
    LobbyOwner: string;
    AllowDataTransferSettingInfo: string;
    ShowConfettiAtRankingSettingInfo: string;
    DiscordUserIdSettingInfo: string;
    BloodscreenCooldownMs: string;
    BloodscreenCooldownMsSettingInfo: string;
    HudAmmoUpdateCooldownMs: string;
    HudAmmoUpdateCooldownMsSettingInfo: string;
    HudHealthUpdateCooldownMs: string;
    HudHealthUpdateCooldownMsSettingInfo: string;
    CheckAFK: string;
    CheckAFKSettingInfo: string;
    MapBorderColor: string;
    NametagDeadColor: string;
    NametagDeadColorSettingInfo: string;
    NametagHealthEmptyColor: string;
    NametagHealthEmptyColorSettingInfo: string;
    NametagArmorEmptyColor: string;
    NametagArmorEmptyColorSettingInfo: string;
    NametagArmorFullColor: string;
    NametagArmorFullColorSettingInfo: string;
    AFKKickAfterSeconds: string;
    AFKKickAfterSecondsSettingInfo: string;
    AFKKickShowWarningLastSeconds: string;
    AFKKickShowWarningLastSecondsSettingInfo: string;
    ShowFloatingDamageInfoDurationMs: string;
    ShowFloatingDamageInfoDurationMsSettingInfo: string;
    Weapons: string;
    Ammo: string;
    HeadMultiplicator: string;
    Melee: string;
    Handguns: string;
    SubmachineGuns: string;
    Shotguns: string;
    AssaultRifles: string;
    LightMachineGuns: string;
    SniperRifles: string;
    HeavyWeapons: string;
    Throwables: string;
    Miscellaneous: string;
    Copied: string;
    WindowsNotificationsInfo: string;
    ChatWidthSettingInfo: string;
    ChatHeightSettingInfo: string;
    ChatFontSizeSettingInfo: string;
    HideDirtyChatInfo: string;
    ShowCursorOnChatOpenInfo: string;
    ChatWidth: string;
    ChatMaxHeight: string;
    ChatFontSize: string;
    HideDirtyChat: string;
    ShowCursorOnChatOpen: string;
    WindowsNotifications: string;
    Scoreboard: string;
    ScoreboardPlayerSorting: string;
    ScoreboardPlayerSortingDesc: string;
    ScoreboardPlaytimeUnit: string;
    Second: string;
    Minute: string;
    HourMinute: string;
    Hour: string;
    Day: string;
    Week: string;
    ScoreboardPlayerSortingInfo: string;
    ScoreboardPlayerSortingDescInfo: string;
    ScoreboardPlaytimeUnitInfo: string;
    KillsDeathsRatio: string;
    KillsDeathsAssistsRatio: string;
    SettingsCommandsInfo: string;
    HideChatInfo: string;
    HideChatInfoInfo: string;
    ChatInfoFontSize: string;
    ChatInfoFontSizeInfo: string;
    ChatInfoMoveTimeMs: string;
    ChatInfoMoveTimeMsInfo: string;
    UseDarkTheme: string;
    UseDarkThemeInfo: string;
    ThemeBackgroundAlphaPercentage: string;
    ThemeBackgroundAlphaPercentageInfo: string;
    ThemeMainColor: string;
    ThemeSecondaryColor: string;
    ThemeWarnColor: string;
    ThemeBackgroundDarkColor: string;
    ThemeBackgroundLightColor: string;

    MainMenu: string;
    Heritage: string;
    Features: string;
    Appearance: string;
    HairAndColors: string;
    Gender: string;
    Male: string;
    Female: string;
    Resemblance: string;
    SkinTone: string;
    Randomize: string;
    Hair: string;
    HairColor: string;
    HairHighlightColor: string;
    EyebrowColor: string;
    FacialHairColor: string;
    EyeColor: string;
    BlushColor: string;
    LipstickColor: string;
    ChestHairColor: string;
    Opacity: string;
    Green: string;
    Emerald: string;
    "Light Blue": string;
    "Ocean Blue": string;
    "Light Brown": string;
    "Dark Brown": string;
    Hazel: string;
    "Dark Gray": string;
    "Light Gray": string;
    Pink: string;
    Yellow: string;
    Purple: string;
    Blackout: string;
    "Shades of Gray": string;
    "Tequila Sunrise": string;
    Atomic: string;
    Warp: string;
    ECola: string;
    "Space Ranger": string;
    "Ying Yang": string;
    Bullseye: string;
    Lizard: string;
    Dragon: string;
    "Extra Terrestrial": string;
    Goat: string;
    Smiley: string;
    Possessed: string;
    Demon: string;
    Infected: string;
    Alien: string;
    Undead: string;
    Zombie: string;
    "Nose Width": string;
    narrow: string;
    wide: string;
    top: string;
    bottom: string;
    grand: string;
    petite: string;
    "Nose Bottom Height": string;
    "Nose Tip Length": string;
    round: string;
    hollow: string;
    upward: string;
    downward: string;
    "Nose Tip Height": string;
    "Nose Broken": string;
    "to right": string;
    "to left": string;
    "Brow Height": string;
    "Brow Depth": string;
    inward: string;
    outward: string;
    "Cheekbone Height": string;
    "Cheekbone Width": string;
    "Cheek Depth": string;
    "Eye Size": string;
    "Lip Thickness": string;
    "Jaw Width": string;
    "Jaw Height": string;
    "Chin Height": string;
    "Chin Depth": string;
    "Chin Width": string;
    "Chin Shape": string;
    "Neck Width": string;
    opened: string;
    closed: string;
    small: string;
    long: string;
    "simple chin": string;
    "double chin": string;
    Blemishes: string;
    "Facial Hair": string;
    Eyebrows: string;
    Ageing: string;
    Makeup: string;
    Blush: string;
    Complexion: string;
    "Sun Damage": string;
    Lipstick: string;
    "Moles & Freckles": string;
    "Chest Hair": string;
    Measles: string;
    Pimples: string;
    Spots: string;
    "Break Out": string;
    Blackheads: string;
    "Build Up": string;
    Pustules: string;
    Zits: string;
    "Full Acne": string;
    Acne: string;
    "Cheek Rash": string;
    "Face Rash": string;
    Picker: string;
    Puberty: string;
    Eyesore: string;
    "Chin Rash": string;
    "Two Face": string;
    "T Zone": string;
    Greasy: string;
    Marked: string;
    "Acne Scarring": string;
    "Full Acne Scarring": string;
    "Cold Sores": string;
    Impetigo: string;
    "Light Stubble": string;
    Balbo: string;
    "Circle Beard": string;
    Goatee: string;
    Chin: string;
    "Chin Fuzz": string;
    "Pencil Chin Strap": string;
    Scruffy: string;
    Musketeer: string;
    Mustache: string;
    "Trimmed Beard": string;
    Stubble: string;
    "Thin Circle Beard": string;
    Horseshoe: string;
    "Pencil and 'Chops": string;
    "Chin Strap Beard": string;
    "Balbo and Sideburns": string;
    "Mutton Chops": string;
    "Scruffy Beard": string;
    Curly: string;
    "Curly & Deep Stranger": string;
    Handlebar: string;
    Faustic: string;
    "Otto & Patch": string;
    "Otto & Full Stranger": string;
    "Light Franz": string;
    "The Hampstead": string;
    "The Ambrose": string;
    "Lincoln Curtain": string;
    Balanced: string;
    Fashion: string;
    Cleopatra: string;
    Quizzical: string;
    Femme: string;
    Seductive: string;
    Pinched: string;
    Triomphe: string;
    Carefree: string;
    Curvaceous: string;
    Rodent: string;
    "Double Tram": string;
    Thin: string;
    Penciled: string;
    "Mother Plucker": string;
    "Straight and Narrow": string;
    Fuzzy: string;
    Unkempt: string;
    Caterpillar: string;
    Regular: string;
    Mediterranean: string;
    Groomed: string;
    Bushels: string;
    Feathered: string;
    Prickly: string;
    Monobrow: string;
    Winged: string;
    "Triple Tram": string;
    "Arched Tram": string;
    Cutouts: string;
    "Fade Away": string;
    "Solo Tram": string;
    "Crow's Feet": string;
    "First Signs": string;
    "Middle Aged": string;
    "Worry Lines": string;
    Depression: string;
    Distinguished: string;
    Aged: string;
    Weathered: string;
    Wrinkled: string;
    Sagging: string;
    "Tough Life": string;
    Vintage: string;
    Retired: string;
    Junkie: string;
    Geriatric: string;
    "Smoky Black": string;
    Bronze: string;
    "Soft Gray": string;
    "Retro Glam": string;
    "Natural Look": string;
    "Cat Eyes": string;
    Chola: string;
    Vamp: string;
    "Vinewood Glamour": string;
    Bubblegum: string;
    "Aqua Dream": string;
    "Pin Up": string;
    "Purple Passion": string;
    "Smoky Cat Eye": string;
    "Smoldering Ruby": string;
    "Pop Princess": string;
    "Kiss My Axe": string;
    "Panda Pussy": string;
    "The Bat": string;
    "Skull in Scarlet": string;
    "Serpentine": string;
    "The Veldt": string;
    "Tribal Lines": string;
    "Tribal Swirls": string;
    "Tribal Orange": string;
    "Tribal Red": string;
    "Trapped in a Box": string;
    Clowning: string;
    Guyliner: string;
    "Stars n Stripes": string;
    "Blood Tears": string;
    "Heavy Metal": string;
    Sorrow: string;
    "Prince of Darkness": string;
    Rocker: string;
    Goth: string;
    Devasted: string;
    "Shadow Demon": string;
    "Fleshy Demon": string;
    "Flayed Demon": string;
    "Sorrow Demon": string;
    "Smiler Demon": string;
    "Cracked Demon": string;
    "Danger Skull": string;
    "Wicked Skull": string;
    "Menace Skull": string;
    "Bone Jaw Skull": string;
    "Flesh Jaw Skull": string;
    "Spirit Skull": string;
    "Ghoul Skull": string;
    "Phantom Skull": string;
    "Gnasher Skull": string;
    "Exposed Skull": string;
    "Ghostly Skull": string;
    "Fury Skull": string;
    "Demi Skull": string;
    "Inbred Skull": string;
    "Spooky Skull": string;
    "Slashed Skull": string;
    "Web Sugar Skull": string;
    "Señor Sugar Skull": string;
    "Swirl Sugar Skull": string;
    "Floral Sugar Skull": string;
    "Mono Sugar Skull": string;
    "Femme Sugar Skull": string;
    "Demi Sugar Skull": string;
    "Scarred Sugar Skull": string;
    Full: string;
    Angled: string;
    Round: string;
    Horizontal: string;
    High: string;
    Sweetheart: string;
    Eighties: string;
    "Rosy Cheeks": string;
    "Stubble Rash": string;
    "Hot Flush": string;
    Sunburn: string;
    Bruised: string;
    Alchoholic: string;
    Totem: string;
    "Blood Vessels": string;
    Damaged: string;
    Pale: string;
    Ghostly: string;
    Uneven: string;
    Sandpaper: string;
    Patchy: string;
    Rough: string;
    Leathery: string;
    Textured: string;
    Coarse: string;
    Rugged: string;
    Creased: string;
    Cracked: string;
    Gritty: string;
    "Color Matte": string;
    "Color Gloss": string;
    "Lined Matte": string;
    "Lined Gloss": string;
    "Heavy Lined Matte": string;
    "Heavy Lined Gloss": string;
    "Lined Nude Matte": string;
    "Liner Nude Gloss": string;
    Smudged: string;
    Geisha: string;
    Cherub: string;
    "All Over": string;
    Irregular: string;
    "Dot Dash": string;
    "Over the Bridge": string;
    "Baby Doll": string;
    Pixie: string;
    "Sun Kissed": string;
    "Beauty Marks": string;
    "Line Up": string;
    Modelesque: string;
    Occasional: string;
    Speckled: string;
    "Rain Drops": string;
    "Double Dip": string;
    "One Sided": string;
    Pairs: string;
    Growth: string;
    Natural: string;
    "The Strip": string;
    "The Tree": string;
    Hairy: string;
    Grisly: string;
    Ape: string;
    "Groomed Ape": string;
    Bikini: string;
    "Lightning Bolt": string;
    "Reverse Lightning": string;
    "Love Heart": string;
    Chestache: string;
    "Happy Face": string;
    Skull: string;
    "Snail Trail": string;
    "Slug and Nips": string;
    "Hairy Arms": string;
    "Close Shave": string;
    "Buzzcut": string;
    "Faux Hawk": string;
    "Hipster": string;
    "Side Parting": string;
    "Shorter Cut": string;
    "Biker": string;
    "Ponytail": string;
    "Cornrows": string;
    "Slicked": string;
    "Short Brushed": string;
    "Spikey": string;
    "Caesar": string;
    "Chopped": string;
    "Dreads": string;
    "Long Hair": string;
    "Shaggy Curls": string;
    "Surfer Dude": string;
    "Short Side Part": string;
    "High Slicked Sides": string;
    "Long Slicked": string;
    "Hipster Youth": string;
    "Mullet": string;
    "Classic Cornrows": string;
    "Palm Cornrows": string;
    "Lightning Cornrows": string;
    "Whipped Cornrows": string;
    "Zig Zag Cornrows": string;
    "Snail Cornrows": string;
    "Hightop": string;
    "Loose Swept Back": string;
    "Undercut Swept Back": string;
    "Undercut Swept Side": string;
    "Spiked Mohawk": string;
    "Mod": string;
    "Layered Mod": string;
    "Flattop": string;
    "Military Buzzcut": string;
    "Short": string;
    "Layered Bob": string;
    "Pigtails": string;
    "Braided Mohawk": string;
    "Braids": string;
    "Bob": string;
    "French Twist": string;
    "Long Bob": string;
    "Loose Tied": string;
    "Shaved Bangs": string;
    "Top Knot": string;
    "Wavy Bob": string;
    "Messy Bun": string;
    "Pin Up Girl": string;
    "Tight Bun": string;
    "Twisted Bob": string;
    "Flapper Bob": string;
    "Big Bangs": string;
    "Braided Top Knot": string;
    "Pinched Cornrows": string;
    "Leaf Cornrows": string;
    "Pigtail Bangs": string;
    "Wave Braids": string;
    "Coil Braids": string;
    "Rolled Quiff": string;
    "Bandana and Braid": string;
    "Skinbyrd": string;
    "Neat Bun": string;
    "Short Bob": string;
    "Body Blemishes": string;
    "Add Body Blemishes": string;
    Father: string;
    Mother: string;
    StartWithMapcreatorError: string;
    LobbyWithNameAlreadyExistsError: string;
    "Error?": string;
    Errorrequired: string;
    Errormaxlength: string;
    Errorminlength: string;
    Errormin: string;
    Errormax: string;
    Errornotenoughteams: string;
    ErrorNotLobbyOwner: string;
    ErrorMapLimitMapCreator: string;
    ErrorTeamSpawnsMapCreator: string;
    ErrorBombPlacesMapCreator: string;
    ErrorTargetMapCreator: string;
    LobbyLeaveInfo: string;
    CursorVisibleInfo: string;
    ShowCursorInfo: string;
    ShowCursorInfoInfo: string;
    ShowLobbyLeaveInfo: string;
    ShowLobbyLeaveInfoInfo: string;

    /////////// Default map names ///////////
    DefaultMapIdsAllWithoutGangwars: string;
    DefaultMapIdsNormals: string;
    DefaultMapIdsBombs: string;
    DefaultMapIdsSnipers: string;
    DefaultMapIdsGangwars: string;
    DefaultMapIdsArmsRaces: string;

    ////////////// Challenges ///////////////
    Challenges: string;
    Challenge_Kills: string;
    Challenge_Assists: string;
    Challenge_Damage: string;
    Challenge_PlayTime: string;
    Challenge_HeadshotKills: string;
    Challenge_RoundPlayed: string;
    Challenge_BombDefuse: string;
    Challenge_BombPlant: string;
    Challenge_Killstreak: string;
    Challenge_BuyMaps: string;
    Challenge_ReviewMaps: string;
    Challenge_ReadTheRules: string;
    Challenge_ReadTheFAQ: string;
    Challenge_ChangeSettings: string;
    Challenge_JoinDiscordServer: string;
    Challenge_WriteHelpfulIssue: string;
    Challenge_CreatorOfAcceptedMap: string;
    Challenge_BeHelpfulEnough: string;

    ////////////////// Stats ////////////////
    SCName: string;
    AdminLvl: string;
    Donation: string;
    IsVip: string;
    Money: string;
    TotalMoney: string;
    PlayTime: string;
    BansInLobbies: string;
    AmountMapsCreated: string;
    MapsRatedAverage: string;
    CreatedMapsAverageRating: string;
    AmountMapsRated: string;
    LastLogin: string;
    RegisterTimestamp: string;
    MuteTime: string;
    VoiceMuteTime: string;
    LobbyStats: string;
    Logs: string;
    Kills: string;
    Assists: string;
    Deaths: string;
    Damage: string;
    TotalKills: string;
    TotalAssists: string;
    TotalDeaths: string;
    TotalDamage: string;
    TotalRounds: string;
    MostKillsInARound: string;
    MostDamageInARound: string;
    MostAssistsInARound: string;
    MostKillsInADay: string;
    MostDamageInADay: string;
    MostAssistsInADay: string;
    TotalMapsBought: string;
    ////////////////////////////////////////

    ///////////// Applications /////////////
    InfosForAdminApplyProcess: string;
    AdminApplyProcessInfo: string;
    Confirmations: string;
    ConfirmRuleAdminApply: string;
    ConfirmTeamAdminApply: string;
    ConfirmNoAbuseAdminApply: string;
    ConfirmStatsVisibleAdminApply: string;
    Application: string;
    Applications: string;
    AdminQuestions: string;
    SendApplication: string;
    AlreadyCreatedApplicationInfo: string;
    ApplicationStatsInfo: string;
    ApplicationAnswersInfo: string;
    ////////////////////////////////////////

    Userpanel: string;
    UserpanelInfo: string;
    Main: string;
    MyStats: string;
    Rules: string;
    RulesUser: string;
    RulesTDSTeam: string;
    RulesVIP: string;
    Settings: string;
    Commands: string;
    CommandsUser: string;
    CommandsTDSTeam: string;
    CommandsDonator: string;
    CommandsVIP: string;
    CommandsLobbyOwner: string;
    SupportUser: string;
    SupportAdmin: string;

    ATTACK: string;
    BACK: string;
    SPREAD_OUT: string;
    TO_BOMB: string;

    target: string;
    dbTarget: string;
    reason: string;
    time: string;
    text: string;
    length: string;
    minutes: string;
    message: string;
    money: string;

    TDSPlayer: string;
    ITDSPlayer: string;
    Players: string;
    Int32: string;
    UInt32: string;
    Single: string;
    Double: string;
    String: string;
    DateTime: string;
    TimeSpan: string;
}
