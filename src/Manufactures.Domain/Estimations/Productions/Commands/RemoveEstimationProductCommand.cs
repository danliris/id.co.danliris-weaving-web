using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class RemoveEstimationProductCommand : ICommand<EstimatedProductionDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}
