using Grpc.Net.Client;
using AspWebApiGbProto;

namespace EmployeeServiceClient;

static class EmployeeServiceClient
{
    public static void Main()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:5001");
        var client = new DictionariesService.DictionariesServiceClient(channel);

        Console.Write("Ender new employee type: ");
        string newEmployeeType = Console.ReadLine();
        
        client.CreateEmployeeType(new CreateEmployeeTypeRequest()
        {
            Description = newEmployeeType
        });
        
        Console.WriteLine(client.GetAllEmployeeTypes(new GetAllEmployeeTypesRequest()));
    }
}