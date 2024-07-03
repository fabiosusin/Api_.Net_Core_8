using DTO.General.Base.Output;
using DTO.Interface;

namespace DTO.General.DAO.Output
{
    public class DAOActionResultOutput : BaseApiOutput
    {
        public DAOActionResultOutput(bool scs) : base(scs) { }

        public DAOActionResultOutput(string msg) : base(msg) { }
        public DAOActionResultOutput(IBaseData data) : base(true) => Data = data;

        public IBaseData Data { get; set; }

    }
}
