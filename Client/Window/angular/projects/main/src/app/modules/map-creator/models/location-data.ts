import { Position3D } from './position3d';

export interface LocationData {
    name: string;
    ipls?: string[];
    iplsToUnload?: string[];
    hash?: string;
    position: Position3D;
}
