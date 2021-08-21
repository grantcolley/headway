using Headway.Core.Helpers;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Headway.Core.Dynamic
{
    public class DynamicModel<T>
    {
        private readonly string idFieldName;
        private readonly string titleFieldName;
        private readonly DynamicTypeHelper<T> typeHelper;

        public DynamicModel(T model, Config config)
        {
            Model = model;
            Config = config;

            var idField = config.ConfigItems.FirstOrDefault(ci => ci.IsIdentity.HasValue && ci.IsIdentity.Value);

            if (idField != null)
            {
                idFieldName = idField.PropertyName;
            }

            var titleField = config.ConfigItems.FirstOrDefault(ci => ci.IsTitle.HasValue && ci.IsTitle.Value);

            if(titleField != null)
            {
                titleFieldName = titleField.PropertyName;
            }

            typeHelper = DynamicTypeHelper.Get<T>();

            BuildDynamicComponents();
        }

        public T Model { get; private set; }
        public Config Config { get; private set; }
        public DynamicContainer RootContainer { get; set; }
        public List<DynamicField> DynamicFields { get; private set; }

        public int Id { get { return Convert.ToInt32(typeHelper.GetValue(Model, idFieldName)); } }

        public string Title { get { return typeHelper.GetValue(Model, titleFieldName)?.ToString(); } }

        private void BuildDynamicComponents()
        {
            DynamicFields = new List<DynamicField>();

            var constantExpression = Expression.Constant(Model);

            DynamicFields = new List<DynamicField>((from p in typeHelper.SupportedProperties
                                join c in Config.ConfigItems on p.Name equals c.PropertyName
                                select CreateDynamicField(Model, constantExpression, p, c)).ToList());

            ComponentArgHelper.AddDynamicArgs(DynamicFields);

            RootContainer = CreateContainer(Config.Containers.Single(cc => cc.IsRootContainer));

            var fieldGroups = from df in DynamicFields
                                group df by df.ConfigContainerId into fieldGroup
                                select fieldGroup;

            MapDynamicContainerFields(RootContainer, fieldGroups);
        }

        private static DynamicField CreateDynamicField(T model, ConstantExpression expression, PropertyInfo propertyInfo, ConfigItem configItem)
        {
            var dynamicField = new DynamicField
            {
                Model = model,
                Label = configItem.Label,
                Order = configItem.Order,
                ConfigContainerId = configItem.ConfigContainer.ConfigContainerId,
                ComponentArgs = configItem.ComponentArgs,
                PropertyInfo = propertyInfo,
                PropertyName = propertyInfo.Name,
                DynamicComponentTypeName = configItem.Component,
                DynamicComponent = Type.GetType(configItem.Component),
                MemberExpression = Expression.Property(expression, propertyInfo.Name)
            };

            dynamicField.Parameters.Add("Field", dynamicField);

            return dynamicField;
        }

        private static DynamicContainer CreateContainer(ConfigContainer configContainer)
        {
            var dynamicContainer = new DynamicContainer
            {
                ContainerId = configContainer.ConfigContainerId,
                Row = configContainer.Row,
                Column = configContainer.Column,
                DynamicComponent = Type.GetType(configContainer.Container),
                DynamicContainerTypeName = configContainer.Container
            };

            dynamicContainer.Parameters.Add("Container", dynamicContainer);

            if(configContainer.ConfigContainers.Any())
            {
                foreach(var container in  configContainer.ConfigContainers)
                {
                    dynamicContainer.DynamicContainers.Add(CreateContainer(container));
                }
            }

            return dynamicContainer;
        }

        private static void MapDynamicContainerFields(DynamicContainer container, IEnumerable<IGrouping<int,DynamicField>> fieldGroups)
        {
            var fieldGroup = fieldGroups.SingleOrDefault(fg => fg.Key.Equals(container.ContainerId));

            if(fieldGroup != null)
            {
                container.DynamicFields.AddRange(fieldGroup.OrderBy(f => f.Order).ToList());
            }

            foreach(var c in container.DynamicContainers)
            {
                MapDynamicContainerFields(c, fieldGroups);
            }
        }
    }
}
