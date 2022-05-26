using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivacyBudgetServerTests.Controllers.Mock
{
    public interface IMockDatabaseCollection<T> : IMongoCollection<T>
    {
        IFindFluent<T, T> Find(FilterDefinition<T> filter, FindOptions options);
        IFindFluent<T, T> Project(ProjectionDefinition<T, T> projection);
        IFindFluent<T, T> Skip(int skip);
        IFindFluent<T, T> Limit(int limit);
        IFindFluent<T, T> Sort(SortDefinition<T> sort);
    }
}
