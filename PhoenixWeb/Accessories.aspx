<%@ Page Title="Accessories" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Accessories.aspx.cs" Inherits="PhoenixWeb.Accessories" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Phoenix Computers</h1>
        <h3>Accessories</h3>
        <p class="lead">This page contains a list of the accessory products available.
        <p></p>
    </div>

    <div class="row">
        <center>

        <table id="dynamicTable"  style="width:90%;" >

            <asp:PlaceHolder ID="placeholder" runat="server"></asp:PlaceHolder>

        </table>
            </center>
     </div>
        

</asp:Content>


