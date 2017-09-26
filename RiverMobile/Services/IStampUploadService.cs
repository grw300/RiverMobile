using RiverMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RiverMobile.Services
{
    public interface IStampUploadService
    {
        void UploadStamp(string stampFile);
    }
}
