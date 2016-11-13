using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data
{
    public enum RegistrationStatus
    {
        Active = 0,
        Confirmed = 1,
        CancelActive = -1,
        CancelConfirmed = -2
    }
}
