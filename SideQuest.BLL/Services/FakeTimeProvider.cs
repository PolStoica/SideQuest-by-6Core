using System;
using System.Collections.Generic;
using System.Text;

namespace SideQuest.BLL.Services
{ 

    public class FakeTimeProvider : ITimeProvider
    {
        public DateTime UtcNow { get; set; }
    }
}
