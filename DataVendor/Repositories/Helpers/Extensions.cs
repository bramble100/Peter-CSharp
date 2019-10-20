using System;

namespace Repositories.Helpers
{
    public static class Extensions
    {
        public static string WrapWithQuotes(this object obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            return $"\"{obj.ToString()}\"";
        }
    }
}
