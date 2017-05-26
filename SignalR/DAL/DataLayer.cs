using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalR.DAL
{
    public class DataLayer: IDataLayer
    {
        public void AddMessage(string name, string message)
        {
            CloudTable table = MessageTable();
            ChatMessage msg1 = new ChatMessage(name, DateTime.Now.ToString(), message);
            var v = table.Execute(TableOperation.InsertOrReplace(msg1));
        }

        private CloudTable MessageTable()
        {
            string storageConnection = "DefaultEndpointsProtocol=https;AccountName=newsignal;AccountKey=d3Ug4bdWUssFO8SwYp5P5c48CdmYNuaFNfxTJpbR5MF56aqGZeWUVGsXHN33H+GAehcIYUabql5tEpn0ojc98g==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            //table.CreateIfNotExists();
            return tableClient.GetTableReference("newsignal");
        }
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