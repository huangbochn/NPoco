using System;
using System.Linq.Expressions;

namespace NPoco.FluentMappings
{
    public class Map<T> : IMap
    {
        private readonly TypeDefinition _petaPocoTypeDefinition;

        public Map() : this(new TypeDefinition(typeof(T)))
        {
        }

        public Map(TypeDefinition petaPocoTypeDefinition)
        {
            _petaPocoTypeDefinition = petaPocoTypeDefinition;
        }

        public Map<T> TableName(string tableName)
        {
            _petaPocoTypeDefinition.TableName = tableName;
            return this;
        }

        public Map<T> Columns(Action<ColumnConfigurationBuilder2<T>> columnConfiguration)
        {
            return Columns(columnConfiguration, null);
        }

        public Map<T> Columns(Action<ColumnConfigurationBuilder2<T>> columnConfiguration, bool? explicitColumns)
        {
            _petaPocoTypeDefinition.ExplicitColumns = explicitColumns;
            columnConfiguration(new ColumnConfigurationBuilder2<T>(_petaPocoTypeDefinition.ColumnConfiguration));
            return this;
        }

        public Map<T> PrimaryKey(Expression<Func<T, object>> column, string sequenceName)
        {
            var propertyInfo = PropertyHelper<T>.GetProperty(column);
            return PrimaryKey(propertyInfo.Name, sequenceName);
        }

        public Map<T> PrimaryKey(Expression<Func<T, object>> column)
        {
            _petaPocoTypeDefinition.AutoIncrement = true;
            return PrimaryKey(column, null);
        }

        public Map<T> PrimaryKey(Expression<Func<T, object>> column, bool autoIncrement)
        {
            var propertyInfo = PropertyHelper<T>.GetProperty(column);
            return PrimaryKey(propertyInfo.Name, autoIncrement);
        }

        public Map<T> CompositePrimaryKey(params Expression<Func<T, object>>[] columns)
        {
            var columnNames = new string[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                columnNames[i] = PropertyHelper<T>.GetProperty(columns[i]).Name;
            }

            _petaPocoTypeDefinition.PrimaryKey = string.Join(",", columnNames);
            return this;
        }

        public Map<T> PrimaryKey(string primaryKeyColumn, bool autoIncrement)
        {
            _petaPocoTypeDefinition.PrimaryKey = primaryKeyColumn;
            _petaPocoTypeDefinition.AutoIncrement = autoIncrement;
            return this;
        }

        public Map<T> PrimaryKey(string primaryKeyColumn, string sequenceName)
        {
            _petaPocoTypeDefinition.PrimaryKey = primaryKeyColumn;
            _petaPocoTypeDefinition.SequenceName = sequenceName;
            return this;
        }

        public Map<T> PrimaryKey(string primaryKeyColumn)
        {
            return PrimaryKey(primaryKeyColumn, null);
        }

        TypeDefinition IMap.TypeDefinition
        {
            get { return _petaPocoTypeDefinition; }
        }
    }
}