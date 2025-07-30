using System.Data;
using Dapper;
using Domain.Customer.Aggregates;
using Domain.Customer.Repositories;

namespace Infrastructure.Persistence.Dapper.Repositories;

public class CustomerDapperRepository(IDbConnection connection): ICustomerQueryRepository
{
    public async Task<Customer?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await connection.QueryFirstOrDefaultAsync<Customer>(
            $"SELECT * FROM [Customer] WHERE Name = @name", new { name });
    }
}