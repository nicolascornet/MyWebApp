using MyWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApp.UnitTests
{
    public class MockDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get; set; } = DateTime.Now;
    }
}
