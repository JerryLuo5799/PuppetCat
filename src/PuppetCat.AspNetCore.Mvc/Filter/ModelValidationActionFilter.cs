using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PuppetCat.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;


public class ModelValidationActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            ResponseNoData res = new ResponseNoData { result = (int)ResponseStatusCode.BadRequest};
          

            foreach (var item in context.ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    res.msg += error.ErrorMessage + ";";
                }
            }
            context.Result = new JsonResult(res);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}