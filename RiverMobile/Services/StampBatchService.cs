using System;
using System.Threading.Tasks;

namespace RiverMobile.Services
{
    public class StampBatchService : IStampBatchService
    {
        public Task BatchStamps()
        {
            Console.WriteLine("In theory, we take the stamps together and send them to background URL sessions per platform. Will have to think abou this more.");
            throw new NotImplementedException();
        }
    }
}
