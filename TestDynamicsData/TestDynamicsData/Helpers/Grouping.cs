using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;

namespace TestDynamicsData.Helpers
{
    //https://stackoverflow.com/questions/52094572/dynamicdata-how-to-bind-to-grouped-data
    public class Grouping<TElement,TKey, TGroupKey> : ObservableCollectionExtended<TElement>, IGrouping<TGroupKey, TElement> , IDisposable
    {
        private readonly IDisposable cleanUp;
        public TGroupKey Key { get; private set; }
        public Grouping(IGroup<TElement,TKey, TGroupKey> group)
        {

            if(group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Key = group.Key;
            cleanUp = group
                .Cache
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(this)
                .DisposeMany()
                .Subscribe();
        }
        public Grouping(IGroup<TElement, TKey, TGroupKey> group, Func<TElement,bool> filter)
        {

            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Key = group.Key;
            cleanUp = group
                .Cache
                .Connect()
                .Filter(filter)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(this)
                .DisposeMany()
                .Subscribe();
        }

        public Grouping(IGroup<TElement, TKey, TGroupKey> group, SortExpressionComparer<TElement> comparer)
        {
            
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Key = group.Key;
            cleanUp = group
                .Cache
                .Connect()
                .Sort(comparer)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(this)
                .DisposeMany()
                .Subscribe();
        }

        public Grouping(IGroup<TElement, TKey, TGroupKey> group, Func<TElement, bool> filter, SortExpressionComparer<TElement> comparer)
        {

            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            Key = group.Key;
            cleanUp = group
                .Cache
                .Connect()
                .Filter(filter)
                .Sort(comparer)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(this)
                .DisposeMany()
                .Subscribe();
        }

        public void Dispose()
        {
            cleanUp?.Dispose();
        }
    }   
}
