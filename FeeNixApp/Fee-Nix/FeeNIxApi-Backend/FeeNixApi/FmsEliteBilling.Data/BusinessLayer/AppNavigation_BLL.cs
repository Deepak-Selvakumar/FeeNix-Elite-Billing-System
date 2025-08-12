using FmsEliteBilling.Data.Oracle;
using FmsEliteBilling.Model.AppNavigationModel;

namespace FmsEliteBilling.Data.BusinessLayer
{
    public class AppNavigation_BLL
    {
        public List<MenuList> GetFMSMenuList(List<MenuList> menulst)
        {
            List<MenuList> resultmenu=new List<MenuList>();
            var menuarray=menulst.ToArray();
            foreach (MenuList item in menuarray)
            {

                var menufilter=resultmenu.FindAll(x=>x.id==item.id);
                if(menufilter.Count==0)
                {
                    MenuList menu=new MenuList();
                    menu=item;
                    menu.subMenu=new List<MenuList>();
                    var list= Array.FindAll(menuarray,x=>x.ParentMenuId==menu.id);
                    foreach(var submenuitem in list)
                    {
                        MenuList submenu=new MenuList();
                        submenu=submenuitem;
                        submenu.subMenu=new List<MenuList>();
                        var submenulist= Array.FindAll(menuarray,x=>x.ParentMenuId==submenu.id);


                       
                        foreach(var subchildmenuItem in submenulist)
                        {
                            MenuList subchildmenu=new MenuList();
                            subchildmenu=submenuitem;
                            submenu.subMenu.Add(subchildmenu);
                           
                            
                        }
                      
                        menulst.RemoveAll(x=>x.ParentMenuId==submenu.id);
                        menu.subMenu.Add(submenu);

                    }
                 
                    menulst.RemoveAll(x=>x.ParentMenuId==menu.id);

                    resultmenu.Add(menu);
                }
                menulst.RemoveAll(x=>x.ParentMenuId==item.id);

            }
            return resultmenu;
        }


        public List<FMSMenu> GetMenuListResult(List<MenuList> menulst)
        {
            List<FMSMenu> resultmenu=new List<FMSMenu>();
           foreach(var item in menulst)
           {
                FMSMenu menu=new FMSMenu();
                menu.id=item.id;
                menu.name=item.name;
                menu.imgUrl=item.Icon;
                menu.url=item.url;
                menu.subMenu=new List<SubMenu>();
                foreach(var submenuitem in item.subMenu!)
                {
                    SubMenu subMenu=new SubMenu();
                    subMenu.title=submenuitem.name;
                    subMenu.menu=new List<ChildMenu>();
                    foreach(var childitem in submenuitem.subMenu!)
                    {
                        ChildMenu childMenu=new ChildMenu();
                        childMenu.id=childitem.id;
                        childMenu.name=childitem.name;
                       
                        childMenu.url=childitem.url;
                        subMenu.menu.Add(childMenu);
                    }
                   
                    menu.subMenu.Add(subMenu);
                }
                resultmenu.Add(menu);
           }
            return resultmenu;
        }
    }
}