import { Language } from '../interfaces/language.interface';

export class English implements Language {
    MapVoting = "Map-Voting";
    MapCreator = "Map-Creator";
    All = "All";
    Normal = "Normal";
    Bomb = "Bomb";
    Favourites = "Favourites";
    Voting = "Voting";
    Vote = "Vote";
    Creator = "Creator";
    Name = "Name";
    MapSettings = "Map-settings";
    German = "German";
    English = "English";
    Description = "description";
    Type = "Type";
    TeamSpawns = "Team-spawns";
    MapLimit = "Map-limit";
    MapCenter = "Map-center";
    Objects = "Objects";
    BombPlaces = "Bomb-places";
    NumberShort = "No.";
    Remove = "Remove";
    AddPos = "Add position";
    RemovePos = "Remove position";
    Teleport = "Teleport";
    MapCenterInfo = `This position is only used for the camera at the countdown.
        Leave the inputs empty if you want the server to calculate the map-center by itself.`;
    MinPlayers = "Min. players";
    MaxPlayers = "Max. players";
    CouldNotDeserialize = "Map could not be created. Please contact a developer.";
    NameAlreadyExists = "The name already exists.";
    Unknown = "Unknown error.";
    Cooldown = "Please wait a bit before sending again.";
    MapCreatedSuccessfully = "The map was successfully created and saved.";
    MapSavedSuccessfully = "The map was successfully saved.";
    SavedMapLoadSuccessful = "Loading map was successful.";
    Cancel = "Cancel";
    ChooseAMapFromList = "Choose a map from the list!";
    Choose = "Choose";
    Map = "Map";
    Chars = "characters";
    FreeroamMenu = "Freeroam-menu";
    Spawn = "Spawn";

    Car = "Car";
    Helicopter = "Helicopter";
    Plane = "Plane";
    Bike = "Bike";
    Boat = "Boat";

    Password = "Password";
    ShowRanking = "Show ranking";
    StartHealth = "Start health";
    StartArmor = "Start armor";
    AmountLifes = "Amount lifes";
    SpawnAgainAfterDeathMs = "Spawn after death time (ms)";
    CreateLobby = "Create lobby";
    Send = "Send";
    Test = "Test";
    New = "New";
    Save = "Save";
    Load = "Load";
    PasswordIncorrect = "The password is incorrect.";
    JoinLobby = "Join lobby";
    MixTeamsAfterRound = "Mix teams after round";
    BombDetonateTimeMs = "Bomb detonate time (ms)";
    BombDefuseTimeMs = "Bomb defuse time (ms)";
    BombPlantTimeMs = "Bomb plant time (ms)";
    RoundTime = "Round time (sec)";
    CountdownTime = "Countdown time (sec)";
    Default = "Default";
    Player = "Player";
    Teams = "Teams";
    Times = "Times";
    KillAfterTime = "Kill after time";
    TeleportBackAfterTime = "Teleport after time";
    Block = "Block";
    None = "None";
    MapLimitType = "Map limit type";
    MapLimitTime = "Map limit time (sec)";
    AreYouSure = "Are you sure?";
    Yes = "Yes";
    No = "No";
    Confirmation = "Confirmation";
    Saved = "Saved";
    Created = "Created";
    Added = "Added";
    OthersSaved = "Others saved";
    OthersCreated = "Others created";
    OthersAdded = "Others added";
    Deactivated = "Deactivated";
    ChangePos = "Change pos";
    Confirm = "Confirm";
    LeaveLobby = "Leave lobby";
    Spectate = "Spectate";
    RandomTeam = "Random team";
    Place = "Place";
    Points = "Points";

    Arena = "Arena";
    Gang = "Gang";
    UserLobbies = "User-Lobbies";

