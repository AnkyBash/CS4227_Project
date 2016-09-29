using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    namespace pReservation
    {
        class LaundryService : Services //derived class from Services abstract class
        {
            //calculate cost override method
            public override double CalculatePrice()
            {
                return 10.5;
            }
        }
    }

}
