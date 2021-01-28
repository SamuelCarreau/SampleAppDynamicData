using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDynamicsData.Models;
using ReactiveUI;
using Xamarin.Forms;
using ReactiveUI.XamForms;
using TestDynamicsData.ViewModels;
using System.Reactive.Disposables;

namespace TestDynamicsData.Pages
{
    public partial class MainPage : ReactiveContentPage<MainPageViewModel>
    {

        public MainPage()
        {
            InitializeComponent();
            this.ViewModel = new MainPageViewModel();
            this.WhenActivated((CompositeDisposable viewDisposable) => 
            {
                this.OneWayBind(ViewModel, vm => vm.Gpus, view => view.gpuCollectionview.ItemsSource)
                .DisposeWith(viewDisposable);

                this.Bind(ViewModel, vm => vm.ShowSoldout, view => view.isSoldOutSwitch.IsToggled)
                .DisposeWith(viewDisposable);
            });
        }
    }
}
