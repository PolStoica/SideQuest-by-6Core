using System;
using System.Collections.Generic;
using System.Text;

namespace SideQuest.BLL.Services
{
    public interface ITimeProvider
    {
        DateTime UtcNow { get; }
    }
    
}
