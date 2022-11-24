<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="capabilities.aspx.cs" Inherits="WURFL.Web.capabilities" %>
<asp:content id="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
    
<h1>Capabilities</h1>
  
        <asp:GridView ID="capabilities_GridView" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True" 
        onrowdatabound="GridView1_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
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
