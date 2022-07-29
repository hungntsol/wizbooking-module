using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace EFCore.Persistence.Data;

public static class EntityExtension
{
    public static EntityKey? GetEntityKey<T>(this DbContext context, T entity) where T : class
    {
        var oc = ((IObjectContextAdapter)context).ObjectContext;
        if (entity is not null && oc.ObjectStateManager.TryGetObjectStateEntry(entity, out ObjectStateEntry ose))
        {
            return ose.EntityKey;
        }

        return null;
    }
}

