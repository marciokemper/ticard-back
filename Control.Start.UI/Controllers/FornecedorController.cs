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
    public class FornecedorController : Controller
    {
        private readonly IFornecedorAppService appService;
        private readonly FornecedorValidator validator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appService"></param>
        /// <param name="validator"></param>
        public FornecedorController(IFornecedorAppService appService, FornecedorValidator validator)
        {
            this.appService = appService;
            this.validator = validator;
        }

        /// <summary>
        /// Faz o upload de arquivo coletao
        /// </summary>
        /// <returns></returns>
        [Route("UploadFile")]
        [HttpPost, DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            try
            {

                var _file = Request.Form.Files[0];
                string folderName = "uploadpdfimg";
                //string webRootPath = _hostingEnvironment.WebRootPath;
                //string newPath = Path.Combine(webRootPath, folderName);
                string newPath = Path.Combine("C:\\Projetos\\ControlContas\\Control.Facilites.UI\\wwwroot", folderName);
                //string newPath = "C:\\Projetos\\ControlContas\\Control.Facilites.UI\\wwwroot\\uploadtmp";
                string fileName = string.Empty;
                string fullPath = string.Empty;

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (_file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(_file.ContentDisposition).FileName.Trim('"');
                    fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        _file.CopyTo(stream);

                    }
                }
                ConvertPdfToImage(fullPath);

                TryToDelete(fullPath);


                return Json("Upload realizado com sucesso.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload falhou: " + ex.Message);
            }
        }

        [Route("ListNaoSelecionadasByEmpresa")]
        [HttpGet]
        public Results.GenericResult<List<Fornecedor>> ListNaoSelecionadasByEmpresa(int empresaId)
        {
            var result = new Results.GenericResult<List<Fornecedor>>();

            try
            {

                result.Result = appService.ListNaoSelecionadas(empresaId);
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        [Route("ListSelecionadasByEmpresa")]
        [HttpGet]
        public Results.GenericResult<List<Fornecedor>> ListSelecionadasByEmpresa(int empresaId)
        {
            var result = new Results.GenericResult<List<Fornecedor>>();

            try
            {

                result.Result = appService.ListSelecionadas(empresaId);
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        private void ConvertPdfToImage(string fullPath)
        {

            try
            {
                PdfDocument doc = new PdfDocument();
                doc.LoadFromFile(fullPath);
                Image bmp = doc.SaveAsImage(0);
                Image emf = doc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Metafile);
                Image zoomImg = new Bitmap((int)(emf.Size.Width * 2), (int)(emf.Size.Height * 2));
                using (Graphics g = Graphics.FromImage(zoomImg))
                {
                    g.ScaleTransform(2.0f, 2.0f);
                    g.DrawImage(emf, new Rectangle(new Point(0, 0), emf.Size), new Rectangle(new Point(0, 0), emf.Size), GraphicsUnit.Pixel);
                }
                bmp.Save(fullPath.Replace(".pdf",".jpg"), ImageFormat.Jpeg);
            }catch
            {

            }
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
        public DataTableResponse Post(int? loggedEmpresaId, [FromBody]DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<Fornecedor>>();

            try
            {
                var start = filter.start;
                var length = filter.length;
                int pageSize = length != 0 ? Convert.ToInt32(length) : 0;

                int skip = start != 0 ? Convert.ToInt32(start) : 0;

                result.Result = appService.ListByEmpresa(filter, loggedEmpresaId);
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

        [Route("GetAllByFilter")]
        [HttpGet, Authorize]
        public Results.GenericResult<List<Fornecedor>> GetAllByFilter(string filter)
        {
            var result = new Results.GenericResult<List<Fornecedor>>();

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

        //[Route("GetAll")]
        //[HttpGet, Authorize]
        //public Results.GenericResult<List<Fornecedor>> GetAll([FromQuery]FornecedorFilterDto filter)
        //{
        //    var result = new Results.GenericResult<List<Fornecedor>>();

        //    try
        //    {
        //        result.Result = appService.GetAll(filter);
        //        result.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Errors = new string[] { ex.Message };
        //    }

        //    return result;
        //}

        [Route("getAllByAtivo")]
        [HttpGet]
        public Results.GenericResult<List<Fornecedor>> getAllByAtivo()
        {
            var result = new Results.GenericResult<List<Fornecedor>>();

            try
            {

                result.Result = appService.getAllByAtivo();
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }


        // GET api/todo/5
        [HttpGet("{id}")]
        public Results.GenericResult<Fornecedor> Get(int id)
        {
            var result = new Results.GenericResult<Fornecedor>();

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
        public Results.GenericResult<Fornecedor> Incluir([FromBody]Fornecedor model)
        {
            var result = new Results.GenericResult<Fornecedor>();

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

        [Route("GetAllByFornecedor")]
        [HttpGet, Authorize]
        public Results.GenericResult<List<Fornecedor>> GetAllByFornecedor(int? loggeduserId)
        {
            var result = new Results.GenericResult<List<Fornecedor>>();

            try
            {
               // result.Result = appService.ListaUsuariosByFornecedor(loggeduserId);
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
        public Results.GenericResult Put(int id, [FromBody]Fornecedor model)
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
