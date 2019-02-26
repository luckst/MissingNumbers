using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PuntosColombia.MissingNumbers.Models
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            Message = string.Empty;
            ShowMessage = false;
            MessageType = MessageTypeEnum.none;
        }
        public bool IsEdit { get; set; }
        public MessageTypeEnum MessageType { get; set; }
        public string Message { get; set; }
        public bool ShowMessage { get; set; }
    }
    public enum MessageTypeEnum
    {
        success = 1,
        danger = 2,
        warning = 3,
        none = 4
    }
}
