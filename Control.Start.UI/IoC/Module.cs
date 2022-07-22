using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Control.Facilites.IoC
{
    public static class Module
    {

        public static List<Type> GetSingleTypes()
        {
            var result = new List<Type>();
            result.Add(typeof(Validators.DocumentoValidator));
            result.Add(typeof(Validators.FornecedorValidator));
            result.Add(typeof(Validators.UsuarioValidator));
            result.Add(typeof(Validators.ConfiguracaoValidator));
            result.Add(typeof(Validators.EmpresaValidator));
            result.Add(typeof(Validators.EmpresaFornecedorValidator));
            return result;
        }
    }
}
