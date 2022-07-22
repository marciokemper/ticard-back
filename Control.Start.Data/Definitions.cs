using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Control.Facilites.Events.Entities;


namespace Control.Facilites.Events
{
    public static class Definitions
    {
        
        

        public static bool Development = false;

        public static string TempDirectory { get; set; } = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "files"); // Diretorio temporario para baixar as faturas: /pasta_de_execucao/files/

        public static bool UseAWS { get; set; } = Development ? false : false;
        public static string AWSAddress { get; set; } = "https://s3.amazonaws.com"; // sem barra final

        public static string ControlFolder { get; set; } = @"/controlstorage/bosch/";

        public const Control.Facilites.Domain.Enum.StorageType StorageToUse  = Control.Facilites.Domain.Enum.StorageType.Local;
        
        public static string LocalServerAddress { get; set; } = "http://162.241.41.48"; // sem barra final

        //public static string UrlAPI { get; set; } = "https://localhost:44351/api/";

        public static string UrlAPI { get; set; } = "http://162.241.41.48:5000/api/";

        public static string CorreioAPI { get; set;  } = "http://localhost:6680/";

       

    }
}
