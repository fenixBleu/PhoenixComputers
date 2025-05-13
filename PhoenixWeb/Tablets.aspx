<%@ Page Title="Tablets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tablets.aspx.cs" Inherits="PhoenixWeb.Tablets" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Phoenix Computers</h1>
        <h3>Tablets</h3>
        <p class="lead">This page contains a list of the tablet products available.
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