namespace FmsEliteBilling.Model.Response
{
    public class GetDetailResponse<T>
    {
         public ResponseModel? response{get;set;}
        public T? value{get;set;}
    }
}