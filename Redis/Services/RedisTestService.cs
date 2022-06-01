namespace Redis.Services;

public class RedisTestService
{
    private readonly IConfiguration _configuration;
    private IRedisListRepository repository;

    public RedisTestService(IConfiguration configuration)
    {
        _configuration = configuration;
        repository = new RedisTestRepository(_configuration);
    }

    public async Task<List<RedisListVM>> GetRedisList(Query request)
    {
        //Redis 測試 {
        //Redis 設定位置
        ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis"));
        //快取儲存時間
        TimeSpan saveTime = TimeSpan.FromSeconds(300);
        // 使用 Redis database
        // Database 預設為-1
        IDatabase cache = conn.GetDatabase();
        // }

        List<RedisListVM> RedisRecordList = new();
        // 要把request序列化 不然會被判定為同一個RedisKeyname
        string RedisKeyName = $"{JsonConvert.SerializeObject(request)}";
        Console.WriteLine(JsonConvert.SerializeObject(request));
        var cachedDeviceRecord = cache.StringGet(RedisKeyName);
        if (!string.IsNullOrEmpty(cachedDeviceRecord))
        {
            Console.WriteLine("有跑進Redis呦");
            RedisRecordList = JsonConvert.DeserializeObject<List<RedisListVM>>(cachedDeviceRecord);

            return RedisRecordList;
        }
        else
        {
            Console.WriteLine("沒有跑進Redis呦");
            var response = await repository.GetRedisList(request);
            var result = response.Select(redis =>
            {
                var log = new RedisListVM
                {
                    FkAlertId = redis.fk_alert_id,
                    RawData = redis.raw_data,
                    CreatedTime = redis.created_time,
                };
                return log;
            }).ToList();

            RedisRecordList = result;
            cache.StringSet(RedisKeyName, JsonConvert.SerializeObject(RedisRecordList), saveTime);

            return RedisRecordList;
        }
    }
}

