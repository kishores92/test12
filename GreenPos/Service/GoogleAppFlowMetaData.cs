using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenPOS.Service
{
    /*
    public class GoogleAppFlowMetaData : FlowMetadata
    {
        static readonly string[] Scopes = { DocsService.Scope.Documents, DocsService.Scope.Drive, DocsService.Scope.DriveFile, DriveService.Scope.Drive, DriveService.Scope.DriveFile };
        private IAuthorizationCodeFlow flow { get; set; }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }

        public GoogleAppFlowMetaData(IDataStore dataStore, string clientID, string clientSecret)
        {
            var flowInitializer = new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                },
                Scopes = Scopes,
                DataStore = dataStore
            };
            flow = new GoogleAuthorizationCodeFlow(flowInitializer);
        }

        public override string GetUserId(Controller controller)
        {
            return "1";
        }

        public override string AuthCallback
        {
            get
            {
                return @"/AuthCallback/IndexAsync";
            }
        }
    }
    */
}


