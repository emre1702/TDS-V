export interface LobbyChoice {
  name: string;
  func: () => void;
  imgUrl: string;
  disabled?: boolean;
}
