using PostmarkDotNet.Model;

[assembly:Amazon.Lambda.Core.LambdaSerializerAttribute(
    typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace app;

public class Function
{
    public record class Person(string name, int age);
    
    public async Task<Person> Handler(InboundMessage message)
    {
        return await Task.FromResult(new Person("asdf", 42));
    }
}
