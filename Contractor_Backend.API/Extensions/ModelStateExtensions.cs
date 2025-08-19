using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Contractor_Backend.API.Extensions
{
    public static class ModelStateExtensions
    {
        public static List<string> ExtractErrors(this ModelStateDictionary modelState)
        {
            return modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
        }
    }
}
