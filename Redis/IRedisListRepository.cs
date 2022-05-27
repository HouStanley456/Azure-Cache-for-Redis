using System;
namespace Redis;

public interface IRedisListRepository
{
    Task<IEnumerable<RedisDTO>> GetRedisList(Query request);
}

