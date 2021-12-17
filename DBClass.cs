using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace products
{
    class DBClass
    {
        private static ProductEntities _context;
        public static ProductEntities GetContext()
        {
            if (_context == null) _context = new ProductEntities();
            return _context;
        }
        public static void ApplyDataBaseChange() => _context?.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
    }
}