    Userpanel = "Userpanel";
    UserpanelInfo = `This is your user interface.
<br><br>
With the navigation button in the upper left corner you can navigate between several menus.
<br><br>
If one of the menus is deactivated (grayed out), it means that it is still in progress.
<br><br>
The user interface is always being worked on, so don't be surprised if you discover new features.`;
    Usage = "Usage";
    Usages = "Usages";
    General = "General";
    Fight = "Fight";
    Voice = "Voice";
    Graphical = "Graphical";
    Revert = "Revert";
    Standard = "Standard";
    Language = "Language";
    AllowDataTransfer = "Allow data transfer";
    ShowConfettiAtRanking = "Confetti at ranking";
    DiscordUserId = "Discord user id";
    Bloodscreen = "Bloodscreen";
    Hitsound = "Hit sound";
    FloatingDamageInfo = "Floating damage info";
    Voice3D = "Voice 3D";
    VoiceAutoVolume = "Voice auto volume";
    VoiceVolume = "Voice volume";
    MapBorder = "Map-border";
    of = "of";
    FirstPageLabel = "First page";
    ItemsPerPage = "Items per page";
    LastPageLabel = "Last page";
    NextPageLabel = "Next page";
    PreviousPageLabel = "Previous page";
    Hold = "Hold";
    PresetLabels = "Preset colors";
    PlayerSkin = "Player skin";
    SkinHash = "Skin";
    BlipColor = "Blip color";
    Color = "Color";
    Colors = "Colors";
    OtherColors = "Other colors";
    ThemeAndDesign = "Theme & Design";
    Transparent = "Transparent";
    HeightArrowDoesntBlink = "Height arrow doesn't blink";
    AddTeam = "Add team";
    RemoveTeam = "Remove team";
    Back = "Back";
    TimeZone = "Time zone";
    Timezone = "Time zone";
    Message = "Message";
    Invite = "Invite";
    Accept = "Accept";
    Reject = "Reject";
    CreateNewRequest = "Create new request";
    TextLengthBetween = "The text length must be between {0} and {1}.";
    Question = "Question";
    Help = "Help";
    Compliment = "Compliment";
    Complaint = "Complaint";
    Title = "Title";
    OfflineMessages = "Offline messages";
    Open = "Open";
    Create = "Create";
    Answer = "Answer";
    Answers = "Answers";
    ToAnswer = "Answer";
    Buy = "Buy";
    SettingsSpecial = "Settings Special";
    SettingsNormal = "Settings Normal";
    SettingsCommands = "Settings Commands";
    Username = "Username";
    UsernameChangeInfo = "Changing the username has a cooldown of {0} days\nand costs ${1} during the cooldown.";
    BuyUsername = "Buy username";
    EmailAddress = "Email address";
    ConfirmPassword = "Confirm password";
    UsernameSettingSaved = "The username was successfully saved.";
    PasswordSettingSaved = "The password was successfully saved.";
    EmailSettingSaved = "The email address was successfully saved.";
    XDaysAgo = "{0} day(s) ago";
    DaysLeft = "{0} day(s) left";
    HoursLeft = "{0} hour(s) left";
    NoTimeLimit = "No time limit";
    SettingsCommandsInfo = `Here you can set your own aliases for the commands.
The old commands will not disappear, but your settings will be taken into account first.
All commands are case-insensitive (e.g. "AdminSay" and "adminsay" are the same).
WARNING! Spaces will get removed and new lines with an already used command will not be saved!`;
    HideChatInfo = "Hide chat info";
    HideChatInfoInfo = "Hides the information line above the chat.";
    ChatInfoFontSize = "Chat info font size";
    ChatInfoFontSizeInfo = "The font size of the information line above the chat.";
    ChatInfoMoveTimeMs = "Chat info duration (ms)";
    ChatInfoMoveTimeMsInfo = "The time in milliseconds until the info above the chat slides from right to left.";
    UseDarkTheme = "Use dark theme";
    UseDarkThemeInfo = "Use the dark theme for your windows instead of the light theme.";
    ThemeBackgroundAlphaPercentage = "Background opacity (%)";
    ThemeBackgroundAlphaPercentageInfo = "The background color for the theme in percentage (0-100)";
    ThemeMainColor = "Main color";
    ThemeSecondaryColor = "Secondary color";
    ThemeWarnColor = "Warn color";
    ThemeBackgroundDarkColor = "Dark background color";
    ThemeBackgroundLightColor = "Light background color";
    ToolbarDesign = "Toolbar design";
    ToolbarDesignInfo = "The design for the toolbar.";

