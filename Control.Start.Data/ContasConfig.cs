using System;
namespace Control.Facilites.Events.Entities
{
    public class ContasConfig
    {
        public string ControlStorageFolder { get; set; }
        public Control.Facilites.Domain.Enum.StorageType StorageToUse { get; set; }
        public string LocalServerAddress { get; set; }
        public string ContasAPIAddress { get; set; }
        public string ContasAPIUser { get; set; } 
        public string ContasAPIPass { get; set; }
        public string CorreioAPIAddress {get;set;}
        public bool SendToCorreio { get; set; }
        public int ParallelRobotsPerRequest { get; set; }
        public string S3Url { get; set; }
        public string S3Key { get; set; }
        public string S3Secret { get; set; }
        public string S3Bucket { get; set; }
        public bool Headless { get; set; }
        
    }
}
