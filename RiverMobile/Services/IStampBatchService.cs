using RiverMobile.Models;
using System;
using System.Threading.Tasks;

namespace RiverMobile.Services
{
    public interface IStampBatchService
    {
        void BatchStamps(Stamp stamp);
    }
}
