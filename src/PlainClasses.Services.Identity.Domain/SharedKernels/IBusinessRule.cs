namespace PlainClasses.Services.Identity.Domain.SharedKernels
{
    public interface IBusinessRule
    {
        bool IsBroken();

        string Message { get; }
    }
}