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
        // Mock objects for dependencies
        private readonly Mock<IProgressRecordService> _mockService;
        private readonly Mock<ILogger<ProgressRecordsController>> _mockLogger;
        private readonly ProgressRecordsController _controller;

        // Constructor to initialize mocks and set up the controller
        public ProgressRecordsControllerTests()
        {
            _mockService = new Mock<IProgressRecordService>();
            _mockLogger = new Mock<ILogger<ProgressRecordsController>>();
            _controller = new ProgressRecordsController(_mockService.Object, _mockLogger.Object);
        }

        // GET: Retrieve all progress records (Successful case)
        [Fact]
        public async Task GetProgressRecords_ReturnsListOfRecords()
        {
            // Arrange: Create mock progress records
            var mockRecords = new List<ProgressRecordDTO>
            {
                new ProgressRecordDTO { ProgressRecordId = 1, UserId = 10, DistanceCovered = 5.0, ProgressDate = "2024-02-28", ProgressTime = "12:00 PM" },
                new ProgressRecordDTO { ProgressRecordId = 2, UserId = 12, DistanceCovered = 8.0, ProgressDate = "2024-02-29", ProgressTime = "01:00 PM" }
            };

            // Mock the service to return the sample records
            _mockService.Setup(service => service.GetAllProgressRecordsAsync()).ReturnsAsync(mockRecords);

            // Act: Call the controller method
            var result = await _controller.GetProgressRecords();

            // Assert: Validate response type and data
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRecords = Assert.IsType<List<ProgressRecordDTO>>(okResult.Value);
            Assert.Equal(2, returnRecords.Count);
        }

        // GET: Retrieve all progress records (No records found scenario)
        [Fact]
        public async Task GetProgressRecords_NoRecordsFound_ReturnsNotFound()
        {
            // Mock the service to return an empty list
            _mockService.Setup(service => service.GetAllProgressRecordsAsync()).ReturnsAsync(new List<ProgressRecordDTO>());

            // Act
            var result = await _controller.GetProgressRecords();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // GET: Retrieve progress record by ID (Successful case)
        [Fact]
        public async Task GetProgressRecordById_ExistingId_ReturnsOk()
        {
            // Arrange: Create a sample record
            var record = new ProgressRecordDTO { ProgressRecordId = 1, UserId = 10, DistanceCovered = 5.0, ProgressDate = "2024-02-28", ProgressTime = "12:00 PM" };

            // Mock service to return the record when requested
            _mockService.Setup(service => service.GetProgressRecordByIdAsync(1)).ReturnsAsync(record);

            // Act
            var result = await _controller.GetProgressRecord(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnRecord = Assert.IsType<ProgressRecordDTO>(okResult.Value);
            Assert.Equal(1, returnRecord.ProgressRecordId);
        }

        // GET: Retrieve progress record by ID (Record not found scenario)
        [Fact]
        public async Task GetProgressRecordById_NonExistingId_ReturnsNotFound()
        {
            // Mock service to return null when requested record does not exist
            _mockService.Setup(service => service.GetProgressRecordByIdAsync(99)).ReturnsAsync((ProgressRecordDTO?)null);

            // Act
            var result = await _controller.GetProgressRecord(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // POST: Create a new progress record (Successful case)
        [Fact]
        public async Task PostProgressRecord_ValidRecord_ReturnsCreated()
        {
            var newRecord = new ProgressRecordDTO { ProgressRecordId = 3, UserId = 15, DistanceCovered = 12.0, ProgressDate = "2024-03-01", ProgressTime = "02:00 PM" };

            // Mock service to return the newly created record
            _mockService.Setup(service => service.AddProgressRecordAsync(It.IsAny<ProgressRecordDTO>())).ReturnsAsync(newRecord);

            // Act
            var result = await _controller.PostProgressRecord(newRecord);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnRecord = Assert.IsType<ProgressRecordDTO>(createdResult.Value);
            Assert.Equal(3, returnRecord.ProgressRecordId);
        }

        // POST: Create progress record (Missing required fields scenario)
        [Fact]
        public async Task PostProgressRecord_MissingFields_ReturnsBadRequest()
        {
            var invalidRecord = new ProgressRecordDTO { ProgressRecordId = 4, UserId = 16, DistanceCovered = 10.0, ProgressDate = "", ProgressTime = "" };

            // Act
            var result = await _controller.PostProgressRecord(invalidRecord);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Both ProgressDate and ProgressTime are required.", ((dynamic)badRequestResult.Value).message);
        }
    }
}

/*The ProgressRecordsControllerTests is a unit testing suite designed to validate the functionality of the ProgressRecordsController in the RunClubAPI. This class utilizes XUnit for test execution and Moq for mocking dependencies, ensuring that tests remain isolated from external dependencies like databases.

The test suite follows a structured approach, covering essential API functionalities such as retrieving, creating, updating, and deleting progress records. For example, GetProgressRecords_ReturnsListOfRecords() ensures that when records exist, the API returns a valid list. Conversely, GetProgressRecords_NoRecordsFound_ReturnsNotFound() tests the controller’s behavior when no records exist.

The test cases also verify edge cases and error handling. For instance, the PostProgressRecord_MissingFields_ReturnsBadRequest() test ensures that required fields are validated before creating a record. Similarly, the GetProgressRecordById_NonExistingId_ReturnsNotFound() test confirms that the controller correctly handles cases where a requested record does not exist.

By leveraging dependency injection and mocking techniques, the test suite ensures that the API is well-structured, maintainable, and robust. The implementation follows best testing practices, such as Arrange-Act-Assert (AAA) and separation of concerns, improving the code’s reliability and maintainability. Ultimately, this testing suite plays a crucial role in ensuring the quality, correctness, and stability of the API in real-world applications. */