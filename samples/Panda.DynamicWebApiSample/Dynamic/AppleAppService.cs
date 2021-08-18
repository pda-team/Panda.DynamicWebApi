using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;
using Panda.DynamicWebApiSample.Dtos;

namespace Panda.DynamicWebApiSample.Dynamic
{

    [DynamicWebApi]
    // [Authorize]
    public class AppleAppService: IDynamicWebApi
    {
        private static readonly Dictionary<int, string> Apples = new Dictionary<int, string>()
        {
            [1] = "Big Apple",
            [2] = "Small Apple"
        };

        [AllowAnonymous]
        public async Task UpdateAppleAsync(UpdateAppleDto dto)
        {
            await Task.Run(() => {
                if (Apples.ContainsKey(dto.Id))
                {
                    Apples[dto.Id] = dto.Name;
                }
            });

        }

        /// <summary>
        /// Get An Apple.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public string Get(int id)
        {
            if (Apples.ContainsKey(id))
            {
                return Apples[id];
            }
            else
            {
                return "No Apple!";
            }
        }

        /// <summary>
        /// Get  All Apple Async.
        /// </summary>
        /// <returns></returns>
        //[NonDynamicMethod]
        public IEnumerable<string> GetAllAsync()
        {
            return Apples.Values;
        }

        /// <summary>
        /// Get All Apple.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
        {
            return Apples.Values;
        }

        /// <summary>
        /// Get All Apple.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetBigApple()
        {
            return Apples.Values;
        }

        /// <summary>
        /// Update Apple
        /// </summary>
        /// <param name="dto"></param>
        public void Update(UpdateAppleDto dto)
        {
            if (Apples.ContainsKey(dto.Id))
            {
                Apples[dto.Id] = dto.Name;
            }
        }

        /// <summary>
        /// Delete Apple
        /// </summary>
        /// <param name="id">Apple Id</param>
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            if (Apples.ContainsKey(id))
            {
                Apples.Remove(id);
            }
        }

    }
}