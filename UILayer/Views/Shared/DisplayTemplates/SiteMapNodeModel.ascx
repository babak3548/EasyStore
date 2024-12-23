<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl`1[[MvcSiteMapProvider.Web.Html.Models.SiteMapNodeModel,MvcSiteMapProvider]]" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="MvcSiteMapProvider.Web.Html.Models" %>

<% if (Model.IsCurrentNode && Model.SourceMetadata["HtmlHelper"].ToString() != "MvcSiteMapProvider.Web.Html.MenuHelper")  { %>
   <h5 class="h5"> <%=Model.Title %></h5>
<% } else if (Model.IsClickable) { %>
    <a href="<%=Model.Url %>"><%=Model.Title %></a>
<% } else { %>
   <h5 class="h5"> <%=Model.Title %></h5>
<% } %>