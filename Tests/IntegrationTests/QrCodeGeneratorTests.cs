using BL.Generator;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject1;

public class QrCodeGeneratorTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public QrCodeGeneratorTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void GenerateQrCode_ReturnsByteArray_ForValidData()
    {
        //Arrange
        using var scope = _factory.Services.CreateScope();
        var qrCodeGenerator = scope.ServiceProvider.GetRequiredService<QrCodeGenerator>();
        
        //Act
        byte[] qrCodeBytes = qrCodeGenerator.GenerateQrCode("TestData");
        
        //Assert
        Assert.NotNull(qrCodeBytes);
        Assert.NotEmpty(qrCodeBytes);
        Assert.True(qrCodeBytes.Length == 552);
    }
    
    [Fact]
    public void GenerateQrCode_ThrowsException_ForEmptyData()
    {
        //Arrange
        using var scope = _factory.Services.CreateScope();
        var qrCodeGenerator = scope.ServiceProvider.GetRequiredService<QrCodeGenerator>();
        
        //Act
        var action = () => qrCodeGenerator.GenerateQrCode("");
        
        //Assert
        Assert.Throws<ArgumentNullException>(action);
    }

}