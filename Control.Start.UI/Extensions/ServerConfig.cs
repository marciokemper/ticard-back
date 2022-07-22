using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Control.Facilites.Extensions
{
    public class ServerConfig
    {
        public MongoDBConfig MongoDB { get; set; } = new MongoDBConfig();
    }
}
