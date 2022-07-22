import { EmpresaFornecedor } from "./empresaFornecedor";

export class Documento {
  id: number;
  empresaFornecedor: EmpresaFornecedor;
  ativo: Boolean;
  dataInclusao: Date;
  situacaoDocumento: Number;
  caminhoAnexo: string;

}
