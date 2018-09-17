using Autofac;
using WAL.EventBus.Abstract;

namespace WAL.EventBus.RabbitMQ
{
    public class EventBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EventBus>().As<IPubSub>().As<IBusRequest>().As<IBusResponse>()
                .InstancePerRequest().InstancePerDependency();
            base.Load(builder);
        }
    }
}