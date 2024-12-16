using Microsoft.AspNetCore.Mvc;
using Webbandothethao.Models;

namespace Webbandothethao.Components
{
    [ViewComponent(Name = "MenuView")]
    public class MenuViewComponent : ViewComponent
    {
        private DataContext _context;

        public MenuViewComponent(DataContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Kiểm tra nếu người dùng đã xác thực

            var listofMenu = (from m in _context.tblMenus
                              where (m.IsActive == true) && (m.Position == 1)
                              select m).ToList();
            return await Task.FromResult((IViewComponentResult)View("Default", listofMenu));

        }
    }
}
