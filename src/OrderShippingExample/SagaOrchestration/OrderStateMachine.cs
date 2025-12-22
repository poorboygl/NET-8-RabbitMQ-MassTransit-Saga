using MassTransit;
using static SharedMessages.Messages;

namespace SagaOrchestration;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State Submitted { get; private set; }
    public State InventoryReserved { get; private set; }
    public State PaymentCompleted { get; private set; }

    public Event<OrderPlaced> OrderPlacedEvent { get; private set; }
    public Event<InventoryReserved> InventoryReservedEvent { get; private set; }
    public Event<PaymentCompleted> PaymentCompletedEvent { get; private set; }

}
