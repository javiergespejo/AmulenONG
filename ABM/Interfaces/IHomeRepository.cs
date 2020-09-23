using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.Interfaces
{
    public class IHomeRepository : IDisposable
    {
        string GetWelcomeText();
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}