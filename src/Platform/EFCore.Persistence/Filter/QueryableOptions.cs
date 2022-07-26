﻿using Microsoft.EntityFrameworkCore.Query;
using SharedCommon.Domain;

namespace EFCore.Persistence.Filter;

// TODO: use this options for query

public class QueryableOptions<T> where T : class, IEntityBase<T>
{
    public bool AsTracking { get; set; }

    public bool IgnoreFilter { get; set; }

    public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? Include { get; set; }

    public string? IncludePattern { get; set; }
}
