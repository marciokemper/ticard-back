export class Empresa {
  id: number;
  nome: string | null;
  logo: number | null;
  empresaPai: Empresa | null;
  empresaPaiId: number | null;
  cnpj: number | null;
  pontoVenda: number | null;
}
