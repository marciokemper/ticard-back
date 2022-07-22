using Control.Facilites.AppServices.Dtos;
using Control.Facilites.AppServices.Interfaces;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Extensions;
using Control.Facilites.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;


namespace Control.Facilites.Controllers
{
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioAppService appService;
        private readonly UsuarioValidator validator;

        public UsuarioController(IUsuarioAppService appService, UsuarioValidator validator)
        {
            this.appService = appService;
            this.validator = validator;
        }
        [HttpPost, Authorize]
        public DataTableResponse Post(int? loggedfornecedorId, int? loggedEmpresaId, [FromBody]DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<Usuario>>();

            try
            {
                var start = filter.start;
                var length = filter.length;
                int pageSize = length != 0 ? Convert.ToInt32(length) : 0;

                int skip = start != 0 ? Convert.ToInt32(start) : 0;

                result.Result = appService.ListByFornecedor(filter, loggedfornecedorId, loggedEmpresaId);
                result.Success = true;

                var _return = new DataTableResponse
                {
                    recordsTotal = result.Result.Count(),
                    recordsFiltered = result.Result.Count(),
                    data = result.Result.Skip(skip).Take(pageSize).ToArray()
                };

                return _return;

            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return null;

        }


        // GET api/todo/5
        [HttpGet("{id}")]
        public Results.GenericResult<Usuario> Get(int id)
        {
            var result = new Results.GenericResult<Usuario>();

            try
            {
                result.Result = appService.GetById(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        [Route("Incluir")]
        [HttpPost]
        public Results.GenericResult<Usuario> Incluir([FromBody]Usuario model)
        {
            
            var result = new Results.GenericResult<Usuario>();

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

        // PUT api/todo/5
        [HttpPut("{id}")]
        public Results.GenericResult Put(int id, [FromBody]Usuario model)
        {
            var result = new Results.GenericResult();

            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid)
            {
                try
                {
                    result.Success = appService.Update(model);
                    if (!result.Success)
                        throw new Exception($"Todo {id} can't be updated");
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

        // DELETE api/todo/5
        [HttpDelete("{id}")]
        public Results.GenericResult Delete(int id)
        {
            var result = new Results.GenericResult();

            try
            {
                result.Success = appService.Remove(id);
                if (!result.Success)
                    throw new Exception($"Cliente {id} não pode ser excluído");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }
    }
}
