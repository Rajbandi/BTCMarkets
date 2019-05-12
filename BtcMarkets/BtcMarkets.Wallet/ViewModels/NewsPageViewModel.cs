using BtcMarkets.Wallet.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BtcMarkets.Wallet.ViewModels
{
    public class NewsPageViewModel : BaseViewModel
    {
        public NewsPageViewModel() : base()
        {
            Title = "Latest News";
            
            LoadMarketNews();
        }

        private MarketNewsItem _selectedItem;
        public MarketNewsItem SelectedNewsItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value, nameof(SelectedNewsItem));
        }
        public ObservableCollection<MarketNewsItem> MarketNews { get; private set; }

        private void LoadMarketNews()
        {
            var appData = AppData.Current;
            var marketNews = appData?.Settings?.Config?.MarketNews;
            if (marketNews != null)
            {
                MarketNews = new ObservableCollection<MarketNewsItem>(marketNews);
            }
            else
            {
                MarketNews = new ObservableCollection<MarketNewsItem>();
            }

            OnPropertyChanged("MarketNews");
        }

        public ICommand OpenCommand
        {
            get
            {
                return new Command(() =>
                {
                    var item = SelectedNewsItem;


                });
            }
        }
    }
}
