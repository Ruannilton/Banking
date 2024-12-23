using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Banking.Onboarding.Domain.Abstractions;
using Banking.Onboarding.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Banking.Onboarding.Infrastructure;
internal class CustomerRepository : ICustomerRepository
{
    private const string tableNameEnv = "ONBOARDINGCUSTOMERINFO_TABLE_NAME";
    private readonly IAmazonDynamoDB dynamoClient;
    private readonly string tableName;
    public CustomerRepository(IAmazonDynamoDB dynamoClient, IConfiguration configuration)
    {
        this.dynamoClient = dynamoClient;
        tableName = configuration[tableNameEnv] ?? throw new Exception($"invalid {tableNameEnv}");
    }
    public async Task AddCustomer(CustomerInfo costumerInfo)
    {
        var obj = new Dictionary<string, AttributeValue>()
        {
            {"cpf", new AttributeValue(){S = costumerInfo.Cpf} },
            {"name", new AttributeValue(){S = costumerInfo.Name} },
            {"email", new AttributeValue(){S = costumerInfo.Email} },
        };

        var request = new PutItemRequest
        {
            TableName = tableName,
            Item = obj
        };

        await dynamoClient.PutItemAsync(request);
    }

    public async Task<CustomerInfo> GetCustomer(string cpf)
    {
        var request = new GetItemRequest
        {
            TableName = tableName,
            Key = new Dictionary<string, AttributeValue>()
            {
                {"cpf", new AttributeValue(){S = cpf} },
            },
            AttributesToGet = new() { "name", "email" }
        };

        var response = await dynamoClient.GetItemAsync(request);

        var name = response.Item["name"].S;
        var email = response.Item["email"].S;

        return new CustomerInfo(name, cpf, email);
    }

    void CreateTable()
    {
        var createRequest = new CreateTableRequest()
        {
            TableName = tableName,
            AttributeDefinitions = new()
            {
                new AttributeDefinition()
                {
                    AttributeName = "cpf",
                    AttributeType = "S"
                },
                new AttributeDefinition()
                {
                    AttributeName = "name",
                    AttributeType = "S"
                },
                new AttributeDefinition()
                {
                    AttributeName = "email",
                    AttributeType = "S"
                },
            },
            KeySchema = new()
            {
                new KeySchemaElement()
                {
                    AttributeName = "cpf",
                    KeyType = "HASH"
                }
            }
        };

        try
        {
            dynamoClient.CreateTableAsync(createRequest).Wait();
        }
        catch (ResourceInUseException)
        {
        }
    }
}
