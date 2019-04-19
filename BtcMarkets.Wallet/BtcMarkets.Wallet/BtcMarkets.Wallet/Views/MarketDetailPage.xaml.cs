using BtcMarkets.Wallet.Models;
using BtcMarkets.Wallet.ViewModels;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BtcMarkets.Wallet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketDetailPage : ContentPage
    {
        public MarketDetailViewModel ViewModel { get; private set; }

        public MarketDetailPage()
        {
            InitializeComponent();
            var market = AppData.Current.Market;
            BindingContext = ViewModel = new MarketDetailViewModel(market);
        }

        public async void InitChart()
        {
            await Task.Delay(100);
            await ViewModel.LoadReport();
            var controller = new PlotController();

            controller.UnbindAll();
            controller.BindTouchDown(new DelegatePlotCommand<OxyTouchEventArgs>((view, controller1, args) => controller1.AddTouchManipulator(view, new TapTouchManipulator(view, this.TapCommand), args)));
            plotView.Controller = controller;

            //  plotView.Model = ViewModel.MarketChart;

        }
        private Command<IEnumerable<HitTestResult>> tapCommand;

        public ICommand TapCommand
        {
            get
            {
                return this.tapCommand = this.tapCommand ?? new Command<IEnumerable<HitTestResult>>(this.DoTapCommand);
            }
        }

        private void DoTapCommand(IEnumerable<HitTestResult> results)
        {
            var resultsArray = results.ToArray();

            if (!resultsArray.Any())
                return;

            var result = resultsArray.First();
            if (result == null)
                return;

            var dp = result.Item as HighLowItem;
            if (dp == null)
                return;


            this.DisplayAlert("Info", $"Touched: {dp.Open},{dp.Close},{dp.High},{dp.Low}", "Close");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitChart();
        }

    }

}