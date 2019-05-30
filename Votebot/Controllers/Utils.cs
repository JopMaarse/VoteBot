using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Votebot.Controllers
{
    public static class Utils
    {
        public static IEnumerable<string> SeparateOptions(string text)
        {
            return text.Split(ResourceController.GetSeparator())
                .Select(o => o.TrimStart().TrimEnd());
        }
    }
}
