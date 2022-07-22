using Control.Facilites.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.DomainServices.IoC
{
    public static class Module
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var dic = new Dictionary<Type, Type>();
            dic.Add(typeof(IDocumentoDomainService), typeof(DocumentoDomainService));
            dic.Add(typeof(IEmpresaDomainService), typeof(EmpresaDomainService));
            dic.Add(typeof(IFornecedorDomainService), typeof(FornecedorDomainService));
            dic.Add(typeof(IUsuarioDomainService), typeof(UsuarioDomainService));
            dic.Add(typeof(IConfiguracaoDomainService), typeof(ConfiguracaoDomainService));
            dic.Add(typeof(IEmpresaFornecedorDomainService), typeof(EmpresaFornecedorDomainService));
            
            return dic;
        }
    }
}
