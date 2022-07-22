using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using Control.Facilites.Domain.Entities;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Control.Facilites.Data.Utils
{
    public class GeneralTransfer
    {
        public static string RaizCNPJ(string documentNumber)
        {
            try
            {
                if (!documentNumber.Contains("/") || documentNumber.Count() < 9)
                    return documentNumber;
                return documentNumber.Substring(0, documentNumber.IndexOf("/"));
            }
            catch
            {
                return documentNumber;
            }
        }
     
        public static Domain.Enum.InvoiceStatus ParseInvoiceStatus(string status)
        {
            if (Regex.Match(status.Trim().ToLower(), "(pendente)|(em aberto)").Success)
                return Domain.Enum.InvoiceStatus.Pending;
            else if (Regex.Match(status.Trim().ToLower(), "pag[a|o]").Success)
                return Domain.Enum.InvoiceStatus.Paid;
            else if (Regex.Match(status.Trim().ToLower(), "contestad[a|o]").Success)
                return Domain.Enum.InvoiceStatus.Disputed;
            else if (Regex.Match(status.Trim().ToLower(), "isent[a|o]").Success)
                return Domain.Enum.InvoiceStatus.Exempt;
            else
                return Domain.Enum.InvoiceStatus.Unknown;
        }
        public static decimal DecimalParse(string text)
        {
            decimal value;
            Decimal.TryParse(text, out value);
            return value;
        }
        public static decimal DecimalParseFromBR2(string text)
        {
            decimal result;
            if (System.Text.RegularExpressions.Regex.Match(text, @"(-?(\d{1,}\.?)*\d{1,},\d{2})\b").Success)
                result = decimal.Parse(text.Replace(".", "").Replace(",", "."));
            else if (System.Text.RegularExpressions.Regex.Match(text, @"(-?(\d{1,}\,?)*\d{1,}.\d{2})\b").Success)
                result = decimal.Parse(text.Replace(",",""));
            else
                result = 0;
            return result;
        }
        public static decimal DecimalParseFromBR(string text)
        {
            string format = String.Empty;
            if (!String.IsNullOrEmpty(text) && (text.Length - 2 > 0) && (text[text.Length - 3] == ','))
                format = text.Replace(".", "").Replace(",", ".");
            else
                format = text;

            decimal value;
            Decimal.TryParse(format, out value);
            return value;
        }

        public static string FormatCNPJAndCPF(string documentNumber)
        {
            return documentNumber.Replace(".", "").Replace("/", "").Replace(" ", "").Replace("-", "");
        }
        public static bool IsZipped(string filePath)
        {
            try
            {
                using (var zipFile = ZipFile.OpenRead(filePath))
                {
                    var entries = zipFile.Entries;
                    return true;
                }
            }
            catch (InvalidDataException)
            {
                return false;
            }
        }
        public static bool UnzipAndRemove(string filePath, string destinationPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(filePath, destinationPath);
                System.IO.File.Delete(filePath);
                return true;
            }
            catch (InvalidDataException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool IsDirectory(string path)
        {
            try
            {
                return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
            }
            catch
            {
                return false;
            }
        }
        public static string ZipAndRemoveFiles(string pathDirectory)
        {
            string zipfile = ZipFiles(pathDirectory);
            foreach(var file in Directory.GetFiles(pathDirectory))
            {
                if (file!=zipfile)
                    File.Delete(file);
            }
            foreach (var dir in Directory.GetDirectories(pathDirectory))
                Directory.Delete(dir,true);
            return zipfile;
        }
        public static string ZipFiles (string pathDirectory)
        {
            try
            {
                var tempDir = Path.Combine(pathDirectory, "faturas");
                Directory.CreateDirectory(tempDir);
                foreach (var file in System.IO.Directory.GetFiles(pathDirectory))
                {
                    var newPath = Path.Combine(tempDir, Path.GetFileName(file));
                    File.Move(file, newPath);
                }
                string fileName = Guid.NewGuid().ToString() + ".zip";
                string zipFile = Path.Combine(pathDirectory, fileName);
                
                ZipFile.CreateFromDirectory(tempDir, zipFile);
                Directory.Delete(tempDir, true);
                return zipFile;
            }
            catch (Exception ex)
            {
                return "erro ao compactar arquivos " + ex.Message;
            }
        }
        public static string RenameFileToFoldersName(string sourceFile)
        {
            var foldersName = new System.IO.DirectoryInfo(Path.GetDirectoryName(sourceFile)).Name;
            var extension = Path.GetExtension(sourceFile);
            var newFileName = Path.Combine(Path.GetDirectoryName(sourceFile), foldersName+extension);
            System.IO.File.Move(sourceFile, newFileName);
            return newFileName;
        }
        public static string CheckIfDirectoryExists(string path)
        {
            try
            {
                System.IO.Directory.CreateDirectory(path);
                return path;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao criar diretório: " + e.Message);
            }
        }
        public static string GetPathToSave(Documento fc = null)
        {

            string aws = @"https://s3.amazonaws.com";
            string storageBasePath = @"/controlstorage/bosch";
            if (Events.Definitions.UseAWS)
                storageBasePath = aws + storageBasePath;
            string path = storageBasePath + "/";

            if (fc != null && fc.EmpresaFornecedor !=null)
            {
                path += fc.EmpresaFornecedor.Id + "/";
            }

            return path;
        }

        public static string GetPathToSave(string x1, Documento fc = null)
        {

            string aws = @"https://s3.amazonaws.com";
            string storageBasePath = @"/controlstorage/constrol_start";
            if (Events.Definitions.UseAWS)
                storageBasePath = aws + storageBasePath;
            string path = storageBasePath + "/";

            if (fc != null && fc.EmpresaFornecedor != null)
            {
                path += fc.EmpresaFornecedor.Id + "/";
            }

            return path;
        }

        public static string GetPathToSaveImages()
        {
            var basePathToSave = GetPathToSave(null);
            basePathToSave += "imgs/";
            return basePathToSave;
        }
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static string GetFileMD5(string sourceFile)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(sourceFile))
                    {
                        var hash = md5.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error trying to calculate file MD5 hash: " + ex.Message);
            }
        }
        public static string Base64Encode(string sourceFile)
        {
            try
            {
                if (!String.IsNullOrEmpty(sourceFile))
                {
                    Byte[] bytes = File.ReadAllBytes(sourceFile);
                    //var rawTextBytes = System.Text.Encoding.UTF8.GetBytes(rawText);
                    return System.Convert.ToBase64String(bytes);
                }
                else
                    throw new Exception("file data is null or empty.");
        }
            catch (Exception ex)
            {
                throw new Exception("Error trying to convert file do base64: " + ex.Message);
            }
        }
        public static string Base64Decode(string destinationPath, string base64Text)
        {
            try
            {
                if (String.IsNullOrEmpty(base64Text))
                {
                    Byte[] bytes = System.Convert.FromBase64String(base64Text);

                    File.WriteAllBytes(destinationPath, bytes);
                    return destinationPath;
                }
                else
                    throw new Exception("file data is null or empty.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error trying to decode file date: " + ex.Message);
            }
        }
        public static string CurrentPath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
        public static string GetBaseCustomFolderPath(string path)
        {
            return Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, path); // retorna combinacao de uma pasta dentro da pasta base do executavel
        }
        public static string CreatePath(string TempFolderName)
        {
            var tmpPlusUnique = Path.Combine(TempFolderName, Guid.NewGuid().ToString()); // cria combinação de caminho entre pasta temporaria e uma pasta com nome único
            return GetBaseCustomFolderPath(tmpPlusUnique); // retorna a união desses dois caminhos com o diretório base do executável
        }
        public static class FormatDate
        {

            public static DateTime TryParseFromBR(string date)
            {

                if (Regex.Match(date, @"[A-z]{3}\/\d{4}").Success) // Jan/2018
                    date = FormatMonth(date);

                string[] formats =  {
                    "MM/yyyy",
                    "MMM/yyyy",
                    "dd/MM/yyyy",
                    "dd/MM/yy",
                    "ddd MMM dd HH:mm:ss BRT yyyy",
                    "MM/dd/yyyy"
                };

                DateTime dateValue;

                try
                {
                    var ok = DateTime.TryParseExact(date, formats, new System.Globalization.CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out dateValue);
                    return dateValue;
                }
                catch
                {
                    throw new Exception("Erro ao interpretar a data!");
                }
            }

            

            public static string GetMonthName(DateTime date)
            {
                switch (date.Month)
                {
                    case 1:
                        return "Janeiro";
                    case 2:
                        return "Fevereiro";
                    case 3:
                        return "Março";
                    case 4:
                        return "Abril";
                    case 5:
                        return "Maio";
                    case 6:
                        return "Junho";
                    case 7:
                        return "Julho";
                    case 8:
                        return "Agosto";
                    case 9:
                        return "Setembro";
                    case 10:
                        return "Outubro";
                    case 11:
                        return "Novembro";
                    case 12:
                        return "Dezembro";
                    default:
                        return date.Month.ToString();
                }
            }
            public static string FormatMonth(string date)
            {
                if (date.Contains("Fev"))
                    return date.Replace("Fev", "Feb");
                else if (date.Contains("Abr"))
                    return date.Replace("Abr", "Apr");
                else if (date.Contains("Mai"))
                    return date.Replace("Mai", "May");
                else if (date.Contains("Ago"))
                    return date.Replace("Ago", "Aug");
                else if (date.Contains("Set"))
                    return date.Replace("Set", "Sep");
                else if (date.Contains("Out"))
                    return date.Replace("Out", "Oct");
                else if (date.Contains("Dez"))
                    return date.Replace("Dez", "Dec");
                else
                    return date;
            }
        }



    }
    public static class ObjectExtensions
    {
        public static T ToObject<T>(this IDictionary<string, string> source)
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                         .GetProperty(item.Key)
                         .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }

        public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return source.GetType().GetProperties(bindingAttr).ToDictionary
            (
                propInfo => propInfo.Name,
                propInfo => propInfo.GetValue(source, null)
            );

        }
    }
}