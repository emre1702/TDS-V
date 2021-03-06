PGDMP     0    '                w           TDSNew    11.2    11.2 �    x           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                       false            y           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                       false            z           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                       false            {           1262    17022    TDSNew    DATABASE     �   CREATE DATABASE "TDSNew" WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'German_Germany.1252' LC_CTYPE = 'German_Germany.1252';
    DROP DATABASE "TDSNew";
             tdsv    false            �            1259    17024 	   log_types    TABLE     i   CREATE TABLE public.log_types (
    "ID" smallint NOT NULL,
    "Name" character varying(50) NOT NULL
);
    DROP TABLE public.log_types;
       public         tdsv    false            �            1259    17027 	   ID_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."ID_ID_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 "   DROP SEQUENCE public."ID_ID_seq";
       public       tdsv    false    196            |           0    0 	   ID_ID_seq    SEQUENCE OWNED BY     B   ALTER SEQUENCE public."ID_ID_seq" OWNED BY public.log_types."ID";
            public       tdsv    false    197            �            1259    17029    server_settings    TABLE       CREATE TABLE public.server_settings (
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
       public         tdsv    false            �            1259    17036    ServerSettings_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."ServerSettings_ID_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 .   DROP SEQUENCE public."ServerSettings_ID_seq";
       public       tdsv    false    198            }           0    0    ServerSettings_ID_seq    SEQUENCE OWNED BY     T   ALTER SEQUENCE public."ServerSettings_ID_seq" OWNED BY public.server_settings."ID";
            public       tdsv    false    199            �            1259    17038    admin_level_names    TABLE     �   CREATE TABLE public.admin_level_names (
    "Level" smallint NOT NULL,
    "Language" smallint NOT NULL,
    "Name" character varying(50) NOT NULL
);
 %   DROP TABLE public.admin_level_names;
       public         tdsv    false            �            1259    17041    admin_levels    TABLE     �   CREATE TABLE public.admin_levels (
    "Level" smallint NOT NULL,
    "ColorR" smallint NOT NULL,
    "ColorG" smallint NOT NULL,
    "ColorB" smallint NOT NULL
);
     DROP TABLE public.admin_levels;
       public         tdsv    false            �            1259    17044    command_alias    TABLE     t   CREATE TABLE public.command_alias (
    "Alias" character varying(100) NOT NULL,
    "Command" smallint NOT NULL
);
 !   DROP TABLE public.command_alias;
       public         tdsv    false            �            1259    17047    command_infos    TABLE     �   CREATE TABLE public.command_infos (
    "ID" smallint NOT NULL,
    "Language" smallint NOT NULL,
    "Info" character varying(500) NOT NULL
);
 !   DROP TABLE public.command_infos;
       public         tdsv    false            �            1259    17050    commands    TABLE     �   CREATE TABLE public.commands (
    "ID" smallint NOT NULL,
    "Command" character varying(50) NOT NULL,
    "NeededAdminLevel" smallint,
    "NeededDonation" smallint,
    "VipCanUse" boolean,
    "LobbyOwnerCanUse" boolean
);
    DROP TABLE public.commands;
       public         tdsv    false            �            1259    17053    commands_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."commands_ID_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public."commands_ID_seq";
       public       tdsv    false    204            ~           0    0    commands_ID_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public."commands_ID_seq" OWNED BY public.commands."ID";
            public       tdsv    false    205            �            1259    17055    freeroam_default_vehicle    TABLE     �   CREATE TABLE public.freeroam_default_vehicle (
    "VehicleTypeId" smallint NOT NULL,
    "VehicleHash" bigint NOT NULL,
    "Note" character varying
);
 ,   DROP TABLE public.freeroam_default_vehicle;
       public         tdsv    false            �            1259    17061 *   freeroam_default_vehicle_VehicleTypeId_seq    SEQUENCE     �   CREATE SEQUENCE public."freeroam_default_vehicle_VehicleTypeId_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 C   DROP SEQUENCE public."freeroam_default_vehicle_VehicleTypeId_seq";
       public       tdsv    false    206                       0    0 *   freeroam_default_vehicle_VehicleTypeId_seq    SEQUENCE OWNED BY     }   ALTER SEQUENCE public."freeroam_default_vehicle_VehicleTypeId_seq" OWNED BY public.freeroam_default_vehicle."VehicleTypeId";
            public       tdsv    false    207            �            1259    17063    freeroam_vehicle_type    TABLE     q   CREATE TABLE public.freeroam_vehicle_type (
    "Id" smallint NOT NULL,
    "Type" character varying NOT NULL
);
 )   DROP TABLE public.freeroam_vehicle_type;
       public         tdsv    false            �            1259    17069    freeroam_vehicle_type_Id_seq    SEQUENCE     �   CREATE SEQUENCE public."freeroam_vehicle_type_Id_seq"
    AS smallint
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 5   DROP SEQUENCE public."freeroam_vehicle_type_Id_seq";
       public       tdsv    false    208            �           0    0    freeroam_vehicle_type_Id_seq    SEQUENCE OWNED BY     a   ALTER SEQUENCE public."freeroam_vehicle_type_Id_seq" OWNED BY public.freeroam_vehicle_type."Id";
            public       tdsv    false    209            �            1259    17071    gangs    TABLE     �   CREATE TABLE public.gangs (
    "ID" integer NOT NULL,
    "TeamID" integer NOT NULL,
    "Short" character varying(5) NOT NULL
);
    DROP TABLE public.gangs;
       public         tdsv    false            �            1259    17074    gangs_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."gangs_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public."gangs_ID_seq";
       public       tdsv    false    210            �           0    0    gangs_ID_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public."gangs_ID_seq" OWNED BY public.gangs."ID";
            public       tdsv    false    211            �            1259    17076    killingspree_rewards    TABLE     �   CREATE TABLE public.killingspree_rewards (
    "KillsAmount" smallint NOT NULL,
    "HealthOrArmor" smallint,
    "OnlyHealth" smallint,
    "OnlyArmor" smallint
);
 (   DROP TABLE public.killingspree_rewards;
       public         tdsv    false            �            1259    17079 	   languages    TABLE     m   CREATE TABLE public.languages (
    "ID" smallint NOT NULL,
    "Language" character varying(50) NOT NULL
);
    DROP TABLE public.languages;
       public         tdsv    false            �            1259    17082    lobbies    TABLE     M  CREATE TABLE public.lobbies (
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
       public         tdsv    false            �            1259    17096    lobbies_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."lobbies_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public."lobbies_ID_seq";
       public       tdsv    false    214            �           0    0    lobbies_ID_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public."lobbies_ID_seq" OWNED BY public.lobbies."Id";
            public       tdsv    false    215            �            1259    17098 
   lobby_maps    TABLE     a   CREATE TABLE public.lobby_maps (
    "LobbyID" integer NOT NULL,
    "MapID" integer NOT NULL
);
    DROP TABLE public.lobby_maps;
       public         tdsv    false            �            1259    17101    lobby_rewards    TABLE     �   CREATE TABLE public.lobby_rewards (
    "LobbyID" integer NOT NULL,
    "MoneyPerKill" double precision NOT NULL,
    "MoneyPerAssist" double precision NOT NULL,
    "MoneyPerDamage" double precision NOT NULL
);
 !   DROP TABLE public.lobby_rewards;
       public         tdsv    false            �            1259    17104    lobby_round_settings    TABLE     �  CREATE TABLE public.lobby_round_settings (
    "LobbyID" integer NOT NULL,
    "RoundTime" integer DEFAULT 240 NOT NULL,
    "CountdownTime" integer DEFAULT 5 NOT NULL,
    "BombDetonateTimeMs" integer DEFAULT 45000 NOT NULL,
    "BombDefuseTimeMs" integer DEFAULT 8000 NOT NULL,
    "BombPlantTimeMs" integer DEFAULT 3000 NOT NULL,
    "MixTeamsAfterRound" boolean DEFAULT false NOT NULL
);
 (   DROP TABLE public.lobby_round_settings;
       public         tdsv    false            �            1259    17113    lobby_types    TABLE     l   CREATE TABLE public.lobby_types (
    "ID" smallint NOT NULL,
    "Name" character varying(100) NOT NULL
);
    DROP TABLE public.lobby_types;
       public         tdsv    false            �            1259    17116    lobby_weapons    TABLE     �   CREATE TABLE public.lobby_weapons (
    "Hash" bigint NOT NULL,
    "Lobby" integer NOT NULL,
    "Ammo" integer NOT NULL,
    "Damage" smallint,
    "HeadMultiplicator" real
);
 !   DROP TABLE public.lobby_weapons;
       public         tdsv    false            �            1259    17119 
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
       public         tdsv    false            �            1259    17126    log_admins_ID_seq    SEQUENCE     |   CREATE SEQUENCE public."log_admins_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public."log_admins_ID_seq";
       public       tdsv    false    221            �           0    0    log_admins_ID_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public."log_admins_ID_seq" OWNED BY public.log_admins."ID";
            public       tdsv    false    222            �            1259    17128 	   log_chats    TABLE     .  CREATE TABLE public.log_chats (
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
       public         tdsv    false            �            1259    17135    log_chats_ID_seq    SEQUENCE     {   CREATE SEQUENCE public."log_chats_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public."log_chats_ID_seq";
       public       tdsv    false    223            �           0    0    log_chats_ID_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public."log_chats_ID_seq" OWNED BY public.log_chats."ID";
            public       tdsv    false    224            �            1259    17137 
   log_errors    TABLE     �   CREATE TABLE public.log_errors (
    "ID" bigint NOT NULL,
    "Source" integer,
    "Info" text NOT NULL,
    "StackTrace" text,
    "Timestamp" timestamp without time zone DEFAULT now() NOT NULL
);
    DROP TABLE public.log_errors;
       public         tdsv    false            �            1259    17144    log_errors_ID_seq    SEQUENCE     |   CREATE SEQUENCE public."log_errors_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 *   DROP SEQUENCE public."log_errors_ID_seq";
       public       tdsv    false    225            �           0    0    log_errors_ID_seq    SEQUENCE OWNED BY     K   ALTER SEQUENCE public."log_errors_ID_seq" OWNED BY public.log_errors."ID";
            public       tdsv    false    226            �            1259    17146 	   log_rests    TABLE       CREATE TABLE public.log_rests (
    "ID" bigint NOT NULL,
    "Type" smallint NOT NULL,
    "Source" integer NOT NULL,
    "Serial" character varying(200),
    "IP" inet,
    "Lobby" integer,
    "Timestamp" timestamp without time zone DEFAULT now() NOT NULL
);
    DROP TABLE public.log_rests;
       public         tdsv    false            �            1259    17153    log_rests_ID_seq    SEQUENCE     {   CREATE SEQUENCE public."log_rests_ID_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE public."log_rests_ID_seq";
       public       tdsv    false    227            �           0    0    log_rests_ID_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE public."log_rests_ID_seq" OWNED BY public.log_rests."ID";
            public       tdsv    false    228            �            1259    17155    maps    TABLE     �   CREATE TABLE public.maps (
    "Id" integer NOT NULL,
    "Name" text NOT NULL,
    "CreatorId" integer,
    "CreateTimestamp" timestamp without time zone DEFAULT now() NOT NULL
);
    DROP TABLE public.maps;
       public         tdsv    false            �            1259    17162    maps_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."maps_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE public."maps_ID_seq";
       public       tdsv    false    229            �           0    0    maps_ID_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE public."maps_ID_seq" OWNED BY public.maps."Id";
            public       tdsv    false    230            �            1259    17164    offlinemessages    TABLE       CREATE TABLE public.offlinemessages (
    "ID" integer NOT NULL,
    "TargetID" integer NOT NULL,
    "SourceID" integer NOT NULL,
    "Message" text NOT NULL,
    "Seen" boolean DEFAULT false NOT NULL,
    "Timestamp" timestamp without time zone DEFAULT now() NOT NULL
);
 #   DROP TABLE public.offlinemessages;
       public         tdsv    false            �            1259    17172    offlinemessages_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."offlinemessages_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 /   DROP SEQUENCE public."offlinemessages_ID_seq";
       public       tdsv    false    231            �           0    0    offlinemessages_ID_seq    SEQUENCE OWNED BY     U   ALTER SEQUENCE public."offlinemessages_ID_seq" OWNED BY public.offlinemessages."ID";
            public       tdsv    false    232            �            1259    17174    player_bans    TABLE       CREATE TABLE public.player_bans (
    "PlayerId" integer NOT NULL,
    "LobbyId" integer DEFAULT 0 NOT NULL,
    "AdminId" integer,
    "Reason" text NOT NULL,
    "StartTimestamp" timestamp with time zone DEFAULT now() NOT NULL,
    "EndTimestamp" timestamp with time zone
);
    DROP TABLE public.player_bans;
       public         tdsv    false            �            1259    17182    player_lobby_stats    TABLE     �  CREATE TABLE public.player_lobby_stats (
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
       public         tdsv    false            �            1259    17193    player_map_favourites    TABLE     m   CREATE TABLE public.player_map_favourites (
    "PlayerID" integer NOT NULL,
    "MapID" integer NOT NULL
);
 )   DROP TABLE public.player_map_favourites;
       public         tdsv    false            �            1259    17196    player_map_ratings    TABLE     �   CREATE TABLE public.player_map_ratings (
    "PlayerID" integer NOT NULL,
    "MapID" integer NOT NULL,
    "Rating" smallint NOT NULL
);
 &   DROP TABLE public.player_map_ratings;
       public         tdsv    false            �            1259    17199    player_settings    TABLE       CREATE TABLE public.player_settings (
    "PlayerID" integer NOT NULL,
    "Language" smallint NOT NULL,
    "Hitsound" boolean NOT NULL,
    "Bloodscreen" boolean NOT NULL,
    "FloatingDamageInfo" boolean NOT NULL,
    "AllowDataTransfer" boolean NOT NULL
);
 #   DROP TABLE public.player_settings;
       public         tdsv    false            �            1259    17202    player_stats    TABLE     -  CREATE TABLE public.player_stats (
    "PlayerID" integer NOT NULL,
    "Money" integer DEFAULT 0 NOT NULL,
    "PlayTime" integer DEFAULT 0 NOT NULL,
    "MuteTime" integer,
    "LoggedIn" boolean DEFAULT false NOT NULL,
    "LastLoginTimestamp" timestamp without time zone DEFAULT now() NOT NULL
);
     DROP TABLE public.player_stats;
       public         tdsv    false            �            1259    17209    players    TABLE     �  CREATE TABLE public.players (
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
       public         tdsv    false            �            1259    17219    players_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."players_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 '   DROP SEQUENCE public."players_ID_seq";
       public       tdsv    false    239            �           0    0    players_ID_seq    SEQUENCE OWNED BY     E   ALTER SEQUENCE public."players_ID_seq" OWNED BY public.players."ID";
            public       tdsv    false    240            �            1259    17221    teams    TABLE     F  CREATE TABLE public.teams (
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
       public         tdsv    false            �            1259    17224    teams_ID_seq    SEQUENCE     �   CREATE SEQUENCE public."teams_ID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public."teams_ID_seq";
       public       tdsv    false    241            �           0    0    teams_ID_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public."teams_ID_seq" OWNED BY public.teams."ID";
            public       tdsv    false    242            �            1259    17226    weapon_types    TABLE     m   CREATE TABLE public.weapon_types (
    "ID" smallint NOT NULL,
    "Name" character varying(100) NOT NULL
);
     DROP TABLE public.weapon_types;
       public         tdsv    false            �            1259    17229    weapons    TABLE     �   CREATE TABLE public.weapons (
    "Hash" bigint NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Type" smallint NOT NULL,
    "DefaultDamage" smallint NOT NULL,
    "DefaultHeadMultiplicator" real DEFAULT 1 NOT NULL
);
    DROP TABLE public.weapons;
       public         tdsv    false            +           2604    17233    commands ID    DEFAULT     n   ALTER TABLE ONLY public.commands ALTER COLUMN "ID" SET DEFAULT nextval('public."commands_ID_seq"'::regclass);
 <   ALTER TABLE public.commands ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    205    204            ,           2604    17234 &   freeroam_default_vehicle VehicleTypeId    DEFAULT     �   ALTER TABLE ONLY public.freeroam_default_vehicle ALTER COLUMN "VehicleTypeId" SET DEFAULT nextval('public."freeroam_default_vehicle_VehicleTypeId_seq"'::regclass);
 W   ALTER TABLE public.freeroam_default_vehicle ALTER COLUMN "VehicleTypeId" DROP DEFAULT;
       public       tdsv    false    207    206            -           2604    17235    freeroam_vehicle_type Id    DEFAULT     �   ALTER TABLE ONLY public.freeroam_vehicle_type ALTER COLUMN "Id" SET DEFAULT nextval('public."freeroam_vehicle_type_Id_seq"'::regclass);
 I   ALTER TABLE public.freeroam_vehicle_type ALTER COLUMN "Id" DROP DEFAULT;
       public       tdsv    false    209    208            .           2604    17236    gangs ID    DEFAULT     h   ALTER TABLE ONLY public.gangs ALTER COLUMN "ID" SET DEFAULT nextval('public."gangs_ID_seq"'::regclass);
 9   ALTER TABLE public.gangs ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    211    210            :           2604    17237 
   lobbies Id    DEFAULT     l   ALTER TABLE ONLY public.lobbies ALTER COLUMN "Id" SET DEFAULT nextval('public."lobbies_ID_seq"'::regclass);
 ;   ALTER TABLE public.lobbies ALTER COLUMN "Id" DROP DEFAULT;
       public       tdsv    false    215    214            B           2604    17238    log_admins ID    DEFAULT     r   ALTER TABLE ONLY public.log_admins ALTER COLUMN "ID" SET DEFAULT nextval('public."log_admins_ID_seq"'::regclass);
 >   ALTER TABLE public.log_admins ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    222    221            D           2604    17239    log_chats ID    DEFAULT     p   ALTER TABLE ONLY public.log_chats ALTER COLUMN "ID" SET DEFAULT nextval('public."log_chats_ID_seq"'::regclass);
 =   ALTER TABLE public.log_chats ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    224    223            F           2604    17240    log_errors ID    DEFAULT     r   ALTER TABLE ONLY public.log_errors ALTER COLUMN "ID" SET DEFAULT nextval('public."log_errors_ID_seq"'::regclass);
 >   ALTER TABLE public.log_errors ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    226    225            H           2604    17241    log_rests ID    DEFAULT     p   ALTER TABLE ONLY public.log_rests ALTER COLUMN "ID" SET DEFAULT nextval('public."log_rests_ID_seq"'::regclass);
 =   ALTER TABLE public.log_rests ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    228    227            (           2604    17242    log_types ID    DEFAULT     i   ALTER TABLE ONLY public.log_types ALTER COLUMN "ID" SET DEFAULT nextval('public."ID_ID_seq"'::regclass);
 =   ALTER TABLE public.log_types ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    197    196            J           2604    17243    maps Id    DEFAULT     f   ALTER TABLE ONLY public.maps ALTER COLUMN "Id" SET DEFAULT nextval('public."maps_ID_seq"'::regclass);
 8   ALTER TABLE public.maps ALTER COLUMN "Id" DROP DEFAULT;
       public       tdsv    false    230    229            M           2604    17244    offlinemessages ID    DEFAULT     |   ALTER TABLE ONLY public.offlinemessages ALTER COLUMN "ID" SET DEFAULT nextval('public."offlinemessages_ID_seq"'::regclass);
 C   ALTER TABLE public.offlinemessages ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    232    231            `           2604    17245 
   players ID    DEFAULT     l   ALTER TABLE ONLY public.players ALTER COLUMN "ID" SET DEFAULT nextval('public."players_ID_seq"'::regclass);
 ;   ALTER TABLE public.players ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    240    239            *           2604    17246    server_settings ID    DEFAULT     {   ALTER TABLE ONLY public.server_settings ALTER COLUMN "ID" SET DEFAULT nextval('public."ServerSettings_ID_seq"'::regclass);
 C   ALTER TABLE public.server_settings ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    199    198            a           2604    17247    teams ID    DEFAULT     h   ALTER TABLE ONLY public.teams ALTER COLUMN "ID" SET DEFAULT nextval('public."teams_ID_seq"'::regclass);
 9   ALTER TABLE public.teams ALTER COLUMN "ID" DROP DEFAULT;
       public       tdsv    false    242    241            I          0    17038    admin_level_names 
   TABLE DATA               H   COPY public.admin_level_names ("Level", "Language", "Name") FROM stdin;
    public       tdsv    false    200   8      J          0    17041    admin_levels 
   TABLE DATA               M   COPY public.admin_levels ("Level", "ColorR", "ColorG", "ColorB") FROM stdin;
    public       tdsv    false    201   �      K          0    17044    command_alias 
   TABLE DATA               ;   COPY public.command_alias ("Alias", "Command") FROM stdin;
    public       tdsv    false    202   �      L          0    17047    command_infos 
   TABLE DATA               A   COPY public.command_infos ("ID", "Language", "Info") FROM stdin;
    public       tdsv    false    203   Z      M          0    17050    commands 
   TABLE DATA               z   COPY public.commands ("ID", "Command", "NeededAdminLevel", "NeededDonation", "VipCanUse", "LobbyOwnerCanUse") FROM stdin;
    public       tdsv    false    204   �      O          0    17055    freeroam_default_vehicle 
   TABLE DATA               Z   COPY public.freeroam_default_vehicle ("VehicleTypeId", "VehicleHash", "Note") FROM stdin;
    public       tdsv    false    206   �	      Q          0    17063    freeroam_vehicle_type 
   TABLE DATA               =   COPY public.freeroam_vehicle_type ("Id", "Type") FROM stdin;
    public       tdsv    false    208   
      S          0    17071    gangs 
   TABLE DATA               8   COPY public.gangs ("ID", "TeamID", "Short") FROM stdin;
    public       tdsv    false    210   M
      U          0    17076    killingspree_rewards 
   TABLE DATA               i   COPY public.killingspree_rewards ("KillsAmount", "HealthOrArmor", "OnlyHealth", "OnlyArmor") FROM stdin;
    public       tdsv    false    212   p
      V          0    17079 	   languages 
   TABLE DATA               5   COPY public.languages ("ID", "Language") FROM stdin;
    public       tdsv    false    213   �
      W          0    17082    lobbies 
   TABLE DATA               @  COPY public.lobbies ("Id", "Owner", "Type", "Name", "Password", "StartHealth", "StartArmor", "AmountLifes", "DefaultSpawnX", "DefaultSpawnY", "DefaultSpawnZ", "AroundSpawnPoint", "DefaultSpawnRotation", "IsTemporary", "IsOfficial", "SpawnAgainAfterDeathMs", "CreateTimestamp", "DieAfterOutsideMapLimitTime") FROM stdin;
    public       tdsv    false    214   �
      Y          0    17098 
   lobby_maps 
   TABLE DATA               8   COPY public.lobby_maps ("LobbyID", "MapID") FROM stdin;
    public       tdsv    false    216   ]      Z          0    17101    lobby_rewards 
   TABLE DATA               f   COPY public.lobby_rewards ("LobbyID", "MoneyPerKill", "MoneyPerAssist", "MoneyPerDamage") FROM stdin;
    public       tdsv    false    217         [          0    17104    lobby_round_settings 
   TABLE DATA               �   COPY public.lobby_round_settings ("LobbyID", "RoundTime", "CountdownTime", "BombDetonateTimeMs", "BombDefuseTimeMs", "BombPlantTimeMs", "MixTeamsAfterRound") FROM stdin;
    public       tdsv    false    218   �      \          0    17113    lobby_types 
   TABLE DATA               3   COPY public.lobby_types ("ID", "Name") FROM stdin;
    public       tdsv    false    219   �      ]          0    17116    lobby_weapons 
   TABLE DATA               _   COPY public.lobby_weapons ("Hash", "Lobby", "Ammo", "Damage", "HeadMultiplicator") FROM stdin;
    public       tdsv    false    220   0      ^          0    17119 
   log_admins 
   TABLE DATA               |   COPY public.log_admins ("ID", "Type", "Source", "Target", "Lobby", "AsDonator", "AsVIP", "Reason", "Timestamp") FROM stdin;
    public       tdsv    false    221   �      `          0    17128 	   log_chats 
   TABLE DATA               {   COPY public.log_chats ("ID", "Source", "Target", "Message", "Lobby", "IsAdminChat", "IsTeamChat", "Timestamp") FROM stdin;
    public       tdsv    false    223   �      b          0    17137 
   log_errors 
   TABLE DATA               W   COPY public.log_errors ("ID", "Source", "Info", "StackTrace", "Timestamp") FROM stdin;
    public       tdsv    false    225   <      d          0    17146 	   log_rests 
   TABLE DATA               a   COPY public.log_rests ("ID", "Type", "Source", "Serial", "IP", "Lobby", "Timestamp") FROM stdin;
    public       tdsv    false    227   �      E          0    17024 	   log_types 
   TABLE DATA               1   COPY public.log_types ("ID", "Name") FROM stdin;
    public       tdsv    false    196   �$      f          0    17155    maps 
   TABLE DATA               L   COPY public.maps ("Id", "Name", "CreatorId", "CreateTimestamp") FROM stdin;
    public       tdsv    false    229   U%      h          0    17164    offlinemessages 
   TABLE DATA               g   COPY public.offlinemessages ("ID", "TargetID", "SourceID", "Message", "Seen", "Timestamp") FROM stdin;
    public       tdsv    false    231   �%      j          0    17174    player_bans 
   TABLE DATA               s   COPY public.player_bans ("PlayerId", "LobbyId", "AdminId", "Reason", "StartTimestamp", "EndTimestamp") FROM stdin;
    public       tdsv    false    233   &      k          0    17182    player_lobby_stats 
   TABLE DATA               �   COPY public.player_lobby_stats ("PlayerID", "LobbyID", "Kills", "Assists", "Deaths", "Damage", "TotalKills", "TotalAssists", "TotalDeaths", "TotalDamage") FROM stdin;
    public       tdsv    false    234   9&      l          0    17193    player_map_favourites 
   TABLE DATA               D   COPY public.player_map_favourites ("PlayerID", "MapID") FROM stdin;
    public       tdsv    false    235   j&      m          0    17196    player_map_ratings 
   TABLE DATA               K   COPY public.player_map_ratings ("PlayerID", "MapID", "Rating") FROM stdin;
    public       tdsv    false    236   �&      n          0    17199    player_settings 
   TABLE DATA               �   COPY public.player_settings ("PlayerID", "Language", "Hitsound", "Bloodscreen", "FloatingDamageInfo", "AllowDataTransfer") FROM stdin;
    public       tdsv    false    237   �&      o          0    17202    player_stats 
   TABLE DATA               u   COPY public.player_stats ("PlayerID", "Money", "PlayTime", "MuteTime", "LoggedIn", "LastLoginTimestamp") FROM stdin;
    public       tdsv    false    238   �&      p          0    17209    players 
   TABLE DATA               �   COPY public.players ("ID", "SCName", "Name", "Password", "Email", "AdminLvl", "IsVIP", "Donation", "GangId", "RegisterTimestamp") FROM stdin;
    public       tdsv    false    239   ?'      G          0    17029    server_settings 
   TABLE DATA               s  COPY public.server_settings ("ID", "GamemodeName", "MapsPath", "NewMapsPath", "ErrorToPlayerOnNonExistentCommand", "ToChatOnNonExistentCommand", "DistanceToSpotToPlant", "DistanceToSpotToDefuse", "SavePlayerDataCooldownMinutes", "SaveLogsCooldownMinutes", "SaveSeasonsCooldownMinutes", "TeamOrderCooldownMs", "ArenaNewMapProbabilityPercent", "SavedMapsPath") FROM stdin;
    public       tdsv    false    198   �(      r          0    17221    teams 
   TABLE DATA               v   COPY public.teams ("ID", "Index", "Name", "Lobby", "ColorR", "ColorG", "ColorB", "BlipColor", "SkinHash") FROM stdin;
    public       tdsv    false    241   )      t          0    17226    weapon_types 
   TABLE DATA               4   COPY public.weapon_types ("ID", "Name") FROM stdin;
    public       tdsv    false    243   �)      u          0    17229    weapons 
   TABLE DATA               f   COPY public.weapons ("Hash", "Name", "Type", "DefaultDamage", "DefaultHeadMultiplicator") FROM stdin;
    public       tdsv    false    244    *      �           0    0 	   ID_ID_seq    SEQUENCE SET     :   SELECT pg_catalog.setval('public."ID_ID_seq"', 1, false);
            public       tdsv    false    197            �           0    0    ServerSettings_ID_seq    SEQUENCE SET     F   SELECT pg_catalog.setval('public."ServerSettings_ID_seq"', 1, false);
            public       tdsv    false    199            �           0    0    commands_ID_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public."commands_ID_seq"', 19, true);
            public       tdsv    false    205            �           0    0 *   freeroam_default_vehicle_VehicleTypeId_seq    SEQUENCE SET     [   SELECT pg_catalog.setval('public."freeroam_default_vehicle_VehicleTypeId_seq"', 1, false);
            public       tdsv    false    207            �           0    0    freeroam_vehicle_type_Id_seq    SEQUENCE SET     M   SELECT pg_catalog.setval('public."freeroam_vehicle_type_Id_seq"', 1, false);
            public       tdsv    false    209            �           0    0    gangs_ID_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."gangs_ID_seq"', 1, false);
            public       tdsv    false    211            �           0    0    lobbies_ID_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."lobbies_ID_seq"', 27, true);
            public       tdsv    false    215            �           0    0    log_admins_ID_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."log_admins_ID_seq"', 4, true);
            public       tdsv    false    222            �           0    0    log_chats_ID_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."log_chats_ID_seq"', 19, true);
            public       tdsv    false    224            �           0    0    log_errors_ID_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('public."log_errors_ID_seq"', 6, true);
            public       tdsv    false    226            �           0    0    log_rests_ID_seq    SEQUENCE SET     B   SELECT pg_catalog.setval('public."log_rests_ID_seq"', 298, true);
            public       tdsv    false    228            �           0    0    maps_ID_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."maps_ID_seq"', 114, true);
            public       tdsv    false    230            �           0    0    offlinemessages_ID_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('public."offlinemessages_ID_seq"', 1, false);
            public       tdsv    false    232            �           0    0    players_ID_seq    SEQUENCE SET     >   SELECT pg_catalog.setval('public."players_ID_seq"', 3, true);
            public       tdsv    false    240            �           0    0    teams_ID_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."teams_ID_seq"', 26, true);
            public       tdsv    false    242            d           2606    17249    log_types ID_pkey 
   CONSTRAINT     S   ALTER TABLE ONLY public.log_types
    ADD CONSTRAINT "ID_pkey" PRIMARY KEY ("ID");
 =   ALTER TABLE ONLY public.log_types DROP CONSTRAINT "ID_pkey";
       public         tdsv    false    196            h           2606    17251 &   admin_level_names PK_admin_level_names 
   CONSTRAINT     w   ALTER TABLE ONLY public.admin_level_names
    ADD CONSTRAINT "PK_admin_level_names" PRIMARY KEY ("Level", "Language");
 R   ALTER TABLE ONLY public.admin_level_names DROP CONSTRAINT "PK_admin_level_names";
       public         tdsv    false    200    200            f           2606    17253 #   server_settings ServerSettings_pkey 
   CONSTRAINT     e   ALTER TABLE ONLY public.server_settings
    ADD CONSTRAINT "ServerSettings_pkey" PRIMARY KEY ("ID");
 O   ALTER TABLE ONLY public.server_settings DROP CONSTRAINT "ServerSettings_pkey";
       public         tdsv    false    198            j           2606    17255    admin_levels admin_levels_pkey 
   CONSTRAINT     a   ALTER TABLE ONLY public.admin_levels
    ADD CONSTRAINT admin_levels_pkey PRIMARY KEY ("Level");
 H   ALTER TABLE ONLY public.admin_levels DROP CONSTRAINT admin_levels_pkey;
       public         tdsv    false    201            l           2606    17257     command_alias command_alias_pkey 
   CONSTRAINT     n   ALTER TABLE ONLY public.command_alias
    ADD CONSTRAINT command_alias_pkey PRIMARY KEY ("Alias", "Command");
 J   ALTER TABLE ONLY public.command_alias DROP CONSTRAINT command_alias_pkey;
       public         tdsv    false    202    202            n           2606    17259     command_infos command_infos_pkey 
   CONSTRAINT     l   ALTER TABLE ONLY public.command_infos
    ADD CONSTRAINT command_infos_pkey PRIMARY KEY ("ID", "Language");
 J   ALTER TABLE ONLY public.command_infos DROP CONSTRAINT command_infos_pkey;
       public         tdsv    false    203    203            p           2606    17261    commands commands_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.commands
    ADD CONSTRAINT commands_pkey PRIMARY KEY ("ID");
 @   ALTER TABLE ONLY public.commands DROP CONSTRAINT commands_pkey;
       public         tdsv    false    204            r           2606    17263 6   freeroam_default_vehicle freeroam_default_vehicle_pkey 
   CONSTRAINT     �   ALTER TABLE ONLY public.freeroam_default_vehicle
    ADD CONSTRAINT freeroam_default_vehicle_pkey PRIMARY KEY ("VehicleTypeId");
 `   ALTER TABLE ONLY public.freeroam_default_vehicle DROP CONSTRAINT freeroam_default_vehicle_pkey;
       public         tdsv    false    206            t           2606    17265 0   freeroam_vehicle_type freeroam_vehicle_type_pkey 
   CONSTRAINT     p   ALTER TABLE ONLY public.freeroam_vehicle_type
    ADD CONSTRAINT freeroam_vehicle_type_pkey PRIMARY KEY ("Id");
 Z   ALTER TABLE ONLY public.freeroam_vehicle_type DROP CONSTRAINT freeroam_vehicle_type_pkey;
       public         tdsv    false    208            v           2606    17267    gangs gangs_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.gangs
    ADD CONSTRAINT gangs_pkey PRIMARY KEY ("ID");
 :   ALTER TABLE ONLY public.gangs DROP CONSTRAINT gangs_pkey;
       public         tdsv    false    210            x           2606    17269 .   killingspree_rewards killingspree_rewards_pkey 
   CONSTRAINT     w   ALTER TABLE ONLY public.killingspree_rewards
    ADD CONSTRAINT killingspree_rewards_pkey PRIMARY KEY ("KillsAmount");
 X   ALTER TABLE ONLY public.killingspree_rewards DROP CONSTRAINT killingspree_rewards_pkey;
       public         tdsv    false    212            z           2606    17271    languages languages_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.languages
    ADD CONSTRAINT languages_pkey PRIMARY KEY ("ID");
 B   ALTER TABLE ONLY public.languages DROP CONSTRAINT languages_pkey;
       public         tdsv    false    213            |           2606    17273    lobbies lobbies_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.lobbies
    ADD CONSTRAINT lobbies_pkey PRIMARY KEY ("Id");
 >   ALTER TABLE ONLY public.lobbies DROP CONSTRAINT lobbies_pkey;
       public         tdsv    false    214                       2606    17275    lobby_maps lobby_maps_pkey 
   CONSTRAINT     h   ALTER TABLE ONLY public.lobby_maps
    ADD CONSTRAINT lobby_maps_pkey PRIMARY KEY ("LobbyID", "MapID");
 D   ALTER TABLE ONLY public.lobby_maps DROP CONSTRAINT lobby_maps_pkey;
       public         tdsv    false    216    216            �           2606    17277     lobby_rewards lobby_rewards_pkey 
   CONSTRAINT     e   ALTER TABLE ONLY public.lobby_rewards
    ADD CONSTRAINT lobby_rewards_pkey PRIMARY KEY ("LobbyID");
 J   ALTER TABLE ONLY public.lobby_rewards DROP CONSTRAINT lobby_rewards_pkey;
       public         tdsv    false    217            �           2606    17279 +   lobby_round_settings lobby_round_infos_pkey 
   CONSTRAINT     p   ALTER TABLE ONLY public.lobby_round_settings
    ADD CONSTRAINT lobby_round_infos_pkey PRIMARY KEY ("LobbyID");
 U   ALTER TABLE ONLY public.lobby_round_settings DROP CONSTRAINT lobby_round_infos_pkey;
       public         tdsv    false    218            �           2606    17281    lobby_types lobby_types_pkey 
   CONSTRAINT     \   ALTER TABLE ONLY public.lobby_types
    ADD CONSTRAINT lobby_types_pkey PRIMARY KEY ("ID");
 F   ALTER TABLE ONLY public.lobby_types DROP CONSTRAINT lobby_types_pkey;
       public         tdsv    false    219            �           2606    17283     lobby_weapons lobby_weapons_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public.lobby_weapons
    ADD CONSTRAINT lobby_weapons_pkey PRIMARY KEY ("Hash");
 J   ALTER TABLE ONLY public.lobby_weapons DROP CONSTRAINT lobby_weapons_pkey;
       public         tdsv    false    220            �           2606    17285    log_admins log_admins_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.log_admins
    ADD CONSTRAINT log_admins_pkey PRIMARY KEY ("ID");
 D   ALTER TABLE ONLY public.log_admins DROP CONSTRAINT log_admins_pkey;
       public         tdsv    false    221            �           2606    17287    log_chats log_chats_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.log_chats
    ADD CONSTRAINT log_chats_pkey PRIMARY KEY ("ID");
 B   ALTER TABLE ONLY public.log_chats DROP CONSTRAINT log_chats_pkey;
       public         tdsv    false    223            �           2606    17289    log_errors log_errors_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.log_errors
    ADD CONSTRAINT log_errors_pkey PRIMARY KEY ("ID");
 D   ALTER TABLE ONLY public.log_errors DROP CONSTRAINT log_errors_pkey;
       public         tdsv    false    225            �           2606    17291    log_rests log_rests_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.log_rests
    ADD CONSTRAINT log_rests_pkey PRIMARY KEY ("ID");
 B   ALTER TABLE ONLY public.log_rests DROP CONSTRAINT log_rests_pkey;
       public         tdsv    false    227            �           2606    17293    maps maps_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.maps
    ADD CONSTRAINT maps_pkey PRIMARY KEY ("Id");
 8   ALTER TABLE ONLY public.maps DROP CONSTRAINT maps_pkey;
       public         tdsv    false    229            �           2606    17295 $   offlinemessages offlinemessages_pkey 
   CONSTRAINT     d   ALTER TABLE ONLY public.offlinemessages
    ADD CONSTRAINT offlinemessages_pkey PRIMARY KEY ("ID");
 N   ALTER TABLE ONLY public.offlinemessages DROP CONSTRAINT offlinemessages_pkey;
       public         tdsv    false    231            �           2606    17297    player_bans player_bans_pkey 
   CONSTRAINT     m   ALTER TABLE ONLY public.player_bans
    ADD CONSTRAINT player_bans_pkey PRIMARY KEY ("PlayerId", "LobbyId");
 F   ALTER TABLE ONLY public.player_bans DROP CONSTRAINT player_bans_pkey;
       public         tdsv    false    233    233            �           2606    17299 *   player_lobby_stats player_lobby_stats_pkey 
   CONSTRAINT     {   ALTER TABLE ONLY public.player_lobby_stats
    ADD CONSTRAINT player_lobby_stats_pkey PRIMARY KEY ("PlayerID", "LobbyID");
 T   ALTER TABLE ONLY public.player_lobby_stats DROP CONSTRAINT player_lobby_stats_pkey;
       public         tdsv    false    234    234            �           2606    17301 0   player_map_favourites player_map_favourites_pkey 
   CONSTRAINT        ALTER TABLE ONLY public.player_map_favourites
    ADD CONSTRAINT player_map_favourites_pkey PRIMARY KEY ("PlayerID", "MapID");
 Z   ALTER TABLE ONLY public.player_map_favourites DROP CONSTRAINT player_map_favourites_pkey;
       public         tdsv    false    235    235            �           2606    17303 *   player_map_ratings player_map_ratings_pkey 
   CONSTRAINT     y   ALTER TABLE ONLY public.player_map_ratings
    ADD CONSTRAINT player_map_ratings_pkey PRIMARY KEY ("PlayerID", "MapID");
 T   ALTER TABLE ONLY public.player_map_ratings DROP CONSTRAINT player_map_ratings_pkey;
       public         tdsv    false    236    236            �           2606    17305 $   player_settings player_settings_pkey 
   CONSTRAINT     j   ALTER TABLE ONLY public.player_settings
    ADD CONSTRAINT player_settings_pkey PRIMARY KEY ("PlayerID");
 N   ALTER TABLE ONLY public.player_settings DROP CONSTRAINT player_settings_pkey;
       public         tdsv    false    237            �           2606    17307    player_stats player_stats_pkey 
   CONSTRAINT     d   ALTER TABLE ONLY public.player_stats
    ADD CONSTRAINT player_stats_pkey PRIMARY KEY ("PlayerID");
 H   ALTER TABLE ONLY public.player_stats DROP CONSTRAINT player_stats_pkey;
       public         tdsv    false    238            �           2606    17309    players players_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.players
    ADD CONSTRAINT players_pkey PRIMARY KEY ("ID");
 >   ALTER TABLE ONLY public.players DROP CONSTRAINT players_pkey;
       public         tdsv    false    239            �           2606    17311    teams teams_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.teams
    ADD CONSTRAINT teams_pkey PRIMARY KEY ("ID");
 :   ALTER TABLE ONLY public.teams DROP CONSTRAINT teams_pkey;
       public         tdsv    false    241            �           2606    17313    weapon_types weapon_types_pkey 
   CONSTRAINT     ^   ALTER TABLE ONLY public.weapon_types
    ADD CONSTRAINT weapon_types_pkey PRIMARY KEY ("ID");
 H   ALTER TABLE ONLY public.weapon_types DROP CONSTRAINT weapon_types_pkey;
       public         tdsv    false    243            �           2606    17315    weapons weapons_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public.weapons
    ADD CONSTRAINT weapons_pkey PRIMARY KEY ("Hash");
 >   ALTER TABLE ONLY public.weapons DROP CONSTRAINT weapons_pkey;
       public         tdsv    false    244            �           1259    17316    Index_maps_name    INDEX     C   CREATE INDEX "Index_maps_name" ON public.maps USING hash ("Name");
 %   DROP INDEX public."Index_maps_name";
       public         tdsv    false    229            }           1259    17317    fki_FK_lobby_maps_maps    INDEX     R   CREATE INDEX "fki_FK_lobby_maps_maps" ON public.lobby_maps USING btree ("MapID");
 ,   DROP INDEX public."fki_FK_lobby_maps_maps";
       public         tdsv    false    216            �           2606    17318 2   admin_level_names FK_admin_level_names_admin_level    FK CONSTRAINT     �   ALTER TABLE ONLY public.admin_level_names
    ADD CONSTRAINT "FK_admin_level_names_admin_level" FOREIGN KEY ("Level") REFERENCES public.admin_levels("Level") ON UPDATE CASCADE ON DELETE CASCADE;
 ^   ALTER TABLE ONLY public.admin_level_names DROP CONSTRAINT "FK_admin_level_names_admin_level";
       public       tdsv    false    201    2922    200            �           2606    17323 /   admin_level_names FK_admin_level_names_language    FK CONSTRAINT     �   ALTER TABLE ONLY public.admin_level_names
    ADD CONSTRAINT "FK_admin_level_names_language" FOREIGN KEY ("Language") REFERENCES public.languages("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 [   ALTER TABLE ONLY public.admin_level_names DROP CONSTRAINT "FK_admin_level_names_language";
       public       tdsv    false    2938    200    213            �           2606    17328 !   commands FK_commands_admin_levels    FK CONSTRAINT     �   ALTER TABLE ONLY public.commands
    ADD CONSTRAINT "FK_commands_admin_levels" FOREIGN KEY ("NeededAdminLevel") REFERENCES public.admin_levels("Level");
 M   ALTER TABLE ONLY public.commands DROP CONSTRAINT "FK_commands_admin_levels";
       public       tdsv    false    2922    201    204            �           2606    17333    lobby_maps FK_lobby_maps_maps    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_maps
    ADD CONSTRAINT "FK_lobby_maps_maps" FOREIGN KEY ("MapID") REFERENCES public.maps("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 I   ALTER TABLE ONLY public.lobby_maps DROP CONSTRAINT "FK_lobby_maps_maps";
       public       tdsv    false    2962    216    229            �           2606    17338 (   command_alias command_alias_Command_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.command_alias
    ADD CONSTRAINT "command_alias_Command_fkey" FOREIGN KEY ("Command") REFERENCES public.commands("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 T   ALTER TABLE ONLY public.command_alias DROP CONSTRAINT "command_alias_Command_fkey";
       public       tdsv    false    2928    204    202            �           2606    17343 #   command_infos command_infos_ID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.command_infos
    ADD CONSTRAINT "command_infos_ID_fkey" FOREIGN KEY ("ID") REFERENCES public.commands("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 O   ALTER TABLE ONLY public.command_infos DROP CONSTRAINT "command_infos_ID_fkey";
       public       tdsv    false    203    204    2928            �           2606    17348 )   command_infos command_infos_Language_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.command_infos
    ADD CONSTRAINT "command_infos_Language_fkey" FOREIGN KEY ("Language") REFERENCES public.languages("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 U   ALTER TABLE ONLY public.command_infos DROP CONSTRAINT "command_infos_Language_fkey";
       public       tdsv    false    203    2938    213            �           2606    17353 D   freeroam_default_vehicle freeroam_default_vehicle_VehicleTypeId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.freeroam_default_vehicle
    ADD CONSTRAINT "freeroam_default_vehicle_VehicleTypeId_fkey" FOREIGN KEY ("VehicleTypeId") REFERENCES public.freeroam_vehicle_type("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 p   ALTER TABLE ONLY public.freeroam_default_vehicle DROP CONSTRAINT "freeroam_default_vehicle_VehicleTypeId_fkey";
       public       tdsv    false    206    208    2932            �           2606    17358    gangs gangs_TeamID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.gangs
    ADD CONSTRAINT "gangs_TeamID_fkey" FOREIGN KEY ("TeamID") REFERENCES public.teams("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 C   ALTER TABLE ONLY public.gangs DROP CONSTRAINT "gangs_TeamID_fkey";
       public       tdsv    false    210    241    2980            �           2606    17363    lobbies lobbies_Owner_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobbies
    ADD CONSTRAINT "lobbies_Owner_fkey" FOREIGN KEY ("Owner") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 F   ALTER TABLE ONLY public.lobbies DROP CONSTRAINT "lobbies_Owner_fkey";
       public       tdsv    false    214    239    2978            �           2606    17368    lobbies lobbies_Type_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobbies
    ADD CONSTRAINT "lobbies_Type_fkey" FOREIGN KEY ("Type") REFERENCES public.lobby_types("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 E   ALTER TABLE ONLY public.lobbies DROP CONSTRAINT "lobbies_Type_fkey";
       public       tdsv    false    214    219    2949            �           2606    17373 "   lobby_maps lobby_maps_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_maps
    ADD CONSTRAINT "lobby_maps_LobbyID_fkey" FOREIGN KEY ("LobbyID") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 N   ALTER TABLE ONLY public.lobby_maps DROP CONSTRAINT "lobby_maps_LobbyID_fkey";
       public       tdsv    false    214    216    2940            �           2606    17378 (   lobby_rewards lobby_rewards_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_rewards
    ADD CONSTRAINT "lobby_rewards_LobbyID_fkey" FOREIGN KEY ("LobbyID") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 T   ALTER TABLE ONLY public.lobby_rewards DROP CONSTRAINT "lobby_rewards_LobbyID_fkey";
       public       tdsv    false    214    2940    217            �           2606    17383 3   lobby_round_settings lobby_round_infos_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_round_settings
    ADD CONSTRAINT "lobby_round_infos_LobbyID_fkey" FOREIGN KEY ("LobbyID") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 _   ALTER TABLE ONLY public.lobby_round_settings DROP CONSTRAINT "lobby_round_infos_LobbyID_fkey";
       public       tdsv    false    2940    218    214            �           2606    17388 %   lobby_weapons lobby_weapons_Hash_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_weapons
    ADD CONSTRAINT "lobby_weapons_Hash_fkey" FOREIGN KEY ("Hash") REFERENCES public.weapons("Hash") ON UPDATE CASCADE ON DELETE CASCADE;
 Q   ALTER TABLE ONLY public.lobby_weapons DROP CONSTRAINT "lobby_weapons_Hash_fkey";
       public       tdsv    false    244    220    2984            �           2606    17393 &   lobby_weapons lobby_weapons_Lobby_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.lobby_weapons
    ADD CONSTRAINT "lobby_weapons_Lobby_fkey" FOREIGN KEY ("Lobby") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 R   ALTER TABLE ONLY public.lobby_weapons DROP CONSTRAINT "lobby_weapons_Lobby_fkey";
       public       tdsv    false    220    2940    214            �           2606    17398    maps maps_CreatorID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.maps
    ADD CONSTRAINT "maps_CreatorID_fkey" FOREIGN KEY ("CreatorId") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE SET NULL;
 D   ALTER TABLE ONLY public.maps DROP CONSTRAINT "maps_CreatorID_fkey";
       public       tdsv    false    239    229    2978            �           2606    17403 -   offlinemessages offlinemessages_SourceID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.offlinemessages
    ADD CONSTRAINT "offlinemessages_SourceID_fkey" FOREIGN KEY ("SourceID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 Y   ALTER TABLE ONLY public.offlinemessages DROP CONSTRAINT "offlinemessages_SourceID_fkey";
       public       tdsv    false    2978    239    231            �           2606    17408 -   offlinemessages offlinemessages_TargetID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.offlinemessages
    ADD CONSTRAINT "offlinemessages_TargetID_fkey" FOREIGN KEY ("TargetID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 Y   ALTER TABLE ONLY public.offlinemessages DROP CONSTRAINT "offlinemessages_TargetID_fkey";
       public       tdsv    false    231    2978    239            �           2606    17413 $   player_bans player_bans_AdminID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_bans
    ADD CONSTRAINT "player_bans_AdminID_fkey" FOREIGN KEY ("AdminId") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE SET NULL;
 P   ALTER TABLE ONLY public.player_bans DROP CONSTRAINT "player_bans_AdminID_fkey";
       public       tdsv    false    2978    233    239            �           2606    17418 $   player_bans player_bans_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_bans
    ADD CONSTRAINT "player_bans_LobbyID_fkey" FOREIGN KEY ("LobbyId") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 P   ALTER TABLE ONLY public.player_bans DROP CONSTRAINT "player_bans_LobbyID_fkey";
       public       tdsv    false    2940    214    233            �           2606    17423 %   player_bans player_bans_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_bans
    ADD CONSTRAINT "player_bans_PlayerID_fkey" FOREIGN KEY ("PlayerId") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 Q   ALTER TABLE ONLY public.player_bans DROP CONSTRAINT "player_bans_PlayerID_fkey";
       public       tdsv    false    239    2978    233            �           2606    17428 2   player_lobby_stats player_lobby_stats_LobbyID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_lobby_stats
    ADD CONSTRAINT "player_lobby_stats_LobbyID_fkey" FOREIGN KEY ("LobbyID") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 ^   ALTER TABLE ONLY public.player_lobby_stats DROP CONSTRAINT "player_lobby_stats_LobbyID_fkey";
       public       tdsv    false    234    214    2940            �           2606    17433 3   player_lobby_stats player_lobby_stats_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_lobby_stats
    ADD CONSTRAINT "player_lobby_stats_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 _   ALTER TABLE ONLY public.player_lobby_stats DROP CONSTRAINT "player_lobby_stats_PlayerID_fkey";
       public       tdsv    false    239    234    2978            �           2606    17438 6   player_map_favourites player_map_favourites_MapID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_map_favourites
    ADD CONSTRAINT "player_map_favourites_MapID_fkey" FOREIGN KEY ("MapID") REFERENCES public.maps("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 b   ALTER TABLE ONLY public.player_map_favourites DROP CONSTRAINT "player_map_favourites_MapID_fkey";
       public       tdsv    false    2962    229    235            �           2606    17443 9   player_map_favourites player_map_favourites_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_map_favourites
    ADD CONSTRAINT "player_map_favourites_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 e   ALTER TABLE ONLY public.player_map_favourites DROP CONSTRAINT "player_map_favourites_PlayerID_fkey";
       public       tdsv    false    2978    235    239            �           2606    17448 0   player_map_ratings player_map_ratings_MapID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_map_ratings
    ADD CONSTRAINT "player_map_ratings_MapID_fkey" FOREIGN KEY ("MapID") REFERENCES public.maps("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 \   ALTER TABLE ONLY public.player_map_ratings DROP CONSTRAINT "player_map_ratings_MapID_fkey";
       public       tdsv    false    236    2962    229            �           2606    17453 3   player_map_ratings player_map_ratings_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_map_ratings
    ADD CONSTRAINT "player_map_ratings_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 _   ALTER TABLE ONLY public.player_map_ratings DROP CONSTRAINT "player_map_ratings_PlayerID_fkey";
       public       tdsv    false    239    2978    236            �           2606    17458 -   player_settings player_settings_Language_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_settings
    ADD CONSTRAINT "player_settings_Language_fkey" FOREIGN KEY ("Language") REFERENCES public.languages("ID") ON UPDATE CASCADE ON DELETE RESTRICT;
 Y   ALTER TABLE ONLY public.player_settings DROP CONSTRAINT "player_settings_Language_fkey";
       public       tdsv    false    237    2938    213            �           2606    17463 -   player_settings player_settings_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_settings
    ADD CONSTRAINT "player_settings_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 Y   ALTER TABLE ONLY public.player_settings DROP CONSTRAINT "player_settings_PlayerID_fkey";
       public       tdsv    false    2978    239    237            �           2606    17468 '   player_stats player_stats_PlayerID_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.player_stats
    ADD CONSTRAINT "player_stats_PlayerID_fkey" FOREIGN KEY ("PlayerID") REFERENCES public.players("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 S   ALTER TABLE ONLY public.player_stats DROP CONSTRAINT "player_stats_PlayerID_fkey";
       public       tdsv    false    2978    239    238            �           2606    17473    players players_AdminLvl_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.players
    ADD CONSTRAINT "players_AdminLvl_fkey" FOREIGN KEY ("AdminLvl") REFERENCES public.admin_levels("Level") ON UPDATE CASCADE ON DELETE CASCADE;
 I   ALTER TABLE ONLY public.players DROP CONSTRAINT "players_AdminLvl_fkey";
       public       tdsv    false    201    2922    239            �           2606    17478    players players_GangId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.players
    ADD CONSTRAINT "players_GangId_fkey" FOREIGN KEY ("GangId") REFERENCES public.gangs("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 G   ALTER TABLE ONLY public.players DROP CONSTRAINT "players_GangId_fkey";
       public       tdsv    false    2934    210    239            �           2606    17483    teams teams_Lobby_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.teams
    ADD CONSTRAINT "teams_Lobby_fkey" FOREIGN KEY ("Lobby") REFERENCES public.lobbies("Id") ON UPDATE CASCADE ON DELETE CASCADE;
 B   ALTER TABLE ONLY public.teams DROP CONSTRAINT "teams_Lobby_fkey";
       public       tdsv    false    2940    241    214            �           2606    17488    weapons weapons_Type_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public.weapons
    ADD CONSTRAINT "weapons_Type_fkey" FOREIGN KEY ("Type") REFERENCES public.weapon_types("ID") ON UPDATE CASCADE ON DELETE CASCADE;
 E   ALTER TABLE ONLY public.weapons DROP CONSTRAINT "weapons_Type_fkey";
       public       tdsv    false    243    2982    244            I   W   x�3�4�-N-�2ഄ0�"���E%`�%�(瘒���Y\R�X��D1�	(��J�.�I��2��$ES�"1z\\\ ��%%      J   3   x�5��	  ��w�H�Rp����	A|��,�ѥ,�۳A�ϲ}�Uz      K   h  x�]�Ko�0���S5��1�*B�b�mՋ+a�ؑqP�����^<��X��J��n�Yi���v#nNV�Ix�"��/鱖��\Y4*{#-v�
�ހ��EpM�6����;E�k�y%nhX?�ϯoG{a�([}��Fڴ^�Ù�}i���Ɔ��]�h-�1���>F}WǵˑM����Y6N������@�8W��$[�3G�$^�)�D/YU^Ӫ�)�<�A� K��.EW�䡫��jK�-�:d�r�����
�!�C��q�H�gw	�5��\�s�J�|�cW݋��"?򰆳ǐ�%C�*}A�t�p�!l�nbB/x��U��1QR�`7��d?/ �F���      L   J  x��UMS�0='�B�0��R��0(e�^z��u,�%W�$���N��Ǻ+�N��tz����ۧ��d0̊ʂ�=�ܗ%h�dQ1��ى��'�l��J�5!G�q�x��v�FjV3,�28��-T��j5�C�S����X9k$(�laj6�kO�``	�U�F��L�̔�W�\:D��h�������_#|b��o3��Wc��4y�f���T���i�0�
�9J�68����v��i�?m'oT�UD_I�P�B6�X�P����];�	.dq��'�G؞0�{q���<8FC^�<_퓾%��Bc��J�گ�5ϖ�
 �W�WDy*J���l��A{�x8}a��Q3}u����|
�ʑ5���\��Y�=!���i��Ě�������)�v~-���<�
�� �4/�e�Z� �Z�Gfu]TN�`�o`��s�[\��lp	|��:�	���f�ي�T�|�܋ �V��d�\��@�i�ϑ��W��~{]�mr�\�e)p@�h��F0Vfme4�����\0�Ԯ�H�7<��a�u��c�>�2�P���^� �#>�����8:�.Z�Gl��Lh����"�k㤗�.�uQp����o�O�M�m�2��íd�Ԏfյ��ST&��X�K��n"�@M���$l~�+ے�gJ˺n�m?����:��Cq����2.ex�=FK�+a��o~p�d�d��C�y���</�����s쫋��Ⱥu��$�4���qٿ��Od�e�n��c����H�v�cM���ą3�@!K���/6�F������h#��DdeB�u �o�����	D��      M   �   x�u���@�ϳ��j}�A�"���U6�+��{ͦJ$�2����h���)t��z�-�*(Y�'�`*xbh�z-*��le\�~���WG�L�d�AQ]$��Ôo�Щ��fY�0]��)�����ɾp�anS��jNK��/1?�+	�Z��1���6��:���`v����v�v_zX9>��]7�R�csV      O   n   x��1�0������'vF���X�T*RQ!���C�d)l���Ϸ��!�HHR�8����B+�{WL�}#�%�)
+.��nGVJ0NU��T��گ��>D���	      Q   6   x�3�tN,�2��H��L�/(I-�2��I�K�2�t��N�2�t�O,����� -G      S      x�3�4������� �V      U   %   x�3�46���".SNS�Ѐ�� �1E���qqq 	-	�      V       x�3�tO-�M���t�K��,������� Vqc      W   x   x�3�4 B���<�ԼR�?NC02AR�@�1�N�,�F�������
��V�&VFF�\� NǢԼDd101�f�%P5��2�ļt����Jb�1�cdhelhe 6'F��� ��'N      Y      x�3��5�����  K      Z      x�3�42�44�4�34@f\F�$c���� M�
f      [   "   x�3�421�4�41500� � ��+F��� N��      \   @   x�3��M���M�+�2�t�L�(��OJ��2�t,J�K�2�tO�K��� �8�&��Bb���� ���      ]   \   x�e͹�@C�x��C�ل+P�uH���F/�	�,�e� ��}�E�jU���c�&B\ʠ��wB��l6�H�w���;�H������W#�      ^   L   x�3�4�4��i@�Z\�id`h�k`�kh�`hiedhej�gndild�e�_���������������1W� �d      `   4  x�}�1n�0Eg��@�"Eʇ��1��K�ɾ?J�i��r�E0���>e�p{�>��v%�r����*���$���g��E�B�S�u��iy���q�0�=}�_�r�s���^�43�]g���G�H�^�9��������CQ��!��E����S��$�}�̄��to�c]֮x��i� �ـ�.��u��W�	�N��Ȋbj��_�ϼmuc�R����x�Q�G����<=���t����VR'{�?O��"גg�'}z��#��������&���n�"�Gj��0"(n ?��      b   a  x���sڸ����BO��N��4m'��$�[f�&[���C�9�7�b%������e!LI���b~/����,듏<|�ʋ��	Jld'�NF$~� ͠A��2��g�ә4����gq+�H�=�&�@�&&�$�@Z���C���RWX�}c��/�|Jcb,��di���O�����!�^�t+�_n���1��\&�ϟW6�9^p%��f4��;
y��RD�8~�j]Ҧ�:j�Uj�j���X5j��J�|��~H;�[k��=���Zy ��GⒹP�yG	��7�V��)�Rp���(&�*�B2���$aS��s��=N��K����n�s��cu���r"����ҝ=�q���)]t���
��Q��@��FI��ɮYu�҉�K�-l���"�7��4�k��|��?Wc��-(�k��>.����#�W�p��ʈ{888���ԎR��KN����9�ەj�\:
]_�(MH���%]ɶp��r����󥹐ns�����Mc���1e�sG���8V���3W�:��몟g�2�+�EO�����'<�|"��|�����Ƿ��\��]d�u}wxU��(�ʨ�����j�ҭ��˚�Zg�TC��d �>�,t�WÐF�(��I#�"c,���Y'5�O��b	�s`�s�#�ܸ똎� _>' ��8��8��8�u�-tD���7� ���|��_�R�.��}M���9�������߰�\2]g��o3��y�?J~��m�>M��]�F������8�M��'���]����ϫQ���ciÁ�J? �t���m�Sj�����b$]�\�X`�9���:H��nԋB��p(�n��C��3%c���7�"P\K���nHLY�R��[��zg)1�Z��m�k��rrKc���u588�ƺ\.�KO2ߋ��VC�μ[�n$ԓ1����қ�� ���S���&�7Hu��y^z�T�9��Lq}Y��$�Tk&��4bš��b�'W��=z������u9�ڍ�<�+ψ��j��?3 ��gi.5�#��x�[J)=�J����w	�`��1�=^癨Ӆ>�e�-��j��*�B��t��.��|�гO�Wf��Y�۶�ٚLsϜqL��YD�j|m��ى����A~o[�]Ns�ēZ�� �d~Ő�#x�P�E��(��М�~�]���$2Hd�2���^���|k����/(J�U$?���s���@H[I������������1�pEg��S|r�R������J�8����M�����J��+v���8 �6	(��q����v<P��ю%��V`��l�P�Y�fUg��	.���+�Ҳ/�%׳��Y_.��z�[������3���\xKZ����;e�*Hk�[��E^����uo!>��0��1�X�F���HJ�8i��c'&|�W<wj0{�}��{�����a�K�=��ˎ��&��^���+o_��^Uފj�]u��n9��}�_�٪����Q�G|dw�=6�k0�1��� ��q�q��� |�v�.|`�C�@q�Q����|��=�{ �Y�D�Hi�#��9�= ���Q8��{�����}��{���;[;�=�{���|���ms�=�#p\s�{ A ��݂������P�x�{������|�Hd�6����@Z�H��r@| � �P�
��Q�
�*�������ו��.*TEzd@�Bv�j��s�*��GT@B��`���B���p=0�@7ET� PA��@�
��Md P�4�V<� P���@( � Tq��@���_�C��@�J�z��Ueowok�"=2 P!��@�A\���ap�k�#* !P�]�[pv!PC`�B���"*T� PA��@�
���&2�@H+i�@ȁ�� �P �8@A��@��/�!P�X�Ҿ����U�^�U��T���)�J�5����*G��8�U�@���g�0��!\%���(�U��W	�\%�Jp��Ȭm"W	����W	�� 
@( ��\%�J����ҏ�*�q�Ru��\�j[[[hy}B      d   '  x��\Mz�ǫ�\>$��jV�$÷���u�#\����_�(�2��@@�>��߇S���%�}����d���OΧl'��&�h���<�s���T�,����9���gnB��R�>9_�bg��d��b.M�Ong��}�/�G��w!��mE�h��k����?x�f������k�[��>�f�.ew��
�SΉYLd�o٥�&j�T]�o�ˍ���E߲K�qjZ���ް����'����.�ꦋ�a�J���P��̯�p>����A-DoY��*u!~S�T$e�����	Vʔss[h�_�A��gMj�0�-��z��Y������R�?��{>X��_*Qie���	o��*P����aJ>qM��m��7�F��e�wb �`F�V^�/H�wʜs-���WLC�1|�7�0�S�'���k����e!]��,|�`�dk��� �2�ѳYڟH����f̘I��A��&��F���� �ĭ��R�E(4!L5q����y��ե�EV�%Ӝ,�!	i�L�@��"7�%>�$�/ɤ���A8�3�Df�Mo�_F	�;�H "�dc�E;^�����ȒHv��N�_��"�EQ�T:�H0~�Vɗ��e�,�Ғ���DY�r��G�o�Otj��u��^R�IJ�;�7L"���e��fR<�)�)KE]RgO��M���.�"
z�(T-2����B��";.�`�L�Π��I&��9�_�������RO���_�R�5�JYd�<��ʗ�	��0��� �Q��L�O>{"��%� ߝD�k����h8��?�+_�U�Q�s: -!�EY�c/�X���~[�R�E���_kV�u��ݢp-��S��_�E�r��]}Wj���|H�1�uj/����c�~�D��S���|�z*k�͖6u&�������z࿠ �hic�ޟ/�Tr5`j�����������(�ŗ6u��ş ��� ��hC���b�UÂRJ�L��7|��Ǜ(F���S!Y?��l�\��'���Z��_�O����9���7���\
p����7��S?3}~>��ϝ�.|�,Y~ɹ�XgaM�
�gΑ�AWd��?�C[��YA-�6�b��V���g���W�� G�FF��ʾev@�F���~����e-�*����7�-3��� ��W�����+Q&B�e1�+&؝�|Io� �*N�Ǿ��כ*�fM&��,>��,}�\�Ԡ���rz���އE�`��3�ۇn�J�p�*j���n��)����� �
*+,2L�W ���r��o�
ʇ{Lw *��s](7���X'�23�2=�\���I�hɹ�j�<�\WDm�,�P�S��t��X��a�zG�w��a�a�iK
3*{�c��uC�����������Z|��a�"bQ�"f!!�֟�(Wª"������Y�a�*#W%��x�d�*vK�!���p���n:��Mq�}2���50�J�2���;���&��1��z��pM8%v�ē1�U f^�v��l�
���+v3c��C�'�Z4�dL�YZ�F�.�>T�+BR���C)[c�{a[#��4��F�F*v�C1�"8�ԃ3�;�VD/�R6g�t��e�0����
�)�8�ZbQUev�@�.g�K_L�q��a�sxϸm*M�Wyjk�ճ��;a�!e�9uGEB.f<��T��*pZ��{���M9�x$gAQ���Gr���|��٭b�L�>�!Lf����Sw�D������VD����\�(���a���� (�+&ʸ�jET�JI�R��r��0*�⑞��5+}�Gr��zQ ՛A�����pF�+ȑ��"Z�y�>�eZ}O,�4
�2u3ӆ��e�v�n�;��^��(Gr6����%�Q&��O�E�Q�����[챔"Gr�$Vss����'�\�>7j�𑞽"*ܧ���^SѫM*bsM������'��c�T�����(��#1�[P�R9���'��"-����越���qcъ��w�+�L�]�>+�,����m ŬŁA�"NN�^�%�-�6��EW��22x�k���+�#Y�T%�q��-a������Q���dQ*�D��P�z�H�oqĶͮfx���3z5
�̂@ġ����UcՀÊ���;_��B:�C5�"�]*���P�zt|2�D,���j�Ax�;T�+���[_����?��C�_��0b^5�Jح�Q�����yB�=�ڼ�uK��[|����U"�#��Q���Wµ�R���aqS0
C�����_��!@�@O�Ȗ����1Q�ڊ�F��5nXA���WO�n�C��Nz߃������Qow� ,q:��[�	M��1��-_��2R��C��u�N�B�u�Qwы�:`'����5���^{��;J_��jH@@̫h_av��N�݅PB��o�{�=OV�j�+�&��y%�+af�q�=�J�T�8�qGp�U7���:�=�-lW�4�3�Ru���{�=�l��2�fE�c�WӾ�K��@�>#�2Xs�-Bu�;D(<5�R�d�q�R��ƪ�ë�_���
�ԚrC�Qw��`��Qc������������k&��^��/+��>*@$�;�&�M#�q��k;z�BjfL�:�<vP���zlv!��1�h�����V�Y\�4u��nl1�ƢH1��lG8-���3�; cY�A�s�����d>(Yu����|�˧q�B2�#)SG��~�#ﵹ�A*�\5�󸌗.�Q�Ϥ˯w~M�t�L��X��>�=�%vԄa��N�Q"$qm!��ԥ̆p�<�����A$.0P��<���uţ�N�x��[�"��5��B��1xC-�h�Bz&��a��^�_f�",֕y��d�%�K�K�~����/�����꙰���ꆠ���mu���J!�D���cǎ���*��n���
yf�~��?��{<#jBmUB��A5�}��A.�a���� �b�q��˹O�Q�����R�T��w\��*��O(�s����oǛ-ݸ��p�?�u=��[�TCx��@�Ѷ�9.���bzh�-����	!�J*ȁ8X&]!�eg6��}ߘtwD�c?�t+B�{}��;�RDZ��,<���a�8�ˣ��;�O9�z�m�Ƨ�V�7M�����[D�W��z�}II��" G��_X��r�����_#�׸��`�r�Pd��ĺ�h����ؚ���}BL�R�!\���wi��h����\G�1Q�L���B��#�����B,!��
��E@,�4���Y��!:�m�D���v�(��p�&�ɵ��Bv�,��������sG�84��+�'�ͣ�;�~�#�d�X5l?WM�j�4� _�/M�س��o�o[��u�зU�%*�ק���/
k]�c���r1��9����P�E��%�3$=b�O���'���i�I��L9~�T�nV���Gqi�~����>�j~��b�i�u�cz��(���Ř�N���
�4z\��؍>�kGi	noS�[�Zf�|��eY����      E   a   x�3���L��2�tJ��2��--I�2��K�(�2���O���2�JM�,.I-�2
%%U�{��-���ĲT.K(l���2�А�=�$�+F��� � *      f   �   x�uν�@�z�)���.�%G���Ƃ�:~��&>�p�!�t�ew$�]ۺ��]G4
L�;X�(X,��p��v�Ko,��e�Y��>ޙ;��f������J���i	�bEӺM�?_�澩�������4W��A48KU�Y�>X����?R-C      h      x������ � �      j      x������ � �      k   !   x�3�4�4@�\�X��b�fP!�+F��� F�      l      x�3�444����� 
�      m      x������ � �      n      x�3��,�@.CNs8��N����� ���      o   W   x�M̱�0��=EH�'@<P3��(�O'0�ǎr��Q}n�#�[��?���)O��cu�U�/�>�'+�G�;���|g0      p   q  x�]��n1Eך�����^�����ʆ��Ĉ�$#��*�u�.%�����ﻭ��=~w���!0�3�q���=�_��۳�?��`(�zl�����N�R��r�Vb���س�'V�TYD#�yV0(CoV��i{;��Uo��v�����x��Xf����ya?�{خ�Q�+q�"��y\i^�p��b�� 5
 $ɢ�\CDK�J�]��cd��C����������7�r:]�7;6s|I�� � ,'v���v�(BwZ��A����>�Α�y�R�4r)�Ȇ���`I`FP�Z������(}�����0�}^����N��~ٶ�c���33ʁp!�G�iz<N��ˀ�$      G   M   x�3�,I��L*�LIO�/J-�/-JN-�/I)��M,(��.��Z�-�L�4BC04600�4®�8�,5��+F��� �*&�      r   v   x�uͱ
�@�Y�%����]�ҥ�����ޟ
��O�_yk�Vj���#�`��j�uZ�؀KdW�0��@�񅔖\k����"9��p5آ��gz���3��@%�      t   m   x�3��M�IM�2��H�KI/��2��ML���KUp�L8��KsJ�2�rR�L9��2R��\3�����6sN��ĲJ���Ă�<.ΐ����<ߒ3(���+F��� *Q#h      u   �  x�]U�n�8<��b�`���8�q��g�;,�";�-X�Y���(Z�3�$�X]Ud!4[#������d����kK��{��??�����y[�$�g,M0�M���Tl�E��%�U��6-s8)/�JR��}STNࡢ\�(I?˺+vqU���"IJ��c烕"u����)��<��AXz���g� �K����a��L��QY�n��d���v%�d���e�m��r�fT4��)K�p�힘t����JZh�(m��`%@٪?��}LBi�����V�m�b��
���3��ec�GZ6U�5o#O�c0	��4��i���6�����s��i�*���V�k�꾨/Mc��Ƕ�^ ����{���1vi�XDP��.Sɑ��c�FhZ4��}�oF������U����dQ(�B�ri��Zm����>6���y;(�jE���ɏMPZ���d&ևKX��$-�s�7�݁�,6��
F�z�~�c�A1��9��2�7r�1f��}wNS0eR�+-5�t�\�U�o�u@䆃�u�^���z+�ad��i�Wթ?M�d.�p���7�a��d�
����$Y`�
c�/�MYǯ�J	acݷɵ�F0��9M�e�=�����`������d�P�,��6����� �\C�R���
w��K`����y�Q|�"x���Rr΋ �._��	d4"���@>�j�������l�������.��E��!����?u�����k�Ё��u$"�a����\65�|��>�0��?b7�`8���P�C ��Gy,�O�E�X+��^ފz_���:�V�k�y��� ����[!C��\���<�=J2��@[����m�H�B�tw>}�}��$0���u�۲�.���li���9�5����!��$@-��p>��j�%2��6]LWŻ����h��2i䗄�PbPJ��Ou�=T�?w�~3l�`m��,�{��#��p𱘐Mr>`<*H�b7�S9c�Is������U�ǈ�1�m��F�!1��7#A�C�o�8/�6V�92<U�F�L剪����x"�K�����)x�`�&q:�JP)f	KKZ-���JY�Z���K�#�����"'�a�yi�㓁�9���"�$�/�x�o3����������!#     