using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beis.Htg.VendorSme.Database.Models;

namespace Beis.HelpToGrow.Voucher.Api.Redeem.Services.Interfaces
{
    public interface IProductRepository
    {
        Task<product> GetProductSingle(long id);
        product GetProductBySku(string productSku, long vendorId);
    }
}