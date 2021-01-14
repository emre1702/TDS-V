import { LocationData } from './models/location-data';
import { LocationGroup } from './models/location-group';

const onlineBunkers: LocationData[] = [
    { name: 'Zancudo Bunker', ipls: ['gr_case10_bunkerclosed'], position: { 0: -3058.714, 1: 3329.19, 2: 12.5844 } },
    { name: 'Route68 Bunker', ipls: ['gr_case9_bunkerclosed'], position: { 0: 24.43542, 1: 2959.705, 2: 58.35517 } },
    { name: 'Oilfields Bunker', ipls: ['gr_case3_bunkerclosed'], position: { 0: 481.0465, 1: 2995.135, 2: 43.96672 } },
    { name: 'Desert Bunker', ipls: ['gr_case0_bunkerclosed'], position: { 0: 848.6175, 1: 2996.567, 2: 45.81612 } },
    { name: 'Smoke Tree Bunker', ipls: ['gr_case1_bunkerclosed'], position: { 0: 2126.785, 1: 3335.04, 2: 48.21422 } },
    { name: 'Scrapyard Bunker', ipls: ['gr_case2_bunkerclosed'], position: { 0: 2493.654, 1: 3140.399, 2: 51.28789 } },
    { name: 'Grapeseed Bunker', ipls: ['gr_case5_bunkerclosed'], position: { 0: 1823.961, 1: 4708.14, 2: 42.4991 } },
    { name: 'Palleto Bunker', ipls: ['gr_case7_bunkerclosed'], position: { 0: -783.0755, 1: 5934.686, 2: 24.31475 } },
    { name: 'Route1 Bunker', ipls: ['gr_case11_bunkerclosed'], position: { 0: -3180.466, 1: 1374.192, 2: 19.9597 } },
    { name: 'Farmhouse Bunker', ipls: ['gr_case6_bunkerclosed'], position: { 0: 1570.372, 1: 2254.549, 2: 78.89397 } },
    { name: 'Raton Canyon Bunker', ipls: ['gr_case4_bunkerclosed'], position: { 0: -391.3216, 1: 4363.728, 2: 58.65862 } },
];

