using Control.Facilites.AppServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.IoC
{
    public static class Module
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var dic = new Dictionary<Type, Type>();
            dic.Add(typeof(IDocumentoAppService), typeof(DocumentoAppService));
            dic.Add(typeof(IEmpresaAppService), typeof(EmpresaAppService));
            dic.Add(typeof(IFornecedorAppService), typeof(FornecedorAppService));
            dic.Add(typeof(IUsuarioAppService), typeof(UsuarioAppService));
            dic.Add(typeof(IConfiguracaoAppService), typeof(ConfiguracaoAppService));
            dic.Add(typeof(IEmpresaFornecedorAppService), typeof(EmpresaFornecedorAppService));
            return dic;
        }
    }
}
