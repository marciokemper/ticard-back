using Control.Facilites.Data.Repositories;
using Control.Facilites.Domain.Interfaces;
using Control.Facilites.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Data.IoC
{
    public static class Module
    {
        /// <summary>
        /// teste
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, Type> GetTypes()
        {
            var dic = new Dictionary<Type, Type>();
            dic.Add(typeof(IDocumentoDapperRepository), typeof(DocumentoDapperRepository));
            dic.Add(typeof(IEmpresaDapperRepository), typeof(EmpresaDapperRepository));
            dic.Add(typeof(IFornecedorDapperRepository), typeof(FornecedorDapperRepository));
            dic.Add(typeof(IUsuarioDapperRepository), typeof(UsuarioDapperRepository));
            dic.Add(typeof(IConfiguracaoDapperRepository), typeof(ConfiguracaoDapperRepository));
            dic.Add(typeof(IEmpresaFornecedorDapperRepository), typeof(EmpresaFornecedorDapperRepository));            
            dic.Add(typeof(IUnitOfWork), typeof(UnitOfWork));
            dic.Add(typeof(IUnitOfWorkTransaction), typeof(UnitOfWorkTransaction));
            return dic;
        }
    }
}
