
@If Request.IsAuthenticated Then
    Using (Html.BeginForm("LogOff", "Account", FormMethod.Post, New With {.id = "logoutForm"}))
        @Html.AntiForgeryToken()
        @<ul class="nav navbar-nav navbar-right">
            <li><a href="#" onclick="return false">Hello @User.Identity.Name!</a></li>
            <li><a href="javascript:document.getElementById('logoutForm').submit()">Log off</a></li>
        </ul>
    End Using
Else
    @<ul Class="nav navbar-nav navbar-right">
        <li>@Html.ActionLink("Log in", "AuthorizeSSO", "Account")</li>
    </ul>
End If
