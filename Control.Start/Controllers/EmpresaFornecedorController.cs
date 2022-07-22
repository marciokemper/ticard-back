using Control.Facilites.AppServices.Dtos;
using Control.Facilites.AppServices.Interfaces;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Extensions;
using Control.Facilites.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;


namespace Control.Facilites.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class EmpresaFornecedorController : Controller
    {
        private readonly IEmpresaFornecedorAppService appService;
        private readonly EmpresaFornecedorValidator validator;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="appService"></param>
        /// <param name="validator"></param>
        public EmpresaFornecedorController(IEmpresaFornecedorAppService appService, EmpresaFornecedorValidator validator)
        {
            this.appService = appService;
            this.validator = validator;
            //this.configuration = iConfiguration;
        }


        [HttpPost("{id}"), Authorize]
        public DataTableResponse Post(int id, [FromBody] DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<EmpresaFornecedor>>();

            try
            {
                var start = filter.start;
                var length = filter.length;
                int pageSize = length != 0 ? Convert.ToInt32(length) : 0;

                int skip = start != 0 ? Convert.ToInt32(start) : 0;

                result.Result = appService.List(id, filter);
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

        // GET: api/todo
        [HttpGet, Authorize]
        public Results.GenericResult<List<EmpresaFornecedor>> Get([FromQuery] EmpresaFornecedorFilterDto filter)
        {
            var result = new Results.GenericResult<List<EmpresaFornecedor>>();

            try
            {
                result.Result = appService.GetAll(filter);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        [HttpGet("{id}"), Authorize]
        public Results.GenericResult<EmpresaFornecedor> Get(int id)
        {
            var result = new Results.GenericResult<EmpresaFornecedor>();

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

        // POST api/todo
        [Route("Incluir")]
        [HttpPost]
        public Results.GenericResult<EmpresaFornecedor> Post(int empresa, int fornecedor)
        {
            var result = new Results.GenericResult<EmpresaFornecedor>();


            try
            {
                result.Result = appService.Add(empresa, fornecedor);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }



            return result;
        }


        [Route("Deletar")]
        [HttpDelete]
        public Results.GenericResult Deletar(int empresa, int fornecedor)
        {
            var result = new Results.GenericResult();

            try
            {
                result.Success = appService.Remove(empresa, fornecedor);
                if (!result.Success)
                    throw new Exception($"Vinculação não pode ser excluído");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;

        }

        // PUT api/todo/5
        [HttpPut("{id}"), Authorize]
        public Results.GenericResult Put(int id, [FromBody] EmpresaFornecedor model)
        {
            var result = new Results.GenericResult();

            if (model == null)
                return null;

            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid)
            {
                try
                {
                    //result.Success = appService.Update(model, int.Parse(HttpContext.User.Claims.ToList()[9].Value));
                    if (!result.Success)
                        throw new Exception($"Fornecedor/Cliente {id} não pode ser alterado");
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
