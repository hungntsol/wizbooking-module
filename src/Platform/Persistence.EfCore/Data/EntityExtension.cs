using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EfCore.Data;

public static class EntityExtension
{
	public static EntityKey? GetEntityKey<T>(this DbContext context, T? entity) where T : class
	{
		var oc = ((IObjectContextAdapter)context).ObjectContext;
		if (entity is not null && oc.ObjectStateManager.TryGetObjectStateEntry(entity, out var ose))
		{
			return ose.EntityKey;
		}

		return null;
	}
}
