using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain.Entities
{
    public class DataTableResponse
    {
        public int draw { get; set; }
        public long recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object[] data { get; set; }
        public string error { get; set; }
    }

    public class DataTableResponse2
    {

        public int Count { get; set; }
        public dynamic Items { get; set; }
        public string error { get; set; }
    }

}
