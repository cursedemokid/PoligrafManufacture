using ProjectSecurity;
using Xunit;

public class PasswordSecurityTests
{
    [Fact]
    public void Salted_hashes_verify_the_password_without_storing_it()
    {
        const string password = "synthetic-Пример-42";
        var first = PasswordSecurity.Hash(password);
        var second = PasswordSecurity.Hash(password);
        Assert.NotEqual(first, second);
        Assert.DoesNotContain(password, first);
        Assert.True(PasswordSecurity.Verify(password, first));
        Assert.False(PasswordSecurity.Verify("wrong-password", first));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("plaintext")]
    [InlineData("pbkdf2-sha256$999999999$salt$hash")]
    [InlineData("pbkdf2-sha256$600000$invalid-base64$invalid-base64")]
    public void Legacy_or_malformed_values_are_rejected(string stored)
        => Assert.False(PasswordSecurity.Verify("plaintext", stored));
}
