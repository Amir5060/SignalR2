﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Table;

namespace SignalR2.Hubs
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public IEnumerable<string> Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.

            string storageConnection = "DefaultEndpointsProtocol=https;AccountName=newsignal;AccountKey=d3Ug4bdWUssFO8SwYp5P5c48CdmYNuaFNfxTJpbR5MF56aqGZeWUVGsXHN33H+GAehcIYUabql5tEpn0ojc98g==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("newsignal");
            table.CreateIfNotExists();

            ChatMessage msg1 = new ChatMessage(name, DateTime.Now.ToString(), message);
            var v = table.Execute(TableOperation.InsertOrReplace(msg1));

            Clients.All.addNewMessageToPage(name, message);

            var vNames = from names in AllMessages()
                    select new { names.username }.ToString();
            return AllMessages().Select(x => x.username).ToArray();
        }


        

        public List<ChatMessage> AllMessages()
        {
            string storageConnection = "DefaultEndpointsProtocol=https;AccountName=newsignal;AccountKey=d3Ug4bdWUssFO8SwYp5P5c48CdmYNuaFNfxTJpbR5MF56aqGZeWUVGsXHN33H+GAehcIYUabql5tEpn0ojc98g==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("newsignal");
            table.CreateIfNotExists();
            TableQuery<ChatMessage> query = new TableQuery<ChatMessage>();
            var list = table.ExecuteQuery(query).ToList();
            return list;
        }

        public string[] GetOnlineUsers()
        {
            string storageConnection = "DefaultEndpointsProtocol=https;AccountName=onlineusers;AccountKey=aKsyG2sYLI+UAL2uzyZc2F6fXKux8L/JN/rYqVlqX4t3RLRD3l6KjnzCuSNUMyR6jcVrM+wx3UcmznzYXY0qcA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("onlineusers");
            table.CreateIfNotExists();
            TableQuery<ChatMessage> query = new TableQuery<ChatMessage>();

        }

        public void AddOnlineUser(string name)
        {
            string storageConnection = "DefaultEndpointsProtocol=https;AccountName=onlineusers;AccountKey=aKsyG2sYLI+UAL2uzyZc2F6fXKux8L/JN/rYqVlqX4t3RLRD3l6KjnzCuSNUMyR6jcVrM+wx3UcmznzYXY0qcA==;EndpointSuffix=core.windows.net";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("onlineusers");
            table.CreateIfNotExists();

            OnlineUser newUser = new OnlineUser(name, DateTime.Now.ToString(), "Online");
            var v = table.Execute(TableOperation.InsertOrReplace(newUser));
        }

        //public void SaveName(string name)
        //{
        //    CloudStorageAccount storageAccount = CloudStorageAccount.Parse();
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

    public class OnlineUser: TableEntity
    {
        public OnlineUser(string user, string time, string status)
        {
            PartitionKey = user;
            RowKey = time;
            Status = status;
            
        }
        public string SessionID { get; set; }
        public string userName { get; set; }
        public string Status { get; set; }

    }
}