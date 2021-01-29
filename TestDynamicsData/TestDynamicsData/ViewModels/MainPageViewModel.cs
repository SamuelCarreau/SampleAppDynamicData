using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using TestDynamicsData.Models;
using TestDynamicsData.Helpers;

namespace TestDynamicsData.ViewModels
{
    public class MainPageViewModel : ReactiveObject, IDisposable
    {
        private readonly SourceCache<GPU, Guid> source;
        private readonly CompositeDisposable cleanUp;
        private IDisposable loader;

        private ReadOnlyObservableCollection<Grouping<GPU, Guid, Manifacturer>> gpus;

        public ReadOnlyObservableCollection<Grouping<GPU, Guid, Manifacturer>> Gpus
        {
            get { return gpus; }
            set { this.RaiseAndSetIfChanged(ref gpus, value); }
        }

        private bool showSoldout = true;
        public bool ShowSoldout
        {
            get { return showSoldout; }
            set { this.RaiseAndSetIfChanged(ref showSoldout, value); }
        }

        public MainPageViewModel()
        {
            cleanUp = new CompositeDisposable();

            source = new SourceCache<GPU, Guid>(t => t.Id);
            source.AddOrUpdate(new List<GPU>
            {
                new GPU { Name = "ASUS ROG STRIX GeForce RTX 3060", Price = 799.99 , Manifacturer = Manifacturer.Asus, IsSoldOut = true },
                new GPU { Name = "EVGA GeForce RTX 3070 XC3 ULTRA GAMING", Price = 949.99 , Manifacturer = Manifacturer.EVGA, IsSoldOut = true },
                new GPU { Name = "ASUS Dual GeForce RTX 3060 Ti DUAL-RTX3060TI-O8G 8GB 256-Bit GDDR6", Price = 649.99 , Manifacturer = Manifacturer.Asus, IsSoldOut = true },
                new GPU { Name = "MSI GeForce RTX 3070 DirectX 12 RTX 3070 SUPRIM X 8G 8GB 256-Bit GDDR6", Price = 889.99 , Manifacturer = Manifacturer.MSI, IsSoldOut = true },
                new GPU { Name = "GIGABYTE GeForce RTX 3080 DirectX 12 GV-N3080GAMING OC-10GD 10GB 320-Bit GDDR6X", Price = 1055.99 , Manifacturer = Manifacturer.Gigabyte, IsSoldOut = true },
                new GPU { Name = "MSI GeForce RTX 3060 Ti DirectX 12 RTX 3060 Ti GAMING X TRIO 8GB 256-Bit GDDR6", Price = 689.99 , Manifacturer = Manifacturer.MSI, IsSoldOut = true }
            });

            var soldOutFilter = this
            .WhenAnyValue(x => x.ShowSoldout)
            .Select(BuildSoldOutFilter);

            var filterSubcription = soldOutFilter
                .Do(_ =>
                {
                    Gpus = null;
                    loader?.Dispose();
                    loader = source
                    .Connect()
                    .Filter(soldOutFilter)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .GroupOnProperty(x => x.Manifacturer)
                    .Transform(group => new Grouping<GPU, Guid, Manifacturer>(group))
                    .Bind(out gpus)
                    .Do(group => this.RaisePropertyChanged(nameof(Gpus)))
                    .Subscribe(new DebugObserver<IChangeSet<Grouping<GPU, Guid, Manifacturer>, Manifacturer>>("loader"))
                    .DisposeWith(cleanUp);
                })
                .Subscribe()
                .DisposeWith(cleanUp);   
        }

        private Func<GPU, bool> BuildSoldOutFilter(bool showSoldOut)
        {
            return gpu => !gpu.IsSoldOut || gpu.IsSoldOut && showSoldOut;
        }

        public void Dispose()
        {
            cleanUp.Dispose();
        }

    }
}
