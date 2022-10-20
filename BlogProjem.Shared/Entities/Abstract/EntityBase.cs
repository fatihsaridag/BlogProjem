﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Shared.Entities.Abstract
{
    public abstract class EntityBase
    {
        public virtual int Id { get; set; } 
        public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual DateTime ModifiedDate { get; set; } = DateTime.Now;
        public virtual string ModifiedByName { get; set; } = "Admin";
        public virtual string CreatedByName { get; set; } = "Admin";
        public virtual bool IsActive { get; set; } = true;
        public virtual bool IsDeleted { get; set; } = false;
        public virtual string Note { get; set; }

    }
}