    SCName = "Socialclub Name";
    AdminLvl = "Admin Level";
    Donation = "Donation";
    IsVip = "Is Vip";
    Money = "Money";
    TotalMoney = "Total Money";
    PlayTime = "Play time";
    BansInLobbies = "Bans in lobbies";
    AmountMapsRated = "Amount maps rated";
    MapsRatedAverage = "Maps rated average";
    AmountMapsCreated = "Amount maps created";
    CreatedMapsAverageRating = "Created maps average rating";
    LastLogin = "Last login";
    JoinDate = "Join date";
    RegisterTimestamp = "Register time";
    "Current:" = "current:";
    AutoFiringMode = "Auto";
    BurstFiringMode = "Burst";
    SingleFiringMode = "Single";
    Target = "Target";
    TargetInfo = `This position will be the main target of the attackers.`;
    Vehicles = "Vehicles";
    AvailableMaps = "Available maps";
    SelectedMaps = "Selected maps";
    User = "User";
    TDSTeam = "TDS team";
    VIP = "VIP";
    LobbyOwner = "Lobby owner";
    AllowDataTransferSettingInfo = "If TDS-V should ever be given to someone else, your data will only be given with your permission.\nMore infos in FAQ.";
    ShowConfettiAtRankingSettingInfo = "(De-)activate the loud confetti in the ranking after each round.";
    DiscordUserIdSettingInfo = "Mention your name with a backslash before (e.g. \\@Bonus) in Discord to get your ID (only numbers).";
    BloodscreenCooldownMs = "Cooldown between bloodscreens (ms)";
    BloodscreenCooldownMsSettingInfo = "The cooldown between two bloodscreens.\nHigher cooldown means better performance while getting damage.";
    HudAmmoUpdateCooldownMs = "Cooldown between ammo updates in hud (ms)";
    HudAmmoUpdateCooldownMsSettingInfo = "The cooldown between two ammo updates for the hud.\nHigher cooldown means better performance while shooting.\nUse -1 to deactivate updates totally.";
    HudHealthUpdateCooldownMs = "Cooldown between health updates in hud (ms)";
    HudHealthUpdateCooldownMsSettingInfo = "The cooldown between two armor and hp updates for the hud.\nHigher cooldown means better performance while getting damage.\nUse -1 to deactivate updates totally.";
    CheckAFK = "Check AFK";
    CheckAFKSettingInfo = "Checks in an official arena lobby if you are AFK, warns and kicks you out of the lobby to prevent that you get killed multiple times while being AFK.\nIt's only for your best so if you don't like it, disable it.";
    MapBorderColor = "Map border color";
    NametagDeadColor = "Nametag dead color";
    NametagDeadColorSettingInfo = "The nametag color to use for a dead player.\nIf you don't use one, the 'health empty color' will be used instead.";
    NametagHealthEmptyColor = "Nametag health empty color";
    NametagHealthEmptyColorSettingInfo = "The nametag color for an empty health.\nThe final nametag health color will be between health empty and health full colors.";
    NametagHealthFullColor = "Nametag health full color";
    NametagHealthFullColorSettingInfo = "The nametag color for a full health.\nThe final nametag health color will be between health empty and health full colors.";
    NametagArmorEmptyColor = "Nametag armor empty color";
    NametagArmorEmptyColorSettingInfo = "The nametag color for an empty armor.\nThe final nametag armor color will be between armor empty (or health nametag) and armor full colors.";
    NametagArmorFullColor = "Nametag armor full color";
    NametagArmorFullColorSettingInfo = "The nametag color for a full armor.\nThe final nametag armor color will be between armor empty (or health nametag) and armor full colors.";
    AFKKickAfterSeconds = "AFK kick after seconds";
    AFKKickAfterSecondsSettingInfo = "Kick yourself out of the lobby after X seconds being AFK (if 'Check AFK' is enabled).";
    AFKKickShowWarningLastSeconds = "AFK warn last seconds";
    AFKKickShowWarningLastSecondsSettingInfo = "Warns you in the last X seconds if you are AFK before kicking you out of the lobby.";
    ShowFloatingDamageInfoDurationMs = "Floating damage info duration (ms)";
    ShowFloatingDamageInfoDurationMsSettingInfo = "The duration in milliseconds for a floating damage info (damage info on hit).";
    Weapons = "Weapons";
    Ammo = "Ammo";
    HeadMultiplicator = "Head multiplicator";
    Melee = "Melee";
    Handguns = "Handguns";
    SubmachineGuns = "Submachine guns";
    Shotguns = "Shotguns";
    AssaultRifles = "Assault rifles";
    LightMachineGuns = "Light machine guns";
    SniperRifles = "Sniper rifles";
    HeavyWeapons = "Heavy weapons";
    Throwables = "Throwables";
    Miscellaneous = "Miscellaneous";
    Copied = "Copied";
    WindowsNotificationsInfo = "Show notifications in Windows";
    ChatWidthSettingInfo = "Width of the chat (% of the resolution)";
    ChatHeightSettingInfo = "Height of the chat (% of the resolution)";
    ChatFontSizeSettingInfo = "Font size in the chat";
    HideDirtyChatInfo = "Hide the dirty chat";
    ShowCursorOnChatOpenInfo = "Show the cursor when chat input is opened";
    ChatWidth = "Chat width (%)";
    ChatMaxHeight = "Chat height (%)";
    ChatFontSize = "Chat font size";
    HideDirtyChat = "Hide dirty chat";
    ShowCursorOnChatOpen = "Show cursor on chat";
    WindowsNotifications = "Windows notifications";
    Scoreboard = "Scoreboard";
    ScoreboardPlayerSorting = "Player sorting";
    ScoreboardPlayerSortingDesc = "Sort descending";
    ScoreboardPlaytimeUnit = "Playtime unit";
    Second = "Second";
    Minute = "Minute";
    HourMinute = "Hour:Minute";
    Hour = "Hour";
    Day = "Day";
    Week = "Week";
    ScoreboardPlayerSortingInfo = "How should the players on the scoreboard in lobbies be sorted (NOT in the main menu)?";
    ScoreboardPlayerSortingDescInfo = "Should the players be sorted in descending order? If not, they will be sorted in ascending order.";
    ScoreboardPlaytimeUnitInfo = "In which time unit should the playtimes be displayed in the scoreboard?";
    KillsDeathsRatio = "KD ratio";
    KillsDeathsAssistsRatio = "KDA ratio";
    MainMenu = "Main menu";
    Heritage = "Heritage";
    Features = "Features";
    Appearance = "Appearance";
    HairAndColors = "Hair and colors";
    Gender = "Gender";
    Male = "Male";
    Female = "Female";
    Resemblance = "Resemblance";
    SkinTone = "Skin tone";
    Randomize = "Randomize";
    Hair = "Hair";
    HairColor = "Hair color";
    HairHighlightColor = "Hair highlight holor";
    EyebrowColor = "Eyebrow color";
    FacialHairColor = "Facial hair color";
    EyeColor = "Eye color";
    BlushColor = "Blush color";
    LipstickColor = "Lipstick color";
    ChestHairColor = "Chest hair color";
    Opacity = "Opacity";
    Green = "Green";
    Emerald = "Emerald";
    "Light Blue" = "Light Blue";
    "Ocean Blue" = "Ocean Blue";
    "Light Brown" = "Light Brown";
    "Dark Brown" = "Dark Brown";
    Hazel = "Hazel";
    "Dark Gray" = "Dark Gray";
    "Light Gray" = "Light Gray";
    Pink = "Pink";
    Yellow = "Yellow";
    Purple = "Purple";
    Blackout = "Blackout";
    "Shades of Gray" = "Shades of Gray";
    "Tequila Sunrise" = "Tequila Sunrise";
    Atomic = "Atomic";
    Warp = "Warp";
    ECola = "ECola";
    "Space Ranger" = "Space Ranger";
    "Ying Yang" = "Ying Yang";
    Bullseye = "Bullseye";
    Lizard = "Lizard";
    Dragon = "Dragon";
    "Extra Terrestrial" = "Extra Terrestrial";
    Goat = "Goat";
    Smiley = "Smiley";
    Possessed = "Possessed";
    Demon = "Demon";
    Infected = "Infected";
    Alien = "Alien";
    Undead = "Undead";
    Zombie = "Zombie";
    "Nose Width" = "Nose Width";
    narrow = "narrow";
    wide = "wide";
    top = "top";
    bottom = "bottom";
    grand = "grand";
    petite = "petite";
    "Nose Bottom Height" = "Nose Bottom Height";
    "Nose Tip Length" = "Nose Tip Length";
    round = "round";
    hollow = "hollow";
    upward = "upward";
    downward = "downward";
    "Nose Tip Height" = "Nose Tip Height";
    "Nose Broken" = "Nose Broken";
    "to right" = "to right";
    "to left" = "to left";
    "Brow Height" = "Brow Height";
    "Brow Depth" = "Brow Depth";
    inward = "inward";
    outward = "outward";
    "Cheekbone Height" = "Cheekbone Height";
    "Cheekbone Width" = "Cheekbone Width";
    "Cheek Depth" = "Cheek Depth";
    "Eye Size" = "Eye Size";
    "Lip Thickness" = "Lip Thickness";
    "Jaw Width" = "Jaw Width";
    "Jaw Height" = "Jaw Height";
    "Chin Height" = "Chin Height";
    "Chin Depth" = "Chin Depth";
    "Chin Width" = "Chin Width";
    "Chin Shape" = "Chin Shape";
    "Neck Width" = "Neck Width";
    opened = "opened";
    closed = "closed";
    small = "small";
    long = "long";
    "simple chin" = "simple chin";
    "double chin" = "double chin";
    Blemishes = "Blemishes";
    "Facial Hair" = "Facial Hair";
    Eyebrows = "Eyebrows";
    Ageing = "Ageing";
    Makeup = "Makeup";
    Blush = "Blush";
    Complexion = "Complexion";
    "Sun Damage" = "Sun Damage";
    Lipstick = "Lipstick";
    "Moles & Freckles" = "Moles & Freckles";
    "Chest Hair" = "Chest Hair";
    Measles = "Measles";
    Pimples = "Pimples";
    Spots = "Spots";
    "Break Out" = "Break Out";
    Blackheads = "Blackheads";
    "Build Up" = "Build Up";
    Pustules = "Pustules";
    Zits = "Zits";
    "Full Acne" = "Full Acne";
    Acne = "Acne";
    "Cheek Rash" = "Cheek Rash";
    "Face Rash" = "Face Rash";
    Picker = "Picker";
    Puberty = "Puberty";
    Eyesore = "Eyesore";
    "Chin Rash" = "Chin Rash";
    "Two Face" = "Two Face";
    "T Zone" = "T Zone";
    Greasy = "Greasy";
    Marked = "Marked";
    "Acne Scarring" = "Acne Scarring";
    "Full Acne Scarring" = "Full Acne Scarring";
    "Cold Sores" = "Cold Sores";
    Impetigo = "Impetigo";
    "Light Stubble" = "Light Stubble";
    Balbo = "Balbo";
    "Circle Beard" = "Circle Beard";
    Goatee = "Goatee";
    Chin = "Chin";
    "Chin Fuzz" = "Chin Fuzz";
    "Pencil Chin Strap" = "Pencil Chin Strap";
    Scruffy = "Scruffy";
    Musketeer = "Musketeer";
    Mustache = "Mustache";
    "Trimmed Beard" = "Trimmed Beard";
    Stubble = "Stubble";
    "Thin Circle Beard" = "Thin Circle Beard";
    Horseshoe = "Horseshoe";
    "Pencil and 'Chops" = "Pencil and 'Chops";
    "Chin Strap Beard" = "Chin Strap Beard";
    "Balbo and Sideburns" = "Balbo and Sideburns";
    "Mutton Chops" = "Mutton Chops";
    "Scruffy Beard" = "Scruffy Beard";
    Curly = "Curly";
    "Curly & Deep Stranger" = "Curly & Deep Stranger";
    Handlebar = "Handlebar";
    Faustic = "Faustic";
    "Otto & Patch" = "Otto & Patch";
    "Otto & Full Stranger" = "Otto & Full Stranger";
    "Light Franz" = "Light Franz";
    "The Hampstead" = "The Hampstead";
    "The Ambrose" = "The Ambrose";
    "Lincoln Curtain" = "Lincoln Curtain";
    Balanced = "Balanced";
    Fashion = "Fashion";
    Cleopatra = "Cleopatra";
    Quizzical = "Quizzical";
    Femme = "Femme";
    Seductive = "Seductive";
    Pinched = "Pinched";
    Triomphe = "Triomphe";
    Carefree = "Carefree";
    Curvaceous = "Curvaceous";
    Rodent = "Rodent";
    "Double Tram" = "Double Tram";
    Thin = "Thin";
    Penciled = "Penciled";
    "Mother Plucker" = "Mother Plucker";
    "Straight and Narrow" = "Straight and Narrow";
    Fuzzy = "Fuzzy";
    Unkempt = "Unkempt";
    Caterpillar = "Caterpillar";
    Regular = "Regular";
    Mediterranean = "Mediterranean";
    Groomed = "Groomed";
    Bushels = "Bushels";
    Feathered = "Feathered";
    Prickly = "Prickly";
    Monobrow = "Monobrow";
    Winged = "Winged";
    "Triple Tram" = "Triple Tram";
    "Arched Tram" = "Arched Tram";
    Cutouts = "Cutouts";
    "Fade Away" = "Fade Away";
    "Solo Tram" = "Solo Tram";
    "Crow's Feet" = "Crow's Feet";
    "First Signs" = "First Signs";
    "Middle Aged" = "Middle Aged";
    "Worry Lines" = "Worry Lines";
    Depression = "Depression";
    Distinguished = "Distinguished";
    Aged = "Aged";
    Weathered = "Weathered";
    Wrinkled = "Wrinkled";
    Sagging = "Sagging";
    "Tough Life" = "Tough Life";
    Vintage = "Vintage";
    Retired = "Retired";
    Junkie = "Junkie";
    Geriatric = "Geriatric";
    "Smoky Black" = "Smoky Black";
    Bronze = "Bronze";
    "Soft Gray" = "Soft Gray";
    "Retro Glam" = "Retro Glam";
    "Natural Look" = "Natural Look";
    "Cat Eyes" = "Cat Eyes";
    Chola = "Chola";
    Vamp = "Vamp";
    "Vinewood Glamour" = "Vinewood Glamour";
    Bubblegum = "Bubblegum";
    "Aqua Dream" = "Aqua Dream";
    "Pin Up" = "Pin Up";
    "Purple Passion" = "Purple Passion";
    "Smoky Cat Eye" = "Smoky Cat Eye";
    "Smoldering Ruby" = "Smoldering Ruby";
    "Pop Princess" = "Pop Princess";
    "Kiss My Axe" = "Kiss My Axe";
    "Panda Pussy" = "Panda Pussy";
    "The Bat" = "The Bat";
    "Skull in Scarlet" = "Skull in Scarlet";
    Serpentine = "Serpentine";
    "The Veldt" = "The Veldt";
    "Tribal Lines" = "Tribal Lines";
    "Tribal Swirls" = "Tribal Swirls";
    "Tribal Orange" = "ribal Orange";
    "Tribal Red" = "Tribal Red";
    "Trapped in a Box" = "Trapped in a Box";
    Clowning = "Clowning";
    Guyliner = "Guyliner";
    "Stars n Stripes" = "Stars n Stripes";
    "Blood Tears" = "Blood Tears";
    "Heavy Metal" = "Heavy Metal";
    Sorrow = "Sorrow";
    "Prince of Darkness" = "Prince of Darkness";
    Rocker = "Rocker";
    Goth = "Goth";
    Devasted = "Devasted";
    "Shadow Demon" = "Shadow Demon";
    "Fleshy Demon" = "Fleshy Demon";
    "Flayed Demon" = "Flayed Demon";
    "Sorrow Demon" = "Sorrow Demon";
    "Smiler Demon" = "Smiler Demon";
    "Cracked Demon" = "Cracked Demon";
    "Danger Skull" = "Danger Skull";
    "Wicked Skull" = "Wicked Skull";
    "Menace Skull" = "Menace Skull";
    "Bone Jaw Skull" = "Bone Jaw Skull";
    "Flesh Jaw Skull" = "Flesh JawSkull";
    "Spirit Skull" = "Spirit Skull";
    "Ghoul Skull" = "Ghoul Skull";
    "Phantom Skull" = "Phantom Skull";
    "Gnasher Skull" = "Gnasher Skull";
    "Exposed Skull" = "Exposed Skull";
    "Ghostly Skull" = "Ghostly Skull";
    "Fury Skull" = "Fury Skull";
    "Demi Skull" = "Demi Skull";
    "Inbred Skull" = "Inbred Skull";
    "Spooky Skull" = "Spooky Skull";
    "Slashed Skull" = "Slashed Skull";
    "Web Sugar Skull" = "Web Sugar Skull";
    "Señor Sugar Skull" = "Señor Sugar Skull";
    "Swirl Sugar Skull" = "Swirl Sugar Skull";
    "Floral Sugar Skull" = "Floral Sugar Skull";
    "Mono Sugar Skull" = "Mono Sugar Skull";
    "Femme Sugar Skull" = "Femme Sugar Skull";
    "Demi Sugar Skull" = "Demi Sugar Skull";
    "Scarred Sugar Skull" = "Scarred Sugar Skull";
    Full = "Full";
    Angled = "Angled";
    Round = "Round";
    Horizontal = "Horizontal";
    High = "High";
    Sweetheart = "Sweetheart";
    Eighties = "Eighties";
    "Rosy Cheeks" = "Rosy Cheeks";
    "Stubble Rash" = "Stubble Rash";
    "Hot Flush" = "Hot Flush";
    Sunburn = "Sunburn";
    Bruised = "Bruised";
    Alchoholic = "Alchoholic";
    Totem = "Totem";
    "Blood Vessels" = "Blood Vessels";
    Damaged = "Damaged";
    Pale = "Pale";
    Ghostly = "Ghostly";
    Uneven = "Uneven";
    Sandpaper = "Sandpaper";
    Patchy = "Patchy";
    Rough = "Rough";
    Leathery = "Leathery";
    Textured = "Textured";
    Coarse = "Coarse";
    Rugged = "Rugged";
    Creased = "Creased";
    Cracked = "Cracked";
    Gritty = "Gritty";
    "Color Matte" = "Color Matte";
    "Color Gloss" = "Color Gloss";
    "Lined Matte" = "Lined Matte";
    "Lined Gloss" = "Lined Gloss";
    "Heavy Lined Matte" = "Heavy Lined Matte";
    "Heavy Lined Gloss" = "Heavy Lined Gloss";
    "Lined Nude Matte" = "Lined Nude Matte";
    "Liner Nude Gloss" = "Liner Nude Gloss";
    Smudged = "Smudged";
    Geisha = "Geisha";
    Cherub = "Cherub";
    "All Over" = "All Over";
    Irregular = "Irregular";
    "Dot Dash" = "Dot Dash";
    "Over the Bridge" = "Over the Bridge";
    "Baby Doll" = "Baby Doll";
    Pixie = "Pixie";
    "Sun Kissed" = "Sun Kissed";
    "Beauty Marks" = "Beauty Marks";
    "Line Up" = "Line Up";
    Modelesque = "Modelesque";
    Occasional = "Occasional";
    Speckled = "Speckled";
    "Rain Drops" = "Rain Drops";
    "Double Dip" = "Double Dip";
    "One Sided" = "One Sided";
    Pairs = "Pairs";
    Growth = "Growth";
    Natural = "Natural";
    "The Strip" = "The Strip";
    "The Tree" = "The Tree";
    Hairy = "Hairy";
    Grisly = "Grisly";
    Ape = "Ape";
    "Groomed Ape" = "Groomed Ape";
    Bikini = "Bikini";
    "Lightning Bolt" = "Lightning Bolt";
    "Reverse Lightning" = "Reverse Lightning";
    "Love Heart" = "Love Heart";
    Chestache = "Chestache";
    "Happy Face" = "Happy Face";
    Skull = "Skull";
    "Snail Trail" = "Snail Trail";
    "Slug and Nips" = "Slug and Nips";
    "Hairy Arms" = "Hairy Arms";
    "Close Shave" = "Close Shave";
    Buzzcut = "Buzzcut";
    "Faux Hawk" = "Faux Hawk";
    "Hipster" = "Hipster";
    "Side Parting" = "Side Parting";
    "Shorter Cut" = "Shorter Cut";
    "Biker" = "Biker";
    "Ponytail" = "Ponytail";
    "Cornrows" = "Cornrows";
    "Slicked" = "Slicked";
    "Short Brushed" = "Short Brushed";
    "Spikey" = "Spikey";
    "Caesar" = "Caesar";
    "Chopped" = "Chopped";
    "Dreads" = "Dreads";
    "Long Hair" = "Long Hair";
    "Shaggy Curls" = "Shaggy Curls";
    "Surfer Dude" = "Surfer Dude";
    "Short Side Part" = "Short Side Part";
    "High Slicked Sides" = "High Slicked Sides";
    "Long Slicked" = "Long Slicked";
    "Hipster Youth" = "Hipster Youth";
    "Mullet" = "Mullet";
    "Classic Cornrows" = "Classic Cornrows";
    "Palm Cornrows" = "Palm Cornrows";
    "Lightning Cornrows" = "Lightning Cornrows";
    "Whipped Cornrows" = "Whipped Cornrows";
    "Zig Zag Cornrows" = "Zig Zag Cornrows";
    "Snail Cornrows" = "Snail Cornrows";
    "Hightop" = "Hightop";
    "Loose Swept Back" = "Loose Swept Back";
    "Undercut Swept Back" = "Undercut Swept Back";
    "Undercut Swept Side" = "Undercut Swept Side";
    "Spiked Mohawk" = "Spiked Mohawk";
    "Mod" = "Mod";
    "Layered Mod" = "Layered Mod";
    "Flattop" = "Flattop";
    "Military Buzzcut" = "Military Buzzcut";
    "Short" = "Short";
    "Layered Bob" = "Layered Bob";
    "Pigtails" = "Pigtails";
    "Braided Mohawk" = "Braided Mohawk";
    "Braids" = "Braids";
    "Bob" = "Bob";
    "French Twist" = "French Twist";
    "Long Bob" = "Long Bob";
    "Loose Tied" = "Loose Tied";
    "Shaved Bangs" = "Shaved Bangs";
    "Top Knot" = "Top Knot";
    "Wavy Bob" = "Wavy Bob";
    "Messy Bun" = "Messy Bun";
    "Pin Up Girl" = "Pin Up Girl";
    "Tight Bun" = "Tight Bun";
    "Twisted Bob" = "Twisted Bob" ;
    "Flapper Bob" = "Flapper Bob";
    "Big Bangs" = "Big Bangs";
    "Braided Top Knot" = "Braided Top Knot";
    "Pinched Cornrows" = "Pinched Cornrows";
    "Leaf Cornrows" = "Leaf Cornrows";
    "Pigtail Bangs" = "Pigtail Bangs";
    "Wave Braids" = "Wave Braids";
    "Coil Braids" = "Coil Braids";
    "Rolled Quiff" = "Rolled Quiff";
    "Bandana and Braid" = "Bandana and Braid";
    "Skinbyrd" = "Skinbyrd";
    "Neat Bun" = "Neat Bun";
    "Short Bob" = "Short Bob";
    "Body Blemishes" = "Body Blemishes";
    "Add Body Blemishes" = "Add Body Blemishes";
    Father = "Father";
    Mother = "Mother";
    StartWithMapcreatorError = "Lobby name must not start with \"Mapcreator\"";
    LobbyWithNameAlreadyExistsError = "Lobby with this name already exists.";
    "Error?" = "Unknown error";
    ThisErrorrequired = "This field is mandatory but has not been filled in.";
    ThisErrormaxlength = "Too many characters used.";
    ThisErrorminlength = "Too less characters used.";
    ThisErrormin = "The number is too low.";
    ThisErrormax = "The number is too high.";
    ThisErrorpattern = "You used an invalid character.";
    ThisErrorblipColorDoesntExists = "This blip color does not exist.";
    Errorrequired = "At least one setting is mandatory but have not been filled in.";
    Errormaxlength = "At least one setting has too many characters.";
    Errorminlength = "At least one setting has too less characters.";
    Errormin = "At least one setting has a too low number.";
    Errormax = "At least one setting has a too high number.";
    Errornotenoughteams = "You don't have enough teams for atleast one map.";
    ErrorNotLobbyOwner = "Only allowed for lobby owners";
    ErrorMapLimitMapCreator = `Your map limit settings are invalid.
You can only have 0 or 3+ edges.`;
    ErrorTeamSpawnsMapCreator = `Your team spawns settings are invalid.
You either don't have enough teams for this map type or too few spawns for at least one team.`;
    ErrorBombPlacesMapCreator = `Your bomb places settings are invalid.
You need at least one bomb place for this mode.`;
    ErrorTargetMapCreator = `Your target settings are invalid.
You need to set the target for this mode.`;
    LobbyLeaveInfo = "You can use /leave to leave the lobby.";
    CursorVisibleInfo = `Your cursor is enabled.
    You won't be able to use any controls.`;
    ShowCursorInfo = "Cursor info";
    ShowCursorInfoInfo = "Shows or hides the info for cursor in the bottom right corner.";
    ShowLobbyLeaveInfo = "Lobby leave info";
    ShowLobbyLeaveInfoInfo = "Shows or hides the info for leaving the lobby in the bottom right corner.";
    OfficialKills = "Official kills";
    DealtDamage = "Dealt damage";
    DealtOfficialDamage = "Dealt official damage";
    AmountShots = "Amount shots";
    AmountOfficialShots = "Amount official shots";
    AmountHits = "Amount hits";
    AmountOfficialHits = "Amount official hits";
    AmountHeadshots = "Amount headshots";
    AmountOfficialHeadshots = "Amount official headshots";
    Head = "Head";
    Neck = "Neck";
    UpperBody = "Upper body";
    Spine = "Spine";
    LowerBody = "Lower body";
    Arm = "Arm";
    Hand = "Hand";
    Leg = "Leg";
    Foot = "Foot";
    Torso = "Torso";
    GenitalRegion = "Genital region";
    GangNameHint = "Full name of the gang";
    GangShortHint = "Short of the gang";
    GangColorHint = "Color used for gang (except for blips)";
    GangBlipColorHint = "Color only used for blip of gang";
    BlipRGBColorCopied = "Blip RGB color copied";
    GangShort = "Gang short";
    RanksPermissions = "Rank permissions";
    GangMenu = "Gang menu";
    Administration = "Administration";
    ManageRanks = "Manage ranks";
    ManageRanksHint = "From which rank should one be allowed to manage the ranks?";
    ManagePermissions = "Manage permissions";
    ManagePermissionsHint = "From which rank should one be allowed to manage the permissions (this window)?";
    Member = "Member";
    Action = "Action";
    InviteMembers = "Invite members";
    InviteMembersHint = "From which rank should one be allowed to invite others to this gang?";
    KickMembers = "Kick members";
    KickMembersHint = "From which rank should one be allowed to kick members (with lower rank)?";
    StartGangwar = "Start gangwar";
    StartGangwarHint = "From which rank should one be allowed to start a gangwar?";
    GangInfo = "Gang info";
    Members = "Members";
    Ranks = "Ranks";
    RanksLevels = "Rank levels";
    AllGangs = "All gangs";
    CreateGang = "Create gang";
    GangSuccessfullyCreatedInfo = "The gang has been created successfully.";
    LeaveGang = "Leave gang";
    SetRanks = "Set ranks";
    SetRanksHint = "From which rank should one be allowed to give rank ups or rank downs to members?";
    CommandExecutedSuccessfully = "Command has been executed successfully.";
    RankLevelsModifyInfo = "Warning: Modifying rank levels could result in members getting rank downs/ups.\nCheck member ranks after saving the ranks!";
    LoadingDataFailed = "Loading data failed.";
    LastLoginDate = "Last login date";
    RankUp = "Rank up";
    RankDown = "Rank down";
    Rank = "Rank";
    ArmsRaceWeapons = "Arms race weapons";
    Clear = "Clear";
    AtKill = "At kill";
    Win = "Win";
    ArmsRaceWeaponsDuplicateError = "There are duplicates in your selected weapons and 'At kill'.\nEvery 'At kill' value has to be unique.";
    ArmsRaceWeaponsFirstWeaponError = "The first weapon with 'At kill' = 0 is missing.";
    ArmsRaceWeaponsWinError = "The win condition was missing and has been added.\nModify the 'At kill' value of it if you want.";
    ArmsRaceWeaponsWinNotLastError = "The win condition has to be the end.\nThe 'At kill' value got fixed, modify it if you want.";
    RankNameHint = "Name of the rank.";
    RankColorHint = "Color of the rank (for e.g. chat)";
    AddRankAfter = "Add rank +1";
    DeleteThisRank = "Delete this rank";
    SaveRanks = "Save ranks";
    SettingSavedSuccessfully = "The settings were saved successfully.";
    CharCreator = "Character creator";
    Announcements = "Announcements";
    Changelogs = "Changelogs";
    Apply = "Apply";
    DamageTestLobby = "Damage test";
    KillInfo = "Kill info";
    KillInfoShowIcon = "Show weapon icons";
    KillInfoShowIconInfo = "Show weapon icons instead of weapon name.";
    KillInfoFontWidth = "Font width";
    KillInfoFontWidthInfo = "Font width of kill info texts. Higher number => bigger texts";
    KillInfoIconWidth = "Weapon icon width (pixel)";
    KillInfoIconWidthInfo = "Width in pixel for weapon icons.";
    KillInfoIconHeight = "Weapon icon height (pixel)";
    KillInfoIconHeightInfo = "Height in pixel for weapon icons.";
    KillInfoSpacing = "Spacing (pixel)";
    KillInfoSpacingInfo = "Spacing in pixel between texts and icon.";
    TestKillInfo = "Test kill info";
    DateTimeFormat = "Date time format";

