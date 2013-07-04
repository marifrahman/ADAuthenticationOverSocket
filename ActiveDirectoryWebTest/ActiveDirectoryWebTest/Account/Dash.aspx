<%@ Page Title="Private Dashborad" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Dash.aspx.cs" Inherits="ActiveDirectoryWebTest.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Dashboard
    </h2>
    <p>
        You have successfuly logged in. !! You are now in a page to which you can get 
        access after your credential is authenticated over Rapid Global active 
        directory.</p>
    <p>
        Please check the follwoing links for the designs how the process actually 
        working
        <a target="_blank" href="http://tinyurl.com/rgad-design">
        http://tinyurl.com/rgad-design</a></p>
</asp:Content>
