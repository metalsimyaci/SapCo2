using System.Collections.Generic;

namespace SapCo2.Query
{
    public class AbapQuery
    {
        #region Variables

        private readonly MultiQueryOperator _set;

        #endregion

        #region Properties

        public List<string> Query { get; set; } = new List<string>();

        #endregion

        #region Methods

        public AbapQuery()
        {
            _set = new MultiQueryOperator(this);
        }

        public MultiQueryOperator Set(QueryOperator sapQueryOperator = null)
        {
            if (sapQueryOperator == null)
            {
                return _set;
            }

            if (sapQueryOperator.ListFilled)
            {
                Query.AddRange(sapQueryOperator.GetQueryList());
                sapQueryOperator.ListFilled = false;
            }
            else
                Query.Add(sapQueryOperator.GetQuery());

            sapQueryOperator.Dispose();

            return _set;
        }

        #endregion
    }
}
