using System.Collections.Generic;

namespace LabCenter.Shared.Models
{
    public class PagingModel<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public int Count { get; set; }
    }
}
