using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows.Win32.Foundation {
    public readonly partial struct HRESULT {
        public bool Ok => Succeeded;
    }
}
