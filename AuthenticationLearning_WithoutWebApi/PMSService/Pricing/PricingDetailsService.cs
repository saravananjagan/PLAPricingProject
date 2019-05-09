using IPMSService.Pricing;
using PMSDAL.Pricing;
using PMSIDAL.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSService.Pricing
{
    public class PricingDetailsService: IPricingDetailsService
    {
        public bool InsertBulkPricingDetails(string ImportValues)
        {
            try
            {
                IPricingDetailsDAL pricingDetailsDAL = new PricingDetailsDAL();
                pricingDetailsDAL.InsertBulkPricingDetails(ImportValues);
            }
            catch (Exception e)
            {

            }
            return true;
        }
    }
}
