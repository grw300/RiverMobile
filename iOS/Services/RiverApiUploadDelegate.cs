using Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiverMobile.iOS.Services
{
    public class RiverApiUploadDelegate : NSUrlSessionTaskDelegate, IRiverApiUploadDelegate
    {
        public override void DidCompleteWithError(NSUrlSession session, NSUrlSessionTask task, NSError error)
        {
            if (error != null)
            {
                Console.WriteLine("shit went pear");
                Console.WriteLine(task.TaskIdentifier);
            }
        }

        public override void DidBecomeInvalid(NSUrlSession session, NSError error)
        {
            Console.WriteLine("DidBecomeInvalid" + (error == null ? "undefined" : error.Description));
        }

        public override void DidFinishEventsForBackgroundSession(NSUrlSession session)
        {
            Console.WriteLine("DidFinishEventsForBackgroundSession");
        }

        public override void DidSendBodyData(NSUrlSession session, NSUrlSessionTask task, long bytesSent, long totalBytesSent, long totalBytesExpectedToSend)
        {
            var syncPercentage = ((double)totalBytesSent / totalBytesExpectedToSend) * 100.0;
            var taskId = Convert.ToInt32(task.TaskIdentifier);
            Console.WriteLine($"{taskId} is at {syncPercentage}%");
        }
    }
}