    /////////// Default map names ///////////
    DefaultMapIdsAllWithoutGangwars = "All without gangwar";
    DefaultMapIdsNormals = "All normal";
    DefaultMapIdsBombs = "All bombs";
    DefaultMapIdsSnipers = "All snipers";
    DefaultMapIdsGangwars = "All gangwars";
    DefaultMapIdsArmsRaces = "All arms races";

    ////////////// Challenges ///////////////
    Challenges = "Challenges";
    Challenge_Kills = "Get {0} kills";
    Challenge_Assists = "Get {0} assists";
    Challenge_Damage = "Do {0} damage";
    Challenge_PlayTime = "Play {0} minutes";
    Challenge_HeadshotKills = "Get {0} headshot kills";
    Challenge_RoundPlayed = "Play {0} rounds";
    Challenge_BombDefuse = "Defuse {0} bombs";
    Challenge_BombPlant = "Plant {0} bombs";
    Challenge_Killstreak = "Get a streak of {0} kills";
    Challenge_BuyMaps = "Buy {0} maps";
    Challenge_ReviewMaps = "Review {0} different maps";
    Challenge_ReadTheRules = "Read the rules (in userpanel)";
    Challenge_ReadTheFAQ = "Read the FAQ (in userpanel)";
    Challenge_ChangeSettings = "Change your settings (in userpanel)";
    Challenge_JoinDiscordServer = "Join the discord server and set your Discord identity (in userpanel)";
    Challenge_WriteHelpfulIssue = "Write a helpful issue in GitHub";
    Challenge_CreatorOfAcceptedMap = "Create a map that passes the testing phase";
    Challenge_BeHelpfulEnough = "Help the server enough to get the \"Contributor\" role in Discord";

