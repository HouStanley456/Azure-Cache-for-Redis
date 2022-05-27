namespace Redis.Repository;

public class RedisTestRepository : IRedisListRepository
{
    private readonly IConfiguration _configuration;

    public RedisTestRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<RedisDTO>> GetRedisList(Query request)
    {
        //連線字串
        var ConnectionStrings = _configuration.GetConnectionString("Postgre");

        var sql = @$"";
        using (var conn = new NpgsqlConnection(ConnectionStrings))
        {
            return await conn.QueryAsync<RedisDTO>(sql);
        }

    }
}


