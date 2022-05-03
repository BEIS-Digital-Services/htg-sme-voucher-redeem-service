using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json.Serialization;
using NuGet.Frameworks;
using NUnit.Framework;
using smevoucherencryption;
using System.Threading.Tasks;
using VoucherRedeemService.interfaces;
using VoucherUpdateService.Controllers;
using VoucherUpdateService.domain.entities;

namespace VoucherRedeemMicroServiceTests
{
    [TestFixture]
    public class VoucherControllerTests
    {
        private VoucherController _voucherController;
        private Mock<IVoucherRedeemService> _voucherRedeemService;
        private Mock<IVendorAPICallStatusServices> _vendorAPICallStatusServices;
        private Mock<ILogger<VoucherController>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<VoucherController>>();
            _vendorAPICallStatusServices = new Mock<IVendorAPICallStatusServices>();
            _vendorAPICallStatusServices.Setup(x => x.CreateLogRequestDetails(It.IsAny<VoucherUpdateRequest>()))
                .Returns(new Beis.Htg.VendorSme.Database.Models.vendor_api_call_status { });
           _voucherRedeemService = new Mock<IVoucherRedeemService>();
            _voucherController = new VoucherController(_logger.Object, _voucherRedeemService.Object, _vendorAPICallStatusServices.Object);
        }

        [Test]
        public async Task RedeemVoucherHappyPathTest()
        {
            VoucherUpdateRequest voucherUpdateRequest = new VoucherUpdateRequest()
            {
                registration = "12345",
                accessCode = "12345",
                voucherCode = "IvMBLZ2PhUVkmJHpAxle0Q",
                authorisationCode = "GHT23RTDWER"
            };

            var voucherUpdateExpectedResponse = new VoucherUpdateResponse()
            {
                voucherCode = "IvMBLZ2PhUVkmJHpAxle0Q",
                status = "OK",
                errorCode = 0,
                message = "XXXXXXXXX"
            };
            
            _voucherRedeemService.Setup(x =>x.GetVoucherResponse(It.IsAny<VoucherUpdateRequest>())).ReturnsAsync(voucherUpdateExpectedResponse);
            ActionResult<VoucherUpdateResponse> actionResult = await _voucherController.CheckVoucher(voucherUpdateRequest);
            VoucherUpdateResponse voucherUpdateResponse = (VoucherUpdateResponse) ((OkObjectResult) actionResult.Result).Value;

            Assert.AreEqual(voucherUpdateRequest.voucherCode, voucherUpdateResponse.voucherCode);
            Assert.AreEqual("OK", voucherUpdateResponse.status);
            Assert.AreEqual(0, voucherUpdateResponse.errorCode);
            Assert.AreEqual("Successful check - proceed", voucherUpdateResponse.message);
        }

        [Test]
        public async Task RedeemVoucherNegativePathTest()
        {
            VoucherUpdateRequest voucherUpdateRequest = new VoucherUpdateRequest()
            {
                registration = "12345",
                accessCode = "12345",
                voucherCode = "IvMBLZ2PhUVkmJHpAxle0Q99",
                authorisationCode = "GHT23RTDWER"
            };

            var voucherUpdateExpectedResponse = new VoucherUpdateResponse()
            {
                voucherCode = "IvMBLZ2PhUVkmJHpAxle0Q",
                status = "ERROR",
                errorCode = 401,
                message = "Invalid voucher code"
            };
            
            _voucherRedeemService.Setup(x =>x.GetVoucherResponse(It.IsAny<VoucherUpdateRequest>())).ReturnsAsync(voucherUpdateExpectedResponse);
            ActionResult<VoucherUpdateResponse> actionResult = await _voucherController.CheckVoucher(voucherUpdateRequest);
            VoucherUpdateResponse voucherUpdateResponse = (VoucherUpdateResponse) ((ObjectResult) actionResult.Result).Value;
            
            Assert.AreEqual("ERROR", voucherUpdateResponse.status);
            Assert.AreEqual(401, voucherUpdateResponse.errorCode);
            Assert.AreEqual("Invalid voucher code", voucherUpdateResponse.message);
        }
        

    }
}