    ////////////////// Stats ////////////////
    Main = "Main";
    MyStats = "My stats";
    MyStatsGeneral = "Stats - General";
    MyStatsWeapon = "Stats - Weapon";
    Rules = "Rules";
    RulesUser = "Rules - User";
    RulesTDSTeam = "Rules - TDS-Team";
    RulesVIP = "Rules - VIP";
    Settings = "Settings";
    Commands = "Commands";
    CommandsUser = "Commands - User";
    CommandsTDSTeam = "Commands - TDS team";
    CommandsDonator = "Commands - Donator";
    CommandsVIP = "Commands - VIP";
    CommandsLobbyOwner = "Commands - Lobby owner";
    SupportUser = "Support";
    SupportAdmin = "Support";
    MuteTime = "Mute time";
    VoiceMuteTime = "Voice mute time";
    LobbyStats = "Lobby stats";
    Logs = "Logs";
    Kills = "Kills";
    Assists = "Assists";
    Deaths = "Deaths";
    Damage = "Damage";
    TotalKills = "Total kills";
    TotalAssists = "Total assists";
    TotalDeaths = "Total deaths";
    TotalDamage = "Total damage";
    TotalRounds = "Total rounds";
    MostKillsInARound = "Most kills in a round";
    MostDamageInARound = "Most damage in a round";
    MostAssistsInARound = "Most assists in a round";
    MostKillsInADay = "Most kills in a day";
    MostDamageInADay = "Most damage in a day";
    MostAssistsInADay = "Most assists in a day";
    TotalMapsBought = "Total maps bought";
    ////////////////////////////////////////

