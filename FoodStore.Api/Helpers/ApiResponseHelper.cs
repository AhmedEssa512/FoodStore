using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodStore.Contracts.Common;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoodStore.Api.Helpers
{
    public class ApiResponseHelper
    {
        public static ApiResponse<Dictionary<string, string[]>> FromModelState(ModelStateDictionary modelState)
        {
            var errors = modelState
            .Where(kvp => kvp.Value is { Errors.Count: > 0 }) // only include fields with errors
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return ApiResponse<Dictionary<string, string[]>>.FailWithErrors(errors,"Validation failed");
        }
    }
}