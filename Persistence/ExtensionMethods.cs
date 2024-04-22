using FastMember;
using Npgsql;

namespace robot_controller_api.Persistence
{
    public static class ExtensionMethods
    {
        public static void MapTo<T>(this NpgsqlDataReader dr, T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var fastMember = TypeAccessor.Create(entity.GetType());
            var props = fastMember.GetMembers().Select(x => x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < dr.FieldCount; i++)
            {
                var prop = props.FirstOrDefault(x => x.Equals(dr.GetName(i), StringComparison.OrdinalIgnoreCase));
                
                if (!string.IsNullOrEmpty(prop))
                    fastMember[entity, prop] = dr.IsDBNull(i) ? null : dr.GetValue(i);
            }
        }
    }
}
