using System;
using System.Collections.Generic;
using System.Text;

namespace SideQuest.BLL.Services
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
