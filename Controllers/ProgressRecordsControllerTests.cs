using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RunClub.Controllers; // ✅ Add this if missing
using RunClubAPI.Interfaces;
using RunClub.DTOs;

namespace RunClub.Tests
{
    public class ProgressRecordsControllerTests
    {
        private readonly Mock<IProgressRecordService> _mockService;
        private readonly ProgressRecordsController _controller;

        public ProgressRecordsControllerTests()
        {
            _mockService = new Mock<IProgressRecordService>();
            _controller = new ProgressRecordsController(_mockService.Object);
        }

        // ✅ Test 1: Get All Progress Records
        [Fact]
        public async Task GetProgressRecords_ReturnsListOfRecords()
        {
            var mockRecords = new List<ProgressRecordDTO>
            {
                new ProgressRecordDTO { ProgressRecordId = 1, UserId = 10, DistanceCovered = 5.0 },
                new ProgressRecordDTO { ProgressRecordId = 2, UserId = 12, DistanceCovered = 8.0 }
            };

            _mockService.Setup(service => service.GetAllProgressRecordsAsync())
                        .ReturnsAsync(mockRecords);

            var result = await _controller.GetProgressRecords();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRecords = Assert.IsType<List<ProgressRecordDTO>>(okResult.Value);
            Assert.Equal(2, returnRecords.Count);
        }

        // ✅ Test 2: Get Progress Record By ID (Found)
        [Fact]
        public async Task GetProgressRecordById_ExistingId_ReturnsOk()
        {
            var record = new ProgressRecordDTO { ProgressRecordId = 1, UserId = 10, DistanceCovered = 5.0 };

            _mockService.Setup(service => service.GetProgressRecordByIdAsync(1))
                        .ReturnsAsync(record);

            var result = await _controller.GetProgressRecord(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRecord = Assert.IsType<ProgressRecordDTO>(okResult.Value);
            Assert.Equal(1, returnRecord.ProgressRecordId);
        }

        // ✅ Test 3: Get Progress Record By ID (Not Found)
        [Fact]
        public async Task GetProgressRecordById_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(service => service.GetProgressRecordByIdAsync(99))
                        .ReturnsAsync((ProgressRecordDTO?)null); // ✅ Fix nullability issue

            var result = await _controller.GetProgressRecord(99);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}

