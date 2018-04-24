using System.ServiceModel;

namespace CloudFoundryWcf
{
    [ServiceContract]
    public interface IValueService
    {

        [OperationContract]
        string GetData();
    }
}
