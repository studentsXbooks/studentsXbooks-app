using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace sXb_service.Helpers.ModelValidation
{
    //code source: https://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/  
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; set; }

        public string Message { get; set; }

        public ValidationError()
        {

        }
        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }   
    }

    public class ValidationResultModel
    {
        public ValidationResultModel()
        {

        }
        public string Message { get; set; }

        public List<ValidationError> Errors { get; set; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Message = "Validation Failed";
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }
}