const onlineApartments: LocationData[] = [
    { name: 'Modern 1 Apartment', ipls: ['apa_v_mp_h_01_a'], position: { 0: -786.8663, 1: 315.7642, 2: 217.6385 } },
    { name: 'Modern 2 Apartment', ipls: ['apa_v_mp_h_01_b'], position: { 0: -786.9563, 1: 315.6229, 2: 187.9136 } },
    { name: 'Modern 3 Apartment', ipls: ['apa_v_mp_h_01_c'], position: { 0: -774.0126, 1: 342.0428, 2: 196.6864 } },
    { name: 'Mody 1 Apartment', ipls: ['apa_v_mp_h_02_a'], position: { 0: -787.0749, 1: 315.8198, 2: 217.6386 } },
    { name: 'Mody 2 Apartment', ipls: ['apa_v_mp_h_02_b'], position: { 0: -786.8195, 1: 315.5634, 2: 187.9137 } },
    { name: 'Mody 3 Apartment', ipls: ['apa_v_mp_h_02_c'], position: { 0: -774.1382, 1: 342.0316, 2: 196.6864 } },
    { name: 'Vibrant 1 Apartment', ipls: ['apa_v_mp_h_03_a'], position: { 0: -786.6245, 1: 315.6175, 2: 217.6385 } },
    { name: 'Vibrant 2 Apartment', ipls: ['apa_v_mp_h_03_b'], position: { 0: -786.9584, 1: 315.7974, 2: 187.9135 } },
    { name: 'Vibrant 3 Apartment', ipls: ['apa_v_mp_h_03_c'], position: { 0: -774.0223, 1: 342.1718, 2: 196.6863 } },
    { name: 'Sharp 1 Apartment', ipls: ['apa_v_mp_h_04_a'], position: { 0: -787.0902, 1: 315.7039, 2: 217.6384 } },
    { name: 'Sharp 2 Apartment', ipls: ['apa_v_mp_h_04_b'], position: { 0: -787.0155, 1: 315.7071, 2: 187.9135 } },
    { name: 'Sharp 3 Apartment', ipls: ['apa_v_mp_h_04_c'], position: { 0: -773.8976, 1: 342.1525, 2: 196.6863 } },
    { name: 'Monochrome 1 Apartment', ipls: ['apa_v_mp_h_05_a'], position: { 0: -786.9887, 1: 315.7393, 2: 217.6386 } },
    { name: 'Monochrome 2 Apartment', ipls: ['apa_v_mp_h_05_b'], position: { 0: -786.8809, 1: 315.6634, 2: 187.9136 } },
    { name: 'Monochrome 3 Apartment', ipls: ['apa_v_mp_h_05_c'], position: { 0: -774.0675, 1: 342.0773, 2: 196.6864 } },
    { name: 'Seductive 1 Apartment', ipls: ['apa_v_mp_h_06_a'], position: { 0: -787.1423, 1: 315.6943, 2: 217.6384 } },
    { name: 'Seductive 2 Apartment', ipls: ['apa_v_mp_h_06_b'], position: { 0: -787.0961, 1: 315.815, 2: 187.9135 } },
    { name: 'Seductive 3 Apartment', ipls: ['apa_v_mp_h_06_c'], position: { 0: -773.9552, 1: 341.9892, 2: 196.6862 } },
    { name: 'Regal 1 Apartment', ipls: ['apa_v_mp_h_07_a'], position: { 0: -787.029, 1: 315.7113, 2: 217.6385 } },
    { name: 'Regal 2 Apartment', ipls: ['apa_v_mp_h_07_b'], position: { 0: -787.0574, 1: 315.6567, 2: 187.9135 } },
    { name: 'Regal 3 Apartment', ipls: ['apa_v_mp_h_07_c'], position: { 0: -774.0109, 1: 342.0965, 2: 196.6863 } },
    { name: 'Aqua 1 Apartment', ipls: ['apa_v_mp_h_08_a'], position: { 0: -786.9469, 1: 315.5655, 2: 217.6383 } },
    { name: 'Aqua 2 Apartment', ipls: ['apa_v_mp_h_08_b'], position: { 0: -786.9756, 1: 315.723, 2: 187.9134 } },
    { name: 'Aqua 3 Apartment', ipls: ['apa_v_mp_h_08_c'], position: { 0: -774.0349, 1: 342.0296, 2: 196.6862 } },
];

const arcadiusBusinessCentre: LocationData[] = [
    { name: 'Executive Rich', ipls: ['ex_dt1_02_office_02b'], position: { 0: -141.1987, 1: -620.913, 2: 168.8205 } },
    { name: 'Executive Cool', ipls: ['ex_dt1_02_office_02c'], position: { 0: -141.5429, 1: -620.9524, 2: 168.8204 } },
    { name: 'Executive Contrast', ipls: ['ex_dt1_02_office_02a'], position: { 0: -141.2896, 1: -620.9618, 2: 168.8204 } },
    { name: 'Old Spice Warm', ipls: ['ex_dt1_02_office_01a'], position: { 0: -141.4966, 1: -620.8292, 2: 168.8204 } },
    { name: 'Old Spice Classical', ipls: ['ex_dt1_02_office_01b'], position: { 0: -141.3997, 1: -620.9006, 2: 168.8204 } },
    { name: 'Old Spice Vintage', ipls: ['ex_dt1_02_office_01c'], position: { 0: -141.5361, 1: -620.9186, 2: 168.8204 } },
    { name: 'Power Broker Ice', ipls: ['ex_dt1_02_office_03a'], position: { 0: -141.392, 1: -621.0451, 2: 168.8204 } },
    { name: 'Power Broker Conservative', ipls: ['ex_dt1_02_office_03b'], position: { 0: -141.1945, 1: -620.8729, 2: 168.8204 } },
    { name: 'Power Broker Polished', ipls: ['ex_dt1_02_office_03a'], position: { 0: -141.4924, 1: -621.0035, 2: 168.8205 } },
    { name: 'Garage 1', ipls: ['imp_dt1_02_cargarage_a'], position: { 0: -191.0133, 1: -579.1428, 2: 135.0 } },
    { name: 'Garage 2', ipls: ['imp_dt1_02_cargarage_b'], position: { 0: -117.4989, 1: -568.1132, 2: 135.0 } },
    { name: 'Garage 3', ipls: ['imp_dt1_02_cargarage_c'], position: { 0: -136.078, 1: -630.1852, 2: 135.0 } },
    { name: 'Mod Shop', ipls: ['imp_dt1_02_modgarage'], position: { 0: -146.6166, 1: -596.6301, 2: 166.0 } },
];

