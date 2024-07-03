using System.Collections.Generic;

namespace DTO.General.Base.Output
{
    public class BaseListOutput<T> : BaseApiOutput
    {
        public BaseListOutput(string msg) : base(msg) { }
        public BaseListOutput(IEnumerable<T> items, long total) : base(true)
        {
            Items = items;
            Total = total;
        }

        public IEnumerable<T> Items { get; set; }
        public long Total { get; set; }
    }
}
