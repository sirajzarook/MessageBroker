using Dropship.Ofs.Processor.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Dropship.IntegrationTests
{
    public class UpdateAddressShould
    {
        [Fact]
        public void ListenOnUpdateAddressAndCallSendOdrer()
        {
            var fakeMessageToSend = new CreateOrderMessage
            {
                Id = Guid.NewGuid(),
                IsTest = true,
                OrderId = 53462,
                OrderName = "Test order",
            };

            var startup = new TestStartup(Ofs.Processor.ApplicationMode.UPDATE_ADDRESS);
            startup.Start();

            startup.SendFakeMessage(fakeMessageToSend);
            //Wait 3 second. 
            Task.Delay(3000);

            //Assert: CheckIn after processing should send IMessage to update address queue.
            Assert.IsType(fakeMessageToSend.GetType(), startup.Message);
            var receivedMessage = (CreateOrderMessage)startup.Message;
            Assert.NotNull(receivedMessage);
            Assert.Equal(fakeMessageToSend.Id, receivedMessage.Id);
            Assert.Equal(fakeMessageToSend.IsTest, receivedMessage.IsTest);
            Assert.Equal(fakeMessageToSend.OrderId, receivedMessage.OrderId);
            Assert.Equal(fakeMessageToSend.OrderName, receivedMessage.OrderName);
            Assert.Equal("sendorder", startup.Topic);
        }
    }
}
