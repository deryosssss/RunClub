using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RunClubAPI.Controllers;
using RunClubAPI.Interfaces;
using RunClubAPI.DTOs;
using Microsoft.Extensions.Logging; // Make sure this is included

namespace RunClubAPI.Tests
{
    public class ProgressRecordsControllerTests
    {
        private readonly Mock<IProgressRecordService> _mockService;
        private readonly Mock<ILogger<ProgressRecordsController>> _mockLogger; // Add logger mock
        private readonly ProgressRecordsController _controller;

        public ProgressRecordsControllerTests()
        {
            _mockService = new Mock<IProgressRecordService>();
            _mockLogger = new Mock<ILogger<ProgressRecordsController>>(); // Initialize logger mock
            _controller = new ProgressRecordsController(_mockService.Object, _mockLogger.Object); // Pass both mocks
        }

        // Test 1: Get All Progress Records
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

        // Test 2: Get Progress Record By ID (Found)
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

        // Test 3: Get Progress Record By ID (Not Found)
        [Fact]
        public async Task GetProgressRecordById_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(service => service.GetProgressRecordByIdAsync(99))
                        .ReturnsAsync((ProgressRecordDTO?)null); // Explicitly cast to nullable

            var result = await _controller.GetProgressRecord(99);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}

