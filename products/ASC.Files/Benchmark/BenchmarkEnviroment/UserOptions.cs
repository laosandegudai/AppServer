using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Files.Benchmark
{
    public class UserOptions
    {
        public const string User = "User";
        public Guid ParentId { get; set; }
        public Guid Id { get; set; }
        public int TenantId { get; set; }
    }
}
