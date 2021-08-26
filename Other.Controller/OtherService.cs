using ServiceAbsAttribute;
using System;
using System.Threading.Tasks;

namespace Other.Controller
{
    [Service("Other.Server")]
    public class OtherService
    {
        public int Show()
        {
            return 100;
        }

        public Task<int> TaskIntAsync()
        {
            return Task.FromResult(100);
        }
    }
}
