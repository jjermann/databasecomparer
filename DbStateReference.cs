using System;
using System.Collections.Generic;

namespace DatabaseComparer
{
    public class DbStateReference
    {
        public DbStateReferenceType DbStateReferenceType {get;set;}
        public DbState DbState {get;set;}
        public string DbStateFilename {get;set;}
        public string DbBackupFilename {get;set;}
        public string DbName {get;set;}
        public string DbServer {get;set;}
        public string DbInstance {get;set;}
        public string DbUser {get;set;}
        public string DbPassword {get;set;}
        public string ConnectionString => null;
        public List<int> GetBusinessIdList() => null;
    }
}
