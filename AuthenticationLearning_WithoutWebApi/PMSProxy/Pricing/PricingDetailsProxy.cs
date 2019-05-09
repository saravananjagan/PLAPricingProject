using IPMSService.Pricing;
using PMSService.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMSProxy.Pricing
{
    public static class PricingDetailsProxy
    {
        public static bool InsertBulkPricingDetails(string ImportValues)
        {
            IPricingDetailsService pricingDetailsService = new PricingDetailsService();
            return pricingDetailsService.InsertBulkPricingDetails(ImportValues);
        }
    }
}
