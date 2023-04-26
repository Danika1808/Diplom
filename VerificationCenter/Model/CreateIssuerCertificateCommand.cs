using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VerificationCenter.Model
{
    public class CreateIssuerCertificateCommand
    {
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Inn { get; set; }
    }
}