const mazeBankBuilding: LocationData[] = [
    { name: 'Executive Rich', ipls: ['ex_dt1_11_office_02b'], position: { 0: -75.8466, 1: -826.9893, 2: 243.3859 } },
    { name: 'Executive Cool', ipls: ['ex_dt1_11_office_02c'], position: { 0: -75.49945, 1: -827.05, 2: 243.386 } },
    { name: 'Executive Contrast', ipls: ['ex_dt1_11_office_02a'], position: { 0: -75.49827, 1: -827.1889, 2: 243.386 } },
    { name: 'Old Spice Warm', ipls: ['ex_dt1_11_office_01a'], position: { 0: -75.44054, 1: -827.1487, 2: 243.3859 } },
    { name: 'Old Spice Classical', ipls: ['ex_dt1_11_office_01b'], position: { 0: -75.63942, 1: -827.1022, 2: 243.3859 } },
    { name: 'Old Spice Vintage', ipls: ['ex_dt1_11_office_01c'], position: { 0: -75.47446, 1: -827.2621, 2: 243.386 } },
    { name: 'Power Broker Ice', ipls: ['ex_dt1_11_office_03a'], position: { 0: -75.56978, 1: -827.1152, 2: 243.3859 } },
    { name: 'Power Broker Conservative', ipls: ['ex_dt1_11_office_03b'], position: { 0: -75.51953, 1: -827.0786, 2: 243.3859 } },
    { name: 'Power Broker Polished', ipls: ['ex_dt1_11_office_03a'], position: { 0: -75.41915, 1: -827.1118, 2: 243.3858 } },
    { name: 'Garage 1', ipls: ['imp_dt1_11_cargarage_a'], position: { 0: -84.2193, 1: -823.0851, 2: 221.0 } },
    { name: 'Garage 2', ipls: ['imp_dt1_11_cargarage_b'], position: { 0: -69.8627, 1: -824.7498, 2: 221.0 } },
    { name: 'Garage 3', ipls: ['imp_dt1_11_cargarage_c'], position: { 0: -80.4318, 1: -813.2536, 2: 221.0 } },
    { name: 'Mod Shop', ipls: ['imp_dt1_11_modgarage'], position: { 0: -73.9039, 1: -821.6204, 2: 284.0 } },
];

const mazeBankWest: LocationData[] = [
    { name: 'Executive Rich', ipls: ['ex_sm_15_office_02b'], position: { 0: -1392.667, 1: -480.4736, 2: 72.04217 } },
    { name: 'Executive Cool', ipls: ['ex_sm_15_office_02c'], position: { 0: -1392.542, 1: -480.4011, 2: 72.04211 } },
    { name: 'Executive Contrast', ipls: ['ex_sm_15_office_02a'], position: { 0: -1392.626, 1: -480.4856, 2: 72.04212 } },
    { name: 'Old Spice Warm', ipls: ['ex_sm_15_office_01a'], position: { 0: -1392.617, 1: -480.6363, 2: 72.04208 } },
    { name: 'Old Spice Classical', ipls: ['ex_sm_15_office_01b'], position: { 0: -1392.532, 1: -480.7649, 2: 72.04207 } },
    { name: 'Old Spice Vintage', ipls: ['ex_sm_15_office_01c'], position: { 0: -1392.611, 1: -480.5562, 2: 72.04214 } },
    { name: 'Power Broker Ice', ipls: ['ex_sm_15_office_03a'], position: { 0: -1392.563, 1: -480.549, 2: 72.0421 } },
    { name: 'Power Broker Conservative', ipls: ['ex_sm_15_office_03b'], position: { 0: -1392.528, 1: -480.475, 2: 72.04206 } },
    { name: 'Power Broker Polished', ipls: ['ex_sm_15_office_03a'], position: { 0: -1392.416, 1: -480.7485, 2: 72.04207 } },
    { name: 'Garage 1', ipls: ['imp_sm_15_cargarage_a'], position: { 0: -1388.84, 1: -478.7402, 2: 56.1 } },
    { name: 'Garage 2', ipls: ['imp_sm_15_cargarage_b'], position: { 0: -1388.86, 1: -478.7574, 2: 48.1 } },
    { name: 'Garage 3', ipls: ['imp_sm_15_cargarage_c'], position: { 0: -1374.682, 1: -474.3586, 2: 56.1 } },
    { name: 'Mod Shop', ipls: ['imp_sm_15_modgarage'], position: { 0: -1391.245, 1: -473.9638, 2: 77.2 } },
];

