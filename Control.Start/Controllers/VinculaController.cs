using Control.Contas.AppServices.Dtos;
using Control.Contas.AppServices.Interfaces;
using Control.Contas.Domain.Entities;
using Control.Contas.Extensions;
using Control.Contas.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Control.Contas.Controllers
{
    [Route("api/[controller]")]
    public class VinculaController : Controller
    {
        private readonly IClienteAppService appService;
        private readonly ClienteValidator validator;

        public VinculaController(IClienteAppService appService, ClienteValidator validator)
        {
            this.appService = appService;
            this.validator = validator;
        }


        [HttpPost, Authorize]
        public DataTableResponse Post([FromBody]DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<Cliente>>();

            try
            {
                var start = filter.start;
                var length = filter.length;
                int pageSize = length != 0 ? Convert.ToInt32(length) : 0;

                int skip = start != 0 ? Convert.ToInt32(start) : 0;

                result.Result = appService.List(filter);
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
        [HttpGet("{id}"), Authorize]
        public Results.GenericResult<Cliente> Get(int id)
        {
            var result = new Results.GenericResult<Cliente>();

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
        [HttpPost, Authorize]
        public Results.GenericResult<Cliente> Incluir([FromBody]Cliente model)
        {
            var result = new Results.GenericResult<Cliente>();

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
        [HttpPut("{id}"), Authorize]
        public Results.GenericResult Put(int id, [FromBody]Cliente model)
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
        [HttpDelete("{id}"), Authorize]
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
