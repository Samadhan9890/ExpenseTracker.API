using ExpenseTracker.Services.Data;

namespace ExpenseTracker.Services.Utilities
{
    public class EntityPropertyUpdaterHelperWithEF<TEntity> where TEntity : class
    {
        private readonly AppDBContext _appDBContext;

        public EntityPropertyUpdaterHelperWithEF(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        /// <summary>
        /// Update Model property with new property
        /// </summary>
        /// <param name="existingEntity"></param>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public TEntity UpdateEntityModifiedProperties(TEntity existingEntity, TEntity newEntity)
        {
            var entityProperties = typeof(TEntity).GetProperties();

            foreach (var property in entityProperties)
            {
                var newValue = property.GetValue(newEntity);
                var existingValue = property.GetValue(existingEntity);

                if (newValue != null && !newValue.Equals(existingValue))
                {
                    property.SetValue(existingEntity, newValue);
                    _appDBContext.Entry(existingEntity).Property(property.Name).IsModified = true;
                }
            }

            return existingEntity;
        }

        /// <summary>
        /// Update model property with provided property
        /// </summary>
        /// <param name="existingEntity"></param>
        /// <param name="newEntity"></param>
        /// <param name="userProvidedProperties"></param>
        /// <returns></returns>
        public TEntity UpdateEntityModifiedProperties(TEntity existingEntity, TEntity newEntity, string userProvidedProperties)
        {
            if (!string.IsNullOrWhiteSpace(userProvidedProperties))
            {
                var entityProperties = typeof(TEntity).GetProperties();
                var providedPropertyNames = userProvidedProperties.Split(',');
                foreach (var propertyName in providedPropertyNames)
                {
                    var property = entityProperties.FirstOrDefault(p => string.Equals(p.Name, propertyName.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (property != null)
                    {
                        var newValue = property.GetValue(newEntity);
                        var existingValue = property.GetValue(existingEntity);
                        if (newValue != null && !newValue.Equals(existingValue))
                        {
                            property.SetValue(existingEntity, newValue);
                            _appDBContext.Entry(existingEntity).Property(property.Name).IsModified = true;
                        }
                    }
                }
            }

            return existingEntity;
        }
    }

}
