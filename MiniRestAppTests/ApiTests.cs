using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers;
using WebAPI.Models;

namespace MiniRestAppTests;

public class ApiTests
{
    private CustomerContext _context;
    private CustomersController _controller;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<CustomerContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new CustomerContext(options);
        _controller = new CustomersController(_context);
    }

    [Test]
    public async Task ShouldSaveAndRetrieveCustomer()
    {
        // Arrange
        var customer = new Customer { CustomerRef = "123", CustomerName = "John Doe" };

        // Act
        var postResult = await _controller.PostCustomer(customer);
        var getResult = await _controller.GetCustomer(1);  // Assumes the customer is assigned an Id of 1

        // Assert
        Assert.IsInstanceOf<CreatedAtActionResult>(postResult.Result);
        Assert.AreEqual("John Doe", ((Customer)((CreatedAtActionResult)postResult.Result).Value).CustomerName);

        Assert.IsInstanceOf<ActionResult<Customer>>(getResult);
        Assert.AreEqual("John Doe", getResult.Value.CustomerName);
    }
    
    [Test]
    public async Task ShouldReturnNotFoundForNonExistentCustomer()
    {
        // Act
        var result = await _controller.GetCustomer(99);  // Assumes there's no customer with this id

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result.Result);
    }
    
    [Test]
    public async Task Get_WhenCalled_ReturnsCustomerById()
    {
        // Arrange
        var testCustomer = new Customer { CustomerRef = "Test1", CustomerName = "John Doe" };
        _context.Customers.Add(testCustomer);
        await _context.SaveChangesAsync();

        var id = testCustomer.Id; // Get the ID of the newly created customer

        // Act
        var result = await _controller.GetCustomer(id);

        // Assert
        Assert.IsNotNull(result.Value);
        Assert.AreEqual("John Doe", result.Value.CustomerName);
    }

}