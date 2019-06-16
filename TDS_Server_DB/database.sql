PGDMP     	                    w           TDSNew    11.2    11.2 �    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                       false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                       false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                       false            �           1262    16403    TDSNew    DATABASE     �   CREATE DATABASE "TDSNew" WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'German_Germany.1252' LC_CTYPE = 'German_Germany.1252';
    DROP DATABASE "TDSNew";
             postgres    false                        3079    16951    pg_stat_statements 	   EXTENSION     F   CREATE EXTENSION IF NOT EXISTS pg_stat_statements WITH SCHEMA public;
 #   DROP EXTENSION pg_stat_statements;
                  false            �           0    0    EXTENSION pg_stat_statements    COMMENT     h   COMMENT ON EXTENSION pg_stat_statements IS 'track execution statistics of all SQL statements executed';
                       false    2            �            1259    16743 	   log_types    TABLE     i   CREATE TABLE public.log_types (
    "ID" smallint NOT NULL,
    "Name" character varying(50) NOT NULL
);
    DROP TABLE public.log_types;
       public         tdsv    false            �            1259    16741 	   ID_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."ID_ID_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 "   DROP SEQUENCE public."ID_ID_seq";
       public       tdsv    false    231            �           0    0 	   ID_ID_seq    SEQUENCE OWNED BY     B   ALTER SEQUENCE public."ID_ID_seq" OWNED BY public.log_types."ID";
            public       tdsv    false    230            �            1259    16902    server_settings    TABLE       CREATE TABLE public.server_settings (
    "ID" smallint NOT NULL,
    "GamemodeName" character varying(50) NOT NULL,
    "MapsPath" character varying(300) NOT NULL,
    "NewMapsPath" character varying(300) NOT NULL,
    "ErrorToPlayerOnNonExistentCommand" boolean NOT NULL,
    "ToChatOnNonExistentCommand" boolean NOT NULL,
    "DistanceToSpotToPlant" real NOT NULL,
    "DistanceToSpotToDefuse" real NOT NULL,
    "SavePlayerDataCooldownMinutes" integer NOT NULL,
    "SaveLogsCooldownMinutes" integer NOT NULL,
    "SaveSeasonsCooldownMinutes" integer NOT NULL,
    "TeamOrderCooldownMs" integer NOT NULL,
    "ArenaNewMapProbabilityPercent" real NOT NULL,
    "SavedMapsPath" character varying(300) DEFAULT 'bridge/resources/tds/savedmaps/'::character varying NOT NULL
);
 #   DROP TABLE public.server_settings;
       public         tdsv    false            �            1259    16900    ServerSettings_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."ServerSettings_ID_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE public."ServerSettings_ID_seq";
       public       tdsv    false    241            �           0    0    ServerSettings_ID_seq    SEQUENCE OWNED BY     T   ALTER SEQUENCE public."ServerSettings_ID_seq" OWNED BY public.server_settings."ID";
            public       tdsv    false    240            �            1259    16410    admin_level_names    TABLE     �   CREATE TABLE public.admin_level_names (
    "Level" smallint NOT NULL,
    "Language" smallint NOT NULL,
    "Name" character varying(50) NOT NULL
);
 %   DROP TABLE public.admin_level_names;
       public         tdsv    false            �            1259    16405    admin_levels    TABLE     �   CREATE TABLE public.admin_levels (
    "Level" smallint NOT NULL,
    "ColorR" smallint NOT NULL,
    "ColorG" smallint NOT NULL,
    "ColorB" smallint NOT NULL
);
     DROP TABLE public.admin_levels;
       public         tdsv    false            �            1259    16445    command_alias    TABLE     t   CREATE TABLE public.command_alias (
    "Alias" character varying(100) NOT NULL,
    "Command" smallint NOT NULL
);
 !   DROP TABLE public.command_alias;
       public         tdsv    false            �            1259    16475    command_infos    TABLE     �   CREATE TABLE public.command_infos (
    "ID" smallint NOT NULL,
    "Language" smallint NOT NULL,
    "Info" character varying(500) NOT NULL
);
 !   DROP TABLE public.command_infos;
       public         tdsv    false            �            1259    16434    commands    TABLE     �   CREATE TABLE public.commands (
    "ID" smallint NOT NULL,
    "Command" character varying(50) NOT NULL,
    "NeededAdminLevel" smallint,
    "NeededDonation" smallint,
    "VipCanUse" boolean,
    "LobbyOwnerCanUse" boolean
);
    DROP TABLE public.commands;
       public         tdsv    false            �            1259    16432    commands_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."commands_ID_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public."commands_ID_seq";
       public       tdsv    false    201            �           0    0    commands_ID_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public."commands_ID_seq" OWNED BY public.commands."ID";
            public       tdsv    false    200            �            1259    16979    freeroam_default_vehicle    TABLE     �   CREATE TABLE public.freeroam_default_vehicle (
    "VehicleTypeId" smallint NOT NULL,
    "VehicleHash" bigint NOT NULL,
    "Note" character varying
);
 ,   DROP TABLE public.freeroam_default_vehicle;
       public         tdsv    false            �            1259    16977 *   freeroam_default_vehicle_VehicleTypeId_seq    SEQUENCE     �   CREATE SEQUENCE public."freeroam_default_vehicle_VehicleTypeId_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 C   DROP SEQUENCE public."freeroam_default_vehicle_VehicleTypeId_seq";
       public       tdsv    false    246            �           0    0 *   freeroam_default_vehicle_VehicleTypeId_seq    SEQUENCE OWNED BY     }   ALTER SEQUENCE public."freeroam_default_vehicle_VehicleTypeId_seq" OWNED BY public.freeroam_default_vehicle."VehicleTypeId";
            public       tdsv    false    245            �            1259    16968    freeroam_vehicle_type    TABLE     q   CREATE TABLE public.freeroam_vehicle_type (
    "Id" smallint NOT NULL,
    "Type" character varying NOT NULL
);
 )   DROP TABLE public.freeroam_vehicle_type;
       public         tdsv    false            �            1259    16966    freeroam_vehicle_type_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."freeroam_vehicle_type_Id_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 5   DROP SEQUENCE public."freeroam_vehicle_type_Id_seq";
       public       tdsv    false    244            �           0    0    freeroam_vehicle_type_Id_seq    SEQUENCE OWNED BY     a   ALTER SEQUENCE public."freeroam_vehicle_type_Id_seq" OWNED BY public.freeroam_vehicle_type."Id";
            public       tdsv    false    243            �            1259    16510    gangs    TABLE     �   CREATE TABLE public.gangs (
    "ID" integer NOT NULL,
    "TeamID" integer NOT NULL,
    "Short" character varying(5) NOT NULL
);
    DROP TABLE public.gangs;
       public         tdsv    false            �            1259    16508    gangs_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."gangs_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public."gangs_ID_seq";
       public       tdsv    false    208            �           0    0    gangs_ID_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public."gangs_ID_seq" OWNED BY public.gangs."ID";
            public       tdsv    false    207            �            1259    16521    killingspree_rewards    TABLE     �   CREATE TABLE public.killingspree_rewards (
    "KillsAmount" smallint NOT NULL,
    "HealthOrArmor" smallint,
    "OnlyHealth" smallint,
    "OnlyArmor" smallint
);
 (   DROP TABLE public.killingspree_rewards;
       public         tdsv    false            �            1259    16415 	   languages    TABLE     m   CREATE TABLE public.languages (
    "ID" smallint NOT NULL,
    "Language" character varying(50) NOT NULL
);
    DROP TABLE public.languages;
       public         tdsv    false            �            1259    16553    lobbies    TABLE     M  CREATE TABLE public.lobbies (
    "Id" integer NOT NULL,
    "Owner" integer NOT NULL,
    "Type" smallint NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Password" character varying(100),
    "StartHealth" smallint DEFAULT 100 NOT NULL,
    "StartArmor" smallint DEFAULT 100 NOT NULL,
    "AmountLifes" smallint,
    "DefaultSpawnX" real DEFAULT 0 NOT NULL,
    "DefaultSpawnY" real DEFAULT 0 NOT NULL,
    "DefaultSpawnZ" real DEFAULT 900 NOT NULL,
    "AroundSpawnPoint" real DEFAULT 3 NOT NULL,
    "DefaultSpawnRotation" real DEFAULT 0 NOT NULL,
    "IsTemporary" boolean NOT NULL,
    "IsOfficial" boolean DEFAULT false NOT NULL,
    "SpawnAgainAfterDeathMs" integer DEFAULT 400 NOT NULL,
    "CreateTimestamp" timestamp without time zone DEFAULT now() NOT NULL,
    "DieAfterOutsideMapLimitTime" integer DEFAULT 10 NOT NULL
);
    DROP TABLE public.lobbies;
       public         tdsv    false            �            1259    16551    lobbies_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."lobbies_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public."lobbies_ID_seq";
       public       tdsv    false    213            �           0    0    lobbies_ID_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public."lobbies_ID_seq" OWNED BY public.lobbies."Id";
            public       tdsv    false    212            �            1259    16607 
   lobby_maps    TABLE     a   CREATE TABLE public.lobby_maps (
    "LobbyID" integer NOT NULL,
    "MapID" integer NOT NULL
);
    DROP TABLE public.lobby_maps;
       public         tdsv    false            �            1259    16580    lobby_rewards    TABLE     �   CREATE TABLE public.lobby_rewards (
    "LobbyID" integer NOT NULL,
    "MoneyPerKill" double precision NOT NULL,
    "MoneyPerAssist" double precision NOT NULL,
    "MoneyPerDamage" double precision NOT NULL
);
 !   DROP TABLE public.lobby_rewards;
       public         tdsv    false            �            1259    16590    lobby_round_settings    TABLE     �  CREATE TABLE public.lobby_round_settings (
    "LobbyID" integer NOT NULL,
    "RoundTime" integer DEFAULT 240 NOT NULL,
    "CountdownTime" integer DEFAULT 5 NOT NULL,
    "BombDetonateTimeMs" integer DEFAULT 45000 NOT NULL,
    "BombDefuseTimeMs" integer DEFAULT 8000 NOT NULL,
    "BombPlantTimeMs" integer DEFAULT 3000 NOT NULL,
    "MixTeamsAfterRound" boolean DEFAULT false NOT NULL
);
 (   DROP TABLE public.lobby_round_settings;
       public         tdsv    false            �            1259    16495    lobby_types    TABLE     l   CREATE TABLE public.lobby_types (
    "ID" smallint NOT NULL,
    "Name" character varying(100) NOT NULL
);
    DROP TABLE public.lobby_types;
       public         tdsv    false            �            1259    16657    lobby_weapons    TABLE     �   CREATE TABLE public.lobby_weapons (
    "Hash" bigint NOT NULL,
    "Lobby" integer NOT NULL,
    "Ammo" integer NOT NULL,
    "Damage" smallint,
    "HeadMultiplicator" real
);
 !   DROP TABLE public.lobby_weapons;
       public         tdsv    false            �            1259    16674 
   log_admins    TABLE     E  CREATE TABLE public.log_admins (
    "ID" bigint NOT NULL,
    "Type" smallint NOT NULL,
    "Source" integer NOT NULL,
    "Target" integer,
    "Lobby" integer,
    "AsDonator" boolean NOT NULL,
    "AsVIP" boolean NOT NULL,
    "Reason" text NOT NULL,
    "Timestamp" timestamp without time zone DEFAULT now() NOT NULL
);
    DROP TABLE public.log_admins;
       public         tdsv    false            �            1259    16672    log_admins_ID_seq    SEQUENCE     |   CREATE SEQUENCE public."log_admins_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public."log_admins_ID_seq";
       public       tdsv    false    223            �           0    0    log_admins_ID_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public."log_admins_ID_seq" OWNED BY public.log_admins."ID";
            public       tdsv    false    222            �            1259    16710 	   log_chats    TABLE     .  CREATE TABLE public.log_chats (
    "ID" bigint NOT NULL,
    "Source" integer NOT NULL,
    "Target" integer,
    "Message" text NOT NULL,
    "Lobby" integer,
    "IsAdminChat" boolean NOT NULL,
    "IsTeamChat" boolean NOT NULL,
    "Timestamp" timestamp without time zone DEFAULT now() NOT NULL
);
    DROP TABLE public.log_chats;
       public         tdsv    false            �            1259    16708    log_chats_ID_seq    SEQUENCE     {   CREATE SEQUENCE public."log_chats_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public."log_chats_ID_seq";
       public       tdsv    false    227            �           0    0    log_chats_ID_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public."log_chats_ID_seq" OWNED BY public.log_chats."ID";
            public       tdsv    false    226            �            1259    16698 
   log_errors    TABLE     �   CREATE TABLE public.log_errors (
    "ID" bigint NOT NULL,
    "Source" integer,
    "Info" text NOT NULL,
    "StackTrace" text,
    "Timestamp" timestamp without time zone DEFAULT now() NOT NULL
);
    DROP TABLE public.log_errors;
       public         tdsv    false            �            1259    16696    log_errors_ID_seq    SEQUENCE     |   CREATE SEQUENCE public."log_errors_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public."log_errors_ID_seq";
       public       tdsv    false    225            �           0    0    log_errors_ID_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public."log_errors_ID_seq" OWNED BY public.log_errors."ID";
            public       tdsv    false    224            �            1259    16731 	   log_rests    TABLE       CREATE TABLE public.log_rests (
    "ID" bigint NOT NULL,
    "Type" smallint NOT NULL,
    "Source" integer NOT NULL,
    "Serial" character varying(200),
    "IP" inet,
    "Lobby" integer,
    "Timestamp" timestamp without time zone DEFAULT now() NOT NULL
);
    DROP TABLE public.log_rests;
       public         tdsv    false            �            1259    16729    log_rests_ID_seq    SEQUENCE     {   CREATE SEQUENCE public."log_rests_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public."log_rests_ID_seq";
       public       tdsv    false    229            �           0    0    log_rests_ID_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public."log_rests_ID_seq" OWNED BY public.log_rests."ID";
            public       tdsv    false    228            �            1259    16619    maps    TABLE     �   CREATE TABLE public.maps (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "CreatorId" integer,
    "CreateTimestamp" timestamp without time zone DEFAULT now() NOT NULL
);
    DROP TABLE public.maps;
       public         tdsv    false            �            1259    16617    maps_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."maps_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE public."maps_ID_seq";
       public       tdsv    false    218            �           0    0    maps_ID_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE public."maps_ID_seq" OWNED BY public.maps."Id";
            public       tdsv    false    217            �            1259    16751    offlinemessages    TABLE       CREATE TABLE public.offlinemessages (
    "ID" integer NOT NULL,
    "TargetID" integer NOT NULL,
    "SourceID" integer NOT NULL,
    "Message" text NOT NULL,
    "Seen" boolean DEFAULT false NOT NULL,
    "Timestamp" timestamp without time zone DEFAULT now() NOT NULL
);
 #   DROP TABLE public.offlinemessages;
       public         tdsv    false            �            1259    16749    offlinemessages_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."offlinemessages_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public."offlinemessages_ID_seq";
       public       tdsv    false    233            �           0    0    offlinemessages_ID_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public."offlinemessages_ID_seq" OWNED BY public.offlinemessages."ID";
            public       tdsv    false    232            �            1259    16773    player_bans    TABLE       CREATE TABLE public.player_bans (
    "PlayerId" integer NOT NULL,
    "LobbyId" integer DEFAULT 0 NOT NULL,
    "AdminId" integer,
    "Reason" text NOT NULL,
    "StartTimestamp" timestamp with time zone DEFAULT now() NOT NULL,
    "EndTimestamp" timestamp with time zone
);
    DROP TABLE public.player_bans;
       public         tdsv    false            �            1259    16798    player_lobby_stats    TABLE     �  CREATE TABLE public.player_lobby_stats (
    "PlayerID" integer NOT NULL,
    "LobbyID" integer NOT NULL,
    "Kills" integer DEFAULT 0 NOT NULL,
    "Assists" integer DEFAULT 0 NOT NULL,
    "Deaths" integer DEFAULT 0 NOT NULL,
    "Damage" integer DEFAULT 0 NOT NULL,
    "TotalKills" integer DEFAULT 0 NOT NULL,
    "TotalAssists" integer DEFAULT 0 NOT NULL,
    "TotalDeaths" integer DEFAULT 0 NOT NULL,
    "TotalDamage" integer DEFAULT 0 NOT NULL
);
 &   DROP TABLE public.player_lobby_stats;
       public         tdsv    false            �            1259    16821    player_map_favourites    TABLE     m   CREATE TABLE public.player_map_favourites (
    "PlayerID" integer NOT NULL,
    "MapID" integer NOT NULL
);
 )   DROP TABLE public.player_map_favourites;
       public         tdsv    false            �            1259    16851    player_map_ratings    TABLE     �   CREATE TABLE public.player_map_ratings (
    "PlayerID" integer NOT NULL,
    "MapID" integer NOT NULL,
    "Rating" smallint NOT NULL
);
 &   DROP TABLE public.player_map_ratings;
       public         tdsv    false            �            1259    16866    player_settings    TABLE       CREATE TABLE public.player_settings (
    "PlayerID" integer NOT NULL,
    "Language" smallint NOT NULL,
    "Hitsound" boolean NOT NULL,
    "Bloodscreen" boolean NOT NULL,
    "FloatingDamageInfo" boolean NOT NULL,
    "AllowDataTransfer" boolean NOT NULL
);
 #   DROP TABLE public.player_settings;
       public         tdsv    false            �            1259    16886    player_stats    TABLE     -  CREATE TABLE public.player_stats (
    "PlayerID" integer NOT NULL,
    "Money" integer DEFAULT 0 NOT NULL,
    "PlayTime" integer DEFAULT 0 NOT NULL,
    "MuteTime" integer,
    "LoggedIn" boolean DEFAULT false NOT NULL,
    "LastLoginTimestamp" timestamp without time zone DEFAULT now() NOT NULL
);
     DROP TABLE public.player_stats;
       public         tdsv    false            �            1259    16528    players    TABLE     �  CREATE TABLE public.players (
    "ID" integer NOT NULL,
    "SCName" character varying(255) NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Password" character varying(100) NOT NULL,
    "Email" character varying(100),
    "AdminLvl" smallint DEFAULT 0 NOT NULL,
    "IsVIP" boolean DEFAULT false NOT NULL,
    "Donation" smallint DEFAULT 0 NOT NULL,
    "GangId" integer,
    "RegisterTimestamp" timestamp(4) without time zone DEFAULT now() NOT NULL
);
    DROP TABLE public.players;
       public         tdsv    false            �            1259    16526    players_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."players_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public."players_ID_seq";
       public       tdsv    false    211            �           0    0    players_ID_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public."players_ID_seq" OWNED BY public.players."ID";
            public       tdsv    false    210            �            1259    16502    teams    TABLE     F  CREATE TABLE public.teams (
    "ID" integer NOT NULL,
    "Index" smallint NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Lobby" integer NOT NULL,
    "ColorR" smallint NOT NULL,
    "ColorG" smallint NOT NULL,
    "ColorB" smallint NOT NULL,
    "BlipColor" smallint NOT NULL,
    "SkinHash" integer NOT NULL
);
    DROP TABLE public.teams;
       public         tdsv    false            �            1259    16500    teams_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."teams_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public."teams_ID_seq";
       public       tdsv    false    206            �           0    0    teams_ID_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public."teams_ID_seq" OWNED BY public.teams."ID";
            public       tdsv    false    205            �            1259    16640    weapon_types    TABLE     m   CREATE TABLE public.weapon_types (
    "ID" smallint NOT NULL,
    "Name" character varying(100) NOT NULL
);
     DROP TABLE public.weapon_types;
       public         tdsv    false            �            1259    16646    weapons    TABLE     �   CREATE TABLE public.weapons (
    "Hash" bigint NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Type" smallint NOT NULL,
    "DefaultDamage" smallint NOT NULL,
    "DefaultHeadMultiplicator" real DEFAULT 1 NOT NULL
);
    DROP TABLE public.weapons;
       public         tdsv    false            /           2604    16437    commands ID    DEFAULT     n   ALTER TABLE ONLY public.commands ALTER COLUMN "ID" SET DEFAULT nextval('public."commands_ID_seq"'::regclass);
 <   ALTER TABLE public.commands ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    200    201    201            i           2604    16982 &   freeroam_default_vehicle VehicleTypeId    DEFAULT     �   ALTER TABLE ONLY public.freeroam_default_vehicle ALTER COLUMN "VehicleTypeId" SET DEFAULT nextval('public."freeroam_default_vehicle_VehicleTypeId_seq"'::regclass);
 W   ALTER TABLE public.freeroam_default_vehicle ALTER COLUMN "VehicleTypeId" DROP DEFAULT;
       public       tdsv    false    245    246    246            h           2604    16971    freeroam_vehicle_type Id    DEFAULT     �   ALTER TABLE ONLY public.freeroam_vehicle_type ALTER COLUMN "Id" SET DEFAULT nextval('public."freeroam_vehicle_type_Id_seq"'::regclass);
 I   ALTER TABLE public.freeroam_vehicle_type ALTER COLUMN "Id" DROP DEFAULT;
       public       tdsv    false    244    243    244            1           2604    16513    gangs ID    DEFAULT     h   ALTER TABLE ONLY public.gangs ALTER COLUMN "ID" SET DEFAULT nextval('public."gangs_ID_seq"'::regclass);
 9   ALTER TABLE public.gangs ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    208    207    208            7           2604    16556 
   lobbies Id    DEFAULT     l   ALTER TABLE ONLY public.lobbies ALTER COLUMN "Id" SET DEFAULT nextval('public."lobbies_ID_seq"'::regclass);
 ;   ALTER TABLE public.lobbies ALTER COLUMN "Id" DROP DEFAULT;
       public       tdsv    false    212    213    213            L           2604    16677    log_admins ID    DEFAULT     r   ALTER TABLE ONLY public.log_admins ALTER COLUMN "ID" SET DEFAULT nextval('public."log_admins_ID_seq"'::regclass);
 >   ALTER TABLE public.log_admins ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    223    222    223            P           2604    16713    log_chats ID    DEFAULT     p   ALTER TABLE ONLY public.log_chats ALTER COLUMN "ID" SET DEFAULT nextval('public."log_chats_ID_seq"'::regclass);
 =   ALTER TABLE public.log_chats ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    227    226    227            N           2604    16701    log_errors ID    DEFAULT     r   ALTER TABLE ONLY public.log_errors ALTER COLUMN "ID" SET DEFAULT nextval('public."log_errors_ID_seq"'::regclass);
 >   ALTER TABLE public.log_errors ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    225    224    225            R           2604    16734    log_rests ID    DEFAULT     p   ALTER TABLE ONLY public.log_rests ALTER COLUMN "ID" SET DEFAULT nextval('public."log_rests_ID_seq"'::regclass);
 =   ALTER TABLE public.log_rests ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    228    229    229            T           2604    16746    log_types ID    DEFAULT     i   ALTER TABLE ONLY public.log_types ALTER COLUMN "ID" SET DEFAULT nextval('public."ID_ID_seq"'::regclass);
 =   ALTER TABLE public.log_types ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    231    230    231            I           2604    16622    maps Id    DEFAULT     f   ALTER TABLE ONLY public.maps ALTER COLUMN "Id" SET DEFAULT nextval('public."maps_ID_seq"'::regclass);
 8   ALTER TABLE public.maps ALTER COLUMN "Id" DROP DEFAULT;
       public       tdsv    false    218    217    218            U           2604    16754    offlinemessages ID    DEFAULT     |   ALTER TABLE ONLY public.offlinemessages ALTER COLUMN "ID" SET DEFAULT nextval('public."offlinemessages_ID_seq"'::regclass);
 C   ALTER TABLE public.offlinemessages ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    232    233    233            2           2604    16531 
   players ID    DEFAULT     l   ALTER TABLE ONLY public.players ALTER COLUMN "ID" SET DEFAULT nextval('public."players_ID_seq"'::regclass);
 ;   ALTER TABLE public.players ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    211    210    211            f           2604    16905    server_settings ID    DEFAULT     {   ALTER TABLE ONLY public.server_settings ALTER COLUMN "ID" SET DEFAULT nextval('public."ServerSettings_ID_seq"'::regclass);
 C   ALTER TABLE public.server_settings ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    241    240    241            0           2604    16505    teams ID    DEFAULT     h   ALTER TABLE ONLY public.teams ALTER COLUMN "ID" SET DEFAULT nextval('public."teams_ID_seq"'::regclass);
 9   ALTER TABLE public.teams ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    205    206    206            N          0    16410    admin_level_names 
   TABLE DATA               H   COPY public.admin_level_names ("Level", "Language", "Name") FROM stdin;
    public       tdsv    false    198   �      M          0    16405    admin_levels 
   TABLE DATA               M   COPY public.admin_levels ("Level", "ColorR", "ColorG", "ColorB") FROM stdin;
    public       tdsv    false    197   �      R          0    16445    command_alias 
   TABLE DATA               ;   COPY public.command_alias ("Alias", "Command") FROM stdin;
    public       tdsv    false    202   0      S          0    16475    command_infos 
   TABLE DATA               A   COPY public.command_infos ("ID", "Language", "Info") FROM stdin;
    public       tdsv    false    203   �      Q          0    16434    commands 
   TABLE DATA               z   COPY public.commands ("ID", "Command", "NeededAdminLevel", "NeededDonation", "VipCanUse", "LobbyOwnerCanUse") FROM stdin;
    public       tdsv    false    201   �
      }          0    16979    freeroam_default_vehicle 
   TABLE DATA               Z   COPY public.freeroam_default_vehicle ("VehicleTypeId", "VehicleHash", "Note") FROM stdin;
    public       tdsv    false    246   �      {          0    16968    freeroam_vehicle_type 
   TABLE DATA               =   COPY public.freeroam_vehicle_type ("Id", "Type") FROM stdin;
    public       tdsv    false    244   ,      X          0    16510    gangs 
   TABLE DATA               8   COPY public.gangs ("ID", "TeamID", "Short") FROM stdin;
    public       tdsv    false    208   r      Y          0    16521    killingspree_rewards 
   TABLE DATA               i   COPY public.killingspree_rewards ("KillsAmount", "HealthOrArmor", "OnlyHealth", "OnlyArmor") FROM stdin;
    public       tdsv    false    209   �      O          0    16415 	   languages 
   TABLE DATA               5   COPY public.languages ("ID", "Language") FROM stdin;
    public       tdsv    false    199   �      ]          0    16553    lobbies 
   TABLE DATA               @  COPY public.lobbies ("Id", "Owner", "Type", "Name", "Password", "StartHealth", "StartArmor", "AmountLifes", "DefaultSpawnX", "DefaultSpawnY", "DefaultSpawnZ", "AroundSpawnPoint", "DefaultSpawnRotation", "IsTemporary", "IsOfficial", "SpawnAgainAfterDeathMs", "CreateTimestamp", "DieAfterOutsideMapLimitTime") FROM stdin;
    public       tdsv    false    213   �      `          0    16607 
   lobby_maps 
   TABLE DATA               8   COPY public.lobby_maps ("LobbyID", "MapID") FROM stdin;
    public       tdsv    false    216   �      ^          0    16580    lobby_rewards 
   TABLE DATA               f   COPY public.lobby_rewards ("LobbyID", "MoneyPerKill", "MoneyPerAssist", "MoneyPerDamage") FROM stdin;
    public       tdsv    false    214   �      _          0    16590    lobby_round_settings 
   TABLE DATA               �   COPY public.lobby_round_settings ("LobbyID", "RoundTime", "CountdownTime", "BombDetonateTimeMs", "BombDefuseTimeMs", "BombPlantTimeMs", "MixTeamsAfterRound") FROM stdin;
    public       tdsv    false    215   �      T          0    16495    lobby_types 
   TABLE DATA               3   COPY public.lobby_types ("ID", "Name") FROM stdin;
    public       tdsv    false    204   1      e          0    16657    lobby_weapons 
   TABLE DATA               _   COPY public.lobby_weapons ("Hash", "Lobby", "Ammo", "Damage", "HeadMultiplicator") FROM stdin;
    public       tdsv    false    221   �      g          0    16674 
   log_admins 
   TABLE DATA               |   COPY public.log_admins ("ID", "Type", "Source", "Target", "Lobby", "AsDonator", "AsVIP", "Reason", "Timestamp") FROM stdin;
    public       tdsv    false    223   �      k          0    16710 	   log_chats 
   TABLE DATA               {   COPY public.log_chats ("ID", "Source", "Target", "Message", "Lobby", "IsAdminChat", "IsTeamChat", "Timestamp") FROM stdin;
    public       tdsv    false    227   �      i          0    16698 
   log_errors 
   TABLE DATA               W   COPY public.log_errors ("ID", "Source", "Info", "StackTrace", "Timestamp") FROM stdin;
    public       tdsv    false    225         m          0    16731 	   log_rests 
   TABLE DATA               a   COPY public.log_rests ("ID", "Type", "Source", "Serial", "IP", "Lobby", "Timestamp") FROM stdin;
    public       tdsv    false    229   �      o          0    16743 	   log_types 
   TABLE DATA               1   COPY public.log_types ("ID", "Name") FROM stdin;
    public       tdsv    false    231   $      b          0    16619    maps 
   TABLE DATA               L   COPY public.maps ("Id", "Name", "CreatorId", "CreateTimestamp") FROM stdin;
    public       tdsv    false    218   �$      q          0    16751    offlinemessages 
   TABLE DATA               g   COPY public.offlinemessages ("ID", "TargetID", "SourceID", "Message", "Seen", "Timestamp") FROM stdin;
    public       tdsv    false    233   !%      r          0    16773    player_bans 
   TABLE DATA               s   COPY public.player_bans ("PlayerId", "LobbyId", "AdminId", "Reason", "StartTimestamp", "EndTimestamp") FROM stdin;
    public       tdsv    false    234   >%      s          0    16798    player_lobby_stats 
   TABLE DATA               �   COPY public.player_lobby_stats ("PlayerID", "LobbyID", "Kills", "Assists", "Deaths", "Damage", "TotalKills", "TotalAssists", "TotalDeaths", "TotalDamage") FROM stdin;
    public       tdsv    false    235   [%      t          0    16821    player_map_favourites 
   TABLE DATA               D   COPY public.player_map_favourites ("PlayerID", "MapID") FROM stdin;
    public       tdsv    false    236   �%      u          0    16851    player_map_ratings 
   TABLE DATA               K   COPY public.player_map_ratings ("PlayerID", "MapID", "Rating") FROM stdin;
    public       tdsv    false    237   �%      v          0    16866    player_settings 
   TABLE DATA               �   COPY public.player_settings ("PlayerID", "Language", "Hitsound", "Bloodscreen", "FloatingDamageInfo", "AllowDataTransfer") FROM stdin;
    public       tdsv    false    238   �%      w          0    16886    player_stats 
   TABLE DATA               u   COPY public.player_stats ("PlayerID", "Money", "PlayTime", "MuteTime", "LoggedIn", "LastLoginTimestamp") FROM stdin;
    public       tdsv    false    239   �%      [          0    16528    players 
   TABLE DATA               �   COPY public.players ("ID", "SCName", "Name", "Password", "Email", "AdminLvl", "IsVIP", "Donation", "GangId", "RegisterTimestamp") FROM stdin;
    public       tdsv    false    211   g&      y          0    16902    server_settings 
   TABLE DATA               s  COPY public.server_settings ("ID", "GamemodeName", "MapsPath", "NewMapsPath", "ErrorToPlayerOnNonExistentCommand", "ToChatOnNonExistentCommand", "DistanceToSpotToPlant", "DistanceToSpotToDefuse", "SavePlayerDataCooldownMinutes", "SaveLogsCooldownMinutes", "SaveSeasonsCooldownMinutes", "TeamOrderCooldownMs", "ArenaNewMapProbabilityPercent", "SavedMapsPath") FROM stdin;
    public       tdsv    false    241   �'      V          0    16502    teams 
   TABLE DATA               v   COPY public.teams ("ID", "Index", "Name", "Lobby", "ColorR", "ColorG", "ColorB", "BlipColor", "SkinHash") FROM stdin;
    public       tdsv    false    206   E(      c          0    16640    weapon_types 
   TABLE DATA               4   COPY public.weapon_types ("ID", "Name") FROM stdin;
    public       tdsv    false    219   �(      d          0    16646    weapons 
   TABLE DATA               f   COPY public.weapons ("Hash", "Name", "Type", "DefaultDamage", "DefaultHeadMultiplicator") FROM stdin;
    public       tdsv    false    220   X)      �           0    0 	   ID_ID_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public."ID_ID_seq"', 1, false);
            public       tdsv    false    230            �           0    0    ServerSettings_ID_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."ServerSettings_ID_seq"', 1, false);
            public       tdsv    false    240            �           0    0    commands_ID_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."commands_ID_seq"', 18, true);
            public       tdsv    false    200            �           0    0 *   freeroam_default_vehicle_VehicleTypeId_seq    SEQUENCE SET     [   SELECT pg_catalog.setval('public."freeroam_default_vehicle_VehicleTypeId_seq"', 1, false);
            public       tdsv    false    245            �           0    0    freeroam_vehicle_type_Id_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('public."freeroam_vehicle_type_Id_seq"', 1, false);
            public       tdsv    false    243            �           0    0    gangs_ID_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."gangs_ID_seq"', 1, false);
            public       tdsv    false    207            �           0    0    lobbies_ID_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."lobbies_ID_seq"', 20, true);
            public       tdsv    false    212            �           0    0    log_admins_ID_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."log_admins_ID_seq"', 2, true);
            public       tdsv    false    222            �           0    0    log_chats_ID_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."log_chats_ID_seq"', 19, true);
            public       tdsv    false    226            �           0    0    log_errors_ID_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."log_errors_ID_seq"', 6, true);
            public       tdsv    false    224            �           0    0    log_rests_ID_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public."log_rests_ID_seq"', 245, true);
            public       tdsv    false    228            �           0    0    maps_ID_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."maps_ID_seq"', 113, true);
            public       tdsv    false    217            �           0    0    offlinemessages_ID_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public."offlinemessages_ID_seq"', 1, false);
            public       tdsv    false    232            �           0    0    players_ID_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public."players_ID_seq"', 3, true);
            public       tdsv    false    210            �           0    0    teams_ID_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."teams_ID_seq"', 19, true);
            public       tdsv    false    205            �           2606    16748    log_types ID_pkey 
   CONSTRAINT     S   ALTER TABLE ONLY public.log_types
    ADD CONSTRAINT "ID_pkey" PRIMARY KEY ("ID");
 =   ALTER TABLE ONLY public.log_types DROP CONSTRAINT "ID_pkey";
       public         tdsv    false    231            m           2606    16426 &   admin_level_names PK_admin_level_names 
   CONSTRAINT     w   ALTER TABLE ONLY public.admin_level_names
    ADD CONSTRAINT "PK_admin_level_names" PRIMARY KEY ("Level", "Language");
 R   ALTER TABLE ONLY public.admin_level_names DROP CONSTRAINT "PK_admin_level_names";
       public         tdsv    false    198    198            �           2606    16910 #   server_settings ServerSettings_pkey 
   CONSTRAINT     e   ALTER TABLE ONLY public.server_settings
    ADD CONSTRAINT "ServerSettings_pkey" PRIMARY KEY ("ID");
 O   ALTER TABLE ONLY public.server_settings DROP CONSTRAINT "ServerSettings_pkey";
       public         tdsv    false    241            k           2606    16409    admin_levels admin_levels_pkey 
   CONSTRAINT     a   ALTER TABLE ONLY public.admin_levels
    ADD CONSTRAINT admin_levels_pkey PRIMARY KEY ("Level");
 H   ALTER TABLE ONLY public.admin_levels DROP CONSTRAINT admin_levels_pkey;
       public         tdsv    false    197            s           2606    16449     command_alias command_alias_pkey 
   CONSTRAINT     n   ALTER TABLE ONLY public.command_alias
    ADD CONSTRAINT command_alias_pkey PRIMARY KEY ("Alias", "Command");
 J   ALTER TABLE ONLY public.command_alias DROP CONSTRAINT command_alias_pkey;
       public         tdsv    false    202    202            u           2606    16919     command_infos command_infos_pkey 
   CONSTRAINT     l   ALTER TABLE ONLY public.command_infos
    ADD CONSTRAINT command_infos_pkey PRIMARY KEY ("ID", "Language");
 J   ALTER TABLE ONLY public.command_infos DROP CONSTRAINT command_infos_pkey;
       public         tdsv    false    203    203            q           2606    16439    commands commands_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.commands
    ADD CONSTRAINT commands_pkey PRIMARY KEY ("ID");
 @   ALTER TABLE ONLY public.commands DROP CONSTRAINT commands_pkey;
       public         tdsv    false    201            �           2606    16984 6   freeroam_default_vehicle freeroam_default_vehicle_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public.freeroam_default_vehicle
    ADD CONSTRAINT freeroam_default_vehicle_pkey PRIMARY KEY ("VehicleTypeId");
 `   ALTER TABLE ONLY public.freeroam_default_vehicle DROP CONSTRAINT freeroam_default_vehicle_pkey;
       public         tdsv    false    246            �           2606    16976 0   freeroam_vehicle_type freeroam_vehicle_type_pkey 
   CONSTRAINT     p   ALTER TABLE ONLY public.freeroam_vehicle_type
    ADD CONSTRAINT freeroam_vehicle_type_pkey PRIMARY KEY ("Id");
 Z   ALTER TABLE ONLY public.freeroam_vehicle_type DROP CONSTRAINT freeroam_vehicle_type_pkey;
       public         tdsv    false    244            {           2606    16515    gangs gangs_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.gangs
    ADD CONSTRAINT gangs_pkey PRIMARY KEY ("ID");
 :   ALTER TABLE ONLY public.gangs DROP CONSTRAINT gangs_pkey;
       public         tdsv    false    208            }           2606    16525 .   killingspree_rewards killingspree_rewards_pkey 
   CONSTRAINT     w   ALTER TABLE ONLY public.killingspree_rewards
    ADD CONSTRAINT killingspree_rewards_pkey PRIMARY KEY ("KillsAmount");
 X   ALTER TABLE ONLY public.killingspree_rewards DROP CONSTRAINT killingspree_rewards_pkey;
       public         tdsv    false    209            o           2606    16419    languages languages_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.languages
    ADD CONSTRAINT languages_pkey PRIMARY KEY ("ID");
 B   ALTER TABLE ONLY public.languages DROP CONSTRAINT languages_pkey;
       public         tdsv    false    199            �           2606    16569    lobbies lobbies_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.lobbies
    ADD CONSTRAINT lobbies_pkey PRIMARY KEY ("Id");
 >   ALTER TABLE ONLY public.lobbies DROP CONSTRAINT lobbies_pkey;
       public         tdsv    false    213            �           2606    16611    lobby_maps lobby_maps_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.lobby_maps
    ADD CONSTRAINT lobby_maps_pkey PRIMARY KEY ("LobbyID", "MapID");
 D   ALTER TABLE ONLY public.lobby_maps DROP CONSTRAINT lobby_maps_pkey;
       public         tdsv    false    216    216            �           2606    16584     lobby_rewards lobby_rewards_pkey 
   CONSTRAINT     e   ALTER TABLE ONLY public.lobby_rewards
    ADD CONSTRAINT lobby_rewards_pkey PRIMARY KEY ("LobbyID");
 J   ALTER TABLE ONLY public.lobby_rewards DROP CONSTRAINT lobby_rewards_pkey;
       public         tdsv    false    214            �           2606    16600 +   lobby_round_settings lobby_round_infos_pkey 
   CONSTRAINT     p   ALTER TABLE ONLY public.lobby_round_settings
    ADD CONSTRAINT lobby_round_infos_pkey PRIMARY KEY ("LobbyID");
 U   ALTER TABLE ONLY public.lobby_round_settings DROP CONSTRAINT lobby_round_infos_pkey;
       public         tdsv    false    215            w           2606    16499    lobby_types lobby_types_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.lobby_types
    ADD CONSTRAINT lobby_types_pkey PRIMARY KEY ("ID");
 F   ALTER TABLE ONLY public.lobby_types DROP CONSTRAINT lobby_types_pkey;
       public         tdsv    false    204            �           2606    16932     lobby_weapons lobby_weapons_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public.lobby_weapons
    ADD CONSTRAINT lobby_weapons_pkey PRIMARY KEY ("Hash");
 J   ALTER TABLE ONLY public.lobby_weapons DROP CONSTRAINT lobby_weapons_pkey;
       public         tdsv    false    221            �           2606    16683    log_admins log_admins_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.log_admins
    ADD CONSTRAINT log_admins_pkey PRIMARY KEY ("ID");
 D   ALTER TABLE ONLY public.log_admins DROP CONSTRAINT log_admins_pkey;
       public         tdsv    false    223            �           2606    16719    log_chats log_chats_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.log_chats
    ADD CONSTRAINT log_chats_pkey PRIMARY KEY ("ID");
 B   ALTER TABLE ONLY public.log_chats DROP CONSTRAINT log_chats_pkey;
       public         tdsv    false    227            �           2606    16707    log_errors log_errors_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.log_errors
    ADD CONSTRAINT log_errors_pkey PRIMARY KEY ("ID");
 D   ALTER TABLE ONLY public.log_errors DROP CONSTRAINT log_errors_pkey;
       public         tdsv    false    225            �           2606    16740    log_rests log_rests_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.log_rests
    ADD CONSTRAINT log_rests_pkey PRIMARY KEY ("ID");
 B   ALTER TABLE ONLY public.log_rests DROP CONSTRAINT log_rests_pkey;
       public         tdsv    false    229            �           2606    16627    maps maps_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.maps
    ADD CONSTRAINT maps_pkey PRIMARY KEY ("Id");
 8   ALTER TABLE ONLY public.maps DROP CONSTRAINT maps_pkey;
       public         tdsv    false    218            �           2606    16761 $   offlinemessages offlinemessages_pkey 
   CONSTRAINT     d   ALTER TABLE ONLY public.offlinemessages
    ADD CONSTRAINT offlinemessages_pkey PRIMARY KEY ("ID");
 N   ALTER TABLE ONLY public.offlinemessages DROP CONSTRAINT offlinemessages_pkey;
       public         tdsv    false    233            �           2606    16943    player_bans player_bans_pkey 
   CONSTRAINT     m   ALTER TABLE ONLY public.player_bans
    ADD CONSTRAINT player_bans_pkey PRIMARY KEY ("PlayerId", "LobbyId");
 F   ALTER TABLE ONLY public.player_bans DROP CONSTRAINT player_bans_pkey;
       public         tdsv    false    234    234            �           2606    16917 *   player_lobby_stats player_lobby_stats_pkey 
   CONSTRAINT     {   ALTER TABLE ONLY public.player_lobby_stats
    ADD CONSTRAINT player_lobby_stats_pkey PRIMARY KEY ("PlayerID", "LobbyID");
 T   ALTER TABLE ONLY public.player_lobby_stats DROP CONSTRAINT player_lobby_stats_pkey;
       public         tdsv    false    235    235            �           2606    16850 0   player_map_favourites player_map_favourites_pkey 
   CONSTRAINT        ALTER TABLE ONLY public.player_map_favourites
    ADD CONSTRAINT player_map_favourites_pkey PRIMARY KEY ("PlayerID", "MapID");
 Z   ALTER TABLE ONLY public.player_map_favourites DROP CONSTRAINT player_map_favourites_pkey;
       public         tdsv    false    236    236            �           2606    16855 *   player_map_ratings player_map_ratings_pkey 
   CONSTRAINT     y   ALTER TABLE ONLY public.player_map_ratings
    ADD CONSTRAINT player_map_ratings_pkey PRIMARY KEY ("PlayerID", "MapID");
 T   ALTER TABLE ONLY public.player_map_ratings DROP CONSTRAINT player_map_ratings_pkey;
       public         tdsv    false    237    237            �           2606    16870 $   player_settings player_settings_pkey 
   CONSTRAINT     j   ALTER TABLE ONLY public.player_settings
    ADD CONSTRAINT player_settings_pkey PRIMARY KEY ("PlayerID");
 N   ALTER TABLE ONLY public.player_settings DROP CONSTRAINT player_settings_pkey;
       public         tdsv    false    238            �           2606    16894    player_stats player_stats_pkey 
   CONSTRAINT     d   ALTER TABLE ONLY public.player_stats
    ADD CONSTRAINT player_stats_pkey PRIMARY KEY ("PlayerID");
 H   ALTER TABLE ONLY public.player_stats DROP CONSTRAINT player_stats_pkey;
       public         tdsv    false    239                       2606    16540    players players_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.players
    ADD CONSTRAINT players_pkey PRIMARY KEY ("ID");
 >   ALTER TABLE ONLY public.players DROP CONSTRAINT players_pkey;
       public         tdsv    false    211            y           2606    16507    teams teams_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.teams
    ADD CONSTRAINT teams_pkey PRIMARY KEY ("ID");
 :   ALTER TABLE ONLY public.teams DROP CONSTRAINT teams_pkey;
       public         tdsv    false    206            �           2606    16644    weapon_types weapon_types_pkey 
   CONSTRAINT     ^   ALTER TABLE ONLY public.weapon_types
    ADD CONSTRAINT weapon_types_pkey PRIMARY KEY ("ID");
 H   ALTER TABLE ONLY public.weapon_types DROP CONSTRAINT weapon_types_pkey;
       public         tdsv    false    219            �           2606    16921    weapons weapons_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.weapons
    ADD CONSTRAINT weapons_pkey PRIMARY KEY ("Hash");
 >   ALTER TABLE ONLY public.weapons DROP CONSTRAINT weapons_pkey;
       public         tdsv    false    220            �           1259    16633    Index_maps_name    INDEX     C   CREATE INDEX "Index_maps_name" ON public.maps USING hash ("Name");
 %   DROP INDEX public."Index_maps_name";
       public         tdsv    false    218            �           1259    16639    fki_FK_lobby_maps_maps    INDEX     R   CREATE INDEX "fki_FK_lobby_maps_maps" ON public.lobby_maps USING btree ("MapID");
 ,   DROP INDEX public."fki_FK_lobby_maps_maps";
       public         tdsv    false    216            �           2606    16420 2   admin_level_names FK_admin_level_names_admin_level    FK CONSTRAINT     �   ALTER TABLE ONLY public.admin_level_names
    ADD CONSTRAINT "FK_admin_level_names_admin_level" FOREIGN KEY ("Level") REFERENCES public.admin_levels("Level") ON UPDATE CASCADE ON DELETE CASCADE;
 ^   ALTER TABLE ONLY public.admin_level_names DROP CONSTRAINT "FK_admin_level_names_admin_level";
       public       tdsv    false    2923    197    198            �           2606    16427 /   admin_level_names FK_admin_level_names_language    FK CONSTRAINT     �   ALTER TABLE ONLY public.admin_level_names
    ADD CONSTRAINT "FK_admin_level_names_language" FOREIGN KEY ("Language") REFERENCES public.languages("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 [   ALTER TABLE ONLY public.admin_level_names DROP CONSTRAINT "FK_admin_level_names_language";
       public       tdsv    false    199    2927    198            �           2606    16440 !   commands FK_commands_admin_levels    FK CONSTRAINT     �   ALTER TABLE ONLY public.commands
    ADD CONSTRAINT "FK_commands_admin_levels" FOREIGN KEY ("NeededAdminLevel") REFERENCES public.admin_levels("Level");
 M   ALTER TABLE ONLY public.commands DROP CONSTRAINT "FK_commands_admin_levels";
       public       tdsv    false    201    197    2923            �           2606    16634    lobby_maps FK_lobby_maps_maps    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_maps
    ADD CONSTRAINT "FK_lobby_maps_maps" FOREIGN KEY ("MapID") REFERENCES public.maps("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 I   ALTER TABLE ONLY public.lobby_maps DROP CONSTRAINT "FK_lobby_maps_maps";
       public       tdsv    false    216    218    2955            �           2606    16490 (   command_alias command_alias_Command_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.command_alias
    ADD CONSTRAINT "command_alias_Command_fkey" FOREIGN KEY ("Command") REFERENCES public.commands("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 T   ALTER TABLE ONLY public.command_alias DROP CONSTRAINT "command_alias_Command_fkey";
       public       tdsv    false    2929    201    202            �           2606    16480 #   command_infos command_infos_ID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.command_infos
    ADD CONSTRAINT "command_infos_ID_fkey" FOREIGN KEY ("ID") REFERENCES public.commands("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 O   ALTER TABLE ONLY public.command_infos DROP CONSTRAINT "command_infos_ID_fkey";
       public       tdsv    false    2929    201    203            �           2606    16485 )   command_infos command_infos_Language_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.command_infos
    ADD CONSTRAINT "command_infos_Language_fkey" FOREIGN KEY ("Language") REFERENCES public.languages("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 U   ALTER TABLE ONLY public.command_infos DROP CONSTRAINT "command_infos_Language_fkey";
       public       tdsv    false    203    2927    199            �           2606    16985 D   freeroam_default_vehicle freeroam_default_vehicle_VehicleTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.freeroam_default_vehicle
    ADD CONSTRAINT "freeroam_default_vehicle_VehicleTypeId_fkey" FOREIGN KEY ("VehicleTypeId") REFERENCES public.freeroam_vehicle_type("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 p   ALTER TABLE ONLY public.freeroam_default_vehicle DROP CONSTRAINT "freeroam_default_vehicle_VehicleTypeId_fkey";
       public       tdsv    false    244    246    2989            �           2606    16516    gangs gangs_TeamID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.gangs
    ADD CONSTRAINT "gangs_TeamID_fkey" FOREIGN KEY ("TeamID") REFERENCES public.teams("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 C   ALTER TABLE ONLY public.gangs DROP CONSTRAINT "gangs_TeamID_fkey";
       public       tdsv    false    206    208    2937            �           2606    16570    lobbies lobbies_Owner_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobbies
    ADD CONSTRAINT "lobbies_Owner_fkey" FOREIGN KEY ("Owner") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 F   ALTER TABLE ONLY public.lobbies DROP CONSTRAINT "lobbies_Owner_fkey";
       public       tdsv    false    211    2943    213            �           2606    16575    lobbies lobbies_Type_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobbies
    ADD CONSTRAINT "lobbies_Type_fkey" FOREIGN KEY ("Type") REFERENCES public.lobby_types("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 E   ALTER TABLE ONLY public.lobbies DROP CONSTRAINT "lobbies_Type_fkey";
       public       tdsv    false    213    2935    204            �           2606    16612 "   lobby_maps lobby_maps_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_maps
    ADD CONSTRAINT "lobby_maps_LobbyID_fkey" FOREIGN KEY ("LobbyID") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 N   ALTER TABLE ONLY public.lobby_maps DROP CONSTRAINT "lobby_maps_LobbyID_fkey";
       public       tdsv    false    216    213    2945            �           2606    16585 (   lobby_rewards lobby_rewards_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_rewards
    ADD CONSTRAINT "lobby_rewards_LobbyID_fkey" FOREIGN KEY ("LobbyID") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 T   ALTER TABLE ONLY public.lobby_rewards DROP CONSTRAINT "lobby_rewards_LobbyID_fkey";
       public       tdsv    false    2945    213    214            �           2606    16601 3   lobby_round_settings lobby_round_infos_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_round_settings
    ADD CONSTRAINT "lobby_round_infos_LobbyID_fkey" FOREIGN KEY ("LobbyID") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 _   ALTER TABLE ONLY public.lobby_round_settings DROP CONSTRAINT "lobby_round_infos_LobbyID_fkey";
       public       tdsv    false    213    215    2945            �           2606    16933 %   lobby_weapons lobby_weapons_Hash_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_weapons
    ADD CONSTRAINT "lobby_weapons_Hash_fkey" FOREIGN KEY ("Hash") REFERENCES public.weapons("Hash") ON UPDATE CASCADE ON DELETE CASCADE;
 Q   ALTER TABLE ONLY public.lobby_weapons DROP CONSTRAINT "lobby_weapons_Hash_fkey";
       public       tdsv    false    2959    221    220            �           2606    16667 &   lobby_weapons lobby_weapons_Lobby_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_weapons
    ADD CONSTRAINT "lobby_weapons_Lobby_fkey" FOREIGN KEY ("Lobby") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 R   ALTER TABLE ONLY public.lobby_weapons DROP CONSTRAINT "lobby_weapons_Lobby_fkey";
       public       tdsv    false    2945    221    213            �           2606    16628    maps maps_CreatorID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.maps
    ADD CONSTRAINT "maps_CreatorID_fkey" FOREIGN KEY ("CreatorId") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE SET NULL;
 D   ALTER TABLE ONLY public.maps DROP CONSTRAINT "maps_CreatorID_fkey";
       public       tdsv    false    218    211    2943            �           2606    16767 -   offlinemessages offlinemessages_SourceID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.offlinemessages
    ADD CONSTRAINT "offlinemessages_SourceID_fkey" FOREIGN KEY ("SourceID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 Y   ALTER TABLE ONLY public.offlinemessages DROP CONSTRAINT "offlinemessages_SourceID_fkey";
       public       tdsv    false    233    2943    211            �           2606    16762 -   offlinemessages offlinemessages_TargetID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.offlinemessages
    ADD CONSTRAINT "offlinemessages_TargetID_fkey" FOREIGN KEY ("TargetID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 Y   ALTER TABLE ONLY public.offlinemessages DROP CONSTRAINT "offlinemessages_TargetID_fkey";
       public       tdsv    false    2943    211    233            �           2606    16793 $   player_bans player_bans_AdminID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_bans
    ADD CONSTRAINT "player_bans_AdminID_fkey" FOREIGN KEY ("AdminId") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE SET NULL;
 P   ALTER TABLE ONLY public.player_bans DROP CONSTRAINT "player_bans_AdminID_fkey";
       public       tdsv    false    2943    234    211            �           2606    16788 $   player_bans player_bans_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_bans
    ADD CONSTRAINT "player_bans_LobbyID_fkey" FOREIGN KEY ("LobbyId") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 P   ALTER TABLE ONLY public.player_bans DROP CONSTRAINT "player_bans_LobbyID_fkey";
       public       tdsv    false    2945    213    234            �           2606    16783 %   player_bans player_bans_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_bans
    ADD CONSTRAINT "player_bans_PlayerID_fkey" FOREIGN KEY ("PlayerId") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 Q   ALTER TABLE ONLY public.player_bans DROP CONSTRAINT "player_bans_PlayerID_fkey";
       public       tdsv    false    234    2943    211            �           2606    16816 2   player_lobby_stats player_lobby_stats_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_lobby_stats
    ADD CONSTRAINT "player_lobby_stats_LobbyID_fkey" FOREIGN KEY ("LobbyID") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 ^   ALTER TABLE ONLY public.player_lobby_stats DROP CONSTRAINT "player_lobby_stats_LobbyID_fkey";
       public       tdsv    false    235    2945    213            �           2606    16811 3   player_lobby_stats player_lobby_stats_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_lobby_stats
    ADD CONSTRAINT "player_lobby_stats_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 _   ALTER TABLE ONLY public.player_lobby_stats DROP CONSTRAINT "player_lobby_stats_PlayerID_fkey";
       public       tdsv    false    235    211    2943            �           2606    16829 6   player_map_favourites player_map_favourites_MapID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_map_favourites
    ADD CONSTRAINT "player_map_favourites_MapID_fkey" FOREIGN KEY ("MapID") REFERENCES public.maps("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 b   ALTER TABLE ONLY public.player_map_favourites DROP CONSTRAINT "player_map_favourites_MapID_fkey";
       public       tdsv    false    236    218    2955            �           2606    16824 9   player_map_favourites player_map_favourites_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_map_favourites
    ADD CONSTRAINT "player_map_favourites_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 e   ALTER TABLE ONLY public.player_map_favourites DROP CONSTRAINT "player_map_favourites_PlayerID_fkey";
       public       tdsv    false    2943    236    211            �           2606    16861 0   player_map_ratings player_map_ratings_MapID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_map_ratings
    ADD CONSTRAINT "player_map_ratings_MapID_fkey" FOREIGN KEY ("MapID") REFERENCES public.maps("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 \   ALTER TABLE ONLY public.player_map_ratings DROP CONSTRAINT "player_map_ratings_MapID_fkey";
       public       tdsv    false    2955    218    237            �           2606    16856 3   player_map_ratings player_map_ratings_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_map_ratings
    ADD CONSTRAINT "player_map_ratings_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 _   ALTER TABLE ONLY public.player_map_ratings DROP CONSTRAINT "player_map_ratings_PlayerID_fkey";
       public       tdsv    false    2943    211    237            �           2606    16876 -   player_settings player_settings_Language_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_settings
    ADD CONSTRAINT "player_settings_Language_fkey" FOREIGN KEY ("Language") REFERENCES public.languages("ID") ON UPDATE CASCADE ON DELETE RESTRICT;
 Y   ALTER TABLE ONLY public.player_settings DROP CONSTRAINT "player_settings_Language_fkey";
       public       tdsv    false    199    238    2927            �           2606    16871 -   player_settings player_settings_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_settings
    ADD CONSTRAINT "player_settings_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 Y   ALTER TABLE ONLY public.player_settings DROP CONSTRAINT "player_settings_PlayerID_fkey";
       public       tdsv    false    2943    238    211            �           2606    16895 '   player_stats player_stats_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_stats
    ADD CONSTRAINT "player_stats_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 S   ALTER TABLE ONLY public.player_stats DROP CONSTRAINT "player_stats_PlayerID_fkey";
       public       tdsv    false    2943    239    211            �           2606    16541    players players_AdminLvl_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.players
    ADD CONSTRAINT "players_AdminLvl_fkey" FOREIGN KEY ("AdminLvl") REFERENCES public.admin_levels("Level") ON UPDATE CASCADE ON DELETE CASCADE;
 I   ALTER TABLE ONLY public.players DROP CONSTRAINT "players_AdminLvl_fkey";
       public       tdsv    false    2923    197    211            �           2606    16546    players players_GangId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.players
    ADD CONSTRAINT "players_GangId_fkey" FOREIGN KEY ("GangId") REFERENCES public.gangs("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 G   ALTER TABLE ONLY public.players DROP CONSTRAINT "players_GangId_fkey";
       public       tdsv    false    2939    208    211            �           2606    16911    teams teams_Lobby_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.teams
    ADD CONSTRAINT "teams_Lobby_fkey" FOREIGN KEY ("Lobby") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 B   ALTER TABLE ONLY public.teams DROP CONSTRAINT "teams_Lobby_fkey";
       public       tdsv    false    2945    213    206            �           2606    16652    weapons weapons_Type_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.weapons
    ADD CONSTRAINT "weapons_Type_fkey" FOREIGN KEY ("Type") REFERENCES public.weapon_types("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 E   ALTER TABLE ONLY public.weapons DROP CONSTRAINT "weapons_Type_fkey";
       public       tdsv    false    219    220    2957            N   W   x�3�4�-N-�2ഄ0�"���E%`�%�(瘒���Y\R�X��D1�	(��J�.�I��2��$ES�"1z\\\ ��%%      M   3   x�5��	  ��w�H�Rp����	A|��,�ѥ,�۳A�ϲ}�Uz      R   b  x�]�_o� ş�fY���X��Y�I��m��7�H� 5����̽p~�Rহ֦�{d�	[�.ص�zY�l� ��M+��7���T�t�7|&4{��uĊ�M�"�[
���4�p%�h�0���/O;aOI6�����������ia�����	]f�9dS�I���>=&}�M��Ұgx��e}�'/3�O�a��7�S�[
��$��/(�eA�!�� s�T�\)�� `� ʸ�y�L-��w����"���~�(ژyLʩ��"�x�1�_oD �o��9��Z�cp��I'�Q�}It+R�\����5�=��,C��C���a��)%�(mZ�P�M&�,#�S&��  �C!ښ      S   *  x��U�R�@<�_1GSE\Q8:�� !T �%��4�V��>�&�|'n��̬6�T�^����<��f�뼲(3���D��+�J�E-�t����w+=:hBF��~~�K#�G�Av�1E��w`�ZM���Dh�93E^7ZX���D���-�S�
h�X��_!�t@�7���%Z]�&�:�4��f���$�¹������r��s�c����[H-H��҄m\L({�M�i{y�Z�2d�J:X`%s�� b9��������!Up.��<�8��y݋���ya�l��������,�Rd�&�<,�-��^1_1Q�x��)7u-tK�����g/��?jf���Or>��O��S9�ml-��V�32�ȝd�ͬ-�K�mD]e!nѯ��K�'��rnq�Rѳn��t�Q� ��Z�G����,�&o)�7�j�۹�).D:�@���l=A»���w�br�*s��܋ �V��ɞ� sY �?#B�^�?�]�,l2�Bq�:�>Z�sG##�.2�5'��B;bQ�4��%QQ�>�[7/
��ʬCMm�ǽ�AQG|����L�h�����Ix���z1��q�K�Mr�(8�LG��o�o��ۍ[;<�J���LCue�B�vT��A��R����n�����*��_m˶$�Ҳ��d�Oo�ui�q��!��L��T���N>FK��_�,��X��Ie}�r�`��y^{ۃ�ો��Ⱥu��,N�96���s9\Z�Ob�e�����h��2�b9�5�1��3.\��,��o��x<�k�rk      Q   �   x�m�M�0E�7?�H�����6��JF%Fÿ�:Q_�l�9�ΛDc��]��9r�*��xG��#����ĉ�.�f�����=�\�ު����f�AQ}�g�8���ӱ��v]�1]���*��<��ɾp�I�R*�O�0!VL�_�Ar2�U]$�4b���=�W�{��8����#ۿ��7�\���Y��Rw�9iM      }   n   x��1�0������'vF���X�T*RQ!���C�d)l���Ϸ��!�HHR�8����B+�{WL�}#�%�)
+.��nGVJ0NU��T��گ��>D���	      {   6   x�3�tN,�2��H��L�/(I-�2��I�K�2�t��N�2�t�O,����� -G      X      x�3�4������� �V      Y   %   x�3�46���".SNS�Ѐ�� �1E���qqq 	-	�      O       x�3�tO-�M���t�K��,������� Vqc      ]   �   x��ϱ
�0�9y��@����6�����]R�⒖�������~>�H��6^R;�UuG�Ǖ2���V��Z�e�7_���"��Q�yH�]����%�ő�>��a��ۿ��#Xz:9�d�i7qg��zEM�c�E�ZT8�|��i�u��Rk}�:q      `      x�3��5�����  K      ^      x�3�42�44�4�34@f\F�$c���� M�
f      _   "   x�3�421�4�41500� � ��+F��� N��      T   @   x�3��M���M�+�2�t�L�(��OJ��2�t,J�K�2�tO�K��� �8�&��Bb���� ���      e   "   x�362204737��4��? ����� SI      g      x������ � �      k   4  x�}�1n�0Eg��@�"Eʇ��1��K�ɾ?J�i��r�E0���>e�p{�>��v%�r����*���$���g��E�B�S�u��iy���q�0�=}�_�r�s���^�43�]g���G�H�^�9��������CQ��!��E����S��$�}�̄��to�c]֮x��i� �ـ�.��u��W�	�N��Ȋbj��_�ϼmuc�R����x�Q�G����<=���t����VR'{�?O��"גg�'}z��#��������&���n�"�Gj��0"(n ?��      i   a  x���sڸ����BO��N��4m'��$�[f�&[���C�9�7�b%������e!LI���b~/����,듏<|�ʋ��	Jld'�NF$~� ͠A��2��g�ә4����gq+�H�=�&�@�&&�$�@Z���C���RWX�}c��/�|Jcb,��di���O�����!�^�t+�_n���1��\&�ϟW6�9^p%��f4��;
y��RD�8~�j]Ҧ�:j�Uj�j���X5j��J�|��~H;�[k��=���Zy ��GⒹP�yG	��7�V��)�Rp���(&�*�B2���$aS��s��=N��K����n�s��cu���r"����ҝ=�q���)]t���
��Q��@��FI��ɮYu�҉�K�-l���"�7��4�k��|��?Wc��-(�k��>.����#�W�p��ʈ{888���ԎR��KN����9�ەj�\:
]_�(MH���%]ɶp��r����󥹐ns�����Mc���1e�sG���8V���3W�:��몟g�2�+�EO�����'<�|"��|�����Ƿ��\��]d�u}wxU��(�ʨ�����j�ҭ��˚�Zg�TC��d �>�,t�WÐF�(��I#�"c,���Y'5�O��b	�s`�s�#�ܸ똎� _>' ��8��8��8�u�-tD���7� ���|��_�R�.��}M���9�������߰�\2]g��o3��y�?J~��m�>M��]�F������8�M��'���]����ϫQ���ciÁ�J? �t���m�Sj�����b$]�\�X`�9���:H��nԋB��p(�n��C��3%c���7�"P\K���nHLY�R��[��zg)1�Z��m�k��rrKc���u588�ƺ\.�KO2ߋ��VC�μ[�n$ԓ1����қ�� ���S���&�7Hu��y^z�T�9��Lq}Y��$�Tk&��4bš��b�'W��=z������u9�ڍ�<�+ψ��j��?3 ��gi.5�#��x�[J)=�J����w	�`��1�=^癨Ӆ>�e�-��j��*�B��t��.��|�гO�Wf��Y�۶�ٚLsϜqL��YD�j|m��ى����A~o[�]Ns�ēZ�� �d~Ő�#x�P�E��(��М�~�]���$2Hd�2���^���|k����/(J�U$?���s���@H[I������������1�pEg��S|r�R������J�8����M�����J��+v���8 �6	(��q����v<P��ю%��V`��l�P�Y�fUg��	.���+�Ҳ/�%׳��Y_.��z�[������3���\xKZ����;e�*Hk�[��E^����uo!>��0��1�X�F���HJ�8i��c'&|�W<wj0{�}��{�����a�K�=��ˎ��&��^���+o_��^Uފj�]u��n9��}�_�٪����Q�G|dw�=6�k0�1��� ��q�q��� |�v�.|`�C�@q�Q����|��=�{ �Y�D�Hi�#��9�= ���Q8��{�����}��{���;[;�=�{���|���ms�=�#p\s�{ A ��݂������P�x�{������|�Hd�6����@Z�H��r@| � �P�
��Q�
�*�������ו��.*TEzd@�Bv�j��s�*��GT@B��`���B���p=0�@7ET� PA��@�
��Md P�4�V<� P���@( � Tq��@���_�C��@�J�z��Ueowok�"=2 P!��@�A\���ap�k�#* !P�]�[pv!PC`�B���"*T� PA��@�
���&2�@H+i�@ȁ�� �P �8@A��@��/�!P�X�Ҿ����U�^�U��T���)�J�5����*G��8�U�@���g�0��!\%���(�U��W	�\%�Jp��Ȭm"W	����W	�� 
@( ��\%�J����ҏ�*�q�Ru��\�j[[[hy}B      m   �  x��\K��
�V�h����w�Vp�����s�[Ρ�t��� lz�o����ޜr�L.Y���q!�[�/�RN�N��M*Q�7�˼���䣫~Ya�o�����>J2}���˧�;��O��b�]�O�������Y~��|Wbm�V��W�*����&��?��[k񟟢Lߥ��%��3��[����G��J��b"��d����[m��7���X�zҗ���D��V���v��S�L��Q���rc�n���,��>!�ʛ�`����$���^���MZ"~	S�T%���ox+e*��%�w��gP�D�Y�V�Ly�4����NV%��`�3e.�s�^���'�R�j��~�^(�V��˔r�[-E{���y������K��?#p+'�)�J�Ki5�+9x�t�S��|!	S;�r"���ϟ��(i����MS�K|�=��{��?�23<W-�1�����M6|�8����g s��I,�rd
M��L\=U��"7�У�*YdX6-Ű�zl��vȄ� ��-r�[�ʱA���M:qMU����'
 }z�\�2J�T��"��S�Q�x��"|�~"�"��R�$���"7�UQ�4:��`��[#Om�˞,�ڳ���DY�r��Gk/�Ot���m��R�Ij.;�;L"���e��3)�ǖ�\��.i�'��.�P�G=Z��������6�d�ŷ��	���P>�$�����h��N�@�F�z�d�ǟ,����*5�$�yd��/�a"�'A�L��|�D~�K �;��i����h8�Ͽ�3_kM����=��Ţ�ӱ�[�)i��֦�,��^��k�ZұZ�s���k��N��7|A�������ݨ'{�
�Xb�uj/�����c�~�D��S���r*z���n�O��7t�C$��X_ ��X�>�����"�JiF L-�7|��a�q"����5J�ԧ��������@�[�v��},� �jXPj�p�������x�7Ŋn|$+����77��	�?�����������'sy�f߀~���g|�7�&�~�G���;��g�O��"E~ɹ�賰fB��ęKd.E�Iq��r���`VP/q��,�o��,48{K6��]�x��d��ev@�F���~���W��q�d�{��o/�������������jT��:[Mf�b�]9[œ���@1۠�<Νw0Owj(�5��֚|(�Y�SgHp�RQ�r���/�#R����2�8��n������y��n�[X5���n �
�(,2L�g ��W�2���v˷G��3�+ �X)�%*�C�3�#։{o�̧L���Tr�a��!zv��ک+��zܣu��Tf.]7�52 <�]����5�8,^�!mIaFe�{�\�nW��}X�^ѩ�s���+"�"1	�&Xz��	��k�Bcfi����*3���&3W�[Axx�tC�.�����iE�2a�l�YƸFY	[��}'�>�DP�"9�2*8�	��ɝx6F�
��M�-a���aWxETXq��r u,4��Z��4R�g��B��@�Pɮ�M�cr��l�>/�m� 4���?�r%�T�bvEp��gOL�����Ŝ�mꑩ{��+<��+��v���xfjE�EUU؍1����K3��SS7��f��=㱩@4���i�kW�.g���K�����A	�j��H�Rk�h˧���6�X⑜E5��7@�YZ�e.n;e��a2��<���"$Jd_��x�jET�S�&�%�2����b�b�=�r�F�P��FQ)�4Z�1���#��'�x�g��g���#9K@KQ ͻA�����p��W�#9{F�x��;R�x�iE,gb٥S�����6�����O3]���F9�������l/%�2���#,��HΞ����9��$��-�ޞ�48��r�F�S_��3��}ztg���m��[��%�\����}S=�g�B��R ��l t9�B�*��\���ia<WtE@�5$���V��儻�p���썰B΂�]([��P�Z�p#�"PC��S�m	k����|�!��޵FO~<atFx$+��3N=b�%���7c�5��J�Y�kQ�s?T��$zFʷxŶϮf���x�.�7g�h ����C9��lW���gW�W�Y�tl�jvEtT�T#]١����d��h���j�Fx�;T�+����ұ���_*��_��0b^5�Hح�Q������\��j���-�ZJ��L�~A4��B��F���	��^��+��uL�*e�ϫ��kG�򁞾"�-%[��c��m���[|a�+K�c^=�@�W��;�}"�;*�e�z��� ��!B'�r�A��C� �Cl�y��{�����={�E�[�PeM<�.z����I�{���n��nxo���u�M45$  �U�����@x'��B(#]ų���;��Z�W�]8>OP整��M�Ax��� �*�;R9 �$��㬺s���9�oe����@��#,7�H�7��d��Q�("Ӽ���p|$!4�=)��ķE����
�Cͩ7{�=�TK<�����jY>���
�ԚrC�Qw��gs�hq���F~hw_����k&��ވ͓W�AU ��Ҳ�����8��y=f	!5�O�d�	;(��{36�j��|�����V�Y|�i��!��bs�E�!�b§َ6p z����; cY�A,���ޔ��M���d�q�:��mi�Ƈ�i�XI�:�o/_~�1�=H�\��x�q�B�-癔a���_S�.�	��?ޛsك�^bGMV��]5bA�R�N]�l�����D���Am�g��-���	ަnl�����{�.��������q�[!K&����{�&Of<#,�ʼ7k�w��K���rJ������      o   a   x�3���L��2�tJ��2��--I�2��K�(�2���O���2�JM�,.I-�2
%%U�{��-���ĲT.K(l���2�А�=�$�+F��� � *      b   �   x�uα�@�z�+��������BcAi��u@M�z�b$���cJ�Է��@x�,�ɵ�&�BU�.h��%+���[�s�����n"# *9�%����v��}�Ⱛ����\�'�&���� <�      q      x������ � �      r      x������ � �      s   &   x�3�4�4@�\�X��b��P!(jd��4F��� ĭ
=      t      x�3�444����� 
�      u      x������ � �      v      x�3��,�@.CNs8��N����� ���      w   X   x�M̱�0��=EH�'<P3��(�O'0��c�	9��Q}��#��E�1^r}D�*�tOMP��?F�VĔ?��X�v43�|(.      [   q  x�]��n1Eך�����^�����ʆ��Ĉ�$#��*�u�.%�����ﻭ��=~w���!0�3�q���=�_��۳�?��`(�zl�����N�R��r�Vb���س�'V�TYD#�yV0(CoV��i{;��Uo��v�����x��Xf����ya?�{خ�Q�+q�"��y\i^�p��b�� 5
 $ɢ�\CDK�J�]��cd��C����������7�r:]�7;6s|I�� � ,'v���v�(BwZ��A����>�Α�y�R�4r)�Ȇ���`I`FP�Z������(}�����0�}^����N��~ٶ�c���33ʁp!�G�iz<N��ˀ�$      y   M   x�3�,I��L*�LIO�/J-�/-JN-�/I)��M,(��.��Z�-�L�4BC04600�4®�8�,5��+F��� �*&�      V   �   x�u���0Dg�_@>'d�`i%�2�4(M�WH�:�哟O'$4|ʫ?{m��٦@	@@��?���q�_FZ?`�Ӕ�*"��DJci�����A��&�8K���Tݯ���i���9o����qd�/�i+�      c   m   x�3��M�IM�2��H�KI/��2��ML���KUp�L8��KsJ�2�rR�L9��2R��\3�����6sN��ĲJ���Ă�<.ΐ����<ߒ3(���+F��� *Q#h      d   l  x�]Uˎ�6<�?&`���<�`�1Fv� �ñ˒��<�>Eђ={�AMvWWW5Y���@��<���|�"b�$�Z�Ry�V��Ϗ���C�bK��	&8��9��]�lc]�Ŀ��ޥc��A�t�!�i޷EU!�傐FI�Y�}���뛊$)��V�Ta[���_�H��B
a�)o��`�l�]��/�6�1"�94�~��[��Jj�FYM�r�6��2%fD4��)K����@L:A�Fi%�4c�6��N��l=���:&�4^[�Lw����^F����Tò�£���雷�'�1��WtA�zp�=k�m�rw��G�tg�DyG�طM�Pԗ"�1s��qm�� +��91ze�]�3N(��r��!�Ƙ���M��Pۉ�t�c�,�[U�1W3YJ��º�D���V���79mN�1�f>���Z�Sq:a�SA�֩�$����	+��������z�=��b��(T0r�s�������GO�	^��y��)�p��Q�}��`ʤ�WZjP��(�L�x��I������V��	�>3���PU��<#�9���C�N}o��l=� �,��3:H�����P�۲�_�J	ac=�ɵ3�F0��9M�;T�������?�}����҆i��VD�kh�@�C�K�go�T6��W��/�G��/!�����k��c�did �&�*��z+����0����Ew�t�YF
L���hO�u��5^�����:�	ˀ0��gٕMM!g��Ç_�1����N��P�|�����oXD��b���孨w�eh�#�B]C���+��ZZxk��/K.S�y����=J2����h�c�u�a�@�����+����CI`d!=��9�ʢ��oRvsH�L���@���P�H4Ѫh�ݩ��W,a�'�h�%��U��H���)�F~�p3�Q�B	�[��{=�U|�~�o�m�m�]V�� ���`$ǃ>3��@��
��1C�gu*g,o����w��*�c���6�y#Ӑ�� ���U�/�6V�{d|��5�U3'�U�݃i�"��>���#���	����*A��%,-i]�hi@KY�Z���K�#����S��?������     