using CatsProj.DAL.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsProj.BLL.Handlers
{
     public class TokenHandler
    {
        public string getToken()
        {
            TokenProvider provider = new TokenProvider();
            string token = provider.getToken();
            return token;
        }
    }
}
