import { EmpresaFornecedor } from "./empresaFornecedor";

export class Usuario {
  id: number | null;
  nome: string;
  login: string;
  email: string;
  senha: string;
  confirmaSenha: string;
  ativo: boolean;
  empresaFornecedor: EmpresaFornecedor;
  empresaFornecedorId: number | null;
}
