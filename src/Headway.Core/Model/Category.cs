using FluentValidation;
using Headway.Core.Attributes;
using Headway.Core.Interface;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Headway.Core.Model
{
    [DynamicModel]
    public class Category : IPermissionable
    {
        public Category()
        {
            MenuItems = new List<MenuItem>();
        }

        public int CategoryId { get; set; }
        public int Order { get; set; }
        public List<MenuItem> MenuItems { get; set; }

        [Required]
        public Module Module { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Icon { get; set; }

        [Required]
        [StringLength(20)]
        public string Permission { get; set; }

        public bool IsPermitted(IEnumerable<string> permissions)
        {
            if(permissions.Contains(Permission))
            {
                var count = MenuItems.Count;

                if (count > 0)
                {
                    for (int i = count - 1; i >= 0; i--)
                    {
                        if (MenuItems[i].IsPermitted(permissions))
                        {
                            continue;
                        }

                        MenuItems.RemoveAt(i);
                    }
                }

                return true;
            }
            else
            {
                MenuItems.Clear();
                return false;
            }
        }
    }

    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(v => v.Module)
                .NotNull().WithMessage("Module is required");

            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 20).WithMessage("Name cannot exceed 20 characters");

            RuleFor(v => v.Icon)
                .NotNull().WithMessage("Icon is required")
                .Length(1, 30).WithMessage("Name cannot exceed 30 characters");

            RuleFor(v => v.Permission)
                .NotNull().WithMessage("Permission is required")
                .Length(1, 20).WithMessage("Permission cannot exceed 20 characters");
        }
    }
}
