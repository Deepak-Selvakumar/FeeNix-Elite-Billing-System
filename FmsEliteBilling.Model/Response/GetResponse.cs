namespace FmsEliteBilling.Model.Response
{
    public class GetResponse<T>
    {
        public ResponseModel? response{get;set;}
        public List<T>? value{get;set;} 
    }
}