import { Usuario } from "./usuario";

export class IProfile {
  token: string;
  expiration: string;
  claims: Claim[];
  currentUsuario: Usuario;
}

interface Claim {
  type: string;
  value: string;
}

