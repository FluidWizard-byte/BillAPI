using BillAPI.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BillAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {


        [HttpGet("{tariffID}/{lastReading}/{currentReading}")]
        public BillDetails Get(int tariffID, int lastReading, int currentReading)
        {

            BillDetails billDetail = new BillDetails();

            DBHandle db = new DBHandle();

            List<TariffDetails> tariffDetails = db.GetTariffDetailsById(tariffID);
            List<decimal> energyRates = new List<decimal>();
            List<int> intervals = new List<int>();

            decimal billAmount = 0;
            int n = tariffDetails.Count;
            int unitsConsumed = currentReading - lastReading;
            var serviceChargeData = tariffDetails.Where(x => x.startUnit <= unitsConsumed && x.endUnit >= unitsConsumed).FirstOrDefault();
            decimal serviceCharge = serviceChargeData.serviceCharge;
            billAmount += serviceCharge;
            foreach (TariffDetails tariffDetail in tariffDetails)
            {
                energyRates.Add(tariffDetail.energyRate);
                intervals.Add(tariffDetail.endUnit - tariffDetail.startUnit + 1);
            }
            for (int i = 0; i < n; i++)
            {
                if (unitsConsumed <= intervals[i])
                {
                    billAmount += energyRates[i] * unitsConsumed;
                    break;
                }
                else
                {
                    billAmount += energyRates[i] * intervals[i];
                    unitsConsumed -= intervals[i];
                }
            }

            billDetail.BillAmount = billAmount;
            return billDetail;

        }

    }
}
