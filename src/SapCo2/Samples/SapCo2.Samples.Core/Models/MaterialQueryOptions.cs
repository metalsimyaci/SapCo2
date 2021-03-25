namespace SapCo2.Samples.Core.Models
{
    public class MaterialQueryOptions
    {
        #region Variables

        private bool _includeAll;
        private bool _includeDefinition;
        private bool _includeMaterialCategory;

        #endregion

        #region Properties

        public bool IncludeAll
        {
            get
            {
                return _includeAll;
            }
            set
            {
                _includeAll = value;
                if (!value)
                    return;
                _includeDefinition = true;
                _includeMaterialCategory = true;
            }
        }

        public bool IncludeDefinition
        {
            get => _includeDefinition;
            set
            {
                _includeDefinition = value;
                SetIncludeAllValue();
            }
        }

        public bool IncludeMaterialCategory
        {
            get => _includeMaterialCategory;
            set
            {
                _includeMaterialCategory = value;
                SetIncludeAllValue();
            }
        }

        #endregion

        #region Methods

        private void SetIncludeAllValue()
        {
            _includeAll = _includeMaterialCategory && _includeDefinition;
        }

        #endregion
    }
}