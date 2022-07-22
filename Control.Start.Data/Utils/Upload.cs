using System;
using System.Collections.Generic;
using System.Text;
using Control.Facilites.Events;
using System.IO;
using Control.Facilites.Domain.Enum;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Events.Utils;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Control.Facilites.Events.Utils
{
    public class ManualUpload
    {
        public ManualUpload() { }

        //public static InvoiceResponse Send(string Source, Control.Facilites.Domain.Entities.FornecedorCliente ClientSupplier, RabbitMQConfig rabbitConfig, ContasConfig config)
        //{
        //    if (ClientSupplier != null && !String.IsNullOrEmpty(Source))
        //    {

        //        string Destination = General.GetPathToSave(ClientSupplier);

        //        List<InvoiceResponse> coletasManuais = new List<InvoiceResponse>();

        //        var uploadManualDeFatura = new InvoiceResponse();
        //        uploadManualDeFatura.Id = Guid.NewGuid().ToString();
        //        uploadManualDeFatura.IdClientProvider = ClientSupplier.FornecedorClienteId; // ID do FornecedorCliente; Obs: upload de fatura só poderá ser permitido para fornecedores cadastrados que possuem parser/template;
        //        uploadManualDeFatura.Fetched = true;
        //        uploadManualDeFatura.FileSourcePath = Source;
        //        uploadManualDeFatura.DirectoryToSave = Destination;
        //        uploadManualDeFatura.Parse = ClientSupplier.Parsear;
        //        uploadManualDeFatura.Post = true;
        //        uploadManualDeFatura.Invoices = new List<InvoiceOnline>();
        //        uploadManualDeFatura.Invoices.Add(new InvoiceOnline()
        //        {
        //            Downloaded = true,
        //            LocalPath = Source,

        //        });

        //        coletasManuais.Add(uploadManualDeFatura);

        //        AutomaticUpload.TransferFilesAndSendEventResponse(rabbitConfig,config, coletasManuais);

        //        return uploadManualDeFatura;
        //    }
        //    else
        //        return null;
        //}
    }
    public class AutomaticUpload
    {
        //public static string TransferFilesLocally(List<InvoiceResponse> InvoicesResponses)
        //{
        //    try
        //    {
        //        foreach (var response in InvoicesResponses)
        //        {
        //            if (response.Invoices.Count > 0)
        //            {
        //                foreach (var inv in response.Invoices)
        //                {
        //                    if (inv.Downloaded && !String.IsNullOrEmpty(inv.LocalPath))
        //                        inv.PublishedURL = Upload.File(inv.LocalPath, response.DirectoryToSave, true);
        //                }
        //            }
        //            else
        //            {
        //                //if (invoice.Fetched && invoice.InvoiceExists)
        //                if (response.FileSourcePath != null)
        //                {
        //                    foreach (var file in Directory.GetFiles(response.FileSourcePath))
        //                    {
        //                        var publishedURL = Upload.File(file, response.DirectoryToSave);
        //                        response.FilesURL.Add(publishedURL);
        //                        if (response.Invoices.Count > 0)
        //                            response.Invoices.Find(x => x.LocalPath == file).PublishedURL = publishedURL;
        //                    }
        //                }
        //            }
        //            if (response.ImgsURL != null && Directory.Exists(response.ImgsURL))
        //            {
        //                foreach (var file in Directory.GetFiles(response.ImgsURL))
        //                    response.ImgsURL = Upload.Screenshots(response.ImgsURL);
        //            }
        //        }
        //        //if (InvoicesResponses.Where(x => x.Post || x.Parse).Count() > 0)
        //            //EventMethods.Send(InvoicesResponses, Definitions.RabbitMQ.InvoicesResponsesQueue);

        //        return InvoicesResponses;
        //    }
        //    catch
        //    {
        //        return InvoicesResponses;
        //    }
        //}

    }

    public class Upload
    {




        public static string File(string sourceFile, string directoryPath = null, Documento notadebito = null, bool getLocalPath = false, ContasConfig config = null)
        {
            //https://solucoes.controlnex.com.br/controlstorage/faturas/8911900d-b2f2-4492-90a4-9b91bb4ede22.pdf



            string directoryToSave = Data.Utils.GeneralTransfer.GetPathToSave("", notadebito);
            // Create Destination Path
            string HostAddress = string.Empty;
            //if (config != null)
            //{
            //    if (config.StorageToUse == StorageType.AWS)
            //        HostAddress = config.S3Url;
            //    else if (config.StorageToUse == StorageType.Local)
            //        HostAddress = @"https://solucoes.controlnex.com.br";// config.LocalServerAddress;
            //}

            //HostAddress = @"https://solucoes.controlnex.com.br";// config.LocalServerAddress;
            HostAddress = @"https://notas.controlrpa.com.br";// config.LocalServerAddress;

            if (string.IsNullOrEmpty(directoryPath))
                directoryPath = GetPathToSave(notadebito);
            var Filename = System.IO.Path.GetFileName(sourceFile);
            if (!HostAddress.EndsWith("/") && !directoryPath.StartsWith("/"))
                HostAddress += "/";

            var DestinationFullPath = HostAddress + directoryPath + Filename;
            if (config != null)
            {
                //switch (config.StorageToUse)
                //{
                //    case StorageType.AWS:
                //        //AWSUp.Upload(sourceFile, DestinationFullPath,config.AWSKey,config.AWSSecret).GetAwaiter().GetResult();
                //        //return DestinationFullPath;
                //        return Data.Utils.AWSUp.Upload(sourceFile, DestinationFullPath, config.S3Url, config.S3Key, config.S3Secret, config.S3Bucket);
                //    case StorageType.Local:
                //        var localPath = Path.Combine(directoryPath, Filename);
                //        Data.Utils.LocalCloud.Upload(sourceFile, localPath);
                //        if (getLocalPath)
                //            return localPath;
                //        else
                //            return DestinationFullPath;
                //    default:
                //        return String.Empty;
                //}

                var localPath = Path.Combine(directoryPath, Filename);
                Data.Utils.LocalCloud.Upload(sourceFile, localPath);
                if (getLocalPath)
                    return localPath;
                else
                    return DestinationFullPath;
            }
            else
            {
                var localPath = Path.Combine(directoryPath, Filename);
                Data.Utils.LocalCloud.Upload(sourceFile, localPath);
                if (getLocalPath)
                    return localPath;
                else
                    return DestinationFullPath;
            }



        }
        public static string FormatImagesPath(string ZipFileSource)
        {
            if (String.IsNullOrEmpty(ZipFileSource))
                return String.Empty;

            if (!Control.Facilites.Data.Utils.GeneralTransfer.IsDirectory(ZipFileSource))
            {

                var imgsDir = Path.GetDirectoryName(ZipFileSource);
                if (imgsDir.EndsWith(Path.DirectorySeparatorChar))         
                    imgsDir = imgsDir.Substring(0, imgsDir.LastIndexOf(Path.DirectorySeparatorChar));

                imgsDir += "_imgs";
                return imgsDir;
            }
            else
                return ZipFileSource + "_imgs";
        }
        public static string GetStorageAddress(StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.AWS:
                    return Definitions.AWSAddress;
                case StorageType.Local:
                    return Definitions.LocalServerAddress;
                default:
                   return String.Empty;
            }            
        }

        public static string GetPathToSave(Documento fc)
        {
            string path = Definitions.ControlFolder;
            string defaultId = "0";

            // Caminho: /controlstorage/faturas/ParceiroId/ClienteId/TipoDeServiço/NomeDoFornecedor/NomeArquivo.pdf
            // Se não existir link entre fornecedorcliente ou não existir parceiro, é definido estes caminhos com o valor padrão 0.          
            if (fc != null && fc.EmpresaFornecedor != null)
                path += fc.EmpresaFornecedor.Id + "/";
            else
                path += defaultId + "/" + defaultId + "/"; // -> 0/0/
            return path;
        }

    }
}


