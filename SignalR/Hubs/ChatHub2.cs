using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

namespace SignalR.Hubs
{
    public class ChatHub2 : Hub
    {
        static ConcurrentDictionary<string, string> dic = new ConcurrentDictionary<string, string>();

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
            CloudTable table = MessageTable();
            ChatMessage msg1 = new ChatMessage(name, DateTime.Now.ToString(), message);
            var v = table.Execute(TableOperation.InsertOrReplace(msg1));
        }

        public void sendToSpecific(string name, string message, string to)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Caller.broadcastMessage(name, message);
            Clients.Client(dic[to]).broadcastMessage(name, message);
        }

        public void Notify(string name, string id)
        {
            if (dic.ContainsKey(name))
            {
                Clients.Caller.differentName();
            }
            else
            {
                dic.TryAdd(name, id);

                foreach (KeyValuePair<String, String> entry in dic)
                {
                    Clients.Caller.online(entry.Key);
                }

                Clients.Others.enters(name);
            }
        }

        public CloudTable MessageTable()
        {
            string storageConnection = "DefaultEndpointsProtocol=https;AccountName=newsignal;AccountKey=d3Ug4bdWUssFO8SwYp5P5c48CdmYNuaFNfxTJpbR5MF56aqGZeWUVGsXHN33H+GAehcIYUabql5tEpn0ojc98g==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            //table.CreateIfNotExists();
            return tableClient.GetTableReference("newsignal");
        }

        //public override Task OnDisconnected()
        //{
        //    var name = dic.FirstOrDefault(x => x.Value == Context.ConnectionId.ToString());
        //    string s;
        //    dic.TryRemove(name.Key, out s);
        //    return Clients.All.disconnected(name.Key);
        //}

    }

    public class ChatMessage : TableEntity
    {
        public ChatMessage() { }
        public ChatMessage(string user, string time, string message)
        {
            PartitionKey = user;
            RowKey = time;
            Message = message;
            username = user;
        }
        public string SessionID { get; set; }
        public string Message { get; set; }

        public string username { get; set; }
    }
}
