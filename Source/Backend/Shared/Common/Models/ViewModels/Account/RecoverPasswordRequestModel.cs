using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manifest.Common.Models.ViewModels.Account
{
    public class RecoverPasswordRequestModel
    {
        public string UserName { get; set; }

        public string ClientRedirectURL { get; set; }

    }

}
