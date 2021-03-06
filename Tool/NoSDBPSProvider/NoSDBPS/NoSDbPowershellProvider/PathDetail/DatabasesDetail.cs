﻿// /*
// * Copyright (c) 2016, Alachisoft. All Rights Reserved.
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// * http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alachisoft.NosDB.Common.Configuration.DOM;
namespace Alachisoft.NosDB.NosDBPS
{
    public class DatabasesDetail:NodeDetail
    {

        internal DatabasesDetail(string nodeName, DatabaseConfigurations configuration, string[] pathChunks)
        {
            NodeName = nodeName;
            NodeType = PathType.Databases;
            IsContainer = true;
            IsValid = true;
            Configuration = configuration;
            PathChunks = pathChunks;
            ChilderanTable = new PrintableTable();
            ChilderanTable.AddHeader("Database name","Provider");
            ChilderanName = new List<string>();
            foreach (var  db in configuration.Configurations.Values)
            {
                ChilderanName.Add(db.Name);
                ChilderanTable.AddRow(db.Name,db.Storage.StorageProvider.StorageProviderType.ToString());
            }

        }

        public override bool TryGetNodeDetail(out NodeDetail nodeDetail)
        {
            NodeDetail thisNode = new EndNodeDetail();
            bool sucess = false;
            if (((DatabaseConfigurations)Configuration).ContainsDatabase(PathChunks[0].ToLower()))
            {
                
                DatabaseConfiguration dbconfig = ((DatabaseConfigurations)Configuration).GetDatabase(PathChunks[0].ToLower());
                string[] childPathChunks = new string[this.PathChunks.Length - 1];
                Array.Copy(this.PathChunks, 1, childPathChunks, 0, this.PathChunks.Length - 1);
                thisNode = new DatabaseValueDetail(PathChunks[0], dbconfig, childPathChunks);
                sucess = true;
            }
            if (PathChunks.Length == 1)
            {
                nodeDetail = thisNode;
                return sucess;
            }
            return thisNode.TryGetNodeDetail(out nodeDetail);
        }
    }
}
