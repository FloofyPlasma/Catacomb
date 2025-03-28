using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Math
{
    public interface IBBPredicate<T> where T : IBBOwner
    {
        bool AppliesTo(T item, double x0, double y0, double x1, double y1);
    }
}
