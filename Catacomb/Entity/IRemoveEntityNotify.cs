﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public interface IRemoveEntityNotify
    {
        public void RemoveEntityNotice(Entity e);
    }
}
