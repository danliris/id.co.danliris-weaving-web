using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.CoreMicroservice
{
    public class MasterDataSettings
    {
        public string Endpoint { get; set; }

        public string TokenEndpoint { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }
    }
}
