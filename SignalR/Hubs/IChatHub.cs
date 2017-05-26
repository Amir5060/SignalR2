using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Hubs
{
    interface IChatHub
    {
        void Send(string name, string message);
        void sendToSpecific(string name, string message, string to);
        void Notify(string name, string id);

    }
}
