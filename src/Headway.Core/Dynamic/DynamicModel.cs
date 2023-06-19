﻿using Headway.Core.Constants;
using Headway.Core.Extensions;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Headway.Core.Dynamic
{
    public class DynamicModel<T> where T : class, new()
    {
        private readonly string idFieldName;
        private readonly string titleFieldName;
        
        public DynamicModel(T model, Config config)
        {
            Model = model;
            Config = config;
            Helper = DynamicTypeHelper.Get<T>();

            var idField = config.ConfigItems.FirstOrDefault(ci => ci.IsIdentity);

            if (idField != null)
            {
                idFieldName = idField.PropertyName;
            }

            var titleField = config.ConfigItems.FirstOrDefault(ci => ci.IsTitle);

            if(titleField != null)
            {
                titleFieldName = titleField.PropertyName;
            }

            SetStaticFields();

            BootstrapFlow();

            BuildDynamicComponents();
        }

        public T Model { get; private set; }
        public Config Config { get; private set; }
        public IFlowContext FlowContext { get; private set; }
        public DynamicTypeHelper<T> Helper { get; private set; }
        public List<DynamicContainer> RootContainers { get; set; }
        public List<DynamicField> DynamicFields { get; private set; }

        public int Id { get; private set; }

        public string Title { get; private set; }

        public bool IsValid
        {
            get 
            {
                foreach(var field in DynamicFields)
                {
                    if(field.ValidationMessagesCount > 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void Reset(T model)
        {
            Model = model;

            foreach(var dynamicField in DynamicFields)
            {
                dynamicField.Model = model;
            }

            SetStaticFields();
        }

        private void SetStaticFields()
        {
            if (!string.IsNullOrWhiteSpace(idFieldName))
            {
                var property = Helper.SupportedProperties.FirstOrDefault(p => p.Name.Equals(idFieldName));
                if (property != null)
                {
                    Id = Convert.ToInt32(property.GetValue(Model));
                }
            }

            if (!string.IsNullOrWhiteSpace(titleFieldName))
            {
                var property = Helper.SupportedProperties.FirstOrDefault(p => p.Name.Equals(titleFieldName));
                if (property != null)
                {
                    Title = property.GetValue(Model)?.ToString();
                }
            }
        }

        private void BootstrapFlow()
        {
            var flowContextPropertyInfo = Helper.SupportedProperties
                .FirstOrDefault(p => typeof(IFlowContext).IsAssignableFrom(p.PropertyType));
            
            if(flowContextPropertyInfo != null) 
            {
                FlowContext = (IFlowContext)flowContextPropertyInfo.GetValue(Model);

                FlowContext.Flow.Bootstrap(FlowContext?.GetFlowHistory(), true);
            }
        }

        private void BuildDynamicComponents()
        {
            SetStaticFields();

            DynamicFields = new List<DynamicField>();

            var constantExpression = Expression.Constant(Model);

            DynamicFields = new List<DynamicField>((from p in Helper.SupportedProperties
                                                    join c in Config.ConfigItems on p.Name equals c.PropertyName
                                select CreateDynamicField(Model, constantExpression, p, c)).ToList());

            DynamicFields.AddDynamicArgs();

            RootContainers = Config.ConfigContainers
                .Where(cc => string.IsNullOrWhiteSpace(cc.ParentCode))
                .OrderBy(cc => cc.Order)
                .Select(cc => CreateContainer(cc, DynamicFields, FlowContext)).ToList();

            var fieldGroups = from df in DynamicFields
                              group df by df.ConfigContainerId into fieldGroup
                              select fieldGroup;

            foreach(var rootContainer in RootContainers)
            {
                MapDynamicContainerFields(rootContainer, fieldGroups);
            }
        }

        private static DynamicField CreateDynamicField(T model, ConstantExpression expression, PropertyInfo propertyInfo, ConfigItem configItem)
        {
            var dynamicField = new DynamicField
            {
                Model = model,
                Label = configItem.Label,
                Tooltip = configItem.Tooltip,
                Order = configItem.Order,
                ConfigName = configItem.ConfigName,
                ConfigContainerId = configItem.ConfigContainer.ConfigContainerId,
                ComponentArgs = configItem.ComponentArgs,
                PropertyInfo = propertyInfo,
                PropertyName = propertyInfo.Name,
                DynamicComponentTypeName = configItem.Component,
                DynamicComponent = Type.GetType(configItem.Component),
                MemberExpression = Expression.Property(expression, propertyInfo.Name)
            };

            dynamicField.Parameters.Add(Parameters.FIELD, dynamicField);

            return dynamicField;
        }

        private static DynamicContainer CreateContainer(ConfigContainer configContainer, List<DynamicField> dynamicFields, IFlowContext flowContext)
        {
            var dynamicContainer = new DynamicContainer
            {
                ContainerId = configContainer.ConfigContainerId,
                DynamicComponent = Type.GetType(configContainer.Container),
                DynamicContainerTypeName = configContainer.Container,
                Label = configContainer.Label,
                Code = configContainer.Code,
                ParentCode = configContainer.ParentCode,
                ComponentArgs = configContainer.ComponentArgs,
                FlowArgs = configContainer.FlowArgs,
                FlowContext = flowContext
            };

            dynamicContainer.AddDynamicArgs(dynamicFields);

            dynamicContainer.Parameters.Add(Parameters.CONTAINER, dynamicContainer);

            if(configContainer.ConfigContainers.Any())
            {
                foreach(var container in  configContainer.ConfigContainers)
                {
                    dynamicContainer.DynamicContainers.Add(CreateContainer(container, dynamicFields, flowContext));
                }
            }

            return dynamicContainer;
        }

        private static void MapDynamicContainerFields(DynamicContainer container, IEnumerable<IGrouping<int,DynamicField>> fieldGroups)
        {
            var fieldGroup = fieldGroups.FirstOrDefault(fg => fg.Key.Equals(container.ContainerId));

            if(fieldGroup != null)
            {
                foreach(var field in fieldGroup)
                {
                    field.ContainerUniqueId = container.UniqueId;
                }

                container.DynamicFields.AddRange(fieldGroup.OrderBy(f => f.Order).ToList());
            }

            foreach(var c in container.DynamicContainers)
            {
                MapDynamicContainerFields(c, fieldGroups);
            }
        }
    }
}
