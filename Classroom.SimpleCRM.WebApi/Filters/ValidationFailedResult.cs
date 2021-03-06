﻿using Classroom.SimpleCRM.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Classroom.SimpleCRM.WebApi.Filters
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ValidationStateModel(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
        public ValidationFailedResult(ModelStateDictionary modelState, int statusCode)
            : base(new ValidationStateModel(modelState))
        {
            StatusCode = statusCode;
        }
        public ValidationFailedResult(IEnumerable<ValidationError> errors)
            : base(new ValidationStateModel(errors))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
        public ValidationFailedResult(IEnumerable<ValidationError> errors, int statusCode)
            : base(new ValidationStateModel(errors))
        {
            StatusCode = statusCode;
        }
        public ValidationFailedResult(IEnumerable<string> errors)
            : base(new ValidationStateModel(errors))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
        public ValidationFailedResult(IEnumerable<string> errors, int statusCode)
            : base(new ValidationStateModel(errors))
        {
            StatusCode = statusCode;
        }
        public ValidationFailedResult(string error)
            : base(new ValidationStateModel(error))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
        public ValidationFailedResult(string error, int statusCode)
            : base(new ValidationStateModel(error))
        {
            StatusCode = statusCode;
        }
    }
}
