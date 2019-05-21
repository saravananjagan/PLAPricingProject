﻿using IPMSService.Pricing;
using PMSDAL.Pricing;
using PMSIDAL.Pricing;
using PMSModel.Pricing;
using System;
using System.Collections.Generic;
using System.Data;
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

        public DataSet FetchPricingDetails()
        {
            DataSet dataSet = new DataSet();
            try
            {
                IPricingDetailsDAL pricingDetailsDAL = new PricingDetailsDAL();
                dataSet = pricingDetailsDAL.FetchPricingDetails(); 
            }
            catch (Exception e)
            {

            }
            return dataSet;
        }
        public bool CUDPricingDetails(PricingData pricingData,string QuerySelector)
        {
            try
            {
                IPricingDetailsDAL pricingDetailsDAL = new PricingDetailsDAL();
                pricingDetailsDAL.CUDPricingDetails(pricingData,QuerySelector);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }            
        }
    }
}
