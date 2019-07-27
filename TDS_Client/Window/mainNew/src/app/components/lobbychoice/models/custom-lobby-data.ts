import { LobbyMapLimitType } from '../enums/lobby-map-limit-type';

export class CustomLobbyData {
  public LobbyId?: number;
  public Name: string;
  public OwnerName?: string;
  public Password: string;
  public StartHealth: number;
  public StartArmor: number;
  public AmountLifes: number;

  public MixTeamsAfterRound: boolean;

  public BombDetonateTimeMs: number;
  public BombDefuseTimeMs: number;
  public BombPlantTimeMs: number;
  public RoundTime: number;
  public CountdownTime: number;

  public SpawnAgainAfterDeathMs: number;
  public MapLimitTime: number;

  public MapLimitType: LobbyMapLimitType;
}