using System.Collections.Generic;
using System.Text;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using System.Linq;
using System;

namespace Control.Facilites.Data.Utils
{
 

    public class CloudStorage : GeneralTransfer
    {
        public enum CloudType
        {
            AWS,
            Nexxera,
            Local
        }



        private CloudType DestinationType { get; set; }
        public CloudStorage()
        {

        }

        public void SetCloudType(CloudType _type)
        {
            DestinationType = _type;
        }

        public bool TransferFiles(string sourcePath, string destinationPath)
        {
            if (!String.IsNullOrEmpty(sourcePath) && !String.IsNullOrEmpty(destinationPath))
            {
                /*
                var cloudi = GetCloud(DestinationType);

                var _type = cloudi.GetType();
                var methodinfo = _type.GetMethod("Upload");
                methodinfo.Invoke(cloudi, new object[0]);

                //cloud.Upload(_sourcePath, _destinationPath);
                */
                try {
                    switch (DestinationType)
                    {
                        case CloudType.AWS:
                            return true;
                        case CloudType.Nexxera:
                            return true;
                        case CloudType.Local:
                            return true;
                        default:
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception("Erro ao fazer upload" + ex.Message);
                }
            }
            return false;
        }

    }
    
    public enum TransferOption
    {
        Files, Folder
    }
    public class LocalCloud : GeneralTransfer
    {
        public LocalCloud()
        {

        }

        public static bool Upload(string sourcePath, string destinationPath, TransferOption transferOption = TransferOption.Folder)
        {

            try
            {
                // se for uma pasta
                if (IsDirectory(sourcePath))
                {
                    if (System.IO.Directory.Exists(sourcePath))
                    {
                        //if (!System.IO.Directory.Exists(destinationPath))
                        //    System.IO.Directory.CreateDirectory(destinationPath);
                        if (transferOption == TransferOption.Folder)
                        {
                            //var _formattedDestination = System.IO.Path.Combine(destinationPath, System.IO.Path.GetFileNameWithoutExtension(sourcePath)) + Path.DirectorySeparatorChar;
                            var _formattedDestination = destinationPath;
                            //Console.WriteLine("Arquivo movido 2 -> Origem: {0} --- Destino: {1}", sourcePath, _formattedDestination);
                            System.IO.Directory.Move(sourcePath, _formattedDestination);
                        }
                        else
                        {
                            foreach (var _file in System.IO.Directory.GetFiles(sourcePath))
                            {
                                var _formattedDestination = Path.Combine(destinationPath, Path.GetFileName(_file));
                                //Console.WriteLine("Arquivo movido 3 -> Origem: {0} --- Destino: {1}", _file, _formattedDestination);
                                System.IO.File.Move(_file, _formattedDestination);
                            }
                        }
                        return true;
                    }
                }
                // se for um arquivo
                else
                {
                    var path = Path.GetDirectoryName(destinationPath);

                    if (File.Exists(sourcePath))
                    {
                        
                        if (!System.IO.Directory.Exists(destinationPath)) {
                            try { System.IO.Directory.CreateDirectory(path); }
                            catch {
                                Console.WriteLine("Erro ao criar arquivo: {0}", path);
                                return false;
                            }
                        }

                        //https://solucoes.controlnex.com.br/controlstorage/bosch/2/ded1d9f8-e5af-4561-990d-c55970e48f83.pdf
                        //var _formattedDestination = Path.Combine(destinationPath, Path.GetFileName(sourcePath));
                        if (File.Exists(destinationPath))
                            System.IO.File.Delete(destinationPath);
                        try {
                            System.IO.File.Copy(sourcePath, destinationPath);
                            //Console.WriteLine("Arquivo movido 1 -> origem: {0} --- destino: {1}", sourcePath, destinationPath);
                            }
                        catch (Exception e){
                            Console.WriteLine("Erro ao mover: " + e.Message);
                        }
                        
                        
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao mover os arquivos localmente!" + ex.Message + " --- " + ex.Source + " --- " + ex.HelpLink);
                //throw new Exception("Erro ao mover os arquivos localmente." + ex.Message);
                return false;
            }
        }

    }
    public class AWSUp
    {
        public AWSUp() { }
        public static string Upload(string sourcePath, string destinationPath,string s3url, string key, string secret, string bucket)
        {
            var segments = new Uri(destinationPath).Segments;

            var fileKey = destinationPath.Substring(destinationPath.IndexOf(segments[1].ToString())).Replace(segments[1].ToString(), "");
            Console.WriteLine("--- Enviando para AWS! Bucket name: " + bucket + " FileKey: " + fileKey + " - Link: " + destinationPath);

            //System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = s3url;
            
            var awsCred = new Amazon.Runtime.BasicAWSCredentials(key, secret);

            using (AmazonS3Client client = new AmazonS3Client(key,secret, config))
            {

                try
                {
                    //ListBucketsResponse response = client.ListBucketsAsync().GetAwaiter().GetResult();
                    //PutBucketRequest request1 = new PutBucketRequest();
                    //request1.BucketName = "control-contas-dev";
                    //client.PutBucketAsync(request1).GetAwaiter().GetResult();

                    var fileTransferUtility = new TransferUtility(client);

                    var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = bucket,
                        FilePath = sourcePath,
                        StorageClass = S3StorageClass.StandardInfrequentAccess,
                        Key = fileKey,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    //await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                    fileTransferUtility.UploadAsync(fileTransferUtilityRequest).GetAwaiter().GetResult();
                    Console.WriteLine("Enviado para S3: " + destinationPath);


                    GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                    request.BucketName = bucket;
                    request.Key = fileKey;
                    request.Protocol = Protocol.HTTPS;
                    string url = client.GetPreSignedURL(request);
                    return url;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    
                }
                return string.Empty;
            }


        }

    }
}