const lomBank: LocationData[] = [
    { name: 'Executive Rich', ipls: ['ex_sm_13_office_02b'], position: { 0: -1579.756, 1: -565.0661, 2: 108.523 } },
    { name: 'Executive Cool', ipls: ['ex_sm_13_office_02c'], position: { 0: -1579.678, 1: -565.0034, 2: 108.5229 } },
    { name: 'Executive Contrast', ipls: ['ex_sm_13_office_02a'], position: { 0: -1579.583, 1: -565.0399, 2: 108.5229 } },
    { name: 'Old Spice Warm', ipls: ['ex_sm_13_office_01a'], position: { 0: -1579.702, 1: -565.0366, 2: 108.5229 } },
    { name: 'Old Spice Classical', ipls: ['ex_sm_13_office_01b'], position: { 0: -1579.643, 1: -564.9685, 2: 108.5229 } },
    { name: 'Old Spice Vintage', ipls: ['ex_sm_13_office_01c'], position: { 0: -1579.681, 1: -565.0003, 2: 108.523 } },
    { name: 'Power Broker Ice', ipls: ['ex_sm_13_office_03a'], position: { 0: -1579.677, 1: -565.0689, 2: 108.5229 } },
    { name: 'Power Broker Conservative', ipls: ['ex_sm_13_office_03b'], position: { 0: -1579.708, 1: -564.9634, 2: 108.5229 } },
    { name: 'Power Broker Polished', ipls: ['ex_sm_13_office_03a'], position: { 0: -1579.693, 1: -564.8981, 2: 108.5229 } },
    { name: 'Garage 1', ipls: ['imp_sm_13_cargarage_a'], position: { 0: -1581.112, 1: -567.245, 2: 85.5 } },
    { name: 'Garage 2', ipls: ['imp_sm_13_cargarage_b'], position: { 0: -1568.739, 1: -562.0455, 2: 85.5 } },
    { name: 'Garage 3', ipls: ['imp_sm_13_cargarage_c'], position: { 0: -1563.557, 1: -574.4314, 2: 85.5 } },
    { name: 'Mod Shop', ipls: ['imp_sm_13_modgarage'], position: { 0: -1578.023, 1: -576.4251, 2: 104.2 } },
];

const clubhousesAndWarehouses: LocationData[] = [
    { name: 'Clubhouse 1', ipls: ['bkr_biker_interior_placement_interior_0_biker_dlc_int_01_milo'], position: { 0: 1107.04, 1: -3157.399, 2: -37.51859 } },
    { name: 'Clubhouse 2', ipls: ['bkr_biker_interior_placement_interior_1_biker_dlc_int_02_milo'], position: { 0: 998.4809, 1: -3164.711, 2: -38.90733 } },
    { name: 'Meth Lab', ipls: ['bkr_biker_interior_placement_interior_2_biker_dlc_int_ware01_milo'], position: { 0: 1009.5, 1: -3196.6, 2: -38.99682 } },
    { name: 'Weed Farm', ipls: ['bkr_biker_interior_placement_interior_3_biker_dlc_int_ware02_milo'], position: { 0: 1051.491, 1: -3196.536, 2: -39.14842 } },
    { name: 'Cocaine Lockup', ipls: ['bkr_biker_interior_placement_interior_4_biker_dlc_int_ware03_milo'], position: { 0: 1093.6, 1: -3196.6, 2: -38.99841 } },
    {
        name: 'Counterfeit Cash Factory',
        ipls: ['bkr_biker_interior_placement_interior_5_biker_dlc_int_ware04_milo'],
        position: { 0: 1121.897, 1: -3195.338, 2: -40.4025 },
    },
    {
        name: 'Document Forgery Office',
        ipls: ['bkr_biker_interior_placement_interior_6_biker_dlc_int_ware05_milo'],
        position: { 0: 1165, 1: -3196.6, 2: -39.01306 },
    },
    {
        name: 'Warehouse Small',
        ipls: ['ex_exec_warehouse_placement_interior_1_int_warehouse_s_dlc_milo'],
        position: { 0: 1094.988, 1: -3101.776, 2: -39.00363 },
    },
    {
        name: 'Warehouse Medium',
        ipls: ['ex_exec_warehouse_placement_interior_0_int_warehouse_m_dlc_milo'],
        position: { 0: 1056.486, 1: -3105.724, 2: -39.00439 },
    },
    {
        name: 'Warehouse Large',
        ipls: ['ex_exec_warehouse_placement_interior_2_int_warehouse_l_dlc_milo'],
        position: { 0: 1006.967, 1: -3102.079, 2: -39.0035 },
    },
    {
        name: 'Vehicle Warehouse',
        ipls: ['imp_impexp_interior_placement_interior_1_impexp_intwaremed_milo_'],
        position: { 0: 994.5925, 1: -3002.594, 2: -39.64699 },
    },
    { name: 'Lost MC Clubhouse', ipls: ['bkr_bi_hw1_13_int'], position: { 0: 982.0083, 1: -100.8747, 2: 74.84512 } },
];

