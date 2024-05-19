using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using WhatsApp.Repository;

namespace backend.Validators
{
    public class UniqueAttribute : ValidationAttribute
    {
        // private readonly Type _repositoryType;
        // private readonly string _propertyName;

        // public UniqueAttribute(Type repositoryType, string propertyName)
        // {
        //     _repositoryType = repositoryType;
        //     _propertyName = propertyName;
        // }

        // protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        // {
        //     // Resolve the repository instance using dependency injection
        //     var repository = (IUserRepository<User>)validationContext.GetService(_repositoryType);

        //     // Check if the value already exists in the repository
        //     var exists = repository.Exists(_propertyName, value);

        //     if (exists)
        //     {
        //         return new ValidationResult(ErrorMessage ?? $"The value '{value}' is not unique.");
        //     }

        //     return ValidationResult.Success;
        // }
    }
}