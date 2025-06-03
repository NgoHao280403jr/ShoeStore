public class ProductRepository : IProductRepository
{
    private readonly IElasticClient _elasticClient;
    public ProductRepository(IElasticClient elasticClient) { _elasticClient = elasticClient; }

    public async Task<List<Product>> SearchProductsAsync(string searchTerm, int page, int pageSize)
    {
        var response = await _elasticClient.SearchAsync<Product>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(p => p.Productname)
                        .Field(p => p.Productdescription)
                    )
                    .Query(searchTerm)
                )
            )
            .From((page - 1) * pageSize)
            .Size(pageSize)
        );
        return response.Documents.ToList();
    }
}
