using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNet_Core6.Fundamentals.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {
            var resultErrors = new List<string>();

            foreach (var value in modelState.Values)

                resultErrors.AddRange(value.Errors.Select(error => error.ErrorMessage));
            return resultErrors;
        }
    }
}