const specialLocations: LocationData[] = [
    { name: 'Normal Cargo Ship', ipls: ['cargoship'], position: { 0: -163.3628, 1: -2385.161, 2: 5.999994 } },
    { name: 'Sunken Cargo Ship', ipls: ['sunkcargoship'], position: { 0: 163.3628, 1: -2385.161, 2: 5.999994 } },
    { name: 'Burning Cargo Ship', ipls: ['SUNK_SHIP_FIRE'], position: { 0: 163.3628, 1: -2385.161, 2: 5.999994 } },
    { name: 'Red Carpet', ipls: ['redCarpet'], position: { 0: 300.5927, 1: 300.5927, 2: 104.3776 } },
    { name: 'Rekt Stilthouse Destroyed', ipls: ['DES_StiltHouse_imapend'], position: { 0: -1020.518, 1: 663.27, 2: 153.5167 } },
    { name: 'Rekt Stilthouse Rebuild', ipls: ['DES_stilthouse_rebuild'], position: { 0: -1020.518, 1: 663.27, 2: 153.5167 } },
    { name: 'Union Depository', ipls: ['FINBANK'], position: { 0: 2.6968, 1: -667.0166, 2: 16.13061 } },
    { name: 'Trevors Trailer Dirty', ipls: ['TrevorsMP'], position: { 0: 1975.552, 1: 3820.538, 2: 33.44833 } },
    { name: 'Trevors Trailer Clean', ipls: ['TrevorsTrailerTidy'], position: { 0: 1975.552, 1: 3820.538, 2: 33.44833 } },
    { name: 'Stadium', ipls: ['SP1_10_real_interior'], position: { 0: -248.6731, 1: -2010.603, 2: 30.14562 } },
    { name: 'Max Renda Shop', ipls: ['refit_unload'], position: { 0: -585.8247, 1: -282.72, 2: 35.45475 } },
    { name: 'Jewel Store', ipls: ['post_hiest_unload'], position: { 0: -630.07, 1: -236.332, 2: 38.05704 } },
    { name: 'FIB Lobby', ipls: ['FIBlobby'], position: { 0: 110.4, 1: -744.2, 2: 45.7496 } },
    {
        name: 'Gunrunning Heist Yacht',
        ipls: [
            'gr_heist_yacht2',
            'gr_heist_yacht2_bar',
            'gr_heist_yacht2_bedrm',
            'gr_heist_yacht2_bridge',
            'gr_heist_yacht2_enginrm',
            'gr_heist_yacht2_lounge',
        ],
        position: { 0: 1373.828, 1: 6737.393, 2: 6.707596 },
    },
    {
        name: 'Dignity Heist Yacht',
        ipls: [
            'hei_yacht_heist',
            'hei_yacht_heist_enginrm',
            'hei_yacht_heist_Lounge',
            'hei_yacht_heist_Bridge',
            'hei_yacht_heist_Bar',
            'hei_yacht_heist_Bedrm',
            'hei_yacht_heist_DistantLights',
            'hei_yacht_heist_LODLights',
        ],
        position: { 0: -2027.946, 1: -1036.695, 2: 6.70758 },
    },
    {
        name: 'Dignity Party Yacht',
        ipls: ['smboat', 'smboat_lod'],
        position: { 0: -2023.643, 1: -1038.119, 2: 5.576781 },
    },
    {
        name: 'Aircraft Carrier',
        ipls: [
            'hei_carrier',
            'hei_carrier_DistantLights',
            'hei_Carrier_int1',
            'hei_Carrier_int2',
            'hei_Carrier_int3',
            'hei_Carrier_int4',
            'hei_Carrier_int5',
            'hei_Carrier_int6',
            'hei_carrier_LODLights',
        ],
        position: { 0: 3084.73, 1: -4770.709, 2: 15.26167 },
    },
    {
        name: 'Bridge Train Crash',
        ipls: ['canyonriver01_traincrash', 'railing_end'],
        position: { 0: 532.1309, 1: 4526.187, 2: 89.79387 },
    },
    {
        name: 'Bridge Train Normal',
        ipls: ['canyonriver01', 'railing_start'],
        position: { 0: 532.1309, 1: 4526.187, 2: 89.79387 },
    },
    {
        name: 'North Yankton',
        ipls: [
            'prologue01',
            'prologue01c',
            'prologue01d',
            'prologue01e',
            'prologue01f',
            'prologue01g',
            'prologue01h',
            'prologue01i',
            'prologue01j',
            'prologue01k',
            'prologue01z',
            'prologue02',
            'prologue03',
            'prologue03b',
            'prologue03_grv_dug',
            'prologue_grv_torch',
            'prologue04',
            'prologue04b',
            'prologue04_cover',
            'des_protree_end',
            'des_protree_start',
            'prologue05',
            'prologue05b',
            'prologue06',
            'prologue06b',
            'prologue06_int',
            'prologue06_pannel',
            'plg_occl_00',
            'prologue_occl',
            'prologuerd',
            'prologuerdb',
        ],
        position: { 0: 3217.697, 1: -4834.826, 2: 111.8152 },
    },
    {
        name: 'ONeils Farm Burnt',
        ipls: ['farmint', 'farm_burnt', 'farm_burnt_props', 'des_farmhs_endimap', 'des_farmhs_end_occl'],
        position: { 0: 2469.03, 1: 4955.278, 2: 45.11892 },
    },
    {
        name: 'ONeils Farm',
        ipls: ['farm', 'farm_props', 'farm_int'],
        position: { 0: 2469.03, 1: 4955.278, 2: 45.11892 },
    },
    {
        name: 'Morgue',
        ipls: ['coronertrash', 'Coroner_Int_On'],
        position: { 0: 275.446, 1: -1361.11, 2: 24.5378 },
    },
    {
        name: 'Cayo pericio heist island',
        position: { 0: 4840.571, 1: -5174.425, 2: 2.0 },
    },
    {
        name: 'Submarine',
        ipls: [
            'entity_set_acetylene',
            'entity_set_brig',
            'entity_set_demolition',
            'entity_set_fingerprint',
            'entity_set_guide',
            'entity_set_hatch_lights_off',
            'entity_set_hatch_lights_on',
            'entity_set_jammer',
            'entity_set_plasma',
            'entity_set_suppressors',
            'entity_set_weapons',
        ],
        position: { 0: 1561.562, 1: 410.45, 2: -48.0 },
    },
    {
        name: 'Casino Nightclub',
        ipls: [
            'EntitySet_DJ_Lighting',
            'dj_01_lights_01',
            'dj_01_lights_02',
            'dj_01_lights_03',
            'dj_01_lights_04',
            'dj_02_lights_01',
            'dj_02_lights_02',
            'dj_02_lights_03',
            'dj_02_lights_04',
            'dj_03_lights_01',
            'dj_03_lights_02',
            'dj_03_lights_03',
            'dj_03_lights_04',
            'dj_04_lights_01',
            'dj_04_lights_02',
            'dj_04_lights_03',
            'dj_04_lights_04',
            'int01_ba_bar_content',
            'int01_ba_booze_01',
            'int01_ba_booze_02',
            'int01_ba_booze_03',
            'int01_ba_dj01',
            'int01_ba_dj02',
            'int01_ba_dj03',
            'int01_ba_dj04',
            'int01_ba_dj_keinemusik',
            'int01_ba_dj_moodyman',
            'int01_ba_dj_palms_trax',
            'int01_ba_dry_ice',
            'int01_ba_equipment_setup',
            'int01_ba_equipment_upgrade',
            'int01_ba_lightgrid_01',
            'int01_ba_lights_screen',
            'int01_ba_screen',
            'int01_ba_security_upgrade',
            'int01_ba_style02_podium',
            'light_rigs_off',
        ],
        position: { 0: 1550.0, 1: 250.0, 2: -48.0 },
    },
    {
        name: 'Island Vault',
        ipls: ['bonds_set', 'files_set', 'panther_set', 'pearl_necklace_set', 'pink_diamond_set', 'tequila_set'],
        position: { 0: 5012.0, 1: -5747.5, 2: 15.0 },
    },
];

