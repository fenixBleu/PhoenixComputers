<%@ Page Title="Locations" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Locations.aspx.cs" Inherits="PhoenixWeb.Locations" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Phoenix Computers</h1>
        <h3>Locations</h3>
        <p class="lead">This page lists our store locations.&nbsp; You can select a City in the drop down list below and click Submit to filter the stores.&nbsp; Reload or reset clears the filter.<p class="lead">&nbsp;<p>
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="False" Width="263px">
        </asp:DropDownList>
        <asp:Label ID="Label1" runat="server" Enabled="False" Visible="False"></asp:Label>
        <asp:Button ID="Submit" runat="server" OnClick="Submit_Click" Text="Submit" />
        <asp:Label ID="Label2" runat="server" Enabled="False" Visible="False"></asp:Label>
        <asp:Button ID="Reset" runat="server" Enabled="False" OnClick="Reset_Click" Text="Reset" />
        </p>
    </div>

    <div class="row">
        <center>

        <table id="dynamicTable"  style="width:90%;" >

            <asp:PlaceHolder ID="placeholder" runat="server"></asp:PlaceHolder>

        </table>
            </center>
     </div>
        

</asp:Content>
