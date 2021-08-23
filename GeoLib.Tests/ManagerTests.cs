using System;
using NUnit.Framework;
using GeoLib.Services;
using GeoLib.Contracts;
using System.Collections.Generic;
using System.Linq;
using Moq;
using GeoLib.Data;

namespace GeoLib.Tests
{
    [TestFixture]
    public class ManagerTests
    {
        [Test]
        public void ShouldGetCityStateByZipCode()
        {
            Mock<IZipCodeRepository> mockZipCodeRepository = new Mock<IZipCodeRepository>();

            ZipCode zipCode = new ZipCode()
            {
                City = "LINCOLN PARK",
                State = new State() { Abbreviation = "NJ" },
                Zip = "07035"
            };

            mockZipCodeRepository.Setup(obj => obj.GetByZip("07035")).Returns(zipCode);

            IGeoService geoService = new GeoManager(mockZipCodeRepository.Object);

            ZipCodeData data = geoService.GetZipInfo("07035");

            Assert.IsTrue(data.City.ToUpper() == "LINCOLN PARK");
            Assert.IsTrue(data.State == "NJ");
        }

        [Test]
        public void ShouldGetZipCodesByState()
        {
            Mock<IZipCodeRepository> mockZipCodeRepository = new Mock<IZipCodeRepository>();

            ZipCode zipCode = new ZipCode()
            {
                City = "GALVESTON",
                State = new State() { Abbreviation = "TX" },
                Zip = "77550"
            };

            IEnumerable<ZipCode> zipCodes = new List<ZipCode>() { zipCode };

            mockZipCodeRepository.Setup(obj => obj.GetByState("Texas")).Returns(zipCodes);

            IGeoService geoService = new GeoManager(mockZipCodeRepository.Object);

            IEnumerable<ZipCodeData> data = geoService.GetZips("Texas");

            Assert.IsTrue(data.ToList().Count > 0);
            Assert.IsTrue(data.ToList()[0].City.ToUpper() == "GALVESTON");
            Assert.IsTrue(data.ToList()[0].State == "TX");
        }
    }
}
