using Headway.Core.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Core.Helpers
{
    public static class GenericTreeHelper
    {
        public static List<K> GetTree<T,K>(
            T model,
            GenericTreeHelperArgs genericTreeHelperArgs) 
            where T : class, new()
            where K : class, new()
        {
            var itemTypeHelper = DynamicTypeHelper.Get<K>();
            var modelTypeHelper = DynamicTypeHelper.Get<T>();
            int modelId = (int)modelTypeHelper.GetValue(model, genericTreeHelperArgs.ModelIdProperty);
            var items = (List<K>)modelTypeHelper.GetValue(model, genericTreeHelperArgs.ItemsProperty);
            var modelItemsProperty = modelTypeHelper.GetPropertyInfo(genericTreeHelperArgs.ItemsProperty);

            List<K> tree = new();

            ValidateFlattenedTree(items, modelId, genericTreeHelperArgs, itemTypeHelper);

            foreach (var item in items)
            {
                var children = items
                    .Where(i => !string.IsNullOrWhiteSpace(itemTypeHelper.GetValue(i, genericTreeHelperArgs.ParentItemCodeProperty)?.ToString())
                                && itemTypeHelper.GetValue(i, genericTreeHelperArgs.ParentItemCodeProperty).ToString().Equals(itemTypeHelper.GetValue(item, genericTreeHelperArgs.ItemCodeProperty)?.ToString()))
                    .OrderBy(i => itemTypeHelper.GetValue(i, genericTreeHelperArgs.OrderByProperty))
                    .ToList();

                foreach(var child in children)
                {
                    modelItemsProperty.PropertyType.GetMethod("Add").Invoke(
                        (List<K>)modelItemsProperty.GetValue(model, null), new[] { child });
                }

                tree.Add(item);
            }

            return tree
                .Where(i => string.IsNullOrWhiteSpace(itemTypeHelper.GetValue(i, genericTreeHelperArgs.ParentItemCodeProperty)?.ToString()))
                .OrderBy(i => itemTypeHelper.GetValue(i, genericTreeHelperArgs.OrderByProperty))
                .ToList();
        }

        public static List<K> GetFlattenedTree<T,K>(
            T model,
            GenericTreeHelperArgs genericTreeHelperArgs)
            where T : class, new()
            where K : class, new()
        {
            var itemTypeHelper = DynamicTypeHelper.Get<K>();
            var modelTypeHelper = DynamicTypeHelper.Get<T>();
            int modelId = (int)modelTypeHelper.GetValue(model, genericTreeHelperArgs.ModelIdProperty);
            var items = (List<K>)modelTypeHelper.GetValue(model, genericTreeHelperArgs.ItemsProperty);

            List<K> flattenedTree = new();

            FlattenTree(items, modelId, flattenedTree, genericTreeHelperArgs, itemTypeHelper);
            ValidateFlattenedTree(flattenedTree, modelId, genericTreeHelperArgs, itemTypeHelper);
            return flattenedTree;
        }

        private static void ValidateFlattenedTree<T>(
            List<T> items, 
            int modelId, 
            GenericTreeHelperArgs genericTreeHelperArgs,
            DynamicTypeHelper<T> typeHelper) 
            where T : class, new()
        {
            if (items.Any(i => string.IsNullOrWhiteSpace(typeHelper.GetValue(i, genericTreeHelperArgs.ItemCodeProperty)?.ToString())))
            {
                throw new Exception($"{typeof(T).Name} {modelId} missing item code");
            }

            var itemCodes = items.Select(i => typeHelper.GetValue(i, genericTreeHelperArgs.ItemCodeProperty)?.ToString()).ToList();

            var children = items
                .Where(i => !string.IsNullOrWhiteSpace(typeHelper.GetValue(i, genericTreeHelperArgs.ParentItemCodeProperty)?.ToString()))
                .ToList();

            foreach (var child in children)
            {
                if (!itemCodes.Any(c => c.Equals(typeHelper.GetValue(child, genericTreeHelperArgs.ParentItemCodeProperty)?.ToString())))
                {
                    typeHelper.SetValue(child, genericTreeHelperArgs.ParentItemCodeProperty, string.Empty);
                }
            }
        }

        private static void FlattenTree<T>(
            List<T> items, 
            int modelId, 
            List<T> tree, 
            GenericTreeHelperArgs genericTreeHelperArgs, 
            DynamicTypeHelper<T> typeHelper) 
            where T : class, new()
        {
            foreach (var item in items)
            {
                typeHelper.SetValue(item, genericTreeHelperArgs.ModelIdProperty, modelId);
                tree.Add(item);
                var children = (List<T>)typeHelper.GetValue(item, genericTreeHelperArgs.ItemsProperty);
                FlattenTree(children, modelId, tree, genericTreeHelperArgs, typeHelper);
                children.Clear();
            }
        }
    }
}