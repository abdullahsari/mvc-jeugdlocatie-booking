<%@ Page Title="Registeren - Locs 4 Youth" Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Locs4Youth.Register.Register" %>
<!DOCTYPE html>
<html lang="nl-be">
<head>
    <meta charset="utf-8" />
    <title>Registreren - Locs 4 Youth</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="theme-color" content="#008eb2" />
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet"/>
    <link href="/Content/css/library.css" rel="stylesheet"/>
    <link href="/Content/css/styles.css" rel="stylesheet"/>
    <script src="/Content/js/modernizr-2.6.2.js"></script>
</head>
<body>
    <header>
        <h1 class="structural">Locs 4 Youth</h1>
        <div class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <a class="navbar-brand" href="/" id="title">Locs 4 Youth</a>
                </div>
                <div id="menu" class="navbar-collapse collapse">
                    <ul class="nav navbar-nav">
                        <li><a href="/">Home</a></li>
                        <li><a href="/">Locaties</a></li>
                        <li><a href="/Home/FAQ">FAQ</a></li>
                        <li><a href="/Home/Contact">Contact</a></li>
                    </ul>
                    <ul class="nav navbar-nav navbar-right">
                        <li><a class="active" href="/Account/SignOn">Registreren</a></li>
                        <li><a href="/Account/SignIn">Inloggen</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </header>
    <div class="container body-content">
        <section>
            <h2>Registreren</h2>
            <form id="form1" runat="server" class="container container--small">
                <h2>Registreren</h2>
                <p>Een L4Y-account is zeer handig. Zo kunt u zeer makkelijk locaties (ver)huren.</p>
                <asp:Label Text="De gebruiker bestaat reeds." ID="lbl_message" CssClass="text-danger" Visible="false" runat="server" />
                <div class="form-group">
                    <asp:Label Text="Naam" runat="server" CssClass="control-label" />
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-user" aria-hidden="true"></i></span>
                        <asp:TextBox ID="Lastname" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="Lastname" runat="server" ErrorMessage="Een achternaam is vereist." CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <asp:Label Text="Voornaam" runat="server" CssClass="control-label" />
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-user" aria-hidden="true"></i></span>
                        <asp:TextBox ID="Firstname" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="Firstname" runat="server" ErrorMessage="Een voornaam is vereist." CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <asp:Label Text="E-mailadres" runat="server" CssClass="control-label" />
                    <div class="input-group">
                        <span class="input-group-addon"><b>@</b></span>
                        <asp:TextBox TextMode="Email" ID="Email" runat="server" CssClass="form-control" />
                    </div>
                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator3" ControlToValidate="Email" runat="server" ErrorMessage="Een e-mailadres is vereist."></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Ongeldig e-mailadres." ControlToValidate="Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger"></asp:RegularExpressionValidator>
                </div>
                <div class="form-group">
                    <asp:Label Text="Wachtwoord" runat="server" CssClass="control-label" />
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-lock" aria-hidden="true"></i></span>
                        <asp:TextBox ID="Password" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator4" ControlToValidate="Password" runat="server" ErrorMessage="Een wachtwoord is vereist."></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <asp:Label Text="Wachtwoord bevestigen" runat="server" CssClass="control-label" />
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-lock" aria-hidden="true"></i></span>
                        <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator CssClass="text-danger" ID="RequiredFieldValidator5" ControlToValidate="ConfirmPassword" runat="server" ErrorMessage="Gelieve het wachtwoord te bevestigen."></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" ControlToCompare="Password" ControlToValidate="ConfirmPassword" runat="server" ErrorMessage="De wachtwoorden komen niet overeen." CssClass="text-danger"></asp:CompareValidator>
                </div>
                <asp:HiddenField ID="antiforgery" runat="server" />
                <div class="form-group">
                    <asp:Button ID="submit" runat="server" Text="Registreren" CssClass="btn btn-success" OnClick="submit_Click" />
                </div>
                <a href="/Account/SignIn">Al geregistreerd? Log in.</a>
            </form>
        </section>        
        <hr />
        <footer class="container">
            <h2 class="structural">Footer</h2>
            <div class="pull-left">
                <img id="logo" alt="Jeugdlocatie-Booking Logo" src="/Content/img/logo.png" />
            </div>
            <div class="pull-right">
                <p>
                    &copy; 2017 - Locs 4 Youth |
                    <a href="/Help" target="_blank">API</a> |
                    <a href="http://matthiashoebeke.ikdoeict.net" target="_blank">Matthias</a> |
                    <a href="http://jlb.decocklaurens.ikdoeict.net/" target="_blank">Laurens</a>
                </p>
            </div>
        </footer>
    </div>
    <script src="/Content/js/jquery-2.1.4.min.js"></script>
    <script src="/Content/js/bootstrap.min.js"></script>
    <script src="/Content/js/respond.min.js"></script>
</body>
</html>