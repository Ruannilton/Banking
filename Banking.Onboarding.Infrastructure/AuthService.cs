using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Banking.Onboarding.Infrastructure;
internal class AuthService : IAuthService
{

    private readonly IAmazonCognitoIdentityProvider cognitoClient;
    private readonly string userPoolId;
    private const string poolIdEnvName = "BANKINGUSERPOOL_USER_POOL_ID";

    public AuthService(IAmazonCognitoIdentityProvider cognitoClient, IConfiguration configuration)
    {
        this.cognitoClient = cognitoClient;
        userPoolId = configuration[poolIdEnvName] ?? throw new Exception($"invalid {poolIdEnvName}");
    }


    public async Task<bool> CreateCustomerAccount(CustomerInfo customerInfo)
    {
        var request = new AdminCreateUserRequest
        {
            UserPoolId = userPoolId,
            Username = customerInfo.Email,
            UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "email", Value = customerInfo.Email },
                new AttributeType { Name = "name", Value = customerInfo.Name },
                new AttributeType { Name = "email_verified", Value = "false" }
            },
            DesiredDeliveryMediums = new List<string> { "EMAIL" },
            MessageAction = MessageActionType.SUPPRESS
        };

        var response = await cognitoClient.AdminCreateUserAsync(request);

        return response.HttpStatusCode is >= HttpStatusCode.OK and < HttpStatusCode.Ambiguous;
    }
}
