<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="Devices.aspx.cs" Inherits="WURFL.Web.devices" %>

<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">


    <br />
    <asp:Label runat="server" Text="Brands" />&nbsp;<asp:DropDownList ID="BrandsList" runat="server" AutoPostBack="false" >
    </asp:DropDownList>&nbsp;<asp:Label runat="server" Text="Name" />&nbsp;<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>&nbsp;<asp:Button ID="Search" runat="server" Text="Search" OnClick="Search_Click" />

    <br />
    <br />
    <asp:Label ID="sms" runat="server" Text="there is no devices" style="display:none"/>
    <div id="div1" runat="server" style="display:none" >
    <asp:Label runat="server" Text="Devices List" />
    <asp:DataList ID="DevicesList" runat="server" RepeatDirection="Horizontal"  OnItemDataBound="Item_Bound"
            RepeatColumns="5" CellPadding="4" ForeColor="#333333" Width="100%" >
                  <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White"  />
                  <HeaderTemplate>
              <table border="1" style="max-width:1000px;">
                    <tr >
                        <th colspan="5" >Title</th>
                    </tr>
            </HeaderTemplate>
                  <ItemStyle BackColor="#E3EAEB" />
            <ItemTemplate>
           
            <asp:HyperLink  style="width:200px;" ID="HyperLink1" runat="server" NavigateUrl="~/devices.aspx"></asp:HyperLink>
            </ItemTemplate>
                  
                  <AlternatingItemStyle BackColor="White" />
                  <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                  
           <FooterTemplate>
                </table>
            </FooterTemplate>
                  <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
        </asp:DataList>

    </div>
</asp:Content>
