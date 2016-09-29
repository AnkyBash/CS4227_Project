using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    namespace pReservation
    {
        class TaxiService : Services
        {
            //calculate cost override method
            public override double CalculatePrice()
            {
                return 25.5;
            }
        }
    }

}
