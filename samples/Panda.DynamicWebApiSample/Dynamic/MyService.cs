using System.Threading.Tasks;

namespace Panda.DynamicWebApiSample.Dynamic
{
    [Service("My.Server")]
    public class MyService
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