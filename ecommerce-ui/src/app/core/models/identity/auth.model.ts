export interface TokenResponse {
  access_token: string;
  token_type: string;
  expires_in: number;
  refresh_token?: string;
  scope: string;
}

export interface UserSession {
  username: string;
  email: string;
  accessToken: string;
  refreshToken?: string;
  expiresAt: number;
  roles: string[];
}
