namespace FmsEliteBilling.Model.AppNavigationModel
{
    public class FMSMenu
    {
        public long id{get;set;}
        public string? name{get;set;}
        public string? url{get;set;}
        public string? imgUrl{get;set;}
        public List<SubMenu>? subMenu{get;set;}
         

    }
}