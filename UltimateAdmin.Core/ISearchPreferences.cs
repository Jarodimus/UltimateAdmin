using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateAdmin.Core
{
    public interface ISearchPreferences
    {
        bool SearchEntireDirectory { get; }
        void SetSearchEntireDirectory(bool searchAll);
    }
}
