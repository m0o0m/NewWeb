using GL.Redis.Data.IRepository;
using GL.Redis.Data.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Redis.Data.Repository
{
    public class WebGateRepository : IWebGateRepository
    {
        private readonly IRedisClient _redisClient;

        public WebGateRepository(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public IList<GateInfo> GetAll()
        {
            using (var typedClient = _redisClient.GetTypedClient<GateInfo>())
            {
                return typedClient.GetAll();
            }
        }
    }
}
