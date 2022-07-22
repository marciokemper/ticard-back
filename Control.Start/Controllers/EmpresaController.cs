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
    public class EmpresaController : Controller
    {
        private readonly IEmpresaAppService appService;
        private readonly EmpresaValidator validator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appService"></param>
        /// <param name="validator"></param>
        public EmpresaController(IEmpresaAppService appService, EmpresaValidator validator)
        {
            this.appService = appService;
            this.validator = validator;
        }
                
        private bool TryToDelete(string f)
        {
            try
            {
                System.IO.File.Delete(f);
                return true;
            }
            catch (IOException)
            {
                // B.
                // We could not delete the file.
                return false;
            }
        }

        [HttpPost]
        public DataTableResponse Post([FromBody]DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<Empresa>>();

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
                    data = result.Result.Skip(skip).Take(10).ToArray()
                };

                return _return;

            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return null;

        }

        [HttpPost]
        [Route("GetFiliais")]
        [HttpPost("{id}"), Authorize]
        public DataTableResponse GetFiliais(int id, [FromBody] DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<Empresa>>();

            try
            {
                var start = filter.start;
                var length = filter.length;
                int pageSize = length != 0 ? Convert.ToInt32(length) : 0;

                int skip = start != 0 ? Convert.ToInt32(start) : 0;

                result.Result = appService.List(id,filter);
                result.Success = true;

                var _return = new DataTableResponse
                {
                    recordsTotal = result.Result.Count(),
                    recordsFiltered = result.Result.Count(),
                    data = result.Result.Skip(skip).Take(10).ToArray()
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
        public Results.GenericResult<Empresa> Get(int id)
        {
            var result = new Results.GenericResult<Empresa>();

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
        public Results.GenericResult<Empresa> Incluir([FromBody]Empresa model)
        {
            var result = new Results.GenericResult<Empresa>();

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

        [Route("GetAll")]
        [HttpGet, Authorize]
        public Results.GenericResult<List<Empresa>> GetAll(string filter)
        {
            var result = new Results.GenericResult<List<Empresa>>();

            try
            {
                result.Result = appService.List(filter);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // PUT api/todo/5
        [HttpPut("{id}"), Authorize]
        public Results.GenericResult Put(int id, [FromBody]Empresa model)
        {
            var result = new Results.GenericResult();

            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid)
            {
                try
                {
                    result.Success = appService.Update(model);
                    if (!result.Success)
                        throw new Exception($"Fornecedor {id} não pode ser alterado");
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
                    throw new Exception($"Fornecedor {id} não pode ser excluído");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }
    }
}