    ///////////// Applications /////////////
    InfosForAdminApplyProcess = "Information about the procedure";
    AdminApplyProcessInfo = `Here you can apply for a supporter position.
The application will be open for one week and all team-member can view it.
Here they see your important statistics and, if available, your answers to all administrator questions.

If an administrator wants you in his team, he sends you an invitation.
You can accept this invitation within the time you can't write a new application.
However, if no administrator wants you, you will not receive an invitation.

After sending the application you can't write a new application for 2 weeks.
At the end of the 2 weeks all application data (including invitations) will be deleted.`;
    Confirmations = "Confirmations";
    ConfirmRuleAdminApply = "I confirm that I have read and accepted the rules.";
    ConfirmTeamAdminApply = "I confirm that I agree with the team hierarchy and will listen to the members above in the hierarchy.";
    ConfirmNoAbuseAdminApply = "I confirm that as a supporter I would never exploit my rights and could even be excluded from the server if exploited.";
    ConfirmStatsVisibleAdminApply = "I confirm that my application will make all my data of interest for the application visible to all team members (even if they might not have permission to do so).";
    Application = "Application";
    Applications = "Applications";
    AdminQuestions = "Administrator questions";
    SendApplication = "Send application";
    ApplicationStatsInfo = "Player stats & infos";
    AlreadyCreatedApplicationInfo = `The last application was sent on '{0}'.

Here you can see all the invitations sent to you:`;
    ApplicationAnswersInfo = "Answers to the administrator questions";
    ////////////////////////////////////////

    ATTACK = "Attack! Go go go!";
    BACK = "Stay back!";
    SPREAD_OUT = "Spread out!";
    TO_BOMB = "Go to the bomb!";

    target = "target (online)";
    dbTarget = "target (online/offline)";
    reason = "reason";
    time = "time";
    text = "text";
    length = "length";
    minutes = "minutes";
    message = "message";
    money = "money";
    pos = "position";

    TDSPlayer = "online player";
    ITDSPlayer = "online player";
    Players = "player";
    Int32 = "integer";
    UInt32 = "positive integer";
    Single = "decimal";
    Double = "decimal";
    String = "string";
    DateTime = "time";
    TimeSpan = "timespan";
    KillInfoDuration = "Kill info duration (sec)";
    KillInfoDurationInfo = "How long the kill info will stay on screen in seconds.";
    CooldownsAndDurations = "Cooldowns and durations";
    FightEffect = "Fight effects";
    IngameColors = "Ingame colors";
    Theme = "Window Theme";
    HeadShotMultiplier = "Headshot multiplier";
}
