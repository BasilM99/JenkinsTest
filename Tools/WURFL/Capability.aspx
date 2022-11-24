<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="Capability.aspx.cs"
    Inherits="WURFL.Web._Default" %>
<asp:content id="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div>
        <p>
            <asp:Label runat="server" text="UserAgent"/>&nbsp; <asp:TextBox ID="txtUserAgent" runat="server" Width="782px"></asp:TextBox>
            <asp:Button ID="Load" runat="server" OnClick="Load_Click" Text="Load" />
        </p>
        <br/>
        
        <asp:GridView ID="GridView1" width="600px" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True" 
        onrowdatabound="GridView1_RowDataBound" ShowHeaderWhenEmpty="True">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"  />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#D4DFE1" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />

        </asp:GridView>


    </div>
</asp:content>
