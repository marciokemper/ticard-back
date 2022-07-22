
namespace Control.Facilites.Domain.Entities
{

    public class EmpresaFornecedor
    {
        public int Id { get; set; }
        public Empresa Empresa { get; set; }
        public Fornecedor Fornecedor { get; set; }   

    }
}