const carGarages: LocationData[] = [
    { name: '2 Car', position: { 0: 173.2903, 1: -1003.6, 2: -99.65707 } },
    { name: '6 Car', position: { 0: 197.8153, 1: -1002.293, 2: -99.65749 } },
    { name: '10 Car', position: { 0: 229.9559, 1: -981.7928, 2: -99.66071 } },
];

const apartments: LocationData[] = [
    { name: 'Low End Apartment', position: { 0: 261.4586, 1: -998.8196, 2: -99.00863 } },
    { name: 'Medium End Apartment', position: { 0: 347.2686, 1: -999.2955, 2: -99.19622 } },
    { name: '4 Integrity Way, Apt 28', position: { 0: -18.07856, 1: -583.6725, 2: 79.46569 } },
    { name: '4 Integrity Way, Apt 30', position: { 0: -35.31277, 1: -580.4199, 2: 88.71221 } },
    { name: 'Dell Perro Heights, Apt 4', position: { 0: -1468.14, 1: -541.815, 2: 73.4442 } },
    { name: 'Dell Perro Heights, Apt 7', position: { 0: -1477.14, 1: -538.7499, 2: 55.5264 } },
    { name: 'Richard Majestic, Apt 2', position: { 0: -915.811, 1: -379.432, 2: 113.6748 } },
    { name: 'Tinsel Towers, Apt 42', position: { 0: -614.86, 1: 40.6783, 2: 97.60007 } },
    { name: 'Eclipse Towers, Apt 3', position: { 0: -773.407, 1: 341.766, 2: 211.397 } },
    { name: '3655 Wild Oats Drive', position: { 0: -169.286, 1: 486.4938, 2: 137.4436 } },
    { name: '2044 North Conker Avenue', position: { 0: 340.9412, 1: 437.1798, 2: 149.3925 } },
    { name: '2045 North Conker Avenue', position: { 0: 373.023, 1: 416.105, 2: 145.7006 } },
    { name: '2862 Hillcrest Avenue', position: { 0: -676.127, 1: 588.612, 2: 145.1698 } },
    { name: '2868 Hillcrest Avenue', position: { 0: -763.107, 1: 615.906, 2: 144.1401 } },
    { name: '2874 Hillcrest Avenue', position: { 0: -857.798, 1: 682.563, 2: 152.6529 } },
    { name: '2677 Whispymound Drive	', position: { 0: 120.5, 1: 549.952, 2: 184.097 } },
    { name: '2133 Mad Wayne Thunder', position: { 0: -1288, 1: 440.748, 2: 97.69459 } },
];

