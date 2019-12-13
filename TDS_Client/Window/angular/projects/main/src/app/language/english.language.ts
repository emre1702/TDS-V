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
    MapCreatedSuccessfully = "The map was successfully created/saved.";
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
    MapLimitTime = "Map limit time";
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
    DiscordIdentity = "Discord identity";
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
    Transparent = "Transparent";
    HeightArrowDoesntBlink = "Height arrow doesn't blink";
    AddTeam = "Add team";
    RemoveTeam = "Remove team";
    Back = "Back";
    TimeZone = "Time zone";
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
    ToAnswer = "Answer";
    Buy = "Buy";
    SettingsSpecial = "Settings Special";
    SettingsNormal = "Settings Normal";
    Username = "Username";
    UsernameChangeInfo = "Changing the username has a cooldown of {0} days.\nChanging it during the cooldown costs ${1}.";
    BuyUsername = "Buy username";
    EmailAddress = "Email address";
    ConfirmPassword = "Confirm password";
    UsernameSettingSaved = "The username was successfully saved.";
    PasswordSettingSaved = "The password was successfully saved.";
    EmailSettingSaved = "The email address was successfully saved.";

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
    RegisterTimestamp = "Register time";

    ////////////////// Stats ////////////////
    Main = "Main";
    MyStats = "My stats (will improve)";
    Rules = "Rules";
    RulesUser = "Rules - User";
    RulesTDSTeam = "Rules - TDS-Team";
    RulesVIP = "Rules - VIP";
    Settings = "Settings";
    Commands = "Commands";
    CommandsUser = "Commands - User";
    CommandsTDSTeam = "Commands - TDS-Team";
    CommandsDonator = "Commands - Donator";
    CommandsVIP = "Commands - VIP";
    CommandsLobbyOwner = "Commands - Lobby-owner";
    SupportUser = "Support";
    SupportAdmin = "Support";
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

    target = "target";
    dbTarget = "target";
    reason = "reason";
    time = "time";
    text = "text";
    length = "length";
    minutes = "minutes";
    message = "message";
    money = "money";

    TDSPlayer = "online player";
    Players = "player";
    Int32 = "integer";
    UInt32 = "positive integer";
    Single = "decimal";
    Double = "decimal";
    String = "string";
    DateTime = "time";
}
