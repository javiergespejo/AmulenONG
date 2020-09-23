using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABM.Interfaces
{
    public interface IHomeRepository : IDisposable
    {
        string GetWelcomeText();
        IEnumerable<HomePageImage> GetHomeSliderImages();
    }
}