const misc: LocationData[] = [
    { name: 'Bunker Interior', position: { 0: 899.5518, 1: -3246.038, 2: -98.04907 } },
    { name: 'Char Creator', position: { 0: 402.5164, 1: -1002.847, 2: -99.2587 } },
    { name: 'Mission Carpark', position: { 0: 405.9228, 1: -954.1149, 2: -99.6627 } },
    { name: 'Torture Room', position: { 0: 136.5146, 1: -2203.149, 2: 7.30914 } },
    { name: "Solomon's Office", position: { 0: -1005.84, 1: -478.92, 2: 50.02733 } },
    { name: "Psychiatrist's Office", position: { 0: -1908.024, 1: -573.4244, 2: 19.09722 } },
    { name: "Omega's Garage", position: { 0: 2331.344, 1: 2574.073, 2: 46.68137 } },
    { name: 'Movie Theatre', position: { 0: -1427.299, 1: -245.1012, 2: 16.8039 } },
    { name: 'Motel', position: { 0: 152.2605, 1: -1004.471, 2: -98.99999 } },
    { name: 'Madrazos Ranch', position: { 0: 1399, 1: 1150, 2: 115 } },
    { name: 'Life Invader Office', position: { 0: -1044.193, 1: -236.9535, 2: 37.96496 } },
    { name: "Lester's House", position: { 0: 1273.9, 1: -1719.305, 2: 54.77141 } },
    { name: 'FBI Top Floor', position: { 0: 134.5835, 1: -749.339, 2: 258.152 } },
    { name: 'FBI Floor 47', position: { 0: 134.5835, 1: -766.486, 2: 234.152 } },
    { name: 'FBI Floor 49', position: { 0: 134.635, 1: -765.831, 2: 242.152 } },
    { name: 'IAA Office', position: { 0: 117.22, 1: -620.938, 2: 206.1398 } },
    { name: "Smuggler's Run Hangar", position: { 0: -1266.802, 1: -3014.837, 2: -49.0 } },
    { name: 'Avenger Interior', position: { 0: 520.0, 1: 4750.0, 2: -70.0 } },
    { name: 'Facility', position: { 0: 345.0041, 1: 4842.001, 2: -59.9997 } },
    { name: 'Server Farm', position: { 0: 2168.0, 1: 2920.0, 2: -84.0 } },
    { name: 'Submarine', position: { 0: 514.33, 1: 4886.18, 2: -62.59 } },
    { name: 'IAA Facility', position: { 0: 2147.91, 1: 2921.0, 2: -61.9 } },
    { name: 'Nightclub', position: { 0: -1604.664, 1: -3012.583, 2: -78.0 } },
    { name: 'Nightclub Warehouse', position: { 0: -1505.783, 1: -3012.587, 2: -80.0 } },
    { name: 'Terrorbyte Interior', position: { 0: -1421.015, 1: -3012.587, 2: -80.0 } },
    {
        name: 'Car Wash',
        ipls: ['carwash_with_spinners'],
        iplsToUnload: ['carwash_without_spinners'],
        position: { 0: 29.426, 1: -1391.922, 2: 29.362 },
    },
];

