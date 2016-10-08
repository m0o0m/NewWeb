using GL.Redis.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Redis.Data.IRepository
{
    public interface IWebGateRepository : IDisposable
    {
        IList<GateInfo> GetAll();
        IDisposable AcquireLock();
        IDisposable AcquireLock(TimeSpan timeOut);

    }
}
