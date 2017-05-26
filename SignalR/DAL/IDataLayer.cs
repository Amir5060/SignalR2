using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DAL
{
    public interface IDataLayer
    {
        void AddMessage(string name, string message);
    }
}
