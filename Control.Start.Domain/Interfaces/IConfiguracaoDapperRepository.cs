using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Filters;
using Control.Facilites.Domain.Interfaces.Repositories.Common;

namespace Control.Facilites.Domain.Interfaces
{
    public interface IConfiguracaoDapperRepository : IRepositoryBase<Configuracao, ConfiguracaoFilter, Filters.DataTableFilter>  {
    }
}
