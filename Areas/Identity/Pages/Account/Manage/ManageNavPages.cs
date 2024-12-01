#nullable disable

using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exam.Areas.Identity.Pages.Account.Manage
{

    // Static class for managing navigation and setting active page styles in the account management section.
    public static class ManageNavPages
    {

        // Determines the active navigation class for the Index page.
        public static string Index => "Index";

        // Determines the active navigation class for the Email page.
        public static string Email => "Email";

        // Determines the active navigation class for the Change Password page.
        public static string ChangePassword => "ChangePassword";
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        // Helper method to determine the active navigation class based on the current page.
        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
