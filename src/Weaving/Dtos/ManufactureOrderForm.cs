using FluentValidation;

namespace Weaving.Dtos
{
    public class ManufactureOrderForm
    {
        public int UnitDeptId { get; set; }
    }

    public class ManufactureOrderFormValidator : AbstractValidator<ManufactureOrderForm>
    {
        public ManufactureOrderFormValidator()
        {
            RuleFor(r => r.UnitDeptId).GreaterThan(0);
        }
    }
}