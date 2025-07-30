using System.Data;
using Dapper;
using Domain.OrderProduct.Aggregates;
using Domain.OrderProduct.Repositories;

namespace Infrastructure.Persistence.Dapper.Repositories;

public class ProductDapperRepository(IDbConnection connection): IProductQueryRepository
{
    public async Task<Product?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await connection.QueryFirstOrDefaultAsync<Product>(
            $"SELECT * FROM [Product] WHERE Name = @name", new { name });
    }
}