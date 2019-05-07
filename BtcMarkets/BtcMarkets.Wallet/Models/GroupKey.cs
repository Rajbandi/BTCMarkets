namespace BtcMarkets.Wallet.Models
{
    public class GroupKey : BaseBindableObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, nameof(Name));
            }
        }

        private string _helpText;
        public string HelpText
        {
            get => _helpText;
            set
            {
                SetProperty(ref _helpText, value, nameof(HelpText));
            }
        }

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value, nameof(Code));
            }
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                SetProperty(ref _isExpanded, value, nameof(IsExpanded));
                if (ExpandIcon != null)
                {
                    ExpandIcon.Value = value;
                }
            }
        }

        private ToggleImage _expandIcon;
        public ToggleImage ExpandIcon
        {
            get => _expandIcon;
            set
            {
                SetProperty(ref _expandIcon, value, nameof(ExpandIcon));
            }
        }

        public GroupKey()
        {

            IsExpanded = true; //default is expanded

            ExpandIcon = new ToggleImage
            {
                OnImage = "expand_less",
                OffImage = "expand_more",
                Value = IsExpanded
            };
        }
        public GroupKey(string code, string name) : this()
        {
            Code = code;
            Name = name;
        }
    }
}
