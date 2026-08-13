using WorkOrderManagement.Domain.Employees;

namespace WorkOrderManagement.Domain.Tests;

public class ValueObjectTests
{
    [Theory]
    [InlineData("user@example.com", "user@example.com")]
    [InlineData("  test.name+alias@domain.co.uk  ", "test.name+alias@domain.co.uk")]
    public void Email_Create_ValidEmail_ReturnsEmailValueObject(string input, string expected)
    {
        var result = Email.Create(input);
        Assert.False(result.IsError);
        Assert.Equal(expected, result.Value.Value);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    public void Email_Create_InvalidEmail_ReturnsError(string invalidEmail)
    {
        var result = Email.Create(invalidEmail);
        Assert.True(result.IsError);
    }

    [Theory]
    [InlineData("+51 987654321", "+51 987654321")]
    [InlineData("123-456-7890", "123-456-7890")]
    public void Phone_Create_ValidPhone_ReturnsPhoneValueObject(string input, string expected)
    {
        var result = Phone.Create(input);
        Assert.False(result.IsError);
        Assert.Equal(expected, result.Value.Value);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("123")]
    public void Phone_Create_InvalidPhone_ReturnsError(string invalidPhone)
    {
        var result = Phone.Create(invalidPhone);
        Assert.True(result.IsError);
    }
}