const diamondCasinoAndResort: LocationData[] = [
    { name: 'Casino', ipls: ['vw_casino_main'], position: { 0: 1100.0, 1: 220.0, 2: -50.0 } },
    { name: 'Garage', ipls: ['vw_casino_garage'], position: { 0: 1295.0, 1: 230.0, 2: -50.0 } },
    { name: 'Car Park', ipls: ['vw_casino_carpark'], position: { 0: 1380.0, 1: 200.0, 2: -50.0 } },
    { name: 'Penthouse', ipls: ['vw_casino_penthouse'], position: { 0: 976.636, 1: 70.295, 2: 115.164 } },
];

export const locations: LocationGroup[] = [
    { groupName: 'Online Bunkers', locations: onlineBunkers },
    { groupName: 'Online Apartments', locations: onlineApartments },
    { groupName: 'Arcadius Business Centre', locations: arcadiusBusinessCentre },
    { groupName: 'Maze Bank Building', locations: mazeBankBuilding },
    { groupName: 'Maze Bank West', locations: mazeBankWest },
    { groupName: 'Lom Bank', locations: lomBank },
    { groupName: 'Clubhouses & Warehouses', locations: clubhousesAndWarehouses },
    { groupName: 'Special Locations', locations: specialLocations },
    { groupName: 'Car Garages', locations: carGarages },
    { groupName: 'Apartments', locations: apartments },
    { groupName: 'Misc', locations: misc },
    { groupName: 'Diamond Casino & Resort', locations: diamondCasinoAndResort },
];

export function getLocationDataByName(locationName: string): LocationData {
    for (const locationGroup of locations) {
        for (const locationData of locationGroup.locations) {
            if (locationData.name === locationName) return locationData;
        }
    }
    return undefined;
}
