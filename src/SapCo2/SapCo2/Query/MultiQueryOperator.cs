using System.Collections.Generic;
using System.Linq;

namespace SapCo2.Query
{
    public class MultiQueryOperator
    {
        #region Variables

        private readonly AbapQuery _abapQuery;

        #endregion

        #region Methods

        #region Constructors

        public MultiQueryOperator(AbapQuery abapQuery)
        {
            _abapQuery = abapQuery;
        }

        #endregion


        public MultiQueryOperator AndInParanthesis(MultiQueryOperator opr)
        {
            _abapQuery.Query.Add(_abapQuery.Query.Any() ? " AND ( " : " ( ");
            _abapQuery.Query.AddRange(opr.GetQuery());
            _abapQuery.Query.Add(" )");

            return this;
        }

        public MultiQueryOperator And(QueryOperator opr)
        {
            if (opr.IsEmpty)
                return this;

            if (opr.ListFilled)
            {
                List<string> list = opr.GetQueryList();

                list[0] = $" AND {list[0]} ";
                _abapQuery.Query.AddRange(list);
                opr.ListFilled = false;
            }
            else
                _abapQuery.Query.Add($" AND {opr.GetQuery()}");

            return this;
        }

        public MultiQueryOperator OrInParanthesis(MultiQueryOperator opr)
        {
            _abapQuery.Query.Add(_abapQuery.Query.Any() ? " OR ( " : " ( ");
            _abapQuery.Query.AddRange(opr.GetQuery());
            _abapQuery.Query.Add(" )");

            return this;
        }

        public MultiQueryOperator Or(QueryOperator opr)
        {
            if (opr.IsEmpty)
                return this;

            if (opr.ListFilled)
            {
                List<string> list = opr.GetQueryList();

                list[0] = $" OR {list[0]} ";
                _abapQuery.Query.AddRange(list);
                opr.ListFilled = false;
            }
            else
                _abapQuery.Query.Add($" OR {opr.GetQuery()}");

            return this;
        }

        public List<string> GetQuery()
        {
            return _abapQuery.Query.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        }

        #endregion
    }
}
