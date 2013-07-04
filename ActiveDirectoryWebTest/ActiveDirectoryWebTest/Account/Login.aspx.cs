using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.DirectoryServices.AccountManagement;

using ActiveDirectoryWebTest.Classes;
using System.Data.SqlClient;


namespace ActiveDirectoryWebTest.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        private string username = "";
        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            //using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "ad.local"))
            //{
            //    // validate the credentials
            //    //pc.UserPrinciple.FindByIdentity
            //    UserPrincipal userprincipal = UserPrincipal.FindByIdentity(pc, IdentityType.Name, "Habijabi");

            //    bool isValid = pc.ValidateCredentials("arahaman", "Allah4all");

            //    if (isValid)
            //        Response.Redirect("~/About.aspx");


            //}


            if (doAdLogin(LoginUser.UserName,LoginUser.Password))
            {
                FormsAuthentication.SetAuthCookie(username, true);
                Response.Redirect("~/Account/Dash.aspx");

            }
        }

        private  bool doAdLogin(string username, string pwd)
        {
            bool isValid = false;
            string sid = "";
            //using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "ad.local"))
            //{

            //    UserPrincipal userprincipal = UserPrincipal.FindByIdentity(pc, username);
            //    if (userprincipal != null)
            //    {
            //        isValid = pc.ValidateCredentials(username, pwd);
            //        //if (isValid)

            //        //else
            //        //    label1.Text = "Not Authenticated";


            //    }
            //    //else
            //    //{
            //    //    label1.Text = "Din find the user";
            //    //}


            //}
            AsynchClientWeb client = new AsynchClientWeb();
            try
            {
                
                client.StartClient(username, pwd);
                //AsynchClientWeb.StartClient(username, pwd);
                sid = client.response;
                if (checkSid(sid))
                    isValid = true;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                client.EndClient();
               
            }

            //Response.Write(sid);
            //Response.End();
            return isValid;
        }

        private bool checkSid(string sid)
        {
            string query = "Select UserName from tblUsers where sid = '" + sid+"'";
            bool isUserIntheList = false;
            using (SqlCommand cmd = new SqlCommand(query, new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["funCon"].ConnectionString)))
            {
                try
                {
                    cmd.Connection.Open();
                    var isSid = cmd.ExecuteScalar();
                    if (isSid != null)
                    {
                        isUserIntheList = true;
                        username = isSid.ToString();
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    cmd.Connection.Close();
                }

            }

            return isUserIntheList;
        }

    }

    
}
