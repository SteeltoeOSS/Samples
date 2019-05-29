Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        Return View()
    End Function


    <Authorize>
    Function Secure() As ActionResult
        ViewBag.Title = "Steeltoe Legacy ASP.NET Security Samples"
        ViewBag.Message = "You're now logged in as " + User.Identity.Name
        Return View("Index")
    End Function



    <CustomClaimsAuthorize("testgroup")>
    Function About() As ActionResult
        ViewData("Message") = "Your application description page."

        Return View()
    End Function

    <CustomClaimsAuthorize("testgroup1")>
    Function Contact() As ActionResult
        ViewData("Message") = "Your contact page."

        Return View()
    End Function
End Class
