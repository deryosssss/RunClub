using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunClubAPI.Controllers;
using RunClubAPI.Interfaces;
using RunClubAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunClubAPI.Tests
{
    public class ProgressRecordsControllerTests
    {
        private readonly Mock<IProgressRecordService> _mockService;
        private readonly Mock<ILogger<ProgressRecordsController>> _mockLogger;
        private readonly ProgressRecordsController _controller;

        public ProgressRecordsControllerTests()
        {
            _mockService = new Mock<IProgressRecordService>();
            _mockLogger = new Mock<ILogger<ProgressRecordsController>>();
            _controller = new ProgressRecordsController(_mockService.Object, _mockLogger.Object);
        }

        // ✅ GET: All Progress Records (Success)
        [Fact]
        public async Task GetProgressRecords_ReturnsListOfRecords()
        {
            var mockRecords = new List<ProgressRecordDTO>
            {
                new ProgressRecordDTO { ProgressRecordId = 1, UserId = 10, DistanceCovered = 5.0, ProgressDate = "2024-02-28", ProgressTime = "12:00 PM" },
                new ProgressRecordDTO { ProgressRecordId = 2, UserId = 12, DistanceCovered = 8.0, ProgressDate = "2024-02-29", ProgressTime = "01:00 PM" }
            };

            _mockService.Setup(service => service.GetAllProgressRecordsAsync())
                        .ReturnsAsync(mockRecords);

            var result = await _controller.GetProgressRecords();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRecords = Assert.IsType<List<ProgressRecordDTO>>(okResult.Value);
            Assert.Equal(2, returnRecords.Count);
        }

        // ✅ GET: All Progress Records (Empty List)
        [Fact]
        public async Task GetProgressRecords_NoRecordsFound_ReturnsNotFound()
        {
            _mockService.Setup(service => service.GetAllProgressRecordsAsync())
                        .ReturnsAsync(new List<ProgressRecordDTO>());

            var result = await _controller.GetProgressRecords();

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // ✅ GET: Progress Record by ID (Found)
        [Fact]
        public async Task GetProgressRecordById_ExistingId_ReturnsOk()
        {
            var record = new ProgressRecordDTO { ProgressRecordId = 1, UserId = 10, DistanceCovered = 5.0, ProgressDate = "2024-02-28", ProgressTime = "12:00 PM" };

            _mockService.Setup(service => service.GetProgressRecordByIdAsync(1))
                        .ReturnsAsync(record);

            var result = await _controller.GetProgressRecord(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRecord = Assert.IsType<ProgressRecordDTO>(okResult.Value);
            Assert.Equal(1, returnRecord.ProgressRecordId);
        }

        // ✅ GET: Progress Record by ID (Not Found)
        [Fact]
        public async Task GetProgressRecordById_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(service => service.GetProgressRecordByIdAsync(99))
                        .ReturnsAsync((ProgressRecordDTO?)null);

            var result = await _controller.GetProgressRecord(99);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // ✅ POST: Create Progress Record (Success)
        [Fact]
        public async Task PostProgressRecord_ValidRecord_ReturnsCreated()
        {
            var newRecord = new ProgressRecordDTO { ProgressRecordId = 3, UserId = 15, DistanceCovered = 12.0, ProgressDate = "2024-03-01", ProgressTime = "02:00 PM" };

            _mockService.Setup(service => service.AddProgressRecordAsync(It.IsAny<ProgressRecordDTO>()))
                        .ReturnsAsync(newRecord);

            var result = await _controller.PostProgressRecord(newRecord);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnRecord = Assert.IsType<ProgressRecordDTO>(createdResult.Value);
            Assert.Equal(3, returnRecord.ProgressRecordId);
        }

        // ✅ POST: Create Progress Record (Bad Request - Missing Fields)
        [Fact]
        public async Task PostProgressRecord_MissingFields_ReturnsBadRequest()
        {
            var invalidRecord = new ProgressRecordDTO { ProgressRecordId = 4, UserId = 16, DistanceCovered = 10.0, ProgressDate = "", ProgressTime = "" };

            var result = await _controller.PostProgressRecord(invalidRecord);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Both ProgressDate and ProgressTime are required.", ((dynamic)badRequestResult.Value).message);
        }

        // ✅ PUT: Update Progress Record (Success)
        [Fact]
        public async Task PutProgressRecord_ValidUpdate_ReturnsNoContent()
        {
            var updatedRecord = new ProgressRecordDTO { ProgressRecordId = 1, UserId = 10, DistanceCovered = 7.0, ProgressDate = "2024-03-02", ProgressTime = "03:00 PM" };

            _mockService.Setup(service => service.UpdateProgressRecordAsync(1, updatedRecord))
                        .ReturnsAsync(true);

            var result = await _controller.PutProgressRecord(1, updatedRecord);

            Assert.IsType<NoContentResult>(result);
        }

        // ✅ PUT: Update Progress Record (Mismatched ID)
        [Fact]
        public async Task PutProgressRecord_IdMismatch_ReturnsBadRequest()
        {
            var updatedRecord = new ProgressRecordDTO { ProgressRecordId = 2, UserId = 10, DistanceCovered = 7.0, ProgressDate = "2024-03-02", ProgressTime = "03:00 PM" };

            var result = await _controller.PutProgressRecord(1, updatedRecord);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("ProgressRecord ID mismatch.", badRequestResult.Value);
        }

        // ✅ PUT: Update Progress Record (Not Found)
        [Fact]
        public async Task PutProgressRecord_NonExistingId_ReturnsNotFound()
        {
            var updatedRecord = new ProgressRecordDTO { ProgressRecordId = 99, UserId = 10, DistanceCovered = 7.0, ProgressDate = "2024-03-02", ProgressTime = "03:00 PM" };

            _mockService.Setup(service => service.UpdateProgressRecordAsync(99, updatedRecord))
                        .ReturnsAsync(false);

            var result = await _controller.PutProgressRecord(99, updatedRecord);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        // ✅ DELETE: Delete Progress Record (Success)
        [Fact]
        public async Task DeleteProgressRecord_ValidId_ReturnsNoContent()
        {
            _mockService.Setup(service => service.DeleteProgressRecordAsync(1))
                        .ReturnsAsync(true);

            var result = await _controller.DeleteProgressRecord(1);

            Assert.IsType<NoContentResult>(result);
        }

        // ✅ DELETE: Delete Progress Record (Not Found)
        [Fact]
        public async Task DeleteProgressRecord_NonExistingId_ReturnsNotFound()
        {
            _mockService.Setup(service => service.DeleteProgressRecordAsync(99))
                        .ReturnsAsync(false);

            var result = await _controller.DeleteProgressRecord(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}

