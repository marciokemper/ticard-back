using Control.Facilites.AppServices.Dtos;
using Control.Facilites.AppServices.Interfaces;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Extensions;
using Control.Facilites.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Control.Facilites.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class ConfiguracaoController : Controller
    {
        private readonly IConfiguracaoAppService appService;
        private readonly ConfiguracaoValidator validator;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="appService"></param>
        /// <param name="validator"></param>
        public ConfiguracaoController(IConfiguracaoAppService appService, ConfiguracaoValidator validator)
        {
            this.appService = appService;
            this.validator = validator;
        }


        /// <summary>
        /// Pesquisa Configurações
        /// </summary>
        /// <returns>Configuração</returns>
        [HttpGet, Authorize]
        public Results.GenericResult<Configuracao> Get()
        {


            var result = new Results.GenericResult<Configuracao>();

            try
            {
                result.Result = appService.Get(1);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }
        /// <summary>
        /// Incluir Configurações
        /// </summary>
        /// <param name="model">dados das configurações</param>
        /// <returns>Configuração incluída</returns>
        [Route("Incluir")]
        [HttpPost, Authorize]
        public Results.GenericResult<Configuracao> Incluir([FromBody]Configuracao model)
        {
            var result = new Results.GenericResult<Configuracao>();

            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid)
            {
                try
                {
                    result.Result = appService.Add(model);
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Errors = new string[] { ex.Message };
                }
            }
            else
                result.Errors = validatorResult.GetErrors();


            return result;
        }

        /// <summary>
        /// Alterar Configuração
        /// </summary>
        /// <param name="id">Codigo da configuração a ser alterada</param>
        /// <param name="model">Dados da configuração</param>
        /// <returns>Configuração alterada</returns>
        [HttpPut("{id}"), Authorize]
        public Results.GenericResult Put(int id, [FromBody]Configuracao model)
        {
            var result = new Results.GenericResult();

            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid)
            {
                try
                {
                    result.Success = appService.Update(model);
                    if (!result.Success)
                        throw new Exception($"Configuração não pode ser alterada");
                }
                catch (Exception ex)
                {
                    result.Errors = new string[] { ex.Message };
                }
            }
            else
                result.Errors = validatorResult.GetErrors();


            return result;

        }

       
    }
}
