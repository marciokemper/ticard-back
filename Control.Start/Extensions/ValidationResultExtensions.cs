﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Control.Facilites.Extensions
{
    public static class ValidationResultExtensions
    {
        public static string[] GetErrors(this ValidationResult validationResult)
        {
            var result = new List<string>();

            if (validationResult != null && validationResult.Errors != null)
                foreach (var error in validationResult.Errors)
                    result.Add(error.ErrorMessage);

            return result.ToArray();
        }
    }
}
