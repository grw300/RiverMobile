using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiverMobile.Services;
using Foundation;
using UIKit;
using RiverMobile.Models;
using System.Threading.Tasks;
using RiverMobile.Messages;
using RiverMobile.Helpers;
using System.IO;

namespace RiverMobile.iOS.Services
{
    public class StampUploadService : IStampUploadService
    {
        const string BackgroundSessionId = "com.River.BackgroundSession";
        readonly NSUrl StampsUrl = new Uri(new Uri(Settings.RiverApiBaseAddress), "stamps");

        Lazy<NSUrlSession> urlSession = new Lazy<NSUrlSession>(() => InitSyncSession());
        IMessageService messageService;

        public StampUploadService(
                IMessageService messageService,
                IRiverApiUploadDelegate riverApiUploadDelegate
            )
        {
            this.messageService = messageService;

            var configuration = NSUrlSessionConfiguration
                .CreateBackgroundSessionConfiguration("com.River.BackgroundSession");

            var session = NSUrlSession.FromConfiguration(configuration, riverApiUploadDelegate, NSOperationQueue.MainQueue);
        }

        public void UploadStamp(string stampFile)
        {
            var id = Guid.NewGuid().ToString();

            NSMutableUrlRequest request = new NSMutableUrlRequest(StampsUrl)
            {
                HttpMethod = "POST",
                ["Content-Type"] = "application/vnd.api+json",
                ["FileName"] = Path.GetFileName(stampFile)
            };

            var uploadTask = urlSession.Value.CreateUploadTask(request, NSUrl.FromFilename(stampFile));

            uploadTask.Resume();
        }

        static NSUrlSession InitSyncSession()
        {
            ////See URL below for configuration options
            ////https://developer.apple.com/library/ios/documentation/Foundation/Reference/NSURLSessionConfiguration_class/index.html

            using (var config = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration(BackgroundSessionId))
            {
                config.HttpMaximumConnectionsPerHost = 4; //iOS Default is 4
                config.TimeoutIntervalForRequest = 600.0; //30min allowance; iOS default is 60 seconds.
                config.TimeoutIntervalForResource = 120.0; //2min; iOS Default is 7 days

                return NSUrlSession.FromConfiguration(config, new RiverApiUploadDelegate() as IRiverApiUploadDelegate, new NSOperationQueue());
            }
        }
    }
}