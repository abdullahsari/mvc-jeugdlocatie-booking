using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Locs4Youth.Utils
{
    public enum MessageType
    {
        Success, Info, Error
    }

    public class Message
    {
        public static void Flash(TempDataDictionary tempData, string message, MessageType type = MessageType.Info)
        {
            tempData.Remove("Message");
            tempData.Remove("MessageStatus");
            tempData.Add("Message", message);
            tempData.Add("MessageStatus", type.ToString());
        }
    }
}