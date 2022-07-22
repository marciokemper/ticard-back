using Control.Facilites.AppServices.Dtos;
using Control.Facilites.AppServices.Interfaces;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Domain.Enum;
using Control.Facilites.Events.Entities;
using Control.Facilites.Extensions;
using Control.Facilites.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Control.Facilites.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class ArquivoController : Controller
    {
        private readonly IDocumentoAppService appService;
        private readonly DocumentoValidator validator;
        //private IConfiguration configuration;


        public ArquivoController(IDocumentoAppService appService, DocumentoValidator validator)
        {
            this.appService = appService;
            this.validator = validator;


            //this.configuration = iConfig;
        }

        [Route("GetAllByFilter")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<Documento>> GetAllByFilter(int? fornecedor, string cnpj, int? cliente, int? situacao)
        {
            var result = new Results.GenericResult<IEnumerable<Documento>>();

            try
            {
                result.Result = appService.GetAllByFilter(fornecedor, cnpj, cliente, situacao);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }


        [Route("UploadFile")]
        [HttpPost, DisableRequestSizeLimit]
        public Results.GenericResult<ActionResult> UploadFile(IFormCollection data)
        {
            var result = new Results.GenericResult<ActionResult>();

            try
            {

                int empresa_fornecedor = int.Parse(data["empresa_fornecedor"]);

                var _file = Request.Form.Files[0];
                string folderName = "uploadtmp";
                string newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
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

                    Documento docto = new Documento();
                    docto.Ativo = true;
                    docto.EmpresaFornecedor = new EmpresaFornecedor();
                    docto.EmpresaFornecedor.Id = empresa_fornecedor;
                    docto.DataInclusao = DateTime.Now;
                    docto.CaminhoAnexo = fullPath;
                    docto.SituacaoDocumento = FluxoDocumento.Aguardando;

                    appService.Add(docto);
                }

                result.Result = Json(new { fullPath = fullPath });
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        private string RenameFile(string nameFile)
        {
            if (string.IsNullOrEmpty(nameFile))
                return string.Empty;


            int position = nameFile.LastIndexOf(".");

            var myUniqueFileName = $@"{Guid.NewGuid()}.{nameFile.Substring(position + 1)}";

            return myUniqueFileName;



        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivoColetadoId"></param>
        /// <param name="situacao"></param>
        /// <returns></returns>
        [Route("UpdateSituacao")]
        [HttpPut, Authorize]
        public Results.GenericResult UpdateSituacao(string arquivoColetadoId, string situacao)
        {
            var result = new Results.GenericResult();

            try
            {
                result.Success = appService.UpdateSituacao(arquivoColetadoId, situacao);
                if (!result.Success)
                    throw new Exception($"Situação do arquivo coletado {arquivoColetadoId} Não pode ser alterado");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }


            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="situacao"></param>
        /// <returns></returns>
        [Route("UpdateByIdsSituacao")]
        [HttpPut, Authorize]
        public Results.GenericResult UpdateByIdsSituacao(string ids, string situacao)
        {
            var result = new Results.GenericResult();

            try
            {
                result.Success = appService.UpdateByIdsSituacao(ids, situacao);
                if (!result.Success)
                    throw new Exception($"Situação do arquivo coletado {ids} Não pode ser alterado(s)");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }


            return result;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivoColetadoId"></param>
        /// <param name="numero"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("UpdateDadosPedido")]
        [HttpPut, Authorize]
        public Results.GenericResult UpdateDadosPedido(string arquivoColetadoId, string numero, string data)
        {
            var result = new Results.GenericResult();


            try
            {
                result.Success = appService.UpdateDadosPedido(arquivoColetadoId, numero, data);
                if (!result.Success)
                    throw new Exception($"Dados do Pedido do arquivo coletado {arquivoColetadoId} Não pode ser alterado");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }


            return result;

        }


        [Route("UploadDocumentoFile")]
        [HttpPost, DisableRequestSizeLimit]
        public ActionResult UploadDocumentoFile()
        {
            try
            {

                var _file = Request.Form.Files[0];
                string folderName = "uploadtmp";

                string newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folderName);
                string fileName = string.Empty;
                string fullPath = string.Empty;

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (_file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(_file.ContentDisposition).FileName.Trim('"');
                    fullPath = Path.Combine(newPath, RenameFile(fileName));
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        _file.CopyTo(stream);

                    }
                }

                //Inserir

                return Json(new { fullPath = fullPath });
            }
            catch
            {
                return Json(new { fullPath = "" });
            }
        }


        [Route("AddManual")]
        [HttpPost]
        public Results.GenericResult<Documento> AddManual([FromBody] Documento model)
        {
            var result = new Results.GenericResult<Documento>();

            // model.CaminhoArquivo = Contas.Events.Utils.Upload.File(model.CaminhoArquivo, GetPathToSave(model.FkFornecedorClienteId));

            // model.CaminhoArquivo = MovePathToSave(model.CaminhoArquivo, model.FkFornecedorClienteId);

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

        [Route("SendEmailNotificacoes")]
        [HttpPost]
        public Results.GenericResult SendEmailNotificacoes()
        {
            var result = new Results.GenericResult();

            try
            {
                appService.SendEmailNotificacoes();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }


            return result;
        }

        

        private string GetDestinFile(string file, string newPath)
        {
            var _position = file.LastIndexOf(@"/");
            if (_position > 0)
            {
                return Path.Combine(newPath, file.Substring(_position + 1).Trim());
            }

            return string.Empty;
        }

        // private void CreateZipFile(string fileName, List<string> files)
        // {
        //     // Create and open a new ZIP file
        //     var zip = ZipFile.Open(fileName, ZipArchiveMode.Create);
        //     foreach (var file in files)
        //     {
        //         // Add the entry for each file
        //         zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
        //     }
        //     // Dispose of the object when we are done
        //     zip.Dispose();
        // }

        private static HttpClient Client { get; } = new HttpClient();


        private MemoryStream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new System.Net.WebClient())
                imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }

        private Tuple<string, string> GetFileInfo(string file)
        {
            var position = file.LastIndexOf(@"/");

            var filename = file.Substring(position + 1);
            position = filename.LastIndexOf(@".");

            var extension = filename.Substring(position + 1);

            return new Tuple<string, string>(filename, extension);
        }

        private List<SourceFile> ConvertToStream(List<dynamic> urls)
        {

            List<SourceFile> sourceFiles = new List<SourceFile>();
            Tuple<string, string> fileInfo;
            HttpWebRequest request;
            HttpWebResponse response;
            string _caminho = string.Empty;
            string _nome = string.Empty;
            string _nome2 = string.Empty;
            for (int i = 0; i < urls.Count; i++)
            {
                try
                {
                    _caminho = urls[i].GetType().GetProperty("Caminho").GetValue(urls[i]);
                    _nome = urls[i].GetType().GetProperty("Nome").GetValue(urls[i]);
                    _nome2 = urls[i].GetType().GetProperty("NomeArquivoManual").GetValue(urls[i]);

                    Console.WriteLine($"ConvertToStream - {_caminho}");
                    Console.WriteLine($"ConvertToStream - {_nome}");
                    Console.WriteLine($"ConvertToStream - {_nome2}");


                    request = (HttpWebRequest)WebRequest.Create(_caminho);
                    response = (HttpWebResponse)request.GetResponse();
                    MemoryStream mem = new MemoryStream();
                    Stream stream = response.GetResponseStream();

                    stream.CopyTo(mem, 4096);

                    fileInfo = GetFileInfo(_caminho);

                    sourceFiles.Add(new SourceFile
                    {
                        FileBytes = mem.ToArray(),
                        Extension = fileInfo.Item2,
                        Name = _nome,
                        Name2 = _nome2
                    });
                }
                catch
                {

                }

            }

            return sourceFiles;
        }

        private List<SourceFile> GetStreamFromUrl(List<string> urls)
        {
            List<SourceFile> sourceFiles = new List<SourceFile>();
            MemoryStream memoryStream;
            byte[] imageData = null;
            var wc = new System.Net.WebClient();
            Tuple<string, string> fileInfo;

            for (int i = 0; i < urls.Count; i++)
            {
                imageData = wc.DownloadData(urls[i]);

                memoryStream = new MemoryStream(imageData);

                fileInfo = GetFileInfo(urls[i]);

                sourceFiles.Add(new SourceFile
                {
                    FileBytes = imageData.ToArray(),
                    Extension = fileInfo.Item2,
                    Name = fileInfo.Item1
                });
            }

            return sourceFiles;

        }

        [Route("GetAllByFornecedor")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<dynamic>> GetAllByFornecedor(int? loggedFornecedorId, int? loggedEmpresaId)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();

            try
            {
                result.Result = appService.GetAllByFornecedor(loggedFornecedorId, loggedEmpresaId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        [Route("GetAllByOficina")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<dynamic>> GetAllByOficina(int oficina, string codigo)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();

            try
            {
                result.Result = appService.GetAllByOficina(oficina, codigo);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        [Route("ListSelecionadasByNotaDebito")]
        [HttpGet]
        public Results.GenericResult<List<Documento>> ListSelecionadasByNotaDebito(int? notadebito)
        {
            var result = new Results.GenericResult<List<Documento>>();

            try
            {

                result.Result = appService.ListByNotaDebito(notadebito);
                result.Success = true;

            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }


        [Route("DownloadArquivos")]
        [HttpGet, DisableRequestSizeLimit]
        public IActionResult DownloadArquivos(int? LoggedclienteId, string centrosCusto, int? clienteId, string vencimentode, string vencimentoate, string tipo, string palavra, string situacao)
        {
            Console.WriteLine($"GetCSVFilesDownload");
            var _files = GetCSVFilesDownload(LoggedclienteId, centrosCusto, clienteId, vencimentode, vencimentoate, tipo, palavra, situacao);

            Console.WriteLine($"ConvertToStream");
            List<SourceFile> sourceFiles = ConvertToStream(_files.Result);
            System.IO.Compression.ZipArchiveEntry zipItem;
            //List<SourceFile> sourceFiles = GetStreamFromUrl(_files.Result);

            byte[] fileBytes = null;

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                using (System.IO.Compression.ZipArchive zip = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
                {

                    foreach (SourceFile f in sourceFiles)
                    {
                        if (string.IsNullOrEmpty(f.Name2))
                            zipItem = zip.CreateEntry($"{f.Name}.{f.Extension}");
                        else
                            zipItem = zip.CreateEntry(f.Name2);

                        using (System.IO.MemoryStream originalFileMemoryStream = new System.IO.MemoryStream(f.FileBytes))
                        {

                            using (System.IO.Stream entryStream = zipItem.Open())
                            {
                                originalFileMemoryStream.CopyTo(entryStream);
                            }
                        }

                    }
                }
                fileBytes = memoryStream.ToArray();
            }

            // download the constructed zip
            Response.Headers.Add("Content-Disposition", "attachment; filename=faturas.zip");
            return File(fileBytes, "application/zip");

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


        /// <summary>
        /// Pesquisa os arquivos coletados
        /// </summary>
        /// <param name="filter">Campos para filtrar</param>
        /// <returns>Coleção de Arquivos coletados</returns>
        // GET: api/todo
        [HttpGet]
        public Results.GenericResult<List<Documento>> Get([FromQuery] DocumentoFilterDto filter)
        {
            var result = new Results.GenericResult<List<Documento>>();

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoggedclienteId"></param>
        /// <param name="centrosCusto"></param>
        /// <returns></returns>
        [Route("GetStatusArquivoColetado")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<dynamic>> Get(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();

            try
            {
                result.Result = appService.GetStatusArquivoColetado(LoggedclienteId, LoggedEmpresaId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoggedclienteId"></param>
        /// <param name="centrosCusto"></param>
        /// <returns></returns>
        [Route("GetValorTipoMes")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<dynamic>> GetValorTipoMes(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();

            try
            {
                result.Result = appService.GetValorTipoMes(LoggedclienteId, LoggedEmpresaId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        [Route("GetTipo")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<dynamic>> GetTipo(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();

            try
            {
                result.Result = appService.GetValorTipo(LoggedclienteId, LoggedEmpresaId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoggedclienteId"></param>
        /// <param name="centrosCusto"></param>
        /// <returns></returns>
        [Route("GetNotificacoes")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<dynamic>> GetNotificacoes(int? LoggedclienteId, string centrosCusto)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();

            try
            {
                result.Result = appService.GetNotificacoes(LoggedclienteId, centrosCusto);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        [Route("GetMesesByLoggedclienteId")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<Documento>> GetMesesByLoggedclienteId(int? LoggedclienteId, string centrosCusto)
        {
            var result = new Results.GenericResult<IEnumerable<Documento>>();

            try
            {
                result.Result = appService.GetMesesByLoggedclienteId(LoggedclienteId, centrosCusto);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoggedclienteId"></param>
        /// <param name="centrosCusto"></param>
        /// <returns></returns>
        [Route("GetUltimosArquivoColetado")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<dynamic>> GetUltimosArquivoColetado(int? LoggedclienteId, int? LoggedEmpresaId)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();

            try
            {
                result.Result = appService.GetUltimosArquivoColetado(LoggedclienteId, LoggedEmpresaId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoggedclienteId"></param>
        /// <param name="LoggedEmpresaId"></param>
        /// <param name="typeInvoice"></param>
        /// <returns></returns>
        [Route("GetValorPorMes")]
        [HttpGet]
        public Results.GenericResult<IEnumerable<dynamic>> GetValorPorMes(int? LoggedclienteId, int? LoggedEmpresaId, int typeInvoice)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();

            try
            {
                result.Result = appService.GetValorPorMes(LoggedclienteId, LoggedEmpresaId, typeInvoice);
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
        public Results.GenericResult<Documento> Incluir([FromBody] Documento model)
        {
            var result = new Results.GenericResult<Documento>();

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
        /// Pesquisa um arquivo coletado de acordo com o parametro
        /// </summary>
        /// <param name="id">Chave do arquivo coletado</param>
        /// <returns>Arquivo Coletado</returns>
        // GET api/todo/5
        [HttpGet("{id}")]
        public Results.GenericResult<Documento> Get(int id)
        {
            var result = new Results.GenericResult<Documento>();

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

        /// <summary>
        /// Adiciona o arquivo coletado 
        /// </summary>
        /// <param name="model">Arquivo coletado a ser inserido</param>
        /// <returns>Arquivo coletado com o Id gerado</returns>
        [Route("Add")]
        [HttpPost]
        public Results.GenericResult<Documento> Post([FromBody] Documento model)
        {
            var result = new Results.GenericResult<Documento>();

            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid)
            {
                try
                {
                    Console.Error.WriteLine("-------------------- Antes GetConfig");
                    var x1 = GetConfig();
                    Console.Error.WriteLine("-------------------- Depois GetConfig");
                    result.Result = appService.Add(x1, model);
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

        //private string GetPathToSave(FornecedorCliente fc = null)
        //{

        //    //string aws = @"https://s3.amazonaws.com";
        //    string storageBasePath = @"/controlstorage/faturas";

        //    string path = storageBasePath + "/";

        //    if (fc != null && !String.IsNullOrEmpty(fc.FkFornecedorId.FkTipoFornecedorId.TipoFornecedorNome) && !String.IsNullOrEmpty(fc.FkFornecedorId.FornecedorNome))
        //    {
        //        // Se houver Parceiro, adiciona o ID do Parceiro no caminho de armazenamento, cc 0 como padrão.
        //        if (fc.FkClienteId != null && fc.FkClienteId.FkParceiroId != null && fc.FkClienteId.FkParceiroId.ParceiroId != 0)
        //            path += fc.FkClienteId.FkParceiroId.ParceiroId + "/";
        //        else
        //            path += "0/";

        //        path += fc.FkClienteId.ClienteId + "/"
        //             + fc.FkFornecedorId.FkTipoFornecedorId.TipoFornecedorNome.ToLower() + "/"
        //             + fc.FkFornecedorId.FornecedorNome.ToLower() + "/";
        //    }

        //    return path;
        //}

        /// <summary>
        /// Artera o arquivo coletado
        /// </summary>
        /// <param name="id">Id arquivo coletado a ser alterado</param>
        /// <param name="model">Novos dados do arquivo coletado</param>
        /// <returns>Arquivo coletado alterado</returns>
        [HttpPut("{id}"), Authorize]
        public Results.GenericResult Put(int id, [FromBody] Documento model)
        {
            var result = new Results.GenericResult();

            var validatorResult = validator.Validate(model);
            if (validatorResult.IsValid)
            {
                try
                {
                    result.Success = appService.Update(model);
                    if (!result.Success)
                        throw new Exception($"Nota Fiscal {id} não pode ser alterado");
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

        private Domain.Entities.ContasConfig GetConfig()
        {
            //var environmentName = Environment.GetEnvironmentVariable("CURRENT_ENVIRONMENT");
            //if (string.IsNullOrWhiteSpace(environmentName))
            //    environmentName = "prod";

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile($"eventsappsettings.prod.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables().Build();


            var contasConfig = new Domain.Entities.ContasConfig()
            {
                ContasAPIAddress = config.GetSection("ContasConfig").GetSection("ContasAPIAddress").Value,
                ContasAPIUser = config.GetSection("ContasConfig").GetSection("ContasAPIUser").Value,
                ContasAPIPass = config.GetSection("ContasConfig").GetSection("ContasAPIPass").Value,
                CorreioAPIAddress = config.GetSection("ContasConfig").GetSection("CorreioAPIAddress").Value,
                LocalServerAddress = config.GetSection("ContasConfig").GetSection("LocalServerAddress").Value,
                ControlStorageFolder = config.GetSection("ContasConfig").GetSection("ControlStorageFolder").Value,
                ParallelRobotsPerRequest = Int32.Parse(config.GetSection("ContasConfig").GetSection("ParallelRobotsPerRequest").Value),

                StorageToUse = (Domain.Enum.StorageType)Int32.Parse(config.GetSection("ContasConfig").GetSection("StorageToUse").Value),
                S3Url = config.GetSection("ContasConfig").GetSection("S3Url").Value,
                S3Key = config.GetSection("ContasConfig").GetSection("S3Key").Value,
                S3Secret = config.GetSection("ContasConfig").GetSection("S3Secret").Value,
                S3Bucket = config.GetSection("ContasConfig").GetSection("S3Bucket").Value,
                Headless = bool.Parse(config.GetSection("ContasConfig").GetSection("Headless").Value),
                SendToCorreio = bool.Parse(config.GetSection("ContasConfig").GetSection("SendToCorreio").Value)
            };

            return contasConfig;
        }


        /// <summary>
        /// Remove o arquivo coletado
        /// </summary>
        /// <param name="id">Id do arquivo coletado a ser removido</param>
        /// <returns></returns>
        [HttpDelete("{id}"), Authorize]
        public Results.GenericResult Delete(int id)
        {
            var result = new Results.GenericResult();

            try
            {
                result.Success = appService.Remove(id);
                if (!result.Success)
                    throw new Exception($"Arquivo Coletado {id} não pode ser excluído");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        private bool ValidaFiltros(string centrosCusto, int? clienteId, string vencimentode, string vencimentoate, string tipo, string palavra, string situacao)
        {

            if (!string.IsNullOrEmpty(centrosCusto))
                return true;

            if (clienteId != null)
                return true;


            if ((!string.IsNullOrEmpty(vencimentode) && vencimentode != "undefined") || ((!string.IsNullOrEmpty(vencimentoate) && vencimentoate != "undefined")))
                return true;

            if (!string.IsNullOrEmpty(situacao) && situacao != "undefined")
                return true;


            if (!string.IsNullOrEmpty(tipo) && tipo != "undefined")
                return true;

            if (!string.IsNullOrEmpty(palavra) && palavra != "undefined")
                return true;

            return false;
        }

        [HttpPost]
        public DataTableResponse Post(int? loggedfornecedorId, int? loggedEmpresaId, string palavra, [FromBody] DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<Documento>>();

            try
            {
                var start = filter.start;
                var length = filter.length;
                int pageSize = length != 0 ? Convert.ToInt32(length) : 0;

                int skip = start != 0 ? Convert.ToInt32(start) : 0;

                result.Result = appService.List(loggedfornecedorId, loggedEmpresaId, palavra, filter);
                result.Success = true;

                var _return = new DataTableResponse
                {
                    recordsTotal = result.Result.Count(),
                    recordsFiltered = result.Result.Count(),
                    data = result.Result.Skip(skip).Take(pageSize).ToArray(),
                    error = string.Empty
                };

                return _return;

            }
            catch (Exception ex)
            {
                var _return = new DataTableResponse
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<Documento>().ToArray(),
                    error = ex.Message
                };

                return _return;
            }



        }

        [Route("GetByMes")]
        [HttpPost]
        public DataTableResponse GetByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, [FromBody] DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<dynamic>>();

            try
            {
                var start = filter.start;
                var length = filter.length;
                int pageSize = length != 0 ? Convert.ToInt32(length) : 0;

                int skip = start != 0 ? Convert.ToInt32(start) : 0;

                result.Result = appService.ListByMes(LoggedclienteId, ano, mes, cliente, emissor, situacao, filter);
                result.Success = true;

                var _return = new DataTableResponse
                {
                    recordsTotal = result.Result.Count(),
                    recordsFiltered = result.Result.Count(),
                    data = result.Result.Skip(skip).Take(pageSize).ToArray(),
                    error = string.Empty
                };

                return _return;

            }
            catch (Exception ex)
            {
                var _return = new DataTableResponse
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<Documento>().ToArray(),
                    error = ex.Message
                };

                return _return;
            }



        }

        [Route("GetResumoByMes")]
        [HttpPost]
        public DataTableResponse GetResumoByMes(int? LoggedclienteId, string ano, string mes, int? cliente, int? emissor, string situacao, [FromBody] DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<dynamic>>();

            try
            {
                //var start = filter.start;
                //var length = filter.length;
                //int pageSize = length != 0 ? Convert.ToInt32(length) : 0;

                //int skip = start != 0 ? Convert.ToInt32(start) : 0;

                result.Result = appService.ListResumoByMes(LoggedclienteId, ano, mes, cliente, emissor, situacao, filter);
                result.Success = true;

                var _return = new DataTableResponse
                {
                    recordsTotal = result.Result.Count(),
                    recordsFiltered = result.Result.Count(),
                    data = result.Result.Skip(0).Take(10).ToArray(),
                    error = string.Empty
                };

                return _return;

            }
            catch (Exception ex)
            {
                var _return = new DataTableResponse
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<Documento>().ToArray(),
                    error = ex.Message
                };

                return _return;
            }



        }

        [Route("GetByFornecedorCliente")]
        [HttpPost]
        public DataTableResponse GetByFornecedorCliente(int fornecedorClienteId, string ano, string mes, [FromBody] DataTableListaDto filter)
        {
            var result = new Results.GenericResult<List<Documento>>();

            try
            {

                var start = filter == null ? 0 : filter.start;
                var length = filter == null ? 10 : filter.length;
                int pageSize = length != 0 ? Convert.ToInt32(length) : 0;

                int skip = start != 0 ? Convert.ToInt32(start) : 0;

                result.Result = appService.ListByMesFornecedorCliente(fornecedorClienteId, ano, mes, filter);
                result.Success = true;

                var _return = new DataTableResponse
                {
                    recordsTotal = result.Result.Count(),
                    recordsFiltered = result.Result.Count(),
                    data = result.Result.Skip(skip).Take(pageSize).ToArray(),
                    error = string.Empty
                };

                return _return;

            }
            catch (Exception ex)
            {
                var _return = new DataTableResponse
                {
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<Documento>().ToArray(),
                    error = ex.Message
                };

                return _return;
            }



        }


        [Route("GetBy")]
        [HttpGet]
        public object GetBy(int? loggedclienteId, string centrosCusto)
        {

            var queryString = Request.Query;
            var result = new Results.GenericResult<List<dynamic>>();
            try
            {

                var data = appService.List(loggedclienteId, centrosCusto);
                string sort = queryString["$orderby"];
                string filter = queryString["$filter"];


                if (sort != null) //Sorting
                {
                    //switch (sort)
                    //{
                    //    case "parceiroId":
                    //        if (sort.Substring(sort.IndexOf(' ') + 1) != null)
                    //            data = data.OrderByDescending(x => x.ParceiroId).ToList();
                    //        else
                    //            data = data.OrderBy(x => x.ParceiroId).ToList();
                    //        break;
                    //    case "parceiroNome":
                    //        if (sort.Substring(sort.IndexOf(' ') + 1) != null)
                    //            data = data.OrderByDescending(x => x.ParceiroNome).ToList();
                    //        else
                    //            data = data.OrderBy(x => x.ParceiroNome).ToList();
                    //        break;
                    //}
                }
                else
                    data = data.OrderByDescending(x => x.DtVencimento).ToList();

                if (filter != null)
                {
                    var newfiltersplits = filter;
                    var filtersplits = newfiltersplits.Split('(', ')', ' ');
                    var filterfield = filtersplits[4].Replace(",", "");
                    var filtervalue = filtersplits[2].Replace("'", ""); //'gene',tolower
                    filtervalue = filtervalue.Substring(0, filtervalue.IndexOf(","));
                    //if (filtersplits.Length != 5)
                    //{
                    //    filterfield = filter.Split('(', ')', '\'')[3];
                    //    filtervalue = filter.Split('(', ')', '\'')[5];
                    //}
                    //switch (filterfield)
                    //{


                    //    case "parceiroid":

                    //        data = (from cust in data
                    //                where cust.ParceiroId.ToString() == filtervalue.ToString()
                    //                select cust).ToList();
                    //        break;
                    //    case "parceironome":
                    //data = (from cust in data
                    //        where cust.ParceiroNome.ToLower().StartsWith(filtervalue.ToString())
                    //        select cust).ToList();
                    //        break;
                    //}
                }

                int skip = Convert.ToInt32(queryString["$skip"]);
                int take = Convert.ToInt32(queryString["$top"]);
                result.Count = data.Count();
                if (take != 0)
                    result.Result = data.Skip(skip).Take(take).ToList();
                else
                    result.Result = data;
                //result.Result =  take != 0 ? new { Items = data.Skip(skip).Take(take).ToList(), Count = data.Count() } : new { Items = data, Count = data.Count() };


                //result.Result = appService.GetAll(null);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }


        [Route("GetCSVFilesDownload")]
        [HttpGet]
        public Results.GenericResult<List<dynamic>> GetCSVFilesDownload(int? LoggedclienteId, string centrosCusto, int? clienteId, string vencimentode, string vencimentoate, string tipo, string palavra, string situacao)
        {
            var result = new Results.GenericResult<List<dynamic>>();

            try
            {

                //if (!ValidaFiltros(centrosCusto, clienteId, vencimentode, vencimentoate, tipo, fornecedor, cliente, situacao))
                //    throw new Exception("Obrigatório pelo menos a definição de um filtro");

                result.Result = appService.GetCSVFilesDownload(LoggedclienteId, centrosCusto, clienteId, vencimentode, vencimentoate, tipo, palavra, situacao);
                result.Success = true;

                return result;

            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
                result.Success = false;
                return result;
            }



        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteId"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        [HttpGet("GetFilesSince/{clienteId}/{startDate}")]
        public Results.GenericResult GetFilesSince(int? clienteId, DateTime startDate)
        {
            var result = new Results.GenericResult<List<Documento>>();

            try
            {

                result.Result = appService.List(clienteId, startDate);
                result.Success = true;

                return result;

            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return null;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fornecedorclienteid"></param>
        /// <param name="dtVencimento"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        [HttpGet("ValidarItemIncluido")]
        public Results.GenericResult<IEnumerable<Documento>> ValidarItemIncluido(int fornecedorclienteid, string dtVencimento, decimal total)
        {
            var result = new Results.GenericResult<IEnumerable<Documento>>();

            try
            {
                Console.WriteLine($" Executar a consulta");
                result.Result = appService.ValidarItemIncluido(fornecedorclienteid, dtVencimento, total);

                Console.WriteLine($" Resultado");
                if (result.Result.Count() > 0)
                {
                    result.Errors = new string[] { "Fatura já cadastrada." };
                    result.Success = false;
                }
                else
                    result.Success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" - Erro ValidarItemIncluido {ex.Message}");
                result.Errors = new string[] { ex.Message };
            }

            Console.WriteLine($" {result.Errors}");
            Console.WriteLine($" {result.Success}");

            return result;
        }


        [Route("ClearLogExecute")]
        [HttpPost]
        public Results.GenericResult ClearLogExecute(int fornecedorclienteid)
        {
            var result = new Results.GenericResult();


            try
            {
                result.Success = appService.ClearLogExecute(fornecedorclienteid);
                if (!result.Success)
                    throw new Exception($"Erro ao limpar log");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }


            return result;

        }

        [Route("SetarSituacaoPaid")]
        [HttpPost]
        public Results.GenericResult SetarSituacaoPaid(int fornecedorclienteid)
        {
            var result = new Results.GenericResult();


            try
            {
                result.Success = appService.SetarSituacaoPaid(fornecedorclienteid);
                if (!result.Success)
                    throw new Exception($"Erro ao mudar situação");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }


            return result;

        }



        [HttpGet("ValidarItemIncluidoByReferencia")]
        public Results.GenericResult<IEnumerable<Documento>> ValidarItemIncluidoByReferencia(int fornecedorclienteid, string referenceDate, decimal total)
        {
            var result = new Results.GenericResult<IEnumerable<Documento>>();

            try
            {
                result.Result = appService.ValidarItemIncluidoByReferencia(fornecedorclienteid, referenceDate, total);

                if (result.Result.Count() > 0)
                {
                    result.Errors = new string[] { "Fatura já cadastrada." };
                    result.Success = false;
                }
                else
                    result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteId"></param>
        /// <param name="fornecedorId"></param>
        /// <param name="dtVencimento"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        [HttpGet("ValidarItemIncluidoManual")]
        public Results.GenericResult<IEnumerable<Documento>> ValidarItemIncluidoManual(int clienteId, int fornecedorId, string dtVencimento, decimal total)
        {
            var result = new Results.GenericResult<IEnumerable<Documento>>();

            try
            {
                result.Result = appService.ValidarItemIncluidoManual(clienteId, fornecedorId, dtVencimento, total);

                if (result.Result.Count() > 0)
                {
                    result.Errors = new string[] { "Fatura já cadastrada." };
                    result.Success = false;
                }
                else
                    result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        /// <summary>
        /// Retorna uma coleção de documentos de acordo com os parametros informados
        /// </summary>
        /// <param name="datade">Data Inicial de Vencimento</param>
        /// <param name="dataate">Data Final de Vencimento</param>
        /// <param name="emissor">Nome do Emissor. (ex.: COPEL)</param>
        /// <param name="cnpj">CPNJ do Cliente</param>
        /// <returns></returns>
        [Route("get_invoices")]
        [HttpGet, Authorize]
        //[HttpGet]
        public Results.GenericResult<IEnumerable<dynamic>> get_invoices(string datade, string dataate, string emissor, string cnpj)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();
            DateTime _datade = Convert.ToDateTime(datade);
            DateTime _dataate = Convert.ToDateTime(dataate);

            try
            {
                result.Result = appService.GetDocumentos(_datade, _dataate, emissor, cnpj);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggedclienteId"></param>
        /// <returns></returns>
        [Route("GetCalendario")]
        [HttpGet, Authorize]
        public Results.GenericResult<IEnumerable<dynamic>> GetCalendario(int? loggedclienteId, int? LoggedEmpresaId)
        {
            var result = new Results.GenericResult<IEnumerable<dynamic>>();

            try
            {
                result.Result = appService.GetCalendario(loggedclienteId, LoggedEmpresaId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        [Route("aprovarDocumento")]
        [HttpPut, Authorize]
        public Results.GenericResult aprovarDocumento([FromBody] Documento model)
        {
            var result = new Results.GenericResult();

            try
            {
                result.Success = appService.AprovarDocumento(model);
                if (!result.Success)
                    throw new Exception($"Nota fiscal Não pode ser aprovada");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;

        }

        [Route("validarNF")]
        [HttpPut, Authorize]
        public Results.GenericResult validarNF([FromBody] Documento model)
        {
            var result = new Results.GenericResult();

            try
            {
                result.Success = appService.ValidarNotaFiscal(model);
                if (!result.Success)
                    throw new Exception($"Nota fiscal não pode ser validada");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;

        }

        [Route("rejeitarDocumento")]
        [HttpPut, Authorize]
        public Results.GenericResult rejeitarDocumento([FromBody] Documento model)
        {
            var result = new Results.GenericResult();

            try
            {
                result.Success = appService.RejeitarDocumento(model);
                if (!result.Success)
                    throw new Exception($"Nota fiscal Não pode ser aprovada");
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;

        }

        [Route("GetRelatorio")]
        [HttpGet, DisableRequestSizeLimit]
        public string GetRelatorio(int dataInicialDia, int dataInicialMes, int dataInicialAno, int dataFinalDia, int dataFinalMes, int dataFinalAno, TipoDocumento tipoRelatorio)
        {
            StringBuilder sb = new StringBuilder();

            DateTime dataInicial = new DateTime(dataInicialAno, dataInicialMes, dataInicialDia, 0, 0, 0);
            DateTime dataFinal = new DateTime(dataFinalAno, dataFinalMes, dataFinalDia, 23, 59, 59);

            DataTable dt = BuscarDados(dataInicial, dataFinal, tipoRelatorio);

            // cabecalho
            var columns = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            sb.Append(string.Join(",", columns));
            sb.AppendLine();

            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                    sb.Append(FormatarCSV(dr[dc.ColumnName].ToString()) + ",");
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        DataTable BuscarDados(DateTime dataInicial, DateTime dataFinal, TipoDocumento tipoRelatorio)
        {
            //List<Relatorio> listaRelatorio = new List<Relatorio>();

            //switch (tipoRelatorio)
            //{
            //    case TipoDocumento.Pecas:
            //        listaRelatorio = appService.GetDadosRelatorioProdutos(dataInicial, dataFinal, tipoRelatorio);
            //        break;

            //    case TipoDocumento.Servicos:
            //        listaRelatorio = appService.GetDadosRelatorioServicos(dataInicial, dataFinal, tipoRelatorio);
            //        break;

            //    case TipoDocumento.Ambos:
            //        listaRelatorio = appService.GetDadosRelatorioTotal(dataInicial, dataFinal, tipoRelatorio);
            //        break;

            //    default:
            //        break;
            //}

            //DataTable dt = new DataTable();

            //dt.Columns.Add("Placa", typeof(string));
            //dt.Columns.Add("Código do orçamento", typeof(string));
            //dt.Columns.Add("Data de Inclusão", typeof(DateTime));
            //dt.Columns.Add("Vencimento", typeof(DateTime));
            //dt.Columns.Add("Razão Social Cliente", typeof(string));
            //dt.Columns.Add("Razão Social Fornecedor", typeof(string));
            //dt.Columns.Add("Valor", typeof(decimal));

            //foreach (var relatorio in listaRelatorio)
            //{
            //    dt.Rows.Add(relatorio.Placa,
            //                relatorio.NumeroOrcamento,
            //                relatorio.DataInclusao,
            //                relatorio.Vencimento,
            //                relatorio.RazaoSocialCliente,
            //                relatorio.RazaoSocialFornecedor,
            //                relatorio.Valor);
            //}

            //return dt;

            return null;
        }

        public static string FormatarCSV(string input)
        {
            try
            {
                if (input == null)
                    return string.Empty;

                bool containsQuote = false;
                bool containsComma = false;
                int len = input.Length;
                for (int i = 0; i < len && (containsComma == false || containsQuote == false); i++)
                {
                    char ch = input[i];
                    if (ch == '"')
                        containsQuote = true;
                    else if (ch == ',')
                        containsComma = true;
                }

                if (containsQuote && containsComma)
                    input = input.Replace("\"", "\"\"");

                if (containsComma)
                    return "\"" + input + "\"";
                else
                    return input;
            }
            catch
            {
                throw;
            }
        }


    }

    public class SourceFile
    {
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Extension { get; set; }
        public Byte[] FileBytes { get; set; }
    }

}
