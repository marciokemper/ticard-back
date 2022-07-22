using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.Domain
{
    public abstract class Identificador
    {
        public virtual int Id { get; set; }

        //public virtual bool HasId { get { return this.Id != 0; } }
    }

}
