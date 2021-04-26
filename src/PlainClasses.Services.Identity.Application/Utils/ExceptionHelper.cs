using PlainClasses.Services.Identity.Domain.SharedKernels;

namespace PlainClasses.Services.Identity.Application.Utils
{
    public static class ExceptionHelper
    {
        public static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }
    }
}