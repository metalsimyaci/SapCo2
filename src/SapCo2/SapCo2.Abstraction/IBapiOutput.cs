using SapCo2.Abstraction.Model;

namespace SapCo2.Abstraction
{
    public interface IBapiOutput
    {
        #region Properties

        public BapiReturnParameter BapiReturn { get; set; }

        #endregion
    }
}